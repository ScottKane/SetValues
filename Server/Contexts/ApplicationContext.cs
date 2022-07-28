using Microsoft.EntityFrameworkCore;
using SetValues.Server.Entities;
using SetValues.Server.Repositories;

namespace SetValues.Server.Contexts;

public class ApplicationContext : AuditableContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
    public DbSet<Customer> Customers { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "Default";
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = "Default";
                    break;
            }
        }

        return await base.SaveChangesAsync("Default", cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
                                        .SelectMany(t => t.GetProperties())
                                        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            property.SetColumnType("decimal(18,2)");

        base.OnModelCreating(builder);

        builder.Entity<Customer>()
            .OwnsOne(o => o.Address);
        builder.Entity<Customer>()
            .OwnsOne(o => o.Contact);
    }
}