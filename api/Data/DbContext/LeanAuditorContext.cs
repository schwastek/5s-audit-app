using Data.Configuration;
using Data.Context;
using Data.Options;
using Domain;
using Domain.Auditing;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;

namespace Data.DbContext;

public class LeanAuditorContext : IdentityDbContext<User>
{
    private readonly ConnectionStringOptions _config = new();
    private readonly string _connectionString;
    private readonly ICurrentUserService _currentUserService;

    public DbSet<Audit> Audits => Set<Audit>();
    public DbSet<AuditAction> AuditActions => Set<AuditAction>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();

    public LeanAuditorContext(DbContextOptions<LeanAuditorContext> options, IOptions<ConnectionStringOptions> config, ICurrentUserService currentUserService)
    : base(options)
    {
        _config = config.Value;
        _connectionString = GetConnectionString();
        _currentUserService = currentUserService;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            //.LogTo(Console.WriteLine)
            //.EnableSensitiveDataLogging()
            .UseSqlite(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new AuditEntityTypeConfiguration().Configure(modelBuilder.Entity<Audit>());
        new AnswerEntityTypeConfiguration().Configure(modelBuilder.Entity<Answer>());
        new AuditActionEntityTypeConfiguration().Configure(modelBuilder.Entity<AuditAction>());
        new QuestionEntityTypeConfiguration().Configure(modelBuilder.Entity<Question>());

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        SetAuditableProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditableProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditableProperties()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            var now = DateTimeOffset.UtcNow;

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(e => e.CreatedBy).CurrentValue = _currentUserService.Username;
                    entry.Property(e => e.CreatedAt).CurrentValue = now;
                    entry.Property(e => e.ModifiedBy).CurrentValue = _currentUserService.Username;
                    entry.Property(e => e.ModifiedAt).CurrentValue = now;
                    break;

                // EF marks an entity as Modified when its scalar (column) properties change, not its navigation collections.
                // Changes to navigation properties are tracked as relationship updates, not entity modifications.
                case EntityState.Modified:
                    entry.Property(e => e.ModifiedBy).CurrentValue = _currentUserService.Username;
                    entry.Property(e => e.ModifiedAt).CurrentValue = now;
                    break;
            }
        }
    }

    private string GetConnectionString()
    {
        // The following configures EF to create a SQLite database file in the
        // special "local" folder for your platform: `C:\Users\{User}\AppData\Local`
        var filename = _config.DefaultConnection;
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, filename);
        var connectionString = $"Data Source={dbPath}";

        return connectionString;
    }
}
