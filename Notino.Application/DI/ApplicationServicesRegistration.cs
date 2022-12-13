using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notino.Application.Behaviours;
using Notino.Application.Caching;
using Notino.Application.Contracts.Caching;
using Notino.Application.Contracts.PersistenceOrchestration;
using Notino.Application.PersistenceOrchestration;
using System.Reflection;

namespace Notino.Application.DI
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = Assembly.GetExecutingAssembly();           

            services.AddMediatR(applicationAssembly);
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResponseAdaptationBehaviour<,>));
            services.AddValidatorsFromAssembly(applicationAssembly);
            services.AddSingleton<IApplicationState, ApplicationMemoryCache>();
            services.AddScoped<IDocumentPersistenceOrchestrator, DocumentPersistenceOrchestrator>();

            return services;
        }
    }
}
