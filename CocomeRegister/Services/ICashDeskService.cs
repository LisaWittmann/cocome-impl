using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface ICashDeskService
    {
        SaleTO CreateSale(int storeId, SaleTO saleTO);
        IEnumerable<Product> GetAvailableProducts(int storeId);
    }
}
