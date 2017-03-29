// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Configures an option instance by using ConfigurationBinder.Bind against an IConfiguration.
    /// </summary>
    /// <typeparam name="TOptions">The type of options to bind.</typeparam>
    public class ConfigureFromConfigurationOptions<TOptions> : ConfigureNamedOptions<TOptions>
        where TOptions : class
    {
        /// <summary>
        /// Constructor that takes the IConfiguration instance to bind against.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="config">The IConfiguration instance.</param>
        public ConfigureFromConfigurationOptions(string name, IConfiguration config)
            : base(name, options => ConfigurationBinder.Bind(config, options))
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
        }
    }
}