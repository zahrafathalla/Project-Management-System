using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Abstractions.Consts;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Helper;
using System.Text.Json;

namespace ProjectManagementSystem.Data.Context;

public static class StoreContextseed
{
    public static async Task seedAsync(ApplicationDBContext dbcontext)
    {
        Role? adminRole = null;
        Role? memberRole = null;

        if (!dbcontext.Set<Role>().Any(r => r.Name == DefaultRoles.Admin))
        {
            adminRole = new Role
            {
                Name = DefaultRoles.Admin
            };

            await dbcontext.Set<Role>().AddAsync(adminRole);
            await dbcontext.SaveChangesAsync();
        }
        else
        {
            adminRole = await dbcontext.Set<Role>().FirstOrDefaultAsync(r => r.Name == DefaultRoles.Admin);
        }

        if (!dbcontext.Set<Role>().Any(r => r.Name == DefaultRoles.Member))
        {
            memberRole = new Role
            {
                Name = DefaultRoles.Member,
                IsDefault = true
            };

            await dbcontext.Set<Role>().AddAsync(memberRole);
            await dbcontext.SaveChangesAsync();
        }
        else
        {
            memberRole = await dbcontext.Set<Role>().FirstOrDefaultAsync(r => r.Name == DefaultRoles.Member);
        }

        if (!dbcontext.Set<User>().Any(u => u.UserName == DefaultUsers.AdminUserName))
        {
            var user = new User
            {
                Country = DefaultUsers.Country,
                Email = DefaultUsers.AdminEmail,
                UserName = DefaultUsers.AdminUserName,
                IsEmailVerified = true,
                PhoneNumber = DefaultUsers.AdminPhoneNumber,
                VerificationToken = null,
                PasswordHash = PasswordHasher.HashPassword(DefaultUsers.AdminPassword)
            };

            await dbcontext.Set<User>().AddAsync(user);
            await dbcontext.SaveChangesAsync();

            if (adminRole != null)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = adminRole.Id,
                };

                await dbcontext.UserRoles.AddAsync(userRole);
                await dbcontext.SaveChangesAsync();
            }
        }
    }
}
