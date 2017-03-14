// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options.Factory
{
    /// <summary>
    /// Used to create configured TOptions instances.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface IOptionsFactory<out TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Creates TOptions instance.
        /// </summary>
        TOptions Get();
    }
}