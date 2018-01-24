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
    public class CurrencyService
    {

        private readonly IDatabaseScope _database;
        private readonly IGenericRepository<Entities.Currency> _currencyReposiotry;

        public CurrencyService(IDatabaseScope database,
            IGenericRepository<Entities.Currency> currencyRepository)
        {
            _database = database;
            _currencyReposiotry = currencyRepository;
        }

        #region Public methods
        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            var result = await _currencyReposiotry.GetAll()
                .ProjectTo<Currency>()
                .ToListAsync();

            return result;
        }


        public async Task<IEnumerable<Currency>> GetAllWithSettingsAsync()
        {
            var result = await _currencyReposiotry.GetAll()
                .ProjectTo<CurrencyWithSettings>()
                .ToListAsync();

            return result;
        }

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

        #region Private methods


        #endregion

        #region Internal methods

        internal static void RegisterMappings(Profile profile)
        {
            //profile.CreateMap<Entities.Currency, Currency>();
            //profile.CreateMap<Entities.Currency, CurrencyWithSettings>()
            //    .ForMember(_ => _.Type365Threshold, _ => _.MapFrom(__ => __.CurrencySettings.Type365Threshold));
        }

        #endregion

    }
}
