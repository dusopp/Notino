using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notino.Application.Behaviours;
using Notino.Application.Contracts.PersistenceOrchestration;
using Notino.Application.PersistenceOrchestration.Document;
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
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResponseAdaptationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            
            services.AddValidatorsFromAssembly(applicationAssembly);           
            services.AddScoped<IDocumentPersistenceOrchestrator, DocumentPersistenceOrchestrator>();

            return services;
        }
    }
}
