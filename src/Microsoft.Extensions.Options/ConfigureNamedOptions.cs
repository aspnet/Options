// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IConfigureNamedOptions.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class ConfigureNamedOptions<TOptions> : IConfigureNamedOptions<TOptions> where TOptions : class
    {
        private readonly string _name;
        private readonly Action<TOptions> _action;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, Action<TOptions> action)
        {
            _name = name;
            _action = action;
        }

        /// <summary>
        /// Invokes the registered configure Action if the name matches.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public virtual void Configure(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Null name is used to configure all named options.
            if (_name == null || name == _name)
            {
                _action?.Invoke(options);
            }
        }

        public void Configure(TOptions options) => Configure(Options.DefaultName, options);
    }
}