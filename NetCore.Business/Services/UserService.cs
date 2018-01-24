using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetCore.Business.Extensions;
using NetCore.Business.Models;
using NetCore.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities = NetCore.Data.Entities;

namespace NetCore.Business.Services
{
    public class UserService
    {
        private readonly IDatabaseScope _database;
        private readonly IGenericRepository<Entities.User> _userRepository;
        private readonly IGenericRepository<Entities.Role> _roleRepository;
        private readonly IGenericRepository<Entities.Country> _countryRepository;
        //private readonly IGenericRepository<Entities.Division> _divisionRepository;
        private readonly IGenericRepository<Entities.Currency> _currencyRepository;

        public UserService(
            IDatabaseScope database,
            IGenericRepository<Entities.User> userRepository,
            IGenericRepository<Entities.Role> roleRepository,
            IGenericRepository<Entities.Country> countryRepository,
            //IGenericRepository<Entities.Division> divisionRepository,
            IGenericRepository<Entities.Currency> currencyRepository)
        {
            _database = database;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _countryRepository = countryRepository;
            //_divisionRepository = divisionRepository;
            _currencyRepository = currencyRepository;
        }

        #region Public methods

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository
                .AsReadOnly()
                .Include(_ => _.Countries)
                //.Include(_ => _.Divisions)
                .ProjectTo<User>()
                .ToListAsync();
        }

        public async Task<User> GetAsync(long id)
        {
            var entity = await _userRepository
                .AsReadOnly()
                .Include(_ => _.Role)
                .Include(_ => _.ParentUser)
                .Include(_ => _.Countries)
                //.Include(_ => _.Divisions)
                .SingleOrDefaultAsync(_ => _.Id == id);

            entity.RejectNotFound();

            return Mapper.Map<Entities.User, User>(entity);
        }

        public async Task<User> CreateAsync(UserBase model)
        {
            model.RejectInvalid();
            await AdditionalValidationAsync(model);

            var entity = new Entities.User();
            UpdateEntity(entity, model);

            _userRepository.Insert(entity);
            await _database.SaveAsync();

            return Mapper.Map<Entities.User, User>(entity);
        }

        public async Task<User> UpdateAsync(long id, UserBase model)
        {
            model.RejectInvalid();
            await AdditionalValidationAsync(model);

            var entity = await _userRepository
                .GetAll()
                .Include(_ => _.Role)
                .Include(_ => _.ParentUser)
                .Include(_ => _.Countries)
                //.Include(_ => _.Divisions)
                .SingleOrDefaultAsync(_ => _.Id == id);

            entity.RejectNotFound();

            UpdateEntity(entity, model);
            await _database.SaveAsync();

            return Mapper.Map<Entities.User, User>(entity);
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _userRepository
                .GetAll()
                .SingleOrDefaultAsync(_ => _.Id == id);

            entity.RejectNotFound();

            _userRepository.Delete(entity);
            await _database.SaveAsync();
        }

        #endregion

        #region Internal methods

        internal static void RegisterMappings(Profile profile)
        {
            profile.CreateMap<Entities.User, User>()
                .ForMember(_ => _.CountryIds, _ => _.MapFrom(__ => __.Countries));
                //.ForMember(_ => _.DivisionIds, _ => _.MapFrom(__ => __.Divisions));

            profile.CreateMap<Entities.UserCountry, long>()
                .ProjectUsing(_ => _.CountryId);

            //profile.CreateMap<Entities.UserDivision, long>()
            //    .ProjectUsing(_ => _.DivisionId);
        }

        #endregion

        #region Private methods

        private void UpdateEntity(Entities.User entity, UserBase model)
        {
            entity.RoleId = model.RoleId;
            entity.ParentUserId = model.ParentUserId;
            entity.UserName = model.UserName;
            entity.Email = model.Email;
            entity.DisplayName = model.DisplayName;

            //entity.Countries.UpdateManyToMany(
            //    source: model.CountryIds,
            //    keySelectorDestination: _ => _.CountryId,
            //    keySelectorSource: _ => _,
            //    converter: _ => new Entities.UserCountry { CountryId = _ });

            //entity.Divisions.UpdateManyToMany(
            //    source: model.DivisionIds,
            //    keySelectorDestination: _ => _.DivisionId,
            //    keySelectorSource: _ => _,
            //    converter: _ => new Entities.UserDivision { DivisionId = _ });
        }

        private async Task AdditionalValidationAsync(UserBase model)
        {
            var role = await _roleRepository
                .AsReadOnly()
                .SingleOrDefaultAsync(_ => _.Id == model.RoleId.Value);

            //if (role == null || role.ParentRoleId.HasValue != model.ParentUserId.HasValue)
            //{
            //    throw new InvalidModelException(); // TODO: Add error
            //}

        }

        #endregion
    }

}
