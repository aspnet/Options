// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Creates IChangeTokens so that IOptionsMonitor gets notified when IConfiguration changes.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class ConfigurationChangeTokenSource<TOptions> : IOptionsChangeTokenSource<TOptions>
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructor taking the IConfiguration instance to watch.
        /// </summary>
        /// <param name="config">The configuration instance.</param>
        public ConfigurationChangeTokenSource(IConfiguration config) : this(Options.DefaultName, config)
        { }

        /// <summary>
        /// Constructor taking the IConfiguration instance to watch.
        /// </summary>
        /// <param name="name">The name of the options instance being watche.</param>
        /// <param name="config">The configuration instance.</param>
        public ConfigurationChangeTokenSource(string name, IConfiguration config)
        {
            Name = name;
            _config = config ?? throw new ArgumentNullException(nameof(config));
            Name = name ?? Options.DefaultName;
        }

        public string Name { get; }

        /// <summary>
        /// Returns the reloadToken from IConfiguration.
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetChangeToken() => _config.GetReloadToken();
    }
}