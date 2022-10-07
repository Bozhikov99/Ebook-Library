using System;
namespace Domain.Exceptions
{
    public class InvalidUserCredentialsException : Exception
    {
        public InvalidUserCredentialsException(string message)
            : base(message)
        {
        }
    }
}

