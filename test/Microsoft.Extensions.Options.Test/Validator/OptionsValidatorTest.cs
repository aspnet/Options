// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Microsoft.Extensions.Options.Tests
{
    public class OptionsValidatorTest
    {
        [Fact]
        public void CanValidateOptionsEagerly()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IOptionsValidator, OptionsValidator>();
            services.AddOptions<ComplexOptions>()
                .Configure(o =>
                {
                    o.Boolean = false;
                    o.Integer = 11;
                    o.Virtual = "wut";
                })
                .Validate(o => o.Boolean)
                .Validate(o => o.Virtual == null, "Virtual")
                .Validate(o => o.Integer > 12, "Integer")
                .ValidatorEnabled();

            var sp = services.BuildServiceProvider();
            var startupValidator = sp.GetRequiredService<IOptionsValidator>();
            var error = Assert.Throws<OptionsValidatorException>(() => startupValidator.Validate());
            OptionsBuilderTest.ValidateFailure<ComplexOptions>(error.ValidatorExceptions.Single(), Options.DefaultName, "A validation error has occured.", "Virtual", "Integer");
        }

        [Fact]
        public void CanValidateMultipleOptionsEagerly()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IOptionsValidator, OptionsValidator>();
            services.AddOptions<ComplexOptions>("bool")
                .Configure(o =>
                {
                    o.Boolean = false;
                })
                .Validate(o => o.Boolean)
                .ValidatorEnabled();
            services.AddOptions<ComplexOptions>("int")
                .Configure(o =>
                {
                    o.Integer = 11;
                })
                .Validate(o => o.Integer != 11, "Not 11.")
                .ValidatorEnabled();
            services.AddOptions<ComplexOptions>("good")
                .ValidatorEnabled();
            services.AddOptions<ComplexOptions>()
                .ValidatorEnabled();

            var sp = services.BuildServiceProvider();

            var startupValidator = sp.GetRequiredService<IOptionsValidator>();

            var error = Assert.Throws<OptionsValidatorException>(() => startupValidator.Validate());
            var failures = error.ValidatorExceptions.ToArray();
            Assert.Equal(2, failures.Length);
            OptionsBuilderTest.ValidateFailure<ComplexOptions>(failures[0], "bool", "A validation error has occured.");
            OptionsBuilderTest.ValidateFailure<ComplexOptions>(failures[1], "int", "Not 11.");
        }
    }
}