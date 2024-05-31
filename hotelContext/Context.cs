using HotelBookingSystem.Enums;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HotelBookingSystem.hotelContext
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
       // public DbSet<ApplicationUser> users { get; set; } 
      //  public DbSet<Client> clients { get; set; }
        public DbSet<Room> rooms { get; set; } 
        public DbSet<Reservation> reservations { get; set; } 
        public DbSet<hotelBranch> branches { get; set; } 

        public Context() : base() { }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().HasIndex(appUser => appUser.Email)
                .IsUnique();

            var adminUserId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123456") 
            });


            builder.Entity<IdentityRole>().HasData(
          new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
          new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
      );

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = adminUserId,
                RoleId = "1" 
            });


            builder.Entity<hotelBranch>().HasData(
                new hotelBranch { id = 1, name = "Main Branch" },
                new hotelBranch { id = 2, name = "Secondary Branch" }
            );

            builder.Entity<Room>().HasData(
                new Room { id = 1, type = RoomType.Single, pricePerDay = 1000m, hotelId = 1 },
                new Room { id = 2, type = RoomType.Double, pricePerDay = 2000m, hotelId = 1 },
                new Room { id = 3, type = RoomType.Suite, pricePerDay = 4000m, hotelId = 1 },
                new Room { id = 4, type = RoomType.Single, pricePerDay = 1000m, hotelId = 2 },
                new Room { id = 5, type = RoomType.Double, pricePerDay = 2000m, hotelId = 2 },
                new Room { id = 6, type = RoomType.Single, pricePerDay = 1000m, hotelId = 1 },
                new Room { id = 7, type = RoomType.Double, pricePerDay = 4000m, hotelId = 2 },
                new Room { id = 8, type = RoomType.Suite, pricePerDay = 4000m, hotelId = 2 }, 
                new Room { id = 9, type = RoomType.Suite, pricePerDay = 4000m, hotelId = 1 }, 
                new Room { id = 10, type = RoomType.Suite, pricePerDay = 4000m, hotelId = 1 }, 
                new Room { id = 11, type = RoomType.Double, pricePerDay = 2000m, hotelId = 1 }, 
                new Room { id = 12, type = RoomType.Single, pricePerDay = 1000m, hotelId = 1 }, 
                new Room { id = 13, type = RoomType.Double, pricePerDay = 2000m, hotelId = 2 }, 
                new Room { id = 14, type = RoomType.Double, pricePerDay = 2000m, hotelId = 2 }, 
                new Room { id = 15, type = RoomType.Single, pricePerDay = 1000m, hotelId = 2 }, 
                new Room { id = 16, type = RoomType.Single, pricePerDay = 1000m, hotelId = 2 }
            );

            builder.Entity<Reservation>()
                   .HasKey(r => new { r.clientId, r.roomId, r.checkIn });

            builder.Entity<Reservation>().HasData(
                new Reservation {  clientId = adminUserId, roomId = 1, checkIn = new DateTime(2024, 5, 1), checkout = new DateTime(2024, 5, 7), name = "admin" , branchId = 1 , phoneNumber = "011155945" , nationalId = "464684", cost = 1000, adultsNum = 1 , childrenNum = 1 },
                new Reservation {  clientId = adminUserId, roomId = 2, checkIn = new DateTime(2024, 6, 1), checkout = new DateTime(2024, 6, 10), name = "admin", branchId = 2, phoneNumber = "01561155945", nationalId = "4964684", cost = 2000, adultsNum = 2 , childrenNum = 2 },
                new Reservation { clientId = adminUserId, roomId = 3, checkIn = new DateTime(2024, 7, 1), checkout = new DateTime(2024, 7, 5), name = "admin", branchId = 1, phoneNumber = "011155945", nationalId = "464684", cost = 4000, adultsNum = 4, childrenNum = 4 }
            );
        }
    }
}
