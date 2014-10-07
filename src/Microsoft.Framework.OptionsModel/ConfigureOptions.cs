// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Framework.OptionsModel
{
    public class ConfigureOptions<TOptions> : IConfigureOptions<TOptions>
    {
        public ConfigureOptions([NotNull]Action<TOptions> action)
        {
            Action = action;
        }

        public Action<TOptions> Action { get; private set; }

        public string TargetOptionsName { get; set; } = "";
        public virtual int Order { get; set; } = OptionsConstants.DefaultOrder;

        public virtual void Configure(string optionsName, [NotNull]TOptions options)
        {
            // Apply any unnamed setup actions or if the options name matches
            if (string.IsNullOrEmpty(TargetOptionsName) || 
                string.Equals(optionsName, TargetOptionsName, StringComparison.OrdinalIgnoreCase))
            {
                Action.Invoke(options);
            }
        }
    }
}