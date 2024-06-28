using HotelSo.Data;
using HotelSo.Models;
using HotelSo.Models.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelSo
{
    public class SeedData
    {
       
       public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (await context.Reservations.AnyAsync())
                {
                    return; // DB has been seeded
                }

                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var adminRoleExists = await roleManager.RoleExistsAsync("Administrator");
                if (!adminRoleExists)
                {
                    var role = new IdentityRole { Name = "Administrator" };
                    await roleManager.CreateAsync(role);
                }

                var userRoleExists = await roleManager.RoleExistsAsync("User");
                if (!userRoleExists)
                {
                    var role = new IdentityRole { Name = "User" };
                    await roleManager.CreateAsync(role);
                }

                // Seed an admin user
                var adminUser = await userManager.FindByEmailAsync("admin@hotelSo.com");
                if (adminUser == null)
                {
                    var admin = new ApplicationUser
                    {
                        UserName = "admin@hotelSo.com",
                        Email = "admin@hotelSo.com",
                        Firstname = "HotelSo",
                        Lastname = "HotelSo",
                        Telephone = "6981694809"
                    };

                    var result = await userManager.CreateAsync(admin, "!Admin123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Administrator");
                        await userManager.AddToRoleAsync(admin, "User");
                    }
                }

                // Seed a user
                var user = await userManager.FindByEmailAsync("GiotaG@gmail.com");
                if (user == null)
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = "GiotaG@gmail.com",
                        Email = "GiotaG@gmail.com",
                        Firstname = "Giota",
                        Lastname = "Geo",
                        Telephone = "6981694809"
                    };

                    var result = await userManager.CreateAsync(newUser, "!User123");
                    if (result.Succeeded)
                    {
                        await  userManager.AddToRoleAsync(newUser, "User");
                    }
                }

                var user1 = await userManager.FindByEmailAsync("NikosD@gmail.com");
                if (user1 == null)
                {
                    var newUser1 = new ApplicationUser
                    {
                        UserName = "NikosD@gmail.com",
                        Email = "NikosD@gmail.com",
                        Firstname = "Nikos",
                        Lastname = "Doe",
                        Telephone = "6979651736"
                    };

                    var result = await userManager.CreateAsync(newUser1, "!User123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser1, "User");
                    }
                }

                if (!await context.Rooms.AnyAsync())
                {
                    // Create Amenities
                    var amenities1 = new AmenitiesPerRoom { AmenitiesPackage = "Basic", Tv = true, AirCondition = true, FreeWiFi = true };
                    var amenities2 = new AmenitiesPerRoom { AmenitiesPackage = "Premium", Tv = true, AirCondition = true, Sauna = true, FreeWiFi = true, MiniBar = true };

                    context.Amenities.Add(amenities1);
                    context.Amenities.Add(amenities2);
                    

                    // Create Rooms
                    var room1 = new Room { Description = "Classic Room", Price = 50m, Bed = Bed.OneD, Size = 15, View = View.Mountain, MaxPersons = MaxPersons.One, RoomType = RoomType.ClassicRoom, Floor = Floor.One, Amenities = amenities1, ProfilePicture="/Content/images/room-1.jpg" };
                    var room2 = new Room { Description = "Premium Room", Price = 100m, Bed = Bed.TwoD, Size = 30, View = View.Sea, MaxPersons = MaxPersons.Two, RoomType = RoomType.PremiumRoom, Floor = Floor.Two, Amenities = amenities2, ProfilePicture = "/Content/images/room-2.jpg" };
                    var room3 = new Room { Description = "Classic Suite", Price = 150m, Bed = Bed.TwoD, Size = 40, View = View.Sea, MaxPersons = MaxPersons.Four, RoomType = RoomType.ClassicSuite, Floor = Floor.Three, Amenities = amenities2, ProfilePicture = "/Content/images/room-8.jpg" };
                    var room4 = new Room { Description = "Premium Suite", Price = 200m, Bed = Bed.TwoD, Size = 50, View = View.Sea, MaxPersons = MaxPersons.Four, RoomType = RoomType.PremiumSuite, Floor = Floor.Four, Amenities = amenities2, ProfilePicture = "/Content/images/room-6.jpg" };

                    context.Rooms.Add(room1);
                    context.Rooms.Add(room2);
                    context.Rooms.Add(room3);
                    context.Rooms.Add(room4);

                }

                await context.SaveChangesAsync();

                user = await userManager.FindByEmailAsync("GiotaG@gmail.com");
                user1 = await userManager.FindByEmailAsync("NikosD@gmail.com");

                // Create Reservations only if the user exists and rooms have been added
                if (user != null && context.Rooms.Any())
                {
                    var room1 = await context.Rooms.FirstOrDefaultAsync(r => r.Description == "Classic Room");
                    if (room1 != null)
                    {
                        var reservation1 = new Reservation { ArrivalDate = DateTime.Now.AddDays(10), DepartureDate = DateTime.Now.AddDays(15), Room = room1, UserId = user.Id };
                        context.Reservations.Add(reservation1);
                    }
                }

                if (user1 != null && context.Rooms.Any())
                {
                    var room2 = await context.Rooms.FirstOrDefaultAsync(r => r.Description == "Premium Room");
                    if (room2 != null)
                    {
                        var reservation2 = new Reservation { ArrivalDate = DateTime.Now.AddDays(20), DepartureDate = DateTime.Now.AddDays(25), Room = room2, UserId = user1.Id };
                        context.Reservations.Add(reservation2);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
