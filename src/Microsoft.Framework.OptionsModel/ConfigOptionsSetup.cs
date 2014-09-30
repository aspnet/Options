// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Framework.ConfigurationModel;
using System;

namespace Microsoft.Framework.OptionsModel
{
    public class ConfigOptionsSetup<TOptions>(
        IConfiguration config,
        int order = OptionsConstants.ConfigurationOrder,
        string optionsName = "")
        : IOptionsSetup<TOptions>
    {
        private IConfiguration Config { get; } = config;

        public int Order { get; set; } = order;

        public string OptionsName { get; set; } = optionsName;

        public virtual void Setup(string optionsName, TOptions options)
        {
            // Apply config settings only if the name matches (null name is the default setup)
            if (string.Equals(optionsName, OptionsName, StringComparison.OrdinalIgnoreCase))
            {
                OptionsServices.ReadProperties(options, Config);
            }
        }
    }
}