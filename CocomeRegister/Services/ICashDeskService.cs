using System;
using System.Collections.Generic;
using CocomeStore.Models;

namespace CocomeStore.Services
{
    public interface ICashDeskService
    {
        void CreateSale(int storeId, IEnumerable<SaleElement> elements);
    }
}
