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

        private class ComplexValidator : ValidateOptions<ValidationTestOptions>
        {
            public ComplexValidator() : base(ValidationStatus.Invalid, "ComplexValidator said that object is invalid")
            {
            }

            protected override IValidationResult ValidateCore(ValidationTestOptions options)
            {
                if (options.IntValue < 5)
                {
                    return Invalid();
                }

                if (!options.DoubleValue.HasValue)
                {
                    return Invalid();
                }

                return Valid();
            }
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
                .AppendLine($"{typeof(ValidationTestOptions).Name} object is invalid:")
                .AppendLine("ComplexValidator said that object is invalid")
                .ToString();

            var sp = services.BuildServiceProvider();

            var validationManager = sp.GetRequiredService<IOptionsValidatorManager>();
            Assert.NotNull(validationManager);

            var ex = Record.Exception(() => validationManager.Validate());

            Assert.IsAssignableFrom<Exception>(ex);
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public void UseValidationAtOptionsCreation()
        {
            var services = new ServiceCollection()
                .AddOptions()
                .AddOptionsValidation(ValidationLevel.Invalid);

            services.AddSingleton<IConfigureOptions<ValidationTestOptions>>(new SetIntValue());

            services.Validate<ValidationTestOptions, ComplexValidator>();

            var expectedErrorMessage = new StringBuilder()
                .AppendLine($"{typeof(ValidationTestOptions).Name} object is invalid:")
                .AppendLine("ComplexValidator said that object is invalid")
                .ToString();

            var sp = services.BuildServiceProvider();

            var options = sp.GetRequiredService<IOptions<ValidationTestOptions>>();
            Assert.NotNull(options);

            var ex = Record.Exception(() => options.Value);

            Assert.IsAssignableFrom<Exception>(ex);
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public void ValidatorThrowsExceptionOnWarningLevelAtStartupButNotOnOptionsCreation()
        {
            var services = new ServiceCollection()
                .AddOptions()
                .AddOptionsValidation();

            services.AddSingleton<IConfigureOptions<ValidationTestOptions>>(new SetIntValue());
            services.AddSingleton<IConfigureOptions<ValidationTestOptions>>(new SetStringValue());
            services.AddSingleton<IConfigureOptions<ValidationTestOptions>>(new SetDoubleValue());

            services.Validate<ValidationTestOptions>(o => o.IntValue > 5, ValidationStatus.Warning, "IntValue is more than 5");
            services.Validate<ValidationTestOptions>(o => !string.IsNullOrEmpty(o.StringValue), ValidationStatus.Warning, "StringValue isn't null or empty");
            services.Validate<ValidationTestOptions>(o => o.DoubleValue.HasValue, ValidationStatus.Warning, "DoubleValue has value");

            var sp = services.BuildServiceProvider();

            var validationManager = sp.GetRequiredService<IOptionsValidatorManager>();
            Assert.NotNull(validationManager);

            var expectedErrorMessage = new StringBuilder()
                .AppendLine($"{typeof(ValidationTestOptions).Name} object has warnings:")
                .AppendLine("IntValue is more than 5")
                .AppendLine("StringValue isn't null or empty")
                .AppendLine("DoubleValue has value")
                .ToString();

            var ex1 = Record.Exception(() => validationManager.Validate(ValidationLevel.Warning));
            Assert.IsAssignableFrom<Exception>(ex1);
            Assert.Equal(expectedErrorMessage, ex1.Message);

            var options = sp.GetRequiredService<IOptions<ValidationTestOptions>>();
            Assert.NotNull(options);

            var ex2 = Record.Exception(() => options.Value);
            Assert.Null(ex2);
        }
    }
}