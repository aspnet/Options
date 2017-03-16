// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptionsFactory.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public class OptionsFactory<TOptions> : IOptionsFactory<TOptions> where TOptions : class, new()
    {
        private readonly Dictionary<string, TOptions> _cache = new Dictionary<string, TOptions>();
        private readonly IEnumerable<IConfigureNamedOptions<TOptions>> _setups;
        private readonly IEnumerable<IValidateNamedOptions<TOptions>> _checks;
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="checks">The validation actions to run.</param>
        public OptionsFactory(IEnumerable<IConfigureNamedOptions<TOptions>> setups, IEnumerable<IValidateNamedOptions<TOptions>> checks)
        {
            _setups = setups;
            _checks = checks;
        }

        public TOptions Get(string name)
        {
            if (_cache.ContainsKey(name))
            {
                return _cache[name];
            }
            else
            {
                lock (_lock)
                {
                    if (_cache.ContainsKey(name))
                    {
                        return _cache[name];
                    }

                    var value = new TOptions();
                    foreach (var setup in _setups)
                    {
                        setup.Configure(name, value);
                    }
                    foreach (var check in _checks)
                    {
                        check.Validate(name, value);
                    }
                    _cache[name] = value;
                    return value;
                }
            }
        }
    }
}