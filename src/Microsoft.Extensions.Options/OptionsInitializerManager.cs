// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Options
{
    internal class OptionsInitializerManager : IOptionsInitializerManager
    {
        private readonly IServiceProvider _provider;

        public OptionsInitializerManager(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Initialize()
        {
            var initializers = _provider.GetServices<IOptionsInitializer>();

            foreach (var initializer in initializers)
            {
                initializer.InitializeOptions();
            }
        }
    }
}