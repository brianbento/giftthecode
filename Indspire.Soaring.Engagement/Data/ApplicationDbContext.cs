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
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
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
        }

        public DbSet<Indspire.Soaring.Engagement.Database.Redemption> Redemption { get; set; }

        public DbSet<Indspire.Soaring.Engagement.Database.User> User { get; set; }

        public DbSet<Indspire.Soaring.Engagement.Database.Award> Award { get; set; }
    }
}
