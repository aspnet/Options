// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;

namespace Microsoft.Extensions.Options.Factory
{
    /// <summary>
    /// Used to create configured TOptions instances and keep until it reset.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public class CachedOptionsFactory<TOptions> : ICachedOptionsFactory<TOptions> where TOptions : class, new()
    {
        private readonly IOptionsFactory<TOptions> _optionsFactory;

        private TOptions _options;
        private bool _optionsCreated;
        private object _optionsLock = new object();

        /// <summary> 
        /// Initializes a new instance with the specified options configurations. 
        /// </summary> 
        /// <param name="optionsFactory">The options factory to create TOptions instance.</param> 
        public CachedOptionsFactory(IOptionsFactory<TOptions> optionsFactory)
        {
            _optionsFactory = optionsFactory;
        }

        /// <summary>
        /// Creates TOptions instance.
        /// </summary>
        public TOptions Get() => LazyInitializer.EnsureInitialized(ref _options, ref _optionsCreated, ref _optionsLock, _optionsFactory.Get);

        /// <summary>
        /// Reset cached TOptions instance.
        /// </summary>
        public void ResetCache()
        {
            _optionsCreated = false;
        }
    }
}