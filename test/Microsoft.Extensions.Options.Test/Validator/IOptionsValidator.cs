// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used to explicitly validate options, i.e. on startup.
    /// </summary>
    public interface IOptionsValidator
    {
        /// <summary>
        /// Should validate all options that require startup validation.
        /// </summary>
        void Validate();
    }
}
