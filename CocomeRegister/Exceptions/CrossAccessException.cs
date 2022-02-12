using System;
namespace CocomeStore.Exceptions
{
    public class CrossAccessException : Exception
    {
        public CrossAccessException()
        {
        }

        public CrossAccessException(string message) : base(message)
        {
        }

    }
}
