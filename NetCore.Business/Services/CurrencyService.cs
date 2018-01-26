using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetCore.Business.Extensions;
using NetCore.Business.Models;
using NetCore.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities = NetCore.Data.Entities;

namespace NetCore.Business.Services
{
    public class CurrencyService : EntityService<Entities.Currency, Currency, Currency>
    {

        private readonly IGenericRepository<Entities.Currency> _currencyReposiotry;

        public CurrencyService(
            IDatabaseScope database,
            IGenericRepository<Entities.Currency> currencyRepository
            ) : base(database, currencyRepository)
        {
            _currencyReposiotry = currencyRepository;
        }

        #region Public methods

        public async Task<IEnumerable<Currency>> UpdateAllAsync(IEnumerable<Currency> exchangeRates)
        {
            var results = await _currencyReposiotry.GetAll().ToListAsync();

            foreach (var rate in exchangeRates)
            {
                var currency = results.SingleOrDefault(_ => _.Id == rate.Id);
                currency.RejectNotFound();

                currency.ToUSD = rate.ToUSD;
                currency.FromUSD = rate.FromUSD;
            }

            await _database.SaveAsync();

            return results.AsQueryable().ProjectTo<Currency>();
        }

        #endregion


        #region EntityService
        protected override IQueryable<Entities.Currency> GetAllEntities(IQueryable<Entities.Currency> query)
        {
            return query;
        }

        protected async override Task<Entities.Currency> GetEntityByIdAsync(IQueryable<Entities.Currency> query, long id)
        {
            return await GetAllEntities(query).SingleOrDefaultAsync(_ => _.Id == id);
        }

        protected override void UpdateEntity(Entities.Currency entity, Currency model)
        {
            Mapper.Map<Currency, Entities.Currency>(model, entity);
        }

        #endregion

        #region Internal methods

        internal static void RegisterMappings(Profile profile)
        {
            profile.CreateMap<Entities.Currency, Currency>();
        }

        #endregion

    }
}
