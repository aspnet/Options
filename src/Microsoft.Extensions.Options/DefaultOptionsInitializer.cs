// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    internal class DefaultOptionsInitializer<TOptions> : OptionsInitializer<TOptions> where TOptions : class, new()
    {
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;

        public DefaultOptionsInitializer(IEnumerable<IConfigureOptions<TOptions>> setups)
        {
            _setups = setups;
        }

        public override void InitializeOptionsCore(TOptions options)
        {
            if (_setups != null)
            {
                foreach (var setup in _setups)
                {
                    setup.Configure(options);
                }
            }
        }
    }
}