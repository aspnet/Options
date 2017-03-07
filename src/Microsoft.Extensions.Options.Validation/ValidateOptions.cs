// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;

namespace Microsoft.Extensions.Options.Validation
{
    internal class ValidateOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        private readonly Expression<Func<TOptions, bool>> _validateExpression;

        public ValidateOptions(Expression<Func<TOptions, bool>> validateExpression)
        {
            _validateExpression = validateExpression;
        }

        public bool Validate(TOptions options)
        {
            return _validateExpression.Compile().Invoke(options);
        }

        public string ErrorMessage => $"{_validateExpression.Body} returned false.";
    }
}