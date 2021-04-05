using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Config
{
    /// <summary>
    ///     OData
    /// </summary>
    public static class ODataConfig
    {
        /// <summary>
        ///     Setup OData
        /// </summary>
        /// <param name="services"></param>
        public static void SetupOData(this IServiceCollection services)
        {
            // OData Support
            services.AddOData();

            // In order to make swagger work with OData
            services.AddMvcCore(options =>
            {
                foreach (OutputFormatter outputFormatter in options.OutputFormatters.OfType<OutputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                foreach (InputFormatter inputFormatter in options.InputFormatters.OfType<InputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
        }
    }
}
