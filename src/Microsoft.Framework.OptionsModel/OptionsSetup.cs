// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Framework.OptionsModel
{
    public class OptionsSetup<TOptions>([NotNull]Action<TOptions> setupAction) : IOptionsSetup<TOptions>
    {
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException("setupAction");
            }
        }

        public virtual Action<TOptions> SetupAction { get; } = setupAction;

        public virtual string OptionsName { get; set; } 

        public virtual int Order { get; set; } = OptionsConstants.DefaultOrder;

        public virtual void Setup(string optionsName, [NotNull]TOptions options)
        {
            // Apply setup action only if the name matches (null name is the default setup)
            if (string.Equals(optionsName, OptionsName, StringComparison.OrdinalIgnoreCase))
            {
                SetupAction(options);
            }
        }
    }
}