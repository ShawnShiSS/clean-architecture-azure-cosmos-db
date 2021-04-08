using MediatR;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Infrastructure.Behaviours
{
    /// <summary>
    ///     MediatR pipeline behavior to handle any unhandled exception.
    ///     For more information: https://github.com/jbogard/MediatR/wiki/Behaviors
    /// </summary>
    /// <typeparam name="TRequest">The request object passed in through IMediator.Send.</typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        /// <summary>
        ///     ctor
        /// </summary>
        public UnhandledExceptionBehaviour()
        {
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="request">The request object passed in through IMediator.Send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="next">An async continuation for the next action in the behavior chain.</param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                string requestName = typeof(TRequest).Name;

                Log.Error(ex, "Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}
