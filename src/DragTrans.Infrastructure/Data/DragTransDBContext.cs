
using DragTrans.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DragTrans.Infrastructure.Data;

public class DragTransDbContext : DbContext
{
    public DragTransDbContext(
        DbContextOptions<DragTransDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<StoredFile> StoredFiles => Set<StoredFile>();
    public DbSet<EmailVerification> EmailVerifications => Set<EmailVerification>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserProfile>()
            .HasKey(profile => profile.UserId);

        modelBuilder.Entity<User>()
            .HasOne(user => user.Profile)
            .WithOne(profile => profile.User)
            .HasForeignKey<UserProfile>(profile => profile.UserId);

        modelBuilder.Entity<StoredFile>()
            .HasOne(file => file.User)
            .WithMany()
            .HasForeignKey(file => file.UserId);

        modelBuilder.Entity<UserProfile>()
            .Ignore(profile => profile.Files);
    }
}
