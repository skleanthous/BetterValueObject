namespace BetterValueObject.Emitter.Exceptions
{
    using System;

    public class MutateableTypeNotAllowedException : Exception
    {
        public MutateableTypeNotAllowedException(string message) : base(message)
        { }
    }
}
