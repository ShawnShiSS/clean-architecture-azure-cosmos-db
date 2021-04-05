using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
            // MediatR, this will scan and register everything that inherits IRequest, IPipelineBehavior
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.Behaviours.ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.ApiExceptions.UnhandledExceptionBehaviour<,>));
        }
    }
}
