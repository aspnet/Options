// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Microsoft.Extensions.Options.Validation
{
    internal static class ValidationResultHandlerFactory
    {
        public static Func<IValidationResult, bool> GetHandleCondition(ValidationLevel validationLevel)
        {
            switch (validationLevel)
            {
                case ValidationLevel.Warning:
                    return vr => vr.Status == ValidationStatus.Invalid || vr.Status == ValidationStatus.Warning;
                case ValidationLevel.Invalid:
                    return vr => vr.Status == ValidationStatus.Invalid;
                case ValidationLevel.None:
                    return vr => false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(validationLevel), validationLevel, null);
            }
        }
    }
}