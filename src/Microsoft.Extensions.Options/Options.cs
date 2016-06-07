namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class Options
    {
        /// <summary>
        /// The order which will be used for the next call to Configure
        /// </summary>
        public static int DefaultConfigureOptionsOrder;

        /// <summary>
        /// The value added to DefaultConfigureOptionsOrder when Configure is called.
        /// </summary>
        public static int DefaultConfigureOptionsOrderIncrement = 10;

        /// <summary>
        /// Default offset used by Configure which binds against ConfigurationSections.
        /// </summary>
        public static int DefaultConfigurationBindOrderOffset = -25000;

        /// <summary>
        /// Returns DefaultConfigureOptionsOrder and increments it by DefaultConfigureOptionsOrderIncrement.
        /// </summary>
        /// <returns></returns>
        public static int NextDefaultConfigureOptionsOrder()
        {
            return DefaultConfigureOptionsOrder += DefaultConfigureOptionsOrderIncrement;
        }

        /// <summary>
        /// Creates a wrapper around an instance of TOptions to return itself as an IOptions.
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IOptions<TOptions> Create<TOptions>(TOptions options) where TOptions : class, new()
        {
            return new OptionsWrapper<TOptions>(options);
        }
    }
}
