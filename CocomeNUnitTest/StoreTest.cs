using CocomeStore.Models;
using CocomeStore.Models.Database;
using CocomeStore.Services;
using CocomeStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace Cocome.NUnitTest
{
    public class StoreTests
    {
        private TestDataProvider testDataProvider;
        StoreService service;

        [SetUp]
        public void Setup()
        {

            testDataProvider = new TestDataProvider();

            var data = testDataProvider._store.AsQueryable();

            var mockSet = new Mock<DbSet<Store>>();
            mockSet.As<IQueryable<Store>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Store>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Store>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Store>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            var mockContext = new Mock<CocomeDbContext>();
            mockContext.Setup(c => c.Stores).Returns(mockSet.Object);

            /*var data2 = testDataProvider._stockItem.AsQueryable();

            var mockSet2 = new Mock<DbSet<StockItem>>();
            mockSet.As<IQueryable<StockItem>>().Setup(m => m.Provider).Returns(data2.Provider);
            mockSet.As<IQueryable<StockItem>>().Setup(m => m.Expression).Returns(data2.Expression);
            mockSet.As<IQueryable<StockItem>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            mockSet.As<IQueryable<StockItem>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            mockContext.Setup(m => m.StockItems).Returns(mockSet2.Object);*/

            var mockMapper = new Mock<ModelMapper>();

            service = new StoreService(mockContext.Object, mockMapper.Object);
        }

        [Test]
        public void FindStoreTest()
        {
            var store = service.GetStore(1);
            Assert.AreEqual("Teststore1", store.Name);

            store = service.GetStore(2);
            Assert.AreEqual("Teststore2", store.Name);

            store = service.GetStore(3);
            Assert.AreEqual("Teststore3", store.Name);
        }

        [Test]
        public void GetInventoryTest()
        {
            var stockItems = service.GetInventory(1).ToArray();

            Assert.AreEqual(3, stockItems.Count());
            Assert.AreEqual(10, stockItems[0].Stock);
            Assert.AreEqual(20, stockItems[1].Stock);
            Assert.AreEqual(30, stockItems[2].Stock);
        }
    }
}