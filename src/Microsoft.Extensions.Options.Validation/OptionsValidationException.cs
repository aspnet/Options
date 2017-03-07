// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.Options.Validation
{
    /// <summary>
    /// Specific Exception class to catch validation exception.
    /// </summary>
    public class OptionsValidationException : Exception
    {
        internal OptionsValidationException(IEnumerable<ValidationResult> validationResults)
            : base (validationResults.Aggregate(new StringBuilder(), (sb, vr) => sb.Append(vr.ErrorMessage)).ToString())
        {
        }
    }
}