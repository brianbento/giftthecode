// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Data
{
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Redemption> Redemption { get; set; }

        public DbSet<Attendee> Attendee { get; set; }

        public DbSet<Award> Award { get; set; }

        public DbSet<AwardLog> AwardLog { get; set; }

        public DbSet<RedemptionLog> RedemptionLog { get; set; }

        public DbSet<Instance> Instance { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Redemption>()
                .Property(i => i.RedemptionID)
                .ValueGeneratedOnAdd();

            builder.Entity<Redemption>()
                .HasKey(i => i.RedemptionID);

            builder.Entity<Redemption>()
                .HasMany(i => i.RedemptionLogs)
                .WithOne(rl => rl.Redemption)
                .HasForeignKey(i => i.RedemptionID);

            builder.Entity<RedemptionLog>()
                .Property(i => i.RedemptionLogID)
                .ValueGeneratedOnAdd();

            builder.Entity<RedemptionLog>()
                .HasKey(i => i.RedemptionLogID);

            builder.Entity<Attendee>()
                .HasKey(i => i.UserID);

            builder.Entity<Attendee>()
                .Property(i => i.UserID)
                .ValueGeneratedOnAdd();

            builder.Entity<Attendee>()
                .HasMany(i => i.RedemptionLogs)
                .WithOne(rl => rl.User)
                .HasForeignKey(i => i.UserID);

            builder.Entity<Award>()
                .HasKey(i => i.AwardID);

            builder.Entity<Award>()
                .Property(i => i.AwardID)
                .ValueGeneratedOnAdd();

            builder.Entity<Award>()
                .HasMany(i => i.AwardLogs)
                .WithOne(rl => rl.Award)
                .HasForeignKey(i => i.AwardID);

            builder.Entity<Attendee>()
                .HasMany(i => i.AwardLogs)
                .WithOne(rl => rl.User)
                .HasForeignKey(i => i.UserID);

            builder.Entity<Instance>()
                .HasKey(i => i.InstanceID);

            builder.Entity<Instance>()
                .HasMany(i => i.Awards)
                .WithOne(i => i.Instance)
                .HasForeignKey(i => i.InstanceID);

            builder.Entity<Instance>()
                .HasMany(i => i.Attendees)
                .WithOne(i => i.Instance)
                .HasForeignKey(i => i.InstanceID);

            builder.Entity<Instance>()
                .HasMany(i => i.Redemptions)
                .WithOne(i => i.Instance)
                .HasForeignKey(i => i.InstanceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
