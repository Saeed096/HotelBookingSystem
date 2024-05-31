using HotelBookingSystem.hotelContext;
using HotelBookingSystem.Interfaces;
using HotelBookingSystem.Models;
using HotelBookingSystem.Repositories;
using HotelBookingSystem.Services;
using HotelBookingSystem.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddDbContext<Context>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
                });


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(   
    options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;

    }
    ).AddEntityFrameworkStores<Context>().AddDefaultTokenProviders(); 



            builder.Services.Configure<MailSettings>
                (builder.Configuration.GetSection("MailSettings"));

            builder.Services.AddTransient<IMailService, MailService>();
            builder.Services.AddScoped<iHotelBranchRepository, hotelBranchRepository>();
            builder.Services.AddScoped<iRoomRepository, RoomRepository>();
            builder.Services.AddScoped<iReservationRepository, reservationRepository>();
            builder.Services.AddScoped<iUnitOfWork, UnitOfWork>();


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireRole("Admin");
                });
            });



            builder.Services.AddHttpClient();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=register}/{id?}");

            app.Run();
        }
    }
}