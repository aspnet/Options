// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Microsoft.Extensions.Options.Validation
{
    /// <summary> 
    /// Base class to be inherited in order to provide custom validation logic. 
    /// </summary> 
    /// <typeparam name="TOptions">The type of options being validated.</typeparam> 
    public abstract class ValidateOptions<TOptions> : ValidationBase<TOptions>, IValidateOptions<TOptions>
        where TOptions : class
    {
        protected readonly ValidationStatus ValidationStatus;
        protected readonly string ViolationMessage;
        protected readonly string Name;

        protected ValidateOptions(string name) : this(name, ValidationStatus.Invalid, null)
        {
        }

        protected ValidateOptions(string name, ValidationStatus validationStatus) : this(name, validationStatus, null)
        {
        }

        protected ValidateOptions(string name, ValidationStatus validationStatus, string violationMessage)
        {
            ValidationStatus = validationStatus;
            ViolationMessage = violationMessage;
            Name = name;
        }

        /// <summary> 
        /// Invoked to validate a TOptions instance. 
        /// </summary> 
        /// <param name="name">The options name.</param>
        /// <param name="options">The options instance to validate.</param> 
        public IValidationResult Validate(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Null name is used to validate all named options.
            if (Name == null || name == Name)
            {                
                return ValidateCore(options);
            }

            return new NotApplicableRuleValidationResult(Name, name);
        }

        protected abstract IValidationResult ValidateCore(TOptions options);

        protected new IValidationResult Invalid() => Result(ValidationStatus.Invalid, ViolationMessage);

        protected new IValidationResult Valid() => Result(ValidationStatus.Valid, ViolationMessage);

        protected new IValidationResult Warning() => Result(ValidationStatus.Warning, ViolationMessage);

        private class NotApplicableRuleValidationResult : IValidationResult
        {
            private readonly string _ruleName;
            private readonly string _optionsName;

            public NotApplicableRuleValidationResult(string ruleName, string optionsName)
            {
                _ruleName = ruleName;
                _optionsName = optionsName;
            }

            public ValidationStatus Status => ValidationStatus.Valid;

            public string Message => $"Validation rule with name '{_ruleName}' is not applicable for option with name '{_optionsName}'";
        }
    }
}