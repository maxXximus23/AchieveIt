using System;

namespace AchieveIt.Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string error) : base(error) { }
        
        public NotFoundException(string itemName, string id) 
            : base($"{itemName} with {id} id is not found.")
        {
            
        }
    }
}