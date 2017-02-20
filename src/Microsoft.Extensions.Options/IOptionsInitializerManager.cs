// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used to initialize all configured with initializers options instances.
    /// </summary>
    public interface IOptionsInitializerManager
    {
        /// <summary>
        /// Initializes all configured with initializers options instances.
        /// </summary>
        void Initialize();
    }
}