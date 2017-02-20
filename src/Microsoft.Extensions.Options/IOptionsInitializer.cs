// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used to initialize TOptions instances.
    /// </summary>
    public interface IOptionsInitializer
    {
        /// <summary>
        /// Initializes a new options instance.
        /// </summary>
        void InitializeOptions(bool reinitialize = true);
    }

    /// <summary>
    /// Used to retrieve initialized TOptions instances.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface IOptionsInitializer<out TOptions> : IOptionsInitializer
        where TOptions : class, new()
    {
        /// <summary>
        /// The initialized options instance.
        /// </summary>
        TOptions Options { get; }
    }
}