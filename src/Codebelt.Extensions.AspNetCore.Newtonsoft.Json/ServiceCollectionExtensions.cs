using System;
using Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Cuemon;
using Microsoft.Extensions.DependencyInjection;

namespace Codebelt.Extensions.AspNetCore.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a <see cref="NewtonsoftJsonFormatterOptions"/> service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="setup">The <see cref="NewtonsoftJsonFormatterOptions"/> which may be configured.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddMinimalNewtonsoftJsonOptions(this IServiceCollection services, Action<NewtonsoftJsonFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(services);
            return services.AddNewtonsoftJsonExceptionResponseFormatter(setup);
        }
    }
}
