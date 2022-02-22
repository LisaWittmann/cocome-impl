﻿using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.EntityFrameworkCore;
using CocomeStore.Exceptions;
using CocomeStore.Models.Database;

namespace CocomeStore.Services.Statsistics
{
    /// <summary>
    /// class <c>DatabaseStatistics</c> implements <see cref="IDatabaseStatistics"/>
    /// and provides functionality to create statistics of applications database
    /// </summary>
    public class DatabaseStatistics : IDatabaseStatistics
    {
        private readonly CocomeDbContext _context;

        public DatabaseStatistics(CocomeDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// method <c>GetProfitOfMonth</c> calculates the profit from all
        /// registered sale of a store in the given timespan
        /// </summary>
        /// <param name="storeId">
        /// unique identifier of the store
        /// </param>
        /// <param name="month">month to filter sales for</param>
        /// <param name="year">year to filter sales for</param>
        /// <returns>total profit of store in request month of year</returns>
        private double GetProfitOfMonth(int storeId, int month, int year)
        {
            var saleElements = _context.SaleElements
                .Include(e => e.Sale)
                .Where(e =>
                    e.Sale.StoreId == storeId &&
                    e.Sale.TimeStamp.Year == year &&
                    e.Sale.TimeStamp.Month == month
                )
                .Include(e => e.Product)
                .ToArray();

            double profit = 0;
            foreach (var element in saleElements)
            {
                var salePrice = element.Product.SalePrice * (1 - element.Discount);
                profit += (salePrice - element.Product.Price) * element.Amount;
            }
            return profit;
        }

        /// <summary>
        /// method <c>GetLastestProfit</c> calculates the profit of the current
        /// year of all stores in database
        /// </summary>
        /// <returns>
        /// list of all profits mapped to the store in current year in a
        /// statistic object
        /// </returns>
        public IEnumerable<Statistic> GetLatestProfit()
        {
            var profits = new List<Statistic>();
            foreach(var store in _context.Stores)
            {
                profits.Add(new Statistic()
                {
                    Label = store.Name,
                    Dataset = GetProfitOfYear(store.Id, DateTime.Now.Year).Dataset
                });
            }
            return profits;
        }

        /// <summary>
        /// method <c>GetProfitOfYear</c> calculates the profit of a store by
        /// the registered sales in the given year
        /// </summary>
        /// <param name="storeId">
        /// unique identifier of the store
        /// </param>
        /// <param name="year">year to filter sales for</param>
        /// <returns>
        /// statistic object containing the year as label and the monthly
        /// profit of the requested year as data
        /// </returns>
        public Statistic GetProfitOfYear(int storeId, int year)
        {
            var dataset = new List<double>();
            for (int month = 1; month <= 12; month++)
            {
                dataset.Add(GetProfitOfMonth(storeId, month, year));
            }
            return new Statistic { Label = year + "", Dataset = dataset.ToArray() };
        }

        /// <summary>
        /// method <c>GetProvidersStatistic</c> calculates the delivery times
        /// of each provider in the database
        /// </summary>
        /// <returns>
        /// list of statistics containing each provider in the database and his
        /// delivery statistic
        /// </returns>
        public IEnumerable<Statistic> GetProvidersStatistic()
        {
            var statistics = new List<Statistic>();
            foreach(var provider in _context.Providers)
            {
                statistics.Add(GetProviderStatistic(provider.Id));
            }
            return statistics;
        }

        /// <summary>
        /// method <c>GetProviderStatistic</c> calculates the delivery times of
        /// the given provider
        /// </summary>
        /// <param name="providerId">
        /// unique identifier of the provider
        /// </param>
        /// <returns>
        /// statistic object containing the providers name as label and the
        /// delivery time in days for each registered and delivered order to the
        /// provider in the database
        /// </returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public Statistic GetProviderStatistic(int providerId)
        {
            Provider provider = _context.Providers.Find(providerId);
            if (provider == null)
            {
                throw new EntityNotFoundException(
                   "provider with id " + providerId + " could not be found");
            }

            var dataset = _context.Orders
                .Where(order =>
                    order.Provider.Id == providerId &&
                    order.DeliveringDate != DateTime.MinValue)
                .OrderBy(order => order.PlacingDate)
                .Select(order => (order.DeliveringDate - order.PlacingDate).TotalDays);

            return new() { Label = provider.Name, Dataset = dataset.ToArray() };
        }

        /// <summary>
        /// method <c>GetStoreProfit</c> calculates the profit of the store by
        /// the registered sales from the first sale entries year to current year
        /// </summary>
        /// <param name="storeId">
        /// unique identifier of the store
        /// </param>
        /// <returns>
        /// array of statistics for each registered year
        /// </returns>
        public IEnumerable<Statistic> GetStoreProfit(int storeId)
        {
            var latest = DateTime.Now.Year;
            var first = _context.Sales
                .Where(sale => sale.StoreId == storeId)
                .OrderBy(sale => sale.TimeStamp.Year)
                .Select(sale => sale.TimeStamp.Year)
                .FirstOrDefault();

            if (first == 0)
            {
                first = latest;
            }

            var statistics = new List<Statistic>();
            for (int year = first; year <= DateTime.Now.Year; year++)
            {
                statistics.Add(GetProfitOfYear(storeId, year));
            }
            return statistics;
        }
    }
}
