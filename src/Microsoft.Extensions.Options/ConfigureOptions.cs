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
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="action">The action to register.</param>
        public ConfigureOptions(Action<TOptions> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Action = action;
        }

        /// <summary>
        /// Used to determine the order which IConfigureOptions will be run.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions> Action { get; }

        /// <summary>
        /// Invokes the registered configure Action.
        /// </summary>
        /// <param name="options"></param>
        public virtual void Configure(TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Action.Invoke(options);
        }
    }
}