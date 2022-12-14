﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notino.Application.Contracts.Persistence;
using Notino.Persistence.MSSQL.Repositories;
using Notino.Persistence.MSSQL.Repositories.Common;
using System.ComponentModel;

namespace Notino.Persistence.MSSQL.DI
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DocumentDbContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("NotinoDocumentManagementConnectionString")));


            services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<DocumentRepository>();


            return services;
        }
    }
}
