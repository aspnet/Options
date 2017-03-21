// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.Extensions.Options.Tests
{
    public class OptionsFactoryTest
    {
        [Fact]
        public void CanResolveNamedOptions()
        {
            var services = new ServiceCollection().AddOptions();

            services.Configure<FakeOptions>("1", options =>
            {
                options.Message = "one";
            });
            services.Configure<FakeOptions>("2", options =>
            {
                options.Message = "two";
            });

            var sp = services.BuildServiceProvider();
            var option = sp.GetRequiredService<IOptions<FakeOptions>>();
            Assert.Equal("one", option.Get("1").Message);
            Assert.Equal("two", option.Get("2").Message);
        }

        [Fact]
        public void FactoryCanConfigureAllOptions()
        {
            var services = new ServiceCollection().AddOptions();
            services.ConfigureAll<FakeOptions>(o => o.Message = "Default");

            var sp = services.BuildServiceProvider();
            var option = sp.GetRequiredService<IOptions<FakeOptions>>();
            Assert.Equal("Default", option.Get("1").Message);
            Assert.Equal("Default", option.Get("2").Message);
        }

        [Fact]
        public void FactoryConfiguresInRegistrationOrder()
        {
            var services = new ServiceCollection().AddOptions();
            services.Configure<FakeOptions>("-", o => o.Message += "-");
            services.ConfigureAll<FakeOptions>(o => o.Message += "A");
            services.Configure<FakeOptions>("+", o => o.Message += "+");
            services.ConfigureAll<FakeOptions>(o => o.Message += "B");
            services.ConfigureAll<FakeOptions>(o => o.Message += "C");
            services.Configure<FakeOptions>("+", o => o.Message += "+");
            services.Configure<FakeOptions>("-", o => o.Message += "-");

            var sp = services.BuildServiceProvider();
            var option = sp.GetRequiredService<IOptions<FakeOptions>>();
            Assert.Equal("ABC", option.Get("1").Message);
            Assert.Equal("A+BC+", option.Get("+").Message);
            Assert.Equal("-ABC-", option.Get("-").Message);
        }

    }
}