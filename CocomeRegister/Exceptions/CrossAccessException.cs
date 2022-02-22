using System;
namespace CocomeStore.Exceptions
{
    /// <summary>
    /// class <c>CrossAccessException</c> is an <see cref="Exception"/> to
    /// signalize that store is tryng to access another stores data
    /// </summary>
    public class CrossAccessException : Exception
    {
        public CrossAccessException() { }

        public CrossAccessException(string message) : base(message) { }
    }
}
