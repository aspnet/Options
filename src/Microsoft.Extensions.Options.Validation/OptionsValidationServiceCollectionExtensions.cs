// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options.Factory;
using Microsoft.Extensions.Options.Validation;
using Microsoft.Extensions.Options.Validation.Factory;
using Microsoft.Extensions.Options.Validation.ValidateOptions;

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
        /// <param name="validationLevel"> The <see cref="ValidationLevel"/> to specify exception throwing level.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddOptionsValidation(this IServiceCollection services, ValidationLevel validationLevel = ValidationLevel.None)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IOptionsValidatorManager, OptionsValidatorManager>();

            if (validationLevel != ValidationLevel.None)
            {
                services.Replace(ServiceDescriptor.Singleton(typeof(IOptionsFactory<>), typeof(OptionsValidationFactory<>)));
                services.TryAddSingleton(ValidationResultHandlerFactory.Get(validationLevel));
            }

            return services;
        }

        /// <summary>
        /// Configures <see cref="Action{T}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateAction">The <see cref="Action{T}"/> that is used for validation.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Action<TOptions> validateAction)
            where TOptions : class, new()
        {
            services.Validate(new ActionValidateOptions<TOptions>(validateAction, ValidationStatus.Invalid, null));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Action{T}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateAction">The <see cref="Action{T}"/> that is used for validation.</param>
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Action<TOptions> validateAction, ValidationStatus validationStatus)
            where TOptions : class, new()
        {
            services.Validate(new ActionValidateOptions<TOptions>(validateAction, validationStatus, null));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Action{T}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateAction">The <see cref="Action{T}"/> that is used for validation.</param>
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param>
        /// <param name="violationMessage">The message that will be used to return <see cref="IValidationResult"/> instance.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Action<TOptions> validateAction, ValidationStatus validationStatus, string violationMessage)
            where TOptions : class, new()
        {
            services.Validate(new ActionValidateOptions<TOptions>(validateAction, validationStatus, violationMessage));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Func<TOptions, bool> validateFunc)
            where TOptions : class, new()
        {
            services.Validate(new FuncValidateOptions<TOptions>(validateFunc, ValidationStatus.Invalid, null));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param>
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Func<TOptions, bool> validateFunc, ValidationStatus validationStatus)
            where TOptions : class, new()
        {
            services.Validate(new FuncValidateOptions<TOptions>(validateFunc, validationStatus, null));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param>
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param>
        /// <param name="violationMessage">The message that will be used to return <see cref="IValidationResult"/> instance.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Func<TOptions, bool> validateFunc, ValidationStatus validationStatus, string violationMessage)
            where TOptions : class, new()
        {
            services.Validate(new FuncValidateOptions<TOptions>(validateFunc, validationStatus, violationMessage));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Func<TOptions, Exception> validateFunc)
            where TOptions : class, new()
        {
            services.Validate(new FuncExceptionValidateOptions<TOptions>(validateFunc, ValidationStatus.Invalid, null));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Func<TOptions, IEnumerable<Exception>> validateFunc)
            where TOptions : class, new()
        {
            services.Validate(new FuncExceptionValidateOptions<TOptions>(validateFunc, ValidationStatus.Invalid, null));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Expression{TDelegate}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateExpression">The <see cref="Expression{TDelegate}"/> that is used for validation.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection ValidateExpr<TOptions>(this IServiceCollection services, Expression<Action<TOptions>> validateExpression)
            where TOptions : class, new()
        {
            services.Validate(new ExpressionActionValidateOptions<TOptions>(validateExpression, ValidationStatus.Invalid));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Expression{TDelegate}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateExpression">The <see cref="Expression{TDelegate}"/> that is used for validation.</param>
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection ValidateExpr<TOptions>(this IServiceCollection services, Expression<Action<TOptions>> validateExpression, ValidationStatus validationStatus)
            where TOptions : class, new()
        {
            services.Validate(new ExpressionActionValidateOptions<TOptions>(validateExpression, validationStatus));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Expression{TDelegate}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateExpression">The <see cref="Expression{TDelegate}"/> that is used for validation.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection ValidateExpr<TOptions>(this IServiceCollection services, Expression<Func<TOptions, bool>> validateExpression)
            where TOptions : class, new()
        {
            services.Validate(new ExpressionFuncValidateOptions<TOptions>(validateExpression, ValidationStatus.Invalid));

            return services;
        }

        /// <summary>
        /// Configures <see cref="Expression{TDelegate}"/> to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateExpression">The <see cref="Expression{TDelegate}"/> that is used for validation.</param>
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection ValidateExpr<TOptions>(this IServiceCollection services, Expression<Func<TOptions, bool>> validateExpression, ValidationStatus validationStatus)
            where TOptions : class, new()
        {
            services.Validate(new ExpressionFuncValidateOptions<TOptions>(validateExpression, validationStatus));

            return services;
        }

        /// <summary>
        /// Configures <see cref="IValidateOptions{TOptions}"/> class to be used for TOptions instance validation.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <typeparam name="TValidateOptions">The type of validate options being used for validation.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
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

        /// <summary>
        /// Configures <see cref="IValidateOptions{TOptions}"/> class instance to be used for TOptions instance validation
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="validateOptions">The instance of <see cref="IValidateOptions{TOptions}"/> being used for validation.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
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

            services.AddSingleton(validateOptions);
            services.AddOptionsValidator<TOptions>();

            return services;
        }
        
        private static void AddOptionsValidator<TOptions>(this IServiceCollection services)
            where TOptions : class, new()
        {
            if(!services.Any(s => s.ServiceType == typeof(IOptionsValidator) && s.ImplementationType == typeof(OptionsValidator<TOptions>)))
            {
                services.AddSingleton<IOptionsValidator, OptionsValidator<TOptions>>();
                services.AddSingleton<IOptionsValidator<TOptions>, InnerOptionsValidator<TOptions>>();
            }
        }
    }
}