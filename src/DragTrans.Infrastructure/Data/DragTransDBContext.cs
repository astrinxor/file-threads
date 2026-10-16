
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
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<StoredFile> StoredFiles => Set<StoredFile>();
    public DbSet<Avatar> Avatars => Set<Avatar>();
    public DbSet<EmailVerification> EmailVerifications
        => Set<EmailVerification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(profile => profile.UserId);

            entity.HasOne(profile => profile.User)
                .WithOne(user => user.Profile)
                .HasForeignKey<Profile>(profile => profile.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(profile => profile.Avatar)
                .WithOne()
                .HasForeignKey<Profile>(profile => profile.AvatarId)
                .IsRequired()
                .OnDelete(DeleteBehavior.SetNull);

            entity.Ignore(profile => profile.Files);
        });

        modelBuilder.Entity<Avatar>(entity =>
        {
            entity.HasKey(avatar => avatar.Id);

            entity.Property(avatar => avatar.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(avatar => avatar.Size)
                .IsRequired();
        });

        modelBuilder.Entity<StoredFile>(entity =>
        {
            entity.HasKey(file => file.Id);

            entity.HasOne(file => file.User)
                .WithMany()
                .HasForeignKey(file => file.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
