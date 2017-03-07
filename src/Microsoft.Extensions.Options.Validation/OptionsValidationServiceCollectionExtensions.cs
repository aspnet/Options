// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Options.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding options services to the DI container.
    /// </summary>
    public static class OptionsValidationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services required for using options validation.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddOptionsValidation(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IOptionsValidatorManager, OptionsValidatorManager>();

            return services;
        }

        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Expression<Func<TOptions, bool>> validateExpression)
            where TOptions : class, new()
        {
            services.Validate<TOptions>(new ValidateOptions<TOptions>(validateExpression));

            return services;
        }

        public static IServiceCollection Validate<TOptions, TValidateOptions>(this IServiceCollection services)
            where TOptions : class, new()
            where TValidateOptions : class, IValidateOptions<TOptions>
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IValidateOptions<TOptions>, TValidateOptions>();
            services.AddOptionsValidator<TOptions>();

            return services;
        }

        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, IValidateOptions<TOptions> validateOptions)
            where TOptions : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (validateOptions == null)
            {
                throw new ArgumentNullException(nameof(validateOptions));
            }

            services.AddSingleton<IValidateOptions<TOptions>>(validateOptions);
            services.AddOptionsValidator<TOptions>();

            return services;
        }
        
        private static void AddOptionsValidator<TOptions>(this IServiceCollection services)
            where TOptions : class, new()
        {
            if(!services.Any(s => s.ServiceType == typeof(IOptionsValidator) && s.ImplementationType == typeof(OptionsValidator<TOptions>)))
            {
                services.AddSingleton<IOptionsValidator, OptionsValidator<TOptions>>();
            }
        }
    }
}