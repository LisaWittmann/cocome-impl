using System.Collections.Generic;
using System.Threading.Tasks;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface ICashDeskService
    {
        Task<SaleTO> UpdateSaleDataAsync(int storeId, SaleTO saleTO);
        Task CreateSaleAsync(SaleTO saleTO);
        IEnumerable<ProductTO> GetAvailableProducts(int storeId);
    }
}
