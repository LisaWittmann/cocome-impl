using System.Threading.Tasks;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services.Bank
{
    public interface IBankService
    {
        Task ConfirmPaymentAsync(CreditCardTO creditCardTO);
    }
}
