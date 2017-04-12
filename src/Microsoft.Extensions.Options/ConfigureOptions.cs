// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IConfigureOptions.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class ConfigureOptions<TOptions> : IConfigureOptions<TOptions> where TOptions : class
    {
        private readonly Action<TOptions> _action;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="action">The action to register.</param>
        public ConfigureOptions(Action<TOptions> action)
        {
            _action = action;
        }

        /// <summary>
        /// Invokes the registered configure Action if the name matches.
        /// </summary>
        /// <param name="options"></param>
        public virtual void Configure(TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _action?.Invoke(options);
        }
    }
}