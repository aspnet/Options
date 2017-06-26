// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Microsoft.Extensions.Options.Validation
{
    /// <summary> 
    /// Used to be returned as a result of validation. 
    /// </summary> 
    public interface IValidationResult
    {
        /// <summary> 
        /// The <see cref="ValidationStatus"/>. 
        /// </summary> 
        ValidationStatus Status { get; }

        /// <summary> 
        /// The validation result message. 
        /// </summary> 
        string Message { get; }
    }
}