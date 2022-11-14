using System;

namespace Api.Exceptions
{
    public sealed class ActionNotFoundException : NotFoundException
    {
        public ActionNotFoundException(Guid actionId) : 
            base($"Action with ID {actionId} does not exist.")
        {
        }
    }
}
