﻿using System.Collections.Generic;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface ICashDeskService
    {
        void CreateSale(int storeId, IEnumerable<SaleElementTO> elements);
    }
}
