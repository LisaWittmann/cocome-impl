using System;
using System.Collections.Generic;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface IDatabaseStatistics
    {
        Statistic GetProfitOfYear(int storeId, int year);
        IEnumerable<Statistic> GetStoreProfit(int storeId);
        IEnumerable<Statistic> GetLatestProfit();
        Statistic GetProviderStatistic(int providerId);
        IEnumerable<Statistic> GetProvidersStatistic();
    }
}
