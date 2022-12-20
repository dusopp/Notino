using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notino.Application.Behaviours;
using Notino.Application.PersistenceOrchestration.Document;
using Notino.Application.ResponseConversion.Factory;
using Notino.Domain.Contracts.PersistenceOrchestration;
using Notino.Domain.Contracts.ResponseConversion.Factory;
using System.Reflection;

namespace Notino.Application.DI
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = Assembly.GetExecutingAssembly();           

            services.AddMediatR(applicationAssembly);
            
            //for pipeline behaviour, ORDER MATTERS!!!
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResponseConversionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            
            services.AddValidatorsFromAssembly(applicationAssembly);           
            services.AddScoped<IDocumentPersistenceOrchestrator, DocumentPersistenceOrchestrator>();
            services.AddScoped<IResponseConverterFactory, ResponseConverterFactory>();

            return services;
        }
    }
}
