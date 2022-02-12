using System;
using System.Collections.Generic;
using System.Linq;
using CocomeStore.Exceptions;
using CocomeStore.Models;

namespace CocomeStore.Services
{
    public class CashDeskService : ICashDeskService
    {
        private CocomeDbContext _context;
        private IStoreService _storeService;

        public CashDeskService(CocomeDbContext context, IStoreService storeService)
        {
            _context = context;
            _storeService = storeService;
        }

        public void CreateSale(int storeId, IEnumerable<SaleElement> elements)
        {
            Store store = _storeService.GetStore(storeId);
            DateTime timeStamp = DateTime.Now;

            Sale sale = new Sale { Store = store, SaleElements = elements.ToArray(), TimeStamp = timeStamp };
            _context.Sales.Add(sale);
            _context.SaveChanges();

            foreach(var element in elements)
            {
                StockItem item = _context.StockItems
                    .Where(item => item.Store.Id == storeId && item.Product.Id == element.Product.Id)
                    .First();

                if (item == null)
                {
                    throw new ItemNotAvailableException(
                        "product with id " + element.Product.Id + "is not in stock of store " + storeId);
                }

                int newStock = item.Stock - element.Amount;
                if (newStock < 0)
                {
                    throw new ItemNotAvailableException(
                        "product with id " + element.Product.Id + "is not in stock of store " + storeId);
                }
                _storeService.UpdateStock(storeId, element.Product.Id, newStock);
            }
        }
    }
}
