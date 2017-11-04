using Indspire.Soaring.Engagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Data
{
    internal class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseInitializer(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //This example just creates an Administrator role and one Admin users
        public async void Initialize(IConfiguration configuration)
        {
            _context.Database.EnsureCreated();

            if (!_context.Roles.Any(r => r.Name == RoleNames.Administrator))
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = RoleNames.Administrator
                });
            }

            // Create the default Admin account and apply the Administrator role
            var username = configuration.GetValue(typeof(string), "Admin.Username") as string;
            var password = configuration.GetValue(typeof(string), "Admin.Password") as string;

            if (!string.IsNullOrWhiteSpace(username) &&
                !string.IsNullOrWhiteSpace(password))
            {
                var result = await _userManager.CreateAsync(

                    new ApplicationUser
                    {
                        UserName = username,
                        Email = username,
                        EmailConfirmed = true
                    },

                    password);

                var adminRole = _roleManager.FindByNameAsync(RoleNames.Administrator);

                var user = await _userManager.FindByNameAsync(username);

                if (user != null)
                {
                    await _userManager.AddToRoleAsync(user, RoleNames.Administrator);
                }
            }
        }
    }
}
