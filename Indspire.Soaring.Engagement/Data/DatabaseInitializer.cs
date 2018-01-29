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
        public async Task Initialize(IConfiguration configuration)
        {
            var databaseExists = (_context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

            if (databaseExists)
            {
                if (!_context.Roles.Any(r => r.Name == RoleNames.Administrator))
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = RoleNames.Administrator
                    });
                }

                if (!_context.Roles.Any(r => r.Name == RoleNames.Editor))
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = RoleNames.Editor
                    });
                }

                var username = configuration["AdminUsername"];
                var password = configuration["AdminPassword"];

                if (!string.IsNullOrWhiteSpace(username) &&
                    !string.IsNullOrWhiteSpace(password))
                {
                    var user = await _userManager.FindByNameAsync(username);

                    if (user == null)
                    {
                        var result = await _userManager.CreateAsync(

                            new ApplicationUser
                            {
                                UserName = username,
                                Email = username,
                                EmailConfirmed = true
                            },

                            password);

                        var createdUser = await _userManager.FindByNameAsync(username);

                        if (createdUser != null)
                        {
                            var adminRole = _roleManager.FindByNameAsync(RoleNames.Administrator);

                            await _userManager.AddToRoleAsync(createdUser, RoleNames.Administrator);
                        }
                    }
                    else
                    {
                        if (!await _userManager.IsInRoleAsync(user, RoleNames.Administrator))
                        {
                            await _userManager.AddToRoleAsync(user, RoleNames.Administrator);
                        }
                    }
                }
            }

            return;
        }        
    }
}
