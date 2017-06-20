// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Linq.Expressions;

namespace Microsoft.Extensions.Options.Validation.ValidateOptions
{
    internal class ExpressionActionValidateOptions<TOptions> : ActionValidateOptions<TOptions> where TOptions : class
    {
        public ExpressionActionValidateOptions(Expression<Action<TOptions>> validateExpression, ValidationStatus validationStatus) :
            base(validateExpression.Compile(), validationStatus, $"{validateExpression.Body} threw an exception")
        {
        }
    }
}