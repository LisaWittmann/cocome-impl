using System;
namespace CocomeStore.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
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
