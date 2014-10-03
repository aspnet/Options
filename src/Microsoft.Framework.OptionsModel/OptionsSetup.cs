// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Framework.OptionsModel
{
    public class OptionsSetup<TOptions> : IOptionsSetup<TOptions>
    {
        public OptionsSetup([NotNull]OptionsAction<TOptions> optionsAction)
        {
            if (optionsAction == null)
            {
                throw new ArgumentNullException(nameof(optionsAction));
            }
            OptionsAction = optionsAction;
        }

        public OptionsAction<TOptions> OptionsAction { get; private set; }

        public virtual int Order { get { return OptionsAction.Order; } }

        public virtual void Setup(string optionsName, [NotNull]TOptions options)
        {
            // Apply any unnamed setup actions or if the options name matches
            if (string.IsNullOrEmpty(OptionsAction.Name) || 
                string.Equals(optionsName, OptionsAction.Name, StringComparison.OrdinalIgnoreCase))
            {
                OptionsAction.Invoke(options);
            }
        }
    }
}