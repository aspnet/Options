// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    public interface IOptionsCache<TOptions> where TOptions : class, new()
    {
        TOptions Get(string namedInstance);
        void Put(string namedInstance, TOptions value);
    }

    public interface IOptionsMonitorCache<TOptions> : IOptionsCache<TOptions> where TOptions : class, new()
    {
    }

    public class DefaultOptionsCache<TOptions> : IOptionsMonitorCache<TOptions> where TOptions : class, new()
    {
        private readonly Dictionary<string, TOptions> _cache = new Dictionary<string, TOptions>();
        private TOptions _value;

        public TOptions Get(string namedInstance)
        {
            if (namedInstance == null)
            {
                return _value;
            }
            if (_cache.ContainsKey(namedInstance))
            {
                return _cache[namedInstance];
            }
            return null;
        }

        public void Put(string namedInstance, TOptions value)
        {
            if (namedInstance == null)
            {
                _value = value;
            }
            else
            {
                _cache[namedInstance] = value;
            }
        }
    }
}