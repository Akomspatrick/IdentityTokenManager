
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections;
using System.Security.Claims;
using WebApplication4IdentityDb.Data;
using WebApplication4IdentityDb.Models;
using static WebApplication4IdentityDb.TypeSafeData;


namespace WebApplication4IdentityDb
{
    public static class ServiceRegistration
    {
        public static async Task<IApplicationBuilder> SeeddataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await ctx.Database.EnsureDeletedAsync();


                if (await ctx.Database.EnsureCreatedAsync())
                {
                    ///Creating Role Entities
                    var adminRole = new IdentityRole(TypeSafeData.Roles.Admin);
                    var userRole = new IdentityRole(TypeSafeData.Roles.User);
                    var engineerRole = new IdentityRole(TypeSafeData.Roles.DesignerEngineer);
                    var prodRole = new IdentityRole(TypeSafeData.Roles.ProductionManager);
                    var assemblerRole = new IdentityRole(TypeSafeData.Roles.Assembler);
                    var roles = new List<IdentityRole> { adminRole, userRole, engineerRole, assemblerRole, prodRole };
                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }

                    ///Creating User Entities , you can add the maniputations for BarCode here 
                    /// that is take any of the attribute , hash it or encrypt it and then put is as the value of the barcode 
                    /// you may provide a function to convert it back also 
                    /// this must be printable as barcode value
                    /// therefore barcode value must be a string
                    /// you get this from barcode into a password filed 
                    /// and then use it to login
                    /// provide a custom find on the user context to return the user based on the barcode value
                    var adminUserName = "admin";
                    var assemblerUserName = "user";
                    var superUserName = "contributor";
                    var adminUser = new AppUser
                    {
                        UserName = adminUserName,
                        Email = "admin@admin.com",
                        BarCode = Encrypt.EncryptDecrypt(adminUserName, 10)

                    };
                    var userUser = new AppUser
                    {
                        UserName = assemblerUserName,
                        Email = "user@user.com",
                        BarCode = Encrypt.EncryptDecrypt(assemblerUserName, 10)
                    };


                    var superUser = new AppUser
                    {
                        UserName = superUserName,
                        Email = "contrib@contrib.com",
                        BarCode = Encrypt.EncryptDecrypt(superUserName, 10)
                    };


                    var users = new List<AppUser> { adminUser, userUser, superUser };
                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, TypeSafeData.DefaultPassword.Admin);
                    };
                    // end of creating users with passwords



                    //Adding Claims to users , 
                    //superUser has all the claims
                    foreach (var claim in TypeSafeData.Features.GetFeaturesAsClaims())
                    {
                        await userManager.AddClaimAsync(adminUser, claim);
                    }

                    //or you can add claims like this
                    // add admin role to admin user


                    await userManager.AddToRoleAsync(adminUser, TypeSafeData.Roles.Admin);
                    await userManager.AddToRoleAsync(userUser, TypeSafeData.Roles.Assembler);
                    await userManager.AddToRoleAsync(superUser, TypeSafeData.Roles.User);


                    // claims to Roles
                    // I want to add all claims to admin role thereforre , i didnt add any claims to admin user it
                    // should be able to do all the operations because of the role

                    // I want to add only few claims to assembler role , particular claims
                    // I am not adding any claim to the user role but since super user already has all the claims 
                    // it should be able to do all the operations
                    // therefore I am not adding any claims to the user role
                    // I am adding only few claims to the assembler role

                    foreach (var claim in TypeSafeData.Features.GetFeaturesAsClaims())
                    {
                        await roleManager.AddClaimAsync(adminRole, claim);

                    }
                    // Add claims to assembler role , only few claims 
                    // the claims now have to be extracted from TypeSafeData.Features
                    // this is where a dropdown of features/menu will be shown to the user
                    //  public static ListDictionary features = new ListDictionary { { "001", "Read" }, { "002", "Write" }, { "003", "Update" }, { "004", "Delete" } };

                    await roleManager.AddClaimAsync(assemblerRole, new Claim("001", "Read"));
                    await roleManager.AddClaimAsync(assemblerRole, new Claim("002", "Write"));

                    // Therefore every user will have a role and a set of claims
                    //Admin has all claims thru Adminrole
                    //Assembler has only few claims thru Assembler role directly added to the user
                    //User has all claims thru SuperUser role getting it programmatically
                    // therefore I should still add claims to assembler role , Design enginer and production manager role
                    // so that once you are give a role , all the claims of that role will be available to you
                    // else you will have to add claims to the user directly through menu at the front end

                }
                else
                {
                    await ctx.Database.EnsureCreatedAsync();
                }

                return app;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static IdentityBuilder AddIdentityBuilder(this IServiceCollection services)
        {
            var x = services.AddIdentityCore<AppUser>(options =>
             {
                 options.Password.RequireDigit = false;
                 options.Password.RequiredLength = 3;
                 options.Password.RequireUppercase = false;
                 options.Password.RequireLowercase = false;
                 options.Password.RequiredUniqueChars = 0;
                 options.Password.RequireNonAlphanumeric = false;
                 options.User.RequireUniqueEmail = true;
                 options.SignIn.RequireConfirmedEmail = false;
                 options.Lockout.MaxFailedAccessAttempts = 5;
                 options.Lockout.AllowedForNewUsers = true;

                 options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                 options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
             });
            x.AddRoles<IdentityRole>();
            var p = x.AddEntityFrameworkStores<ApplicationDbContext>();
            //var q = p.AddRoleValidator<RoleValidator<IdentityRole>>();
            var r = p.AddRoleManager<RoleManager<IdentityRole>>();
            var z = r.AddSignInManager<SignInManager<AppUser>>();
            var m = z.AddDefaultTokenProviders();

            return m;
            //var builder = services.AddIdentityCore<AppUser>(options =>
            //{
            //    options.Password.RequireDigit = true;
            //    options.Password.RequiredLength = 8;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequiredUniqueChars = 1;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.User.RequireUniqueEmail = true;
            //    options.SignIn.RequireConfirmedEmail = true;
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            //    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //});
            //builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            //builder.AddEntityFrameworkStores<ApplicationDbContext>();
            //builder.AddRoleValidator<RoleValidator<IdentityRole>>();
            //builder.AddRoleManager<RoleManager<IdentityRole>>();
            //builder.AddSignInManager<SignInManager<AppUser>>();
            //builder.AddDefaultTokenProviders();
            //return builder;
        }
    }
}
