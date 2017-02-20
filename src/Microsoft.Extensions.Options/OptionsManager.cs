// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptions.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class OptionsManager<TOptions> : IOptions<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class, new()
    {
        private readonly Func<TOptions> _createOptions;
        private object _optionsLock = new object();
        private bool _optionsInitialized;
        private TOptions _options;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="initializer">The initializer options object to initialize the options.</param>
        public OptionsManager(IEnumerable<IConfigureOptions<TOptions>> setups, IOptionsInitializer<TOptions> initializer = null)
        {
            initializer = initializer ?? new DefaultOptionsInitializer<TOptions>(setups);

            _createOptions = () =>
            {
                initializer.InitializeOptions(reinitialize: false);

                var result = initializer.Options;

                return result;
            };
        }

        /// <summary>
        /// The configured options instance.
        /// </summary>
        public virtual TOptions Value => LazyInitializer.EnsureInitialized(ref _options, ref _optionsInitialized, ref _optionsLock, _createOptions);
    }
}