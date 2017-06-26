// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Linq.Expressions;

namespace Microsoft.Extensions.Options.Validation.ValidateOptions
{
    internal class ExpressionFuncValidateOptions<TOptions> : FuncValidateOptions<TOptions> where TOptions : class
    {
        public ExpressionFuncValidateOptions(string name, Expression<Func<TOptions, bool>> validateExpression, ValidationStatus validationStatus) :
            base(name, validateExpression.Compile(), validationStatus, $"{validateExpression.Body} returned false")
        {
        }
    }
}