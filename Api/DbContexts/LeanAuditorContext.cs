using Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Api.DbContexts
{
  public class LeanAuditorContext : IdentityDbContext<User>
    {
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditAction> AuditActions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public LeanAuditorContext(DbContextOptions<LeanAuditorContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging()
                .UseSqlite(@"Data Source=audits.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AuditsDataInitializer.Seed(modelBuilder);
        }
    }
}
