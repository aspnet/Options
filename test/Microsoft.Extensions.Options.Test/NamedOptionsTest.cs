// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.Extensions.Options.Tests
{
    public class NamedOptionsTest
    {
        public class MyNameSelector : IOptionsNameSelector
        {
            private static int _count;

            public MyNameSelector()
            {
                _count++;
            }

            public string ResolveName()
            {
                return _count.ToString();
            }
        }

        [Fact]
        public void CanResolveDifferentOptionsBasedOnScopeName()
        {
            var services = new ServiceCollection()
                .AddScoped<IOptionsNameSelector, MyNameSelector>()
                .AddOptions();

            services.Configure<FakeOptions>("1", options =>
            {
                options.Message = "one";
            });
            services.Configure<FakeOptions>("2", options =>
            {
                options.Message = "two";
            });

            var sp = services.BuildServiceProvider();

            var factory = sp.GetRequiredService<IServiceScopeFactory>();
            using (var scope = factory.CreateScope())
            {
                Assert.Equal("one", scope.ServiceProvider.GetRequiredService<IOptions<FakeOptions>>().Value.Message);
            }
            using (var scope = factory.CreateScope())
            {
                Assert.Equal("two", scope.ServiceProvider.GetRequiredService<IOptions<FakeOptions>>().Value.Message);
            }
        }
    }
}
