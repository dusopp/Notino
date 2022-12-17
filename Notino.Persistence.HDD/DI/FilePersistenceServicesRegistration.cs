using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notino.Application.Contracts.Persistence;
using Notino.Persistence.HDD.Repositories;
using Notino.Persistence.HDD.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Persistence.HDD.DI
{
    public static class FilePersistenceServicesRegistration
    {
        public static IServiceCollection ConfigureSecondaryPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
       
            
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
