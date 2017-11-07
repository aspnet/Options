// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding configuration related options services to the DI container via <see cref="OptionsConfigurator{TOptions}"/>.
    /// </summary>
    public static class OptionsConfiguratorConfigurationExtensions
    {
        /// <summary>
        /// Registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="optionsConfigurator">The options builder to add the services to.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns>The <see cref="OptionsConfigurator{TOptions}"/> so that additional calls can be chained.</returns>
        public static OptionsConfigurator<TOptions> Configure<TOptions>(this OptionsConfigurator<TOptions> optionsConfigurator, IConfiguration config) where TOptions : class
        {
            if (optionsConfigurator == null)
            {
                throw new ArgumentNullException(nameof(optionsConfigurator));
            }

            optionsConfigurator.Services.Configure<TOptions>(optionsConfigurator.Name, config);
            return optionsConfigurator;
        }
    }
}