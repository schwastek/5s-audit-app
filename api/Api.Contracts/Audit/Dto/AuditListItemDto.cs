﻿using System;

namespace Api.Contracts.Audit.Dto
{
    public class AuditListItemDto
    {
        /// <example>d0a5a4ec-7b84-48c7-8028-133f1dd74b06</example>
        public Guid AuditId { get; set; }

        /// <example>John</example>
        public string Author { get; set; } = null!;

        /// <example>Warehouse</example>
        public string Area { get; set; } = null!;

        /// <example>2021-07-19T11:09:34.543Z</example>
        public DateTime StartDate { get; set; }

        /// <example>2021-07-19T11:09:44.543Z</example>
        public DateTime EndDate { get; set; }

        /// <example>0.6</example>
        public double Score { get; set; }
    }
}
