// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding configuration related options services to the DI container.
    /// </summary>
    public static class OptionsConfigurationServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, IConfiguration config)
            where TOptions : class
        {
            var setups = services.Where(s => s.ServiceType == typeof(IConfigureOptions<TOptions>) && s.ImplementationInstance != null && s.ImplementationType == typeof(ConfigureFromConfigurationOptions<TOptions>));
            var order = -10000; // REVIEW: should we expose this default constant for config?
            if (setups.Any())
            {
                order = setups.Select(s => s.ImplementationInstance)
                    .Cast<ConfigureFromConfigurationOptions<TOptions>>()
                    .Select(c => c.Order)
                    .Max() + 10;
            }

            return services.Configure<TOptions>(config, order);
        }

        /// <summary>
        /// Registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        /// <param name="order">The order used to determine when the options binding will be executed.</param>
        /// <returns></returns>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, IConfiguration config, int order)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            return services.AddSingleton<IConfigureOptions<TOptions>>(new ConfigureFromConfigurationOptions<TOptions>(config) { Order = order });
        }
    }
}