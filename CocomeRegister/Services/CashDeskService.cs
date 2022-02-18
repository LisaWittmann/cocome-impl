using System;
using System.Collections.Generic;
using System.Linq;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

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

        public Sale CreateSale(int storeId, IEnumerable<SaleElementTO> elements)
        {
            Sale sale = new() { StoreId = storeId, TimeStamp = DateTime.Now };

            foreach (var element in elements)
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
            return sale;
        }

    }
}
