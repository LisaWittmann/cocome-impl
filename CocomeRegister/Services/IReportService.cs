using System;
using System.Collections.Generic;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface IReportService
    {
        Report GetProfitOfYear(int storeId, int year);
        IEnumerable<Report> GetStoreProfit(int storeId);
        IEnumerable<Report> GetLatestProfit();
        Report GetDeliveryReport(int providerId);
        IEnumerable<Report> GetGeneralDeliveryReports();
    }
}
