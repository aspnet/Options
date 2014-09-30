// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Framework.ConfigurationModel;
using System;

namespace Microsoft.Framework.OptionsModel
{
    public class ConfigOptionsSetup<TOptions> : IOptionsSetup<TOptions>
    {
        public ConfigOptionsSetup(IConfiguration config,
            int order = OptionsConstants.ConfigurationOrder,
            string optionsName = "")
        {
            Config = config;
            Order = order;
            OptionsName = optionsName;
        }


        private IConfiguration Config { get; set; }

        public int Order { get; set; }

        public string OptionsName { get; set; }

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