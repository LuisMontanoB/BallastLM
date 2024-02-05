using System.Diagnostics.CodeAnalysis;
using Ballast.Business.Repository.Interfaces;
using Ballast.Business.Repository;
using Ballast.Business.Services.Interfaces;
using Ballast.Business.Services;

namespace Ballast.API
{
    [ExcludeFromCodeCoverage]
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IHashService, HashService>();

            return services;
        }
    }
}
