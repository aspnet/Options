﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{

    /// <summary>
    /// Implementation of IValidateNamedOptions.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public class ValidateNamedOptions<TOptions> : IValidateNamedOptions<TOptions> where TOptions : class
    {
        public ValidateNamedOptions() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="action">The action to register.</param>
        public ValidateNamedOptions(string name, Action<TOptions> action)
        {
            Name = name;
            Action = action;
        }

        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions> Action { get; set; }

        /// <summary>
        /// Invokes the registered validate Action if the name matches.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public virtual void Validate(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Null name is used to configure all named options.
            if (Name == null || name == Name)
            {
                Action?.Invoke(options);
            }
        }
    }
}