using Entities = NetCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Business.Models;
using NetCore.Data;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetCore.Business.Extensions;
using AutoMapper.QueryableExtensions;

namespace NetCore.Business.Services
{
    public class CountryService : EntityService<Entities.Country, Country, CountryBase>
    {
        private readonly CurrentUserInfo _currentUser;
        private readonly IGenericRepository<Entities.Country> _countriesRepository;
        private readonly IGenericRepository<Entities.UserCountry> _userCountryRepository;

        public CountryService(IDatabaseScope database,
            CurrentUserInfo currentUser,
            IGenericRepository<Entities.Country> countriesRepository,
            IGenericRepository<Entities.UserCountry> userCountryRepository)
            : base(database, countriesRepository)
        {
            _currentUser = currentUser;
            _countriesRepository = countriesRepository;
            _userCountryRepository = userCountryRepository;
        }

        #region Public methods

        public async override Task<Country> CreateAsync(CountryBase model)
        {
            model.RejectInvalid();

            var entity = new Entities.Country()
            {
                IsActive = true
            };

            UpdateEntity(entity, model);

            _entityRepository.Insert(entity);
            await _database.SaveAsync();

            return Mapper.Map<Country>(entity);
        }

        public async override Task DeleteAsync(long id)
        {
            var entity = await GetEntityByIdAsync(_entityRepository.GetAll(), id);
            entity.RejectNotFound();

            entity.IsActive = false;

            await _database.SaveAsync();
        }

        public async Task<IEnumerable<Country>> GetAllActiveAsync()
        {
            var countryIds = await _userCountryRepository.AsReadOnly().Where(_ => _.UserId == _currentUser.Id).Select(_ => _.CountryId).ToListAsync();

            var query = _countriesRepository.AsReadOnly()
               .Where(_ => _.IsActive == true);

            //if (!_currentUser.IsInAnyRole(new string[] { Entities.Role.DivisionalFD_NAME, Entities.Role.Administrator_NAME }))
            //{
            //    query = query.Where(_ => countryIds.Contains(_.Id));
            //}

            var result = await query
                .ProjectTo<Country>()
                .ToListAsync();

            return result;
        }

        #endregion

        #region EntityService
        protected override IQueryable<Entities.Country> GetAllEntities(IQueryable<Entities.Country> query)
        {
            return query;
        }

        protected async override Task<Entities.Country> GetEntityByIdAsync(IQueryable<Entities.Country> query, long id)
        {
            return await GetAllEntities(query).SingleOrDefaultAsync(_ => _.Id == id);
        }

        protected override void UpdateEntity(Entities.Country entity, CountryBase model)
        {
            Mapper.Map<CountryBase, Entities.Country>(model, entity);
        }
        #endregion

        #region Internal methods
        internal static void RegisterMappings(Profile profile)
        {
            profile.CreateMap<Entities.Country, Country>();
            profile.CreateMap<CountryBase, Entities.Country>();
        }
        #endregion
    }
}
