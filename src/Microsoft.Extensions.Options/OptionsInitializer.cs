// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptionsInitializer.
    /// Inherit to provider the custom implementation for IOptionsInitializer.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public abstract class OptionsInitializer<TOptions> : IOptionsInitializer<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Initializes a new options instance.
        /// </summary>
        public void InitializeOptions(bool reinitialize = true)
        {
            if (Options == null || reinitialize)
            {
                Options = new TOptions();

                InitializeOptionsCore(Options);
            }
        }

        /// <summary>
        /// Configures an instance of options.
        /// Used to provide the custom initialization.
        /// </summary>
        public abstract void InitializeOptionsCore(TOptions options);

        /// <summary>
        /// The initialized options instance.
        /// </summary>
        public TOptions Options { get; private set; }
    }
}