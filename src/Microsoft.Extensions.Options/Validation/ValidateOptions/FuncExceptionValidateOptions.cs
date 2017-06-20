// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.Options.Validation.ValidateOptions
{
    internal class FuncExceptionValidateOptions<TOptions> : ValidateOptions<TOptions> where TOptions : class
    {
        private readonly Func<TOptions, IEnumerable<Exception>> _validateFunc;

        public FuncExceptionValidateOptions(Func<TOptions, Exception> validateFunc, ValidationStatus validationStatus, string violationMessage)
            : this(Cast(validateFunc), validationStatus, violationMessage)
        {
        }

        public FuncExceptionValidateOptions(Func<TOptions, IEnumerable<Exception>> validateFunc, ValidationStatus validationStatus, string violationMessage) : base(validationStatus, violationMessage)
        {
            _validateFunc = validateFunc;
        }

        protected override IValidationResult ValidateCore(TOptions options) => Aggregate(_validateFunc(options).Where(ve => ve != null).Select(Exception));

        private static Func<TOptions, IEnumerable<Exception>> Cast(Func<TOptions, Exception> validateFunc) => options => new List<Exception> { validateFunc(options) };
    }
}