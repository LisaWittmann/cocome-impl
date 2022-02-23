using System.Threading.Tasks;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface IPrinterService
    {
        Task<byte[]> CreateBillAsync(SaleTO saleTO);
    }
}
