using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.EntityFrameworkCore;
using CocomeStore.Exceptions;

namespace CocomeStore.Services
{
    public class DatabaseStatistics : IDatabaseStatistics
    {
        private readonly CocomeDbContext _context;

        public DatabaseStatistics(CocomeDbContext context)
        {
            _context = context;
        }

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
                var salePrice = element.Discount != null ?
                   element.Product.SalePrice * (1 - element.Discount.Percentage)
                   : element.Product.SalePrice;
                profit += (salePrice - element.Product.Price) * element.Amount;
            }
            return profit;
        }

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

        public Statistic GetProfitOfYear(int storeId, int year)
        {
            var dataset = new List<double>();
            for (int month = 1; month <= 12; month++)
            {
                dataset.Add(GetProfitOfMonth(storeId, month, year));
            }
            return new Statistic { Label = year + "", Dataset = dataset.ToArray() };
        }

        public IEnumerable<Statistic> GetProvidersStatistic()
        {
            var statistics = new List<Statistic>();
            foreach(var provider in _context.Providers)
            {
                statistics.Add(GetProviderStatistic(provider.Id));
            }
            return statistics;
        }

        public Statistic GetProviderStatistic(int providerId)
        {
            Provider provider = _context.Providers.Find(providerId);
            if (provider == null)
            {
                throw new EntityNotFoundException(
                   "provider with id " + providerId + " could not be found");
            }

            var dataset = _context.Orders
                .Where(order => order.Provider.Id == providerId && order.Delivered)
                .OrderBy(order => order.PlacingDate)
                .Select(order => (order.DeliveringDate - order.PlacingDate).TotalDays);

            return new() { Label = provider.Name, Dataset = dataset.ToArray() };
        }

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
