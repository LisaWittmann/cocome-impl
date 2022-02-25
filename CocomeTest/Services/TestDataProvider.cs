using CocomeStore.Models;
using CocomeStore.Models.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocomeTest.Services
{
    
    internal class TestDataProvider
    {

        public List<Store> _store { get; set; }
        public List<Provider> _provider { get; set; }
        public List<Product> _product { get; set; }
        public List<OrderTO> _order { get; set; }
        public List<StockItem> _stockItem { get; set; }
        public List<SaleTO> _sale { get; set; }

        public TestDataProvider()
        {
            _store = new List<Store>()
            {
                new Store() { Id = 1, City = "Teststadt", Name = "Teststore1", PostalCode = 1234},
                new Store() { Id = 2, City = "Teststadt", Name = "Teststore2", PostalCode = 1234},
                new Store() { Id = 3, City = "Teststadt", Name = "Teststore3", PostalCode = 1234},
                new Store() { Id = 4, City = "Teststadt", Name = "Teststore4", PostalCode = 1234}
            };
            _provider = new List<Provider>() {
                new Provider() { Id = 1, Name = "Testprovider1"},
                new Provider() { Id = 2, Name = "Testprovider2" }
            };
            _product = new List<Product>()
            {
                new Product() { Id = 1,
                    Name = "Testprodukt1", Price=1f, SalePrice = 1f, Description="Das ist das Testprodukt Numemr 1", Provider=_provider[0]},
                new Product() { Id = 2,
                    Name = "Testprodukt2", Price=2f, SalePrice = 2f, Description="Das ist das Testprodukt Numemr 2", Provider=_provider[1]},
                new Product() { Id = 3,
                    Name = "Testprodukt3", Price=3f, SalePrice = 3f, Description="Das ist das Testprodukt Numemr 3", Provider=_provider[0]},
                new Product() { Id = 4,
                    Name = "Testprodukt4", Price=4f, SalePrice = 4f, Description="Das ist das Testprodukt Numemr 4", Provider=_provider[1]}
            };
            _order = new List<OrderTO>()
            {
                new OrderTO() { Id = 1, Closed=false, Store=_store[0], PlacingDate=new DateTime().AddDays(-1), Provider=_provider[0]},
                new OrderTO() { Id = 2, Closed=false, Store=_store[0], PlacingDate=new DateTime().AddDays(-2), Provider=_provider[1]},
                new OrderTO() { Id = 3, Closed=true, Store=_store[0], PlacingDate=new DateTime().AddDays(-3), Provider=_provider[0], DeliveringDate= new DateTime().AddDays(-2)},
            };
            _stockItem = new List<StockItem>()
            {
                new StockItem() { Id = 1, Product=_product[0], ProductId=_product[0].Id, Store=_store[0], StoreId=_store[0].Id, Minimum=1, Stock=10 },
                new StockItem() { Id = 2, Product=_product[1], ProductId=_product[1].Id, Store=_store[0], StoreId=_store[0].Id, Minimum=2, Stock=20 },
                new StockItem() { Id = 3, Product=_product[2], ProductId=_product[2].Id, Store=_store[0], StoreId=_store[0].Id, Minimum=3, Stock=30 }
            };

            _sale = new List<SaleTO>()
            {
                
            };
        }

        static ProductTO ChangeProductToTo(Product product)
        {
            return new ProductTO()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                SalePrice = product.SalePrice,
                Description = product.Description,
                Provider = product.Provider,
            };
        }

        internal static List<ProductTO> ChangeProductsToTo(List<Product> product)
        {
            List<ProductTO> result = new List<ProductTO>();
            foreach (var item in product)
            {
                result.Add(ChangeProductToTo(item));
            }
            return result;
        }

    }
}
