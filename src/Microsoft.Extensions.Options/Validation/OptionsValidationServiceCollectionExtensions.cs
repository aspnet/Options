// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Options.Validation;
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
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection AddOptionsValidation(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IOptionsValidatorManager, OptionsValidatorManager>();

            return services;
        }

        /// <summary> 
        /// Configures <see cref="Action{T}"/> to be used for all TOptions instances validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateAction">The <see cref="Action{T}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <param name="violationMessage">The message that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection ValidateAll<TOptions>(this IServiceCollection services, Action<TOptions> validateAction, ValidationStatus validationStatus = ValidationStatus.Invalid, string violationMessage = null)
            where TOptions : class, new()
            => services.Validate(null, new ActionValidateOptions<TOptions>(null, validateAction, validationStatus, violationMessage));

        /// <summary> 
        /// Configures <see cref="Action{T}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateAction">The <see cref="Action{T}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <param name="violationMessage">The message that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Action<TOptions> validateAction, ValidationStatus validationStatus = ValidationStatus.Invalid, string violationMessage = null)
            where TOptions : class, new()
            => services.Validate(Options.Options.DefaultName, new ActionValidateOptions<TOptions>(Options.Options.DefaultName, validateAction, validationStatus, violationMessage));

        /// <summary> 
        /// Configures <see cref="Action{T}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="name">The name of the options instance.</param>
        /// <param name="validateAction">The <see cref="Action{T}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <param name="violationMessage">The message that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, string name, Action<TOptions> validateAction, ValidationStatus validationStatus = ValidationStatus.Invalid, string violationMessage = null)
            where TOptions : class, new()
            => services.Validate(name, new ActionValidateOptions<TOptions>(name, validateAction, validationStatus, violationMessage));

        /// <summary> 
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <param name="violationMessage">The message that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection ValidateAll<TOptions>(this IServiceCollection services, Func<TOptions, bool> validateFunc, ValidationStatus validationStatus = ValidationStatus.Invalid, string violationMessage = null)
            where TOptions : class, new()
            => services.Validate(null, new FuncValidateOptions<TOptions>(null, validateFunc, validationStatus, violationMessage));

        /// <summary> 
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <param name="violationMessage">The message that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Func<TOptions, bool> validateFunc, ValidationStatus validationStatus = ValidationStatus.Invalid, string violationMessage = null)
            where TOptions : class, new()
            => services.Validate(Options.Options.DefaultName, new FuncValidateOptions<TOptions>(Options.Options.DefaultName, validateFunc, validationStatus, violationMessage));

        /// <summary> 
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="name">The name of the options instance.</param>
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <param name="violationMessage">The message that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, string name, Func<TOptions, bool> validateFunc, ValidationStatus validationStatus = ValidationStatus.Invalid, string violationMessage = null)
            where TOptions : class, new()
            => services.Validate(name, new FuncValidateOptions<TOptions>(name, validateFunc, validationStatus, violationMessage));

        /// <summary> 
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection ValidateAll<TOptions>(this IServiceCollection services, Func<TOptions, Exception> validateFunc)
            where TOptions : class, new()
            => services.Validate(null, new FuncExceptionValidateOptions<TOptions>(null, validateFunc, ValidationStatus.Invalid, null));

        /// <summary> 
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Func<TOptions, Exception> validateFunc)
            where TOptions : class, new()
            => services.Validate(Options.Options.DefaultName, new FuncExceptionValidateOptions<TOptions>(Options.Options.DefaultName, validateFunc, ValidationStatus.Invalid, null));

        /// <summary> 
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="name">The name of the options instance.</param>
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, string name, Func<TOptions, Exception> validateFunc)
            where TOptions : class, new()
            => services.Validate(name, new FuncExceptionValidateOptions<TOptions>(name, validateFunc, ValidationStatus.Invalid, null));

        /// <summary> 
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection ValidateAll<TOptions>(this IServiceCollection services, Func<TOptions, IEnumerable<Exception>> validateFunc)
            where TOptions : class, new()
            => services.Validate(null, new FuncExceptionValidateOptions<TOptions>(null, validateFunc, ValidationStatus.Invalid, null));

        /// <summary> 
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, Func<TOptions, IEnumerable<Exception>> validateFunc)
            where TOptions : class, new()
            => services.Validate(Options.Options.DefaultName, new FuncExceptionValidateOptions<TOptions>(Options.Options.DefaultName, validateFunc, ValidationStatus.Invalid, null));

        /// <summary> 
        /// Configures <see cref="Func{T, TResult}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="name">The name of the options instance.</param>
        /// <param name="validateFunc">The <see cref="Func{T, TResult}"/> that is used for validation.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, string name, Func<TOptions, IEnumerable<Exception>> validateFunc)
            where TOptions : class, new()
            => services.Validate(name, new FuncExceptionValidateOptions<TOptions>(name, validateFunc, ValidationStatus.Invalid, null));

        /// <summary> 
        /// Configures <see cref="Expression{TDelegate}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateExpression">The <see cref="Expression{TDelegate}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection ValidateExprAll<TOptions>(this IServiceCollection services, Expression<Action<TOptions>> validateExpression, ValidationStatus validationStatus = ValidationStatus.Invalid)
            where TOptions : class, new()
            => services.Validate(null, new ExpressionActionValidateOptions<TOptions>(null, validateExpression, validationStatus));

        /// <summary> 
        /// Configures <see cref="Expression{TDelegate}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateExpression">The <see cref="Expression{TDelegate}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection ValidateExpr<TOptions>(this IServiceCollection services, Expression<Action<TOptions>> validateExpression, ValidationStatus validationStatus = ValidationStatus.Invalid)
            where TOptions : class, new()
            => services.Validate(Options.Options.DefaultName, new ExpressionActionValidateOptions<TOptions>(Options.Options.DefaultName, validateExpression, validationStatus));

        /// <summary> 
        /// Configures <see cref="Expression{TDelegate}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="name">The name of the options instance.</param>
        /// <param name="validateExpression">The <see cref="Expression{TDelegate}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection ValidateExpr<TOptions>(this IServiceCollection services, string name, Expression<Action<TOptions>> validateExpression, ValidationStatus validationStatus = ValidationStatus.Invalid)
            where TOptions : class, new()
            => services.Validate(name, new ExpressionActionValidateOptions<TOptions>(name, validateExpression, validationStatus));

        /// <summary> 
        /// Configures <see cref="Expression{TDelegate}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateExpression">The <see cref="Expression{TDelegate}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection ValidateExprAll<TOptions>(this IServiceCollection services, Expression<Func<TOptions, bool>> validateExpression, ValidationStatus validationStatus = ValidationStatus.Invalid)
            where TOptions : class, new()
            => services.Validate(null, new ExpressionFuncValidateOptions<TOptions>(null, validateExpression, validationStatus));

        /// <summary> 
        /// Configures <see cref="Expression{TDelegate}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="validateExpression">The <see cref="Expression{TDelegate}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection ValidateExpr<TOptions>(this IServiceCollection services, Expression<Func<TOptions, bool>> validateExpression, ValidationStatus validationStatus = ValidationStatus.Invalid)
            where TOptions : class, new()
            => services.Validate(Options.Options.DefaultName, new ExpressionFuncValidateOptions<TOptions>(Options.Options.DefaultName, validateExpression, validationStatus));

        /// <summary> 
        /// Configures <see cref="Expression{TDelegate}"/> to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="name">The name of the options instance.</param>
        /// <param name="validateExpression">The <see cref="Expression{TDelegate}"/> that is used for validation.</param> 
        /// <param name="validationStatus">The <see cref="ValidationStatus"/> that will be used to return <see cref="IValidationResult"/> instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection ValidateExpr<TOptions>(this IServiceCollection services, string name, Expression<Func<TOptions, bool>> validateExpression, ValidationStatus validationStatus = ValidationStatus.Invalid)
            where TOptions : class, new()
            => services.Validate(name, new ExpressionFuncValidateOptions<TOptions>(name, validateExpression, validationStatus));

        /// <summary> 
        /// Configures <see cref="IValidateOptions{TOptions}"/> class to be used for TOptions instance validation. 
        /// </summary> 
        /// <typeparam name="TOptions">The type of options being configured.</typeparam> 
        /// <typeparam name="TValidateOptions">The type of validate options being used for validation.</typeparam> 
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param> 
        /// <param name="name">The name of the options instance.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection Validate<TOptions, TValidateOptions>(this IServiceCollection services, string name)
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
        /// <param name="name">The name of the options instance.</param> 
        /// <param name="validateOptions">The instance of <see cref="IValidateOptions{TOptions}"/> being used for validation.</param> 
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns> 
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, string name, IValidateOptions<TOptions> validateOptions)
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
            if (!services.Any(s => s.ServiceType == typeof(IOptionsValidator) && s.ImplementationType == typeof(OptionsValidator<TOptions>)))
            {
                services.AddSingleton<IOptionsValidator, OptionsValidator<TOptions>>();
                services.AddSingleton<IOptionsValidator<TOptions>, InnerOptionsValidator<TOptions>>();
            }
        }
    }
}