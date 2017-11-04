using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Indspire.Soaring.Engagement.Models;
using Indspire.Soaring.Engagement.Database;

namespace Indspire.Soaring.Engagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

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

            builder.Entity<User>()
                .HasKey(i => i.UserID);

            builder.Entity<User>()
                .Property(i => i.UserID)
                .ValueGeneratedOnAdd();

            builder.Entity<Award>()
                .HasKey(i => i.AwardID);

            builder.Entity<Award>()
                .Property(i => i.AwardID)
                .ValueGeneratedOnAdd();

            builder.Entity<Award>()
                .HasMany(i => i.AwardLogs)
                .WithOne(rl => rl.Award)
                .HasForeignKey(i => i.AwardID);
        }

        public DbSet<Indspire.Soaring.Engagement.Database.Redemption> Redemption { get; set; }

        public DbSet<Indspire.Soaring.Engagement.Database.User> User { get; set; }

        public DbSet<Indspire.Soaring.Engagement.Database.Award> Award { get; set; }

        public DbSet<Indspire.Soaring.Engagement.Database.AwardLog> AwardLog { get; set; }

        public DbSet<Indspire.Soaring.Engagement.Database.RedemptionLog> RedemptionLog { get; set; }
    }
}
