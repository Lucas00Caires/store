
using Domain.Model.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> UserManager)
        {
            if(!UserManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "lucas",
                    Email = "lucas@email",
                    UserName = "lucas@test.com",
                    Address = new Address
                    {
                        FirstName = "lucas",
                        LastName = "luc",
                        Street = "strees 123",
                        City = "citytest",
                        State = "statetest",
                        ZipCode = "1234"
                    }
                };

                await UserManager.CreateAsync(user, "Pa$$w0rd");
            }

        }
    }
}
