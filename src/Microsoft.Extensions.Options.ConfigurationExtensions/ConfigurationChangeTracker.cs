// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.OptionsModel
{
    public class ConfigurationChangeTracker<TOptions> : IOptionsChangeTracker<TOptions>
    {
        private IConfiguration _config;

        public ConfigurationChangeTracker(IConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            _config = config;
        } 

        public IChangeToken GetChangeToken()
        {
            return _config.GetReloadToken();
        }

        public void Changed()
        {
            _config.Root.Reload();
        }
    }
}