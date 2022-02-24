using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;
using CocomeStore.Services;

namespace CocomeStore.Test.Services
{
    
    public class CashDeskTestService : ICashDeskService
    {
        private readonly List<ProductTO> _produkt;
        private readonly List<SaleTO> _sale;
        private readonly List<Store> _store;

        public CashDeskTestService()
        {
            Provider testProvider1 = new Provider()
            {Id = 1, Name = "Testprovider1"};
            Provider testProvider2 = new Provider()
            { Id = 2, Name = "Testprovider2" };
            _produkt = new List<ProductTO>()
            {
                new ProductTO() { Id = 1,
                    Name = "Testprodukt1", Price=1f, SalePrice = 1f, Description="Das ist das Testprodukt Numemr 1", Provider=testProvider1},
                new ProductTO() { Id = 2,
                    Name = "Testprodukt2", Price=2f, SalePrice = 2f, Description="Das ist das Testprodukt Numemr 2", Provider=testProvider2},
                new ProductTO() { Id = 3,
                    Name = "Testprodukt3", Price=3f, SalePrice = 3f, Description="Das ist das Testprodukt Numemr 3", Provider=testProvider1}
            };
            _sale = new List<SaleTO>()
            {

            };

            _store = new List<Store>()
            {
                new Store() { Id = 1, City = "Teststadt", Name="Teststore1", PostalCode=1234}
            };
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
            return _produkt;
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
