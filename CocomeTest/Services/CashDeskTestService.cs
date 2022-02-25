using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;
using CocomeStore.Services;

namespace CocomeTest.Services
{
    
    public class CashDeskTestService : ICashDeskService
    {
        private readonly List<Provider> _provider;
        private readonly List<ProductTO> _product;
        private readonly List<SaleTO> _sale;
        private readonly List<Store> _store;

        public CashDeskTestService()
        {
            TestDataProvider dataProvider = new TestDataProvider();
            _provider = dataProvider._provider;
            _product = TestDataProvider.ChangeProductsToTo(dataProvider._product);
            _sale = dataProvider._sale;
            _store = dataProvider._store;
        }

        
        public async Task CreateSaleAsync(SaleTO saleTO)
        {
            var sale = _mapper.CreateSale(saleTO);

            foreach (var element in saleTO.SaleElements)
            {
                StockItem item = _context.StockItems
                    .Where(item =>
                        item.StoreId == saleTO.Store.Id &&
                        item.ProductId == element.Product.Id)
                    .SingleOrDefault();

                if (item == null || item.Stock - element.Amount < 0)
                {
                    throw new ItemNotAvailableException();
                }
                item.Stock -= element.Amount;
                await _context.SaleElements.AddAsync(
                    _mapper.CreateSaleElement(sale, saleTO.Store.Id, element));
            }

            await _context.SaveChangesAsync();
        }

        public IEnumerable<ProductTO> GetAvailableProducts(int storeId)
        {
            return _product;
        }

        public async Task<SaleTO> UpdateSaleDataAsync(int storeId, SaleTO saleTO)
        {
            var store = await _context.Stores.FindAsync(storeId);
            if (store == null)
            {
                throw new EntityNotFoundException();
            }
            saleTO.Store = store;
            return saleTO;
        }
    }
}
