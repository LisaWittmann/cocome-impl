using System;
namespace CocomeStore.Exceptions
{
    /// <summary>
    /// class <c>ItemNotAvailableException</c> is an <see cref="Exception"/> to
    /// signalize that a requested item is currently not available
    /// </summary>
    public class ItemNotAvailableException : Exception
    {
        public ItemNotAvailableException() { }

        public ItemNotAvailableException(string message) : base(message) { }
    }
}
