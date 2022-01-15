using System;

namespace AchieveIt.Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string error) : base(error) { }
    }
}