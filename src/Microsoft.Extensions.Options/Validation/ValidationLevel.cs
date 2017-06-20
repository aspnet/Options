// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Microsoft.Extensions.Options.Validation
{
    /// <summary> 
    /// Used to specify the validation level that will be used to throw an exception if there are any <see cref="IValidationResult"/> with the same level. 
    /// </summary> 
    public enum ValidationLevel
    {
        /// <summary> 
        /// Says that exception should be thrown of there are any <see cref="IValidationResult"/> with <see cref="ValidationStatus.Invalid"/> or <see cref="ValidationStatus.Warning"/> statuses. 
        /// </summary> 
        Warning = 0,

        /// <summary> 
        /// Says that exception should be thrown of there are any <see cref="IValidationResult"/> with <see cref="ValidationStatus.Invalid"/> status. 
        /// </summary> 
        Invalid = 1,

        /// <summary> 
        /// Says that exception should not be thrown at all (regardless of status). 
        /// </summary> 
        None = 2
    }
}