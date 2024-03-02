using Data.Options;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Data.DbContext;

public class LeanAuditorContext : IdentityDbContext<User>
{
    private readonly ConnectionStringOptions config = new();
    private string connectionString { get; }
    public DbSet<Audit> Audits => Set<Audit>();
    public DbSet<AuditAction> AuditActions => Set<AuditAction>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();

    public LeanAuditorContext(DbContextOptions<LeanAuditorContext> options, IOptions<ConnectionStringOptions> config)
    : base(options)
    {
        this.config = config.Value;
        this.connectionString = GetConnectionString();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            //.LogTo(Console.WriteLine)
            //.EnableSensitiveDataLogging()
            .UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    private string GetConnectionString()
    {
        // The following configures EF to create a SQLite database file in the
        // special "local" folder for your platform: `C:\Users\{User}\AppData\Local`
        var filename = config.DefaultConnection;
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, filename);
        var connectionString = $"Data Source={dbPath}";

        return connectionString;
    }
}
