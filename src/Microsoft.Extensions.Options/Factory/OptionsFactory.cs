// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.Options.Factory
{
    /// <summary>
    /// Used to create configured TOptions instances.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public class OptionsFactory<TOptions> : IOptionsFactory<TOptions> where TOptions : class, new()
    {
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;

        /// <summary> 
        /// Initializes a new instance with the specified options configurations. 
        /// </summary> 
        /// <param name="setups">The configuration actions to run for TOptions instance creation.</param> 
        public OptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups)
        {
            _setups = setups;
        }

        /// <summary>
        /// Creates TOptions instance.
        /// </summary>
        public virtual TOptions Get()
        {
            var result = new TOptions();

            if (_setups != null)
            {
                foreach (var setup in _setups)
                {
                    setup.Configure(result);
                }
            }

            return result;
        }
    }
}