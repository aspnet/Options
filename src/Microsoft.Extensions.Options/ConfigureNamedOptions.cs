// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IConfigureNamedOptions.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class ConfigureNamedOptions<TOptions> : IConfigureNamedOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, Action<TOptions> action)
        {
            Name = name;
            Action = action;
        }

        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions> Action { get; }

        /// <summary>
        /// Invokes the registered configure Action if the name matches.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public virtual void Configure(string name, TOptions options)
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

        public void Configure(TOptions options) => Configure(Options.DefaultName, options);
    }

    /// <summary>
    /// Implementation of IConfigureOptions.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="TDep"></typeparam>
    public class ConfigureNamedOptions<TOptions, TDep> : IConfigureNamedOptions<TOptions>
        where TOptions : class
        where TDep : class
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="dependency">A dependency.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, TDep dependency, Action<TOptions, TDep> action)
        {
            Name = name;
            Action = action;
            Dependency = dependency;
        }

        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions, TDep> Action { get; }

        public TDep Dependency { get; }

        public virtual void Configure(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Null name is used to configure all named options.
            if (Name == null || name == Name)
            {
                Action?.Invoke(options, Dependency);
            }
        }

        public void Configure(TOptions options) => Configure(Options.DefaultName, options);
    }

    /// <summary>
    /// Implementation of IConfigureOptions.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="TDep"></typeparam>
    /// <typeparam name="TDep2"></typeparam>
    public class ConfigureNamedOptions<TOptions, TDep, TDep2> : IConfigureNamedOptions<TOptions>
        where TOptions : class
        where TDep : class
        where TDep2 : class
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="dependency">A dependency.</param>
        /// <param name="dependency2">A second dependency.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, TDep dependency, TDep2 dependency2, Action<TOptions, TDep, TDep2> action)
        {
            Name = name;
            Action = action;
            Dependency = dependency;
            Dependency2 = dependency2;
        }

        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions, TDep, TDep2> Action { get; }

        public TDep Dependency { get; }

        public TDep2 Dependency2 { get; }

        public virtual void Configure(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Null name is used to configure all named options.
            if (Name == null || name == Name)
            {
                Action?.Invoke(options, Dependency, Dependency2);
            }
        }

        public void Configure(TOptions options) => Configure(Options.DefaultName, options);
    }

    /// <summary>
    /// Implementation of IConfigureOptions.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="TDep"></typeparam>
    /// <typeparam name="TDep2"></typeparam>
    /// <typeparam name="TDep3"></typeparam>
    public class ConfigureNamedOptions<TOptions, TDep, TDep2, TDep3> : IConfigureNamedOptions<TOptions>
        where TOptions : class
        where TDep : class
        where TDep2 : class
        where TDep3 : class
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the options.</param>
        /// <param name="dependency">A dependency.</param>
        /// <param name="dependency2">A second dependency.</param>
        /// <param name="dependency3">A thirddependency.</param>
        /// <param name="action">The action to register.</param>
        public ConfigureNamedOptions(string name, TDep dependency, TDep2 dependency2, TDep3 dependency3, Action<TOptions, TDep, TDep2, TDep3> action)
        {
            Name = name;
            Action = action;
            Dependency = dependency;
            Dependency2 = dependency2;
            Dependency3 = dependency3;
        }

        /// <summary>
        /// The options name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The configuration action.
        /// </summary>
        public Action<TOptions, TDep, TDep2, TDep3> Action { get; }

        public TDep Dependency { get; }

        public TDep2 Dependency2 { get; }

        public TDep3 Dependency3 { get; }


        public virtual void Configure(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Null name is used to configure all named options.
            if (Name == null || name == Name)
            {
                Action?.Invoke(options, Dependency, Dependency2, Dependency3);
            }
        }

        public void Configure(TOptions options) => Configure(Options.DefaultName, options);
    }

}