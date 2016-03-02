// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    public class OptionsInitializer<TOptions> : IOptionsInitializer<TOptions> where TOptions : class
    {
        private readonly IEnumerable<IConfigureOptions<TOptions>> _configurations;

        public OptionsInitializer(IEnumerable<IConfigureOptions<TOptions>> configurations)
        {
            _configurations = configurations;
        } 

        /// <summary>
        /// Initializes the options instance.
        /// </summary>
        /// <param name="instance">The options instance.</param>
        /// <returns></returns>
        public virtual TOptions Initialize(TOptions instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            foreach (var configure in _configurations)
            {
                configure.Configure(instance);
            }
            return instance;
        }
    }
}