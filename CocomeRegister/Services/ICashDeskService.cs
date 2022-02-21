using System.Collections.Generic;
using System.Threading.Tasks;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface ICashDeskService
    {
        Task<SaleTO> CreateSale(int storeId, SaleTO saleTO);
        IEnumerable<Product> GetAvailableProducts(int storeId);
    }
}
