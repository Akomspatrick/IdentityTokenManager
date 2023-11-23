
using Microsoft.EntityFrameworkCore;
using WebApplication4IdentityDb.Auth;
using WebApplication4IdentityDb.Data;

namespace WebApplication4IdentityDb;

public static class APIServiceCollection
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration configuration)
    {
        //var applicationAssembly = typeof(APIServiceCollection).Assembly;
        services.AddDbContext<ApplicationDbContext>(option => option.UseMySql(configuration.GetConnectionString("Constr")!, new MySqlServerVersion(new Version(8, 0))));

        //services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("Constr")));
        //services.AddCors();


        // services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());
        return services;
    }



}

