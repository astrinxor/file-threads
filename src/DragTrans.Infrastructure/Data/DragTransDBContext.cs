
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
    public DbSet<SettingProfile> SettingProfiles => Set<SettingProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SettingProfile>()
            .HasKey(profile => profile.UserId);

        modelBuilder.Entity<User>()
            .HasOne(user => user.Profile)
            .WithOne(profile => profile.User)
            .HasForeignKey<SettingProfile>(profile => profile.UserId);

        modelBuilder.Entity<StoredFile>()
            .HasOne(file => file.User)
            .WithMany()
            .HasForeignKey(file => file.UserId);

        modelBuilder.Entity<SettingProfile>()
            .Ignore(profile => profile.Files);
    }
}
