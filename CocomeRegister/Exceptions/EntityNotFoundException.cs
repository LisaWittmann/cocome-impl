using System;
namespace CocomeStore.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {   
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
