using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetCore.Business.Extensions;
using NetCore.Business.Services;
using NetCore.Business.Validation;
using NetCore.Data;
using NetCore.Data.Common;
using System;
using System.Threading.Tasks;
using Entities = NetCore.Data.Entities;

namespace NetCore.Business.Authentication
{
    public class AuthenticationService
    {
        private IDatabaseScope _database;
        private IGenericRepository<Entities.User> _userRepository;
        private CryptographyService _cryptographyService;
        private EmailService _emailService;
        private ITokenProvider _tokenProvider;

        public AuthenticationService(IDatabaseScope database,
            IGenericRepository<Entities.User> userRepository,
            CryptographyService cryptographyService,
            EmailService emailService,
            ITokenProvider tokenProvider)
        {
            _database = database;
            _userRepository = userRepository;
            _cryptographyService = cryptographyService;
            _emailService = emailService;
            _tokenProvider = tokenProvider;
        }

        public async Task<AuthenticationInfo> AuthenticateAsync(AuthenticationModel model)
        {
            model.RejectInvalid();

            var user = await _userRepository.AsReadOnly().Include(_ => _.Role).SingleOrDefaultAsync(_ => _.UserName == model.UserName);
            user.RejectNotFound();

            var passwordHash = _cryptographyService.CreateHash(model.Password, user.PasswordSalt);
            if (passwordHash != user.PasswordHash) throw new InvalidModelException();

            var tokenData = Mapper.Map<Entities.User, TokenData>(user);

            var tokenTask = _tokenProvider.CreateTokenAsync(tokenData);
            var refreshTokenTask = _tokenProvider.CreateRefreshTokenAsync(tokenData);

            return new AuthenticationInfo()
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Token = await tokenTask,
                RefreshToken = await refreshTokenTask
            };
        }

        public async Task<AuthenticationInfo> RefreshAuthenticationAsync(AuthenticationRefreshModel model)
        {
            model.RejectInvalid();

            var tokenData = _tokenProvider.GetTokenData(model.Token);
            var refreshTokenData = _tokenProvider.GetTokenData(model.RefreshToken);

            refreshTokenData.RejectUnauthorized(_ => _.UserId != tokenData.UserId);

            var userTokenData = await _userRepository
                .AsReadOnly()
                .ProjectTo<TokenData>()
                .SingleOrDefaultAsync(_ => _.UserId == refreshTokenData.UserId);

            userTokenData.RejectUnauthorized(
                _ => _ == null || _.UserName != tokenData.UserName || _.DisplayName != tokenData.DisplayName || _.Role != tokenData.Role || _.IssuedAt >= refreshTokenData.IssuedAt);

            return new AuthenticationInfo()
            {
                Token = await _tokenProvider.CreateTokenAsync(userTokenData),
                RefreshToken = model.RefreshToken
            };
        }

        public async Task RequestPasswordResetAsync(ResetPasswordModel model)
        {
            model.RejectInvalid();

            var user = await _userRepository.GetAll().SingleOrDefaultAsync(_ => _.UserName == model.Username);
            user.RejectNotFound();

            if (user.PasswordSalt == null)
                user.PasswordSalt = _cryptographyService.CreateSalt();

            // reset code is 6 digit number
            var resetCode = new Random(DateTime.UtcNow.Second).Next(100000, 1000000).ToString();
            user.PasswordResetCode = _cryptographyService.CreateHash(resetCode, user.PasswordSalt);

            await _database.SaveAsync();

            // var placeholders = resetCode.ToPlaceholderDictionary(Placeholder.SECURITY_CODE);
            var succeded = await _emailService.SendEmailAsync(user.Email, EmailTemplateType.RequestPasswordReset, null);
            if (!succeded) throw new InvalidModelException();
        }

        public async Task SetPasswordAsync(SetPasswordModel model)
        {
            model.RejectInvalid();

            var user = await _userRepository.GetAll().SingleOrDefaultAsync(_ => _.UserName == model.UserName);
            user.RejectNotFound();

            var resetCodeHash = _cryptographyService.CreateHash(model.ResetCode, user.PasswordSalt);
            if (resetCodeHash != user.PasswordResetCode) throw new InvalidModelException();

            user.PasswordSalt = _cryptographyService.CreateSalt();
            user.PasswordHash = _cryptographyService.CreateHash(model.Password, user.PasswordSalt);
            user.LastPasswordReset = DateTime.UtcNow;
            user.PasswordResetCode = null;

            await _database.SaveAsync();
        }

        #region Internal methods
        internal static void RegisterMappings(Profile profile)
        {
            profile.CreateMap<Entities.User, TokenData>()
                .ForMember(_ => _.IssuedAt, _ => _.MapFrom(__ => __.LastPasswordReset))
                .ForMember(_ => _.UserId, _ => _.MapFrom(__ => __.Id))
                .ForMember(_ => _.Role, _ => _.MapFrom(__ => __.Role.Name));
        }
        #endregion
    }

}
