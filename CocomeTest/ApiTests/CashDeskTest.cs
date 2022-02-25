using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocomeStore.Controllers;
using CocomeStore.Services;
using CocomeTest.Services;

namespace CocomeTest.ApiTests
{
    
    internal class CashDeskTest
    {
        private readonly CashDeskController _controller;
        private readonly ICashDeskService _service;
    
        public CashDeskTest()
        {
            _service = new CashDeskTestService();
            _controller = CashDeskController(_service);
        }
    }
}
