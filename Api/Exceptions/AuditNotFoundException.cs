using System;

namespace Api.Exceptions
{
    public sealed class AuditNotFoundException : NotFoundException
    {
        public AuditNotFoundException(Guid auditId) : 
            base($"Audit with ID {auditId} does not exist.")
        {
        }
    }
}
