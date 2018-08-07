// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of <see cref="IValidateOptions{TOptions}"/>
    /// </summary>
    /// <typeparam name="TOptions">The instance being validated.</typeparam>
    public class DataAnnotationValidateOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        public DataAnnotationValidateOptions(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            // Null name is used to configure all named options.
            if (Name == null || name == Name)
            {
                var validationResults = new List<ValidationResult>();
                var vc = new ValidationContext(options, serviceProvider: null, items: null);
                var isValid = Validator.TryValidateObject(options, vc, validationResults, validateAllProperties: true);

                if (isValid)
                {
                    return ValidateOptionsResult.Success;
                }


                var errors = String.Join(" : ", validationResults.Select(r => r.ErrorMessage));
                return ValidateOptionsResult.Fail(errors);
            }

            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;
        }
    }
}