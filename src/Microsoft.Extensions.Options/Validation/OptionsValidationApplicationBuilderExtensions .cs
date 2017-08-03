// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options.Validation;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary> 
    /// Extension methods for adding options services to the DI container. 
    /// </summary> 
    public static class OptionsValidationApplicationBuilderExtensions
    {
        /// <summary> 
        /// Validated all configured Options. 
        /// </summary> 
        /// <param name="appBuilder">The <see cref="IApplicationBuilder"/>.</param> 
        /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns> 
        /// <exception cref="OptionsValidationException">Thrown when some Options instance is invalid.</exception> 
        /// <exception cref="Exception">Thrown when Options validation wasn't configured properly (<see cref="OptionsValidationServiceCollectionExtensions.AddOptionsValidation" /> has to be called).</exception> 
        public static IApplicationBuilder ValidateOptions(this IApplicationBuilder appBuilder)
        {
            if (appBuilder == null)
            {
                throw new ArgumentNullException(nameof(appBuilder));
            }

            var validatorManager = appBuilder.ApplicationServices.GetService<IOptionsValidatorManager>();

            if (validatorManager == null)
            {
                throw new Exception("Options validation wasn't configured properly");
            }

            validatorManager.Validate();

            return appBuilder;
        }
    }
}