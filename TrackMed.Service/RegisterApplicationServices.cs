using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TrackMed.Repository.Context;

namespace TrackMed.Service
{
    public static class RegisterApplicationServices
    {
        public static void RegisterAppServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            RegisterDatabaseConnection(serviceCollection, configuration);
            RegisterServices(serviceCollection);
            RegisterRepositories(serviceCollection);
        }
        private static void RegisterDatabaseConnection(IServiceCollection servicesCollection, IConfiguration configuration)
        {
            servicesCollection.AddDbContext<TrackMedAppContext>(options => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); });
        }
        private static void RegisterServices(IServiceCollection servicesCollection)
        {
            
           
        }
        private static void RegisterRepositories(IServiceCollection servicesCollection)
        {
           
           
        }
    }
}
