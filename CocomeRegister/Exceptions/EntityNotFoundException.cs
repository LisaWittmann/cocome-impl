using System;
namespace CocomeStore.Exceptions
{
    //// <summary>
    /// class <c>CrossAccessException</c> is an <see cref="Exception"/> to
    /// signalize that an entity was not found in the applications database
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string message) : base(message) { }
    }
}
