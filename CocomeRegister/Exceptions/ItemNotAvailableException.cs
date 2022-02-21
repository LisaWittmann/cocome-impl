using System;
namespace CocomeStore.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
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
