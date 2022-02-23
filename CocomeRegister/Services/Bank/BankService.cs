using System;
using System.Threading.Tasks;
using CocomeStore.Exceptions;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services.Bank
{
    public class BankService : IBankService
    {
        public async Task ConfirmPaymentAsync(CreditCardTO creditCardTO)
        {
            await Task.Delay(100);
            var random = new Random();
            if (random.Next(100) >= 80)
            {
                throw new InvalidPaymentException();
            }
        }
    }
}
