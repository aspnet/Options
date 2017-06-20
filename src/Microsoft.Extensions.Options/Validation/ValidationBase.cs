﻿// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.Options.Validation
{
    /// <summary> 
    /// Base class that contains helper methods. 
    /// </summary> 
    /// <typeparam name="TOptions">The type of options being requested.</typeparam> 
    public abstract class ValidationBase<TOptions>
    {
        protected static IValidationResult Result(ValidationStatus validationStatus, string message) => new ValidationResult(validationStatus, message);

        protected static IValidationResult Exception(Exception exception) => Result(ValidationStatus.Invalid, exception.Message);

        protected static IValidationResult Invalid() => Result(ValidationStatus.Invalid, null);

        protected static IValidationResult Invalid(string message) => Result(ValidationStatus.Invalid, message);

        protected static IValidationResult Valid() => Result(ValidationStatus.Valid, null);

        protected static IValidationResult Valid(string message) => Result(ValidationStatus.Valid, message);

        protected static IValidationResult Warning() => Result(ValidationStatus.Warning, null);

        protected static IValidationResult Warning(string message) => Result(ValidationStatus.Warning, message);

        protected static IValidationResult Aggregate(IEnumerable<IValidationResult> validationResults) => new AggregateValidationResult(validationResults);

        private class ValidationResult : IValidationResult
        {
            public ValidationResult(ValidationStatus validationStatus, string message)
            {
                Status = validationStatus;
                Message = message;
            }

            public ValidationStatus Status { get; }

            public string Message { get; }
        }

        private class AggregateValidationResult : IValidationResult
        {
            private readonly IEnumerable<IValidationResult> _validationResults;

            public AggregateValidationResult(IEnumerable<IValidationResult> validationResults)
            {
                _validationResults = validationResults;
            }

            public ValidationStatus Status
            {
                get
                {
                    if (_validationResults.Any(vr => vr.Status == ValidationStatus.Invalid))
                    {
                        return ValidationStatus.Invalid;
                    }

                    if (_validationResults.Any(vr => vr.Status == ValidationStatus.Warning))
                    {
                        return ValidationStatus.Warning;
                    }

                    return ValidationStatus.Valid;
                }
            }

            public string Message
            {
                get
                {
                    var status = Status;

                    if (status == ValidationStatus.Valid)
                    {
                        return null;
                    }

                    var message = new StringBuilder();

                    if (status == ValidationStatus.Invalid)
                    {
                        message.AppendLine($"{typeof(TOptions).Name} object is invalid:");
                    }

                    if (status == ValidationStatus.Warning)
                    {
                        message.AppendLine($"{typeof(TOptions).Name} object has warnings:");
                    }

                    foreach (var validationResult in _validationResults)
                    {
                        message.AppendLine(validationResult.Message);
                    }

                    return message.ToString();
                }
            }
        }
    }
}