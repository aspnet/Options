using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class Options
    {
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

        internal static TOptions GetOrUpdate<TOptions>(this IOptionsCache<TOptions> cache, IEnumerable<IConfigureOptions<TOptions>> setups, string name)
            where TOptions : class, new()
        {
            var result = cache.Get(name);
            if (result == null)
            {
                lock (cache)
                {
                    result = cache.Get(name);
                    if (result == null)
                    {
                        var filteredSetups = setups.Where(s => s.NamedInstance == name);
                        result = new TOptions();
                        foreach (var setup in filteredSetups)
                        {
                            setup.Configure(result);
                        }
                        cache.Put(name, result);
                    }
                }
            }
            return result;
        }
    }
}
