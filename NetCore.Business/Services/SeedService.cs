using NetCore.Business.Authentication;
using NetCore.Data;
using NetCore.Data.Common;
using NetCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCore.Business.Services
{
    public class SeedService
    {
        private readonly IDatabaseScope _database;
        private readonly IGenericRepository<Currency> _currencyRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<EmailTemplate> _emailTemplateRepository;
        //private readonly IGenericRepository<Settings> _settingsRepository;
        private readonly CryptographyService _cryptographyService;

        private readonly Dictionary<string, Role> allRoles;

        public SeedService(IDatabaseScope database,
            IGenericRepository<Currency> currencyRepository,
            IGenericRepository<Role> roleRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<EmailTemplate> emailTemplateRepository,
            //IGenericRepository<Settings> settingsRepository,
            CryptographyService cryptographyService)
        {
            _database = database;
            _currencyRepository = currencyRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _cryptographyService = cryptographyService;
            _emailTemplateRepository = emailTemplateRepository;
            //_settingsRepository = settingsRepository;

            allRoles = new Dictionary<string, Role>();
        }

        public void Seed()
        {
            SeedCurrencies();
            SeedRoles();
            SeedUsers();
            SeedEmailTemplates();
            //SeedSettings();

            _database.SaveAsync().Wait();
        }

        private void SeedCurrencies()
        {
            var requiredCurrencies = new(string DisplayName, decimal ToUSD, decimal FromUSD, decimal Type365Threshold)[]
            {
                // default currencies values - displayName, toUsd, fromUsd, type 365 threshold
                ("GBP", 1.3029m, 0.7675m, 8000m),
                ("EUR", 1.1201m, 0.8928m, 9080m),
                ("CHF", 1.0285m, 0.9723m, 10624m),
                ("SEK", 0.1166m, 8.5751m, 90168m),
                ("NOK", 0.1234m, 8.1015m, 89311m),
                ("DKK", 0.1506m, 6.6399m, 67590m),
                ("PLZ", 0.2614m, 3.8254m, 38241m),
            };

            var existingCurrencies = _currencyRepository.GetAll().ToList();

            var currenciesToAdd = requiredCurrencies.Where(_ => !existingCurrencies.Any(__ => _.DisplayName == __.DisplayName));
            //var currenciesToModify = existingCurrencies.Where(__ => __.CurrencySettingsId == null);

            foreach (var currencyToAdd in currenciesToAdd)
            {
                var currency = new Currency
                {
                    DisplayName = currencyToAdd.DisplayName,
                    ToUSD = currencyToAdd.ToUSD,
                    FromUSD = currencyToAdd.FromUSD,
                    //CurrencySettings = new CurrencySettings()
                    //{
                    //    Type365Threshold = currencyToAdd.Type365Threshold
                    //}
                };

                _currencyRepository.Insert(currency);
            }

            //foreach (var currencyToModify in currenciesToModify)
            //{
            //    var threshold = requiredCurrencies.Where(_ => _.DisplayName == currencyToModify.DisplayName).Select(_ => _.Type365Threshold).FirstOrDefault();

            //    currencyToModify.CurrencySettings = new CurrencySettings()
            //    {
            //        Type365Threshold = threshold
            //    };
            //}
        }

        private void SeedRoles()
        {
            var requiredRoles = new Dictionary<string, (string DisplayName, RoleRelationCount CountrySpecific, RoleRelationCount DivisionSpecific, string ParentRoleName)>
            {
                //[Role.SalesRep_NAME] = (Role.SalesRep_DISPLAY_NAME, Role.SalesRep_COUNTRY_SPECIFIC, Role.SalesRep_DIVISION_SPECIFIC, Role.SalesRep_PARENT_ROLE_NAME),
                [Role.Administrator_NAME] = (Role.Administrator_DISPLAY_NAME, Role.Administrator_COUNTRY_SPECIFIC, Role.Administrator_DIVISION_SPECIFIC, Role.Administrator_PARENT_ROLE_NAME)
            };

            var existingRoles = _roleRepository.GetAll()
                .ToDictionary(_ => _.Name, _ => _);

            foreach (var requiredRole in requiredRoles)
            {
                if (!existingRoles.TryGetValue(requiredRole.Key, out Role role))
                {
                    role = _roleRepository.Insert(new Role { Name = requiredRole.Key });
                }

                role.DisplayName = requiredRole.Value.DisplayName;
                //role.CountrySpecific = requiredRole.Value.CountrySpecific;
                //role.DivisionSpecific = requiredRole.Value.DivisionSpecific;

                allRoles[requiredRole.Key] = role;
            }

            foreach (var existingRole in existingRoles)
            {
                if (!requiredRoles.ContainsKey(existingRole.Key))
                {
                    _roleRepository.Delete(existingRole.Value);
                }
            }

            //foreach (var withParent in requiredRoles.Where(_ => _.Value.ParentRoleName != null))
            //{
            //    if (allRoles.TryGetValue(withParent.Key, out Role child) && allRoles.TryGetValue(withParent.Value.ParentRoleName, out Role parent))
            //    {
            //        child.ParentRole = parent;
            //    }
            //}
        }

        private void SeedUsers()
        {
            var requiredUsers = new(string Email, string DisplayName, string Password, string Role)[]
            {
                ("admin@netcore.eu", "Administrator", "adminP4$$word", Role.Administrator_NAME)
            };

            var existingUserNames = _userRepository.AsReadOnly().Select(_ => _.UserName).ToList();
            var usersToAdd = requiredUsers.Where(_ => !existingUserNames.Contains(_.Email));
            var utcNow = DateTime.UtcNow;

            foreach (var userToAdd in usersToAdd)
            {
                var passwordSalt = _cryptographyService.CreateSalt();
                var passwordHash = _cryptographyService.CreateHash(userToAdd.Password, passwordSalt);

                var user = new User
                {
                    UserName = userToAdd.Email,
                    Email = userToAdd.Email,
                    DisplayName = userToAdd.DisplayName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    LastPasswordReset = utcNow,
                    Role = allRoles.TryGetValue(userToAdd.Role, out Role role) ? role : null
                };

                _userRepository.Insert(user);
            }
        }

        private void SeedEmailTemplates()
        {
            var requiredTemplates = new List<EmailTemplate>()
            {
                new EmailTemplate { Type = EmailTemplateType.Save, Subject = "App project", Body = $"<p><span>A Flex Financial project has been created. Details can be found in the attached pdf.</span></p>" },
            };

            var existingTemplates = _emailTemplateRepository.GetAll().ToList();

            foreach (var required in requiredTemplates)
            {
                var exists = existingTemplates.Any(_ => _.Type == required.Type);

                if (!exists)
                {
                    _emailTemplateRepository.Insert(required);
                }
            }
        }

        //private void SeedSettings()
        //{
        //    var settings = new(SettingType Type, decimal Value)[]
        //    {
        //        (SettingType.DaysToProjectDeletion, 90m),
        //        (SettingType.ProCareDiscountThreshold, 15m)
        //    };

        //    var existingSettings = _settingsRepository.GetAll().ToList();

        //    var settingsToAdd = settings.Where(_ => !existingSettings.Any(__ => _.Type == __.Type));

        //    foreach (var settingToAdd in settingsToAdd)
        //    {
        //        var set = new Settings
        //        {
        //            Type = settingToAdd.Type,
        //            Value = settingToAdd.Value
        //        };

        //        _settingsRepository.Insert(set);
        //    }
        //}
    }

}
