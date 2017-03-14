// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options.Factory
{
    /// <summary>
    /// Used to create configured TOptions instances and keep until it reset.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface ICachedOptionsFactory<out TOptions> : IOptionsFactory<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Reset cached TOptions instance.
        /// </summary>
        void ResetCache();
    }
}