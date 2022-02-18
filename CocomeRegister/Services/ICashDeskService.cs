using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface ICashDeskService
    {
        Sale CreateSale(int storeId, IEnumerable<SaleElementTO> elements);
    }
}
