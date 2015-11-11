// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.OptionsModel;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OptionsConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, IConfiguration config)
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

            services.ConfigureOptions(new ConfigureFromConfigurationOptions<TOptions>(config));
            return services;
        }

        public static IServiceCollection TrackChanges<TOptions>(this IServiceCollection services, IConfiguration config)
            where TOptions : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.AddInstance<IOptionsChangeTracker<TOptions>>(new ConfigurationChangeTracker<TOptions>(config));
            return services;
        }
    }
}