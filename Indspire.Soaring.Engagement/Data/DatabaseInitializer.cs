// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Configuration;

    internal class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext context;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        public DatabaseInitializer(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // This example just creates an Administrator role and one Admin users
        public async Task Initialize(IConfiguration configuration)
        {
            var databaseExists = (this.context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

            if (databaseExists)
            {
                if (!this.context.Roles.Any(r => r.Name == RoleNames.Administrator))
                {
                    await this.roleManager.CreateAsync(new IdentityRole
                    {
                        Name = RoleNames.Administrator
                    });
                }

                if (!this.context.Roles.Any(r => r.Name == RoleNames.Editor))
                {
                    await this.roleManager.CreateAsync(new IdentityRole
                    {
                        Name = RoleNames.Editor
                    });
                }

                var username = configuration["AdminUsername"];
                var password = configuration["AdminPassword"];

                if (!string.IsNullOrWhiteSpace(username) &&
                    !string.IsNullOrWhiteSpace(password))
                {
                    var user = await this.userManager.FindByNameAsync(username);

                    if (user == null)
                    {
                        var newUser = new ApplicationUser
                        {
                            UserName = username,
                            Email = username,
                            EmailConfirmed = true
                        };

                        var result = await this.userManager
                            .CreateAsync(newUser, password);

                        var createdUser = await this.userManager.FindByNameAsync(username);

                        if (createdUser != null)
                        {
                            var adminRole = this.roleManager.FindByNameAsync(RoleNames.Administrator);

                            await this.userManager.AddToRoleAsync(createdUser, RoleNames.Administrator);
                        }
                    }
                    else
                    {
                        if (!await this.userManager.IsInRoleAsync(user, RoleNames.Administrator))
                        {
                            await this.userManager.AddToRoleAsync(user, RoleNames.Administrator);
                        }
                    }
                }
            }

            return;
        }
    }
}
