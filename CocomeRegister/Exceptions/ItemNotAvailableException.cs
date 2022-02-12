using System;
namespace CocomeStore.Exceptions
{
    public class ItemNotAvailableException : Exception
    {
        public ItemNotAvailableException()
        {
        }

        public ItemNotAvailableException(string message) : base(message)
        {
        }
    }
}
