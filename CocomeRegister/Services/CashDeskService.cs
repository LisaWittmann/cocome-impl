using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Database;
using CocomeStore.Models.Transfer;
using CocomeStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CocomeStore.Services
{
    /// <summary>
    /// class <c>CashDeskService</c> is a transient implementation of
    /// <see cref="ICashDeskService"/>
    /// and provides functionalities for a store cashdesk
    /// </summary>
    public class CashDeskService : ICashDeskService
    {
        private readonly CocomeDbContext _context;
        private readonly IExchangeService _exchangeService;
        private readonly IModelMapper _mapper;

        public CashDeskService(
            CocomeDbContext context,
            IExchangeService exchangeService,
            IModelMapper mapper
        )
        {
            _context = context;
            _exchangeService = exchangeService;
            _mapper = mapper;
        }

        /// <summary>
        /// methon <c>Create Sale</c> creates a new sale objects
        /// from the given transfer object and stores saves new entries
        /// in the databse
        /// </summary>
        /// <param name="storeId">unique identifier of a store in the dabase
        /// which provokes the sale</param>
        /// <param name="saleTO">data transfer object containing the information
        /// of the sale</param>
        /// <returns>modified transfer object with billing information</returns>
        /// <exception cref="ItemNotAvailableException"></exception>
        public async Task<SaleTO> CreateSale(int storeId, SaleTO saleTO)
        {
            var store = await _context.Stores.FindAsync(storeId);
            if (store == null)
            {
                throw new EntityNotFoundException();
            }

            Sale sale = new() { StoreId = storeId, TimeStamp = DateTime.Now, PaymentMethod = saleTO.PaymentMethod };
            float total = 0;

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
                total += element.Amount * element.Product.SalePrice * (1 - element.Discount);
                await _context.SaleElements.AddAsync(_mapper.CreateSaleElement(sale, storeId, element));
            }

            await _context.SaveChangesAsync();
            _exchangeService.CheckForExchanges(storeId);

            saleTO.Store = store;
            saleTO.Total = total;
            return saleTO;
        }

        /// <summary>
        /// method <c>GetAvailableProducts</c> filters all products of
        /// the store with the given id that are currently in stock
        /// </summary>
        /// <param name="storeId">unique identifier of a store in the database</param>
        /// <returns>enumerable entries of products</returns>
        public IEnumerable<Product> GetAvailableProducts(int storeId)
        {
            return _context.StockItems
                .Include(item => item.Product)
                .Where(item => item.StoreId == storeId && item.Stock > 0)
                .Select(item => item.Product);
        }
    }
}
