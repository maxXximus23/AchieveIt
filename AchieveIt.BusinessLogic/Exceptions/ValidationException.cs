using System;

namespace AchieveIt.BusinessLogic.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string error) : base(error) { }
    }
}