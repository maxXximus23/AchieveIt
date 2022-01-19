using System;

namespace AchieveIt.Shared.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string error) : base(error) { }
    }
}