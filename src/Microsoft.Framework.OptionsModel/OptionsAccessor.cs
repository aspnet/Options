// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Framework.OptionsModel
{
    public class OptionsAccessor<TOptions> : IOptionsAccessor<TOptions> where TOptions : class,new()
    {
        private object _lock = new object();
        private object _mapLock = new object();
        private TOptions _options;
        private Dictionary<string, TOptions> _namedOptions = new Dictionary<string, TOptions>(StringComparer.OrdinalIgnoreCase);
        private IEnumerable<IOptionsSetup<TOptions>> _setups;

        public OptionsAccessor(IEnumerable<IOptionsSetup<TOptions>> setups)
        {
            _setups = setups;
        }

        public virtual TOptions GetNamedOptions([NotNull] string name)
        {
            if (!_namedOptions.ContainsKey(name))
            {
                lock (_mapLock)
                {
                    if (!_namedOptions.ContainsKey(name))
                    {
                        _namedOptions[name] = SetupOptions(name);
                    }
                }
            }
            return _namedOptions[name];
        }

        public virtual TOptions SetupOptions(string name = null)
        {
            if (_setups == null)
            {
                return new TOptions();
            }
            return _setups
                .Where(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase))
                .OrderBy(setup => setup.Order)
                .Aggregate(
                    new TOptions(),
                    (options, setup) =>
                    {
                        setup.Setup(options);
                        return options;
                    });
        }

        public virtual TOptions Options
        {
            get
            {
                if (_options == null)
                {
                    lock (_lock)
                    {
                        if (_options == null)
                        {
                            _options = SetupOptions();
                        }
                    }
                }
                return _options;
            }
        }
    }
}