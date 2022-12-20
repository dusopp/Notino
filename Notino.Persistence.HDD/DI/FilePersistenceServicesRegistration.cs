using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notino.Domain.Contracts.Persistence;
using Notino.Persistence.HDD.Repositories;

namespace Notino.Persistence.HDD.DI
{
    public static class FilePersistenceServicesRegistration
    {
        public static IServiceCollection ConfigureSecondaryPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {    
            
            services.AddScoped<IDocumentRepository, DocumentRepository>();            

            return services;
        }
    }
}
