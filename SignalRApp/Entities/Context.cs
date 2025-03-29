using Microsoft.EntityFrameworkCore;

namespace SignalRApp.Entities;

public class Context
    : DbContext
{
    public Context() { }
    public Context(DbContextOptions<Context> options)
        : base(options) { }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("db_inMemory");
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email, "idx_users_email")
                .IsUnique();
        });
                
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        base.OnModelCreating(modelBuilder);
    }
}
