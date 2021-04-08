using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitectureCosmosDB.WebAPI.Config
{
    /// <summary>
    ///     MediatR config
    /// </summary>
    public static class MediatrConfig
    {
        /// <summary>
        ///     Setup mediatr to use command/query pattern and pipeline behaviors
        /// </summary>
        /// <param name="services"></param>
        public static void SetupMediatr(this IServiceCollection services)
        {
            // MediatR, this will scan and register everything that inherits IRequest
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Register MediatR pipeline behaviors, in the same order the behaviors should be called.
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.Behaviours.ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.Behaviours.UnhandledExceptionBehaviour<,>));
        }
    }
}
