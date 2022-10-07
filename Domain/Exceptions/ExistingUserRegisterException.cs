using System;
namespace Domain.Exceptions
{
    public class ExistingUserRegisterException : Exception
    {
        public ExistingUserRegisterException(string message)
            : base(message)
        {
        }
    }
}

