// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OptionsServiceCollectionExtensions
    {
        public static IServiceCollection AddOptionsMonitor<TOptions>(this IServiceCollection services) where TOptions : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptionsMonitor<>), typeof(OptionsMonitor<>)));
            return services;
        }


        public static IServiceCollection AddOptions<TOptions>(this IServiceCollection services) where TOptions : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(serviceProvider =>
            {
                var options = new TOptions();
                foreach (var c in serviceProvider.GetRequiredService<IEnumerable<IConfigureOptions<TOptions>>>())
                {
                    c.Configure(options);
                }
                return options;
            });
            return services;
        }

        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.AddSingleton<IConfigureOptions<TOptions>>(new ConfigureOptions<TOptions>(configureOptions));
            return services;
        }
    }
}