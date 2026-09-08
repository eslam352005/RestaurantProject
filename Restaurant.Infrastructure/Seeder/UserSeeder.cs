using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using Restaurant.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;


namespace Restaurant.Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbcontext context)
        {
            if (await userManager.Users.AnyAsync())
            {
                return;
            }
            var adminUser = new ApplicationUser
            {
                UserName = "admin@restaurant.com",
                Email = "admin@restaurant.com",
                PhoneNumber = "1234567890",
                FullName = "System Admin",
                EmailConfirmed = true,
                IsActive = true
            };
            await userManager.CreateAsync(adminUser, "Admin123");
            await userManager.AddToRoleAsync(adminUser, "Admin");

            var staffUser = new Staff
            {
                ApplicationUserId = adminUser.Id,
                BranchId = 1,
                FullName = adminUser.FullName,
                Role = StaffRole.Admin
            };
            await context.Set<Staff>().AddAsync(staffUser);
            await userManager.AddToRoleAsync(adminUser, "Waiter");
            await context.SaveChangesAsync();

        }
    }
}
