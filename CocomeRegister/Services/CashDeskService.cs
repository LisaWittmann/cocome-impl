using System;
using System.Collections.Generic;
using System.Linq;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Database;
using CocomeStore.Models.Transfer;
using CocomeStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CocomeStore.Services
{
    public class CashDeskService : ICashDeskService
    {
        private readonly CocomeDbContext _context;
        private readonly IModelMapper _mapper;


        public CashDeskService(
            CocomeDbContext context,
            IModelMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateSale(int storeId, SaleTO saleTO)
        {
            Sale sale = new() { StoreId = storeId, TimeStamp = DateTime.Now, PaymentMethod = saleTO.PaymentMethod };

            foreach (var element in saleTO.SaleElements)
            {
                StockItem item = _context.StockItems
                    .Where(item => item.StoreId == storeId && item.ProductId == element.Product.Id)
                    .SingleOrDefault();

                if (item == null || item.Stock - element.Amount < 0)
                {
                    throw new ItemNotAvailableException(
                        "product with id " + element.Product.Id + "is not in stock of store " + storeId);
                }
                item.Stock -= element.Amount;
                _context.SaleElements.Add(_mapper.CreateSaleElement(sale, storeId, element));
            }

            _context.SaveChanges();
        }

        public IEnumerable<Product> GetAvailableProducts(int storeId)
        {
            return _context.StockItems
                .Include(item => item.Product)
                .Where(item => item.StoreId == storeId && item.Stock > 0)
                .Select(item => item.Product);
        }
    }
}
