using Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Api.Options;
using Microsoft.Extensions.Options;

namespace Api.DbContexts
{
    public class LeanAuditorContext : IdentityDbContext<User>
    {
        private readonly ConnectionStringOptions _config;
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditAction> AuditActions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public LeanAuditorContext(DbContextOptions<LeanAuditorContext> options, IOptions<ConnectionStringOptions> config)
        : base(options)
        {
            _config = config.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging()
                .UseSqlite(_config.DefaultConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
