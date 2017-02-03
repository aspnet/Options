// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used for notifications when TOptions instances change.
    /// </summary>
    /// <typeparam name="TOptions">The options type.</typeparam>
    public interface IOptionsMonitor<out TOptions>
    {
        /// <summary>
        /// Returns the current TOptions instance.
        /// </summary>
        TOptions CurrentValue { get; }

        /// <summary>
        /// Returns the current TOptions instance for the name.
        /// </summary>
        /// <param name="name">The name of the configured options.</param>
        /// <returns>The configured instance for the name.</returns>
        TOptions GetNamedCurrentValue(string name);

        /// <summary>
        /// Registers a listener to be called whenever TOptions changes.
        /// </summary>
        /// <param name="listener">The action to be invoked when TOptions has changed.</param>
        /// <returns>An IDisposable which should be disposed to stop listening for changes.</returns>
        IDisposable OnChange(Action<TOptions> listener);
    }
}