﻿using System.Threading.Tasks;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services.Documents
{
    public interface IDocumentService
    {
        Task<byte[]> CreateBillAsync(SaleTO saleTO);
    }
}