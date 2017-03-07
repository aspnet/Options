// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options.Validation;
using Xunit;

namespace Microsoft.Extensions.Options.Tests
{
    public class OptionsValidatorTest
    {
        private class ValidationTestOptions
        {
            public int IntValue { get; set; }

            public string StringValue { get; set; }

            public double? DoubleValue { get; set; }
        }

        private class SetIntValue : IConfigureOptions<ValidationTestOptions>
        {
            public void Configure(ValidationTestOptions options)
            {
                options.IntValue = 3;
            }
        }

        private class SetStringValue : IConfigureOptions<ValidationTestOptions>
        {
            public void Configure(ValidationTestOptions options)
            {
                options.StringValue = "TestValue";
            }
        }

        private class SetDoubleValue : IConfigureOptions<ValidationTestOptions>
        {
            public void Configure(ValidationTestOptions options)
            {
                options.DoubleValue = 1.23;
            }
        }

        [Fact]
        public void ValidatorDetectsInvalidConfigurationAtStartup()
        {
            var services = new ServiceCollection()
                .AddOptions()
                .AddOptionsValidation();

            services.AddSingleton<IConfigureOptions<ValidationTestOptions>>(new SetIntValue());

            services.Validate<ValidationTestOptions>(o => o.IntValue > 5);
            services.Validate<ValidationTestOptions>(o => !string.IsNullOrEmpty(o.StringValue));
            services.Validate<ValidationTestOptions>(o => o.DoubleValue.HasValue);

            var sp = services.BuildServiceProvider();

            var validationManager = sp.GetRequiredService<IOptionsValidatorManager>();
            Assert.NotNull(validationManager);

            var ex = Record.Exception(() => validationManager.Validate());
            Assert.IsAssignableFrom<Exception>(ex);
        }

        [Fact]
        public void ValidatorPassesValidConfigurationAtStartup()
        {
            var services = new ServiceCollection()
                .AddOptions()
                .AddOptionsValidation();

            services.AddSingleton<IConfigureOptions<ValidationTestOptions>>(new SetIntValue());
            services.AddSingleton<IConfigureOptions<ValidationTestOptions>>(new SetStringValue());
            services.AddSingleton<IConfigureOptions<ValidationTestOptions>>(new SetDoubleValue());

            services.Validate<ValidationTestOptions>(o => o.IntValue > 2);
            services.Validate<ValidationTestOptions>(o => !string.IsNullOrEmpty(o.StringValue));
            services.Validate<ValidationTestOptions>(o => o.DoubleValue.HasValue);

            var sp = services.BuildServiceProvider();

            var validationManager = sp.GetRequiredService<IOptionsValidatorManager>();
            Assert.NotNull(validationManager);

            var ex = Record.Exception(() => validationManager.Validate());

            Assert.Null(ex);
        }

        private class ComplexValidator : IValidateOptions<ValidationTestOptions>
        {
            public bool Validate(ValidationTestOptions options)
            {
                if (options.IntValue < 5)
                {
                    return false;
                }

                if (!options.DoubleValue.HasValue)
                {
                    return false;
                }

                return true;
            }

            public string ErrorMessage => "ComplexValidator returned false";
        }

        [Fact]
        public void ComplexValidatorDetectsInvalidConfigurationAtStartup()
        {
            var services = new ServiceCollection()
                .AddOptions()
                .AddOptionsValidation();

            services.AddSingleton<IConfigureOptions<ValidationTestOptions>>(new SetIntValue());

            services.Validate<ValidationTestOptions, ComplexValidator>();

            var expectedErrorMessage = new StringBuilder()
                .AppendLine($"{typeof(ValidationTestOptions)} object is invalid:")
                .AppendLine("ComplexValidator returned false")
                .ToString();

            var sp = services.BuildServiceProvider();

            var validationManager = sp.GetRequiredService<IOptionsValidatorManager>();
            Assert.NotNull(validationManager);

            var ex = Record.Exception(() => validationManager.Validate());

            Assert.IsAssignableFrom<Exception>(ex);
            Assert.Equal(expectedErrorMessage, ex.Message);
        }
    }
}