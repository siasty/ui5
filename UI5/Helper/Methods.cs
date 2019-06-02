using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI5.Data;

namespace UI5.Helper
{
    public class Methods
    {
        public static async Task CreateRolesAsync(IServiceProvider serviceProvider)
        {

            using (var hgsManager = new MyDbContext())
            {
                var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();


                var sudo = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@hgs3.pl"
                };
                string UserPassword = "Th9S38vk@@";
                var _user = await UserManager.FindByEmailAsync(sudo.Email);

                if (_user == null)
                {
                    var createPowerUser = await UserManager.CreateAsync(sudo, UserPassword);
                    if (createPowerUser.Succeeded)
                    {
                    }
                }
            }
        }

    }
}
