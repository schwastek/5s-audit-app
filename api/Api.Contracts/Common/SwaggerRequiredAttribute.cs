using System;

namespace Api.Contracts.Common
{
    /// <summary>
    /// Mark property as required in Swagger without using <c>[Required]</c> 
    /// from Data Annotations (ASP.NET model validation, requires external package).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerRequiredAttribute : Attribute
    {
    }
}
