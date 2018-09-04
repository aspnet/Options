// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used to store the validation actions which the validator will run to validate options instances.
    /// </summary>
    public class OptionsValidatorOptions
    {
        /// <summary>
        /// List of actions that the validator will use to validate options instances.
        /// </summary>
        public IList<Action> Actions { get; set; } = new List<Action>();
    }
}
