// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Framework.ConfigurationModel;

namespace Microsoft.Framework.OptionsModel
{
    public class ConfigOptionsSetup<TOptions>(
        IConfiguration config,
        int order = OptionsConstants.ConfigurationOrder,
        string name = null)
        : IOptionsSetup<TOptions>
    {
        private IConfiguration Config { get; } = config;

        public int Order { get; set; } = order;

        public string Name { get; set; } = name;

        public virtual void Setup(TOptions options)
        {
            OptionsServices.ReadProperties(options, Config);
        }
    }
}