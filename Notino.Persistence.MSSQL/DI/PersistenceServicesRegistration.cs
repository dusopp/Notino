using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notino.Domain.Contracts.Persistence;
using Notino.Persistence.MSSQL.Repositories;
using Notino.Persistence.MSSQL.Repositories.Common;

namespace Notino.Persistence.MSSQL.DI
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePrimaryPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NotinoDbContext>(
                options => {
                    options.UseSqlServer(
                        configuration.GetConnectionString("NotinoDocumentManagementConnectionString"));
            });

            services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped<IDocumentRepository, DocumentRepository>();

            return services;
        }
    }
}
