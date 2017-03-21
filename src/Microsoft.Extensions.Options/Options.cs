using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptionsFactory.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public class OptionsService<TOptions> : IOptions<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class, new()
    {
        private readonly IOptionsCache<TOptions> _cache;
        private readonly IOptionsFactory<TOptions> _factory;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="cache">The cache to use.</param>
        /// <param name="factory">The factory to use to create options.</param>
        public OptionsService(IOptionsCache<TOptions> cache, IOptionsFactory<TOptions> factory)
        {
            _cache = cache;
            _factory = factory;
        }

        public TOptions Value
        {
            get
            {
                return Get("");
            }
        }

        public virtual void Add(string name, TOptions options)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (!_cache.TryAdd(name, options))
            {
                throw new InvalidOperationException("An option named {name} already exists.");
            }
        }

        public virtual TOptions Get(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            return _cache.GetOrAdd(name, () => _factory.Create(name));
        }

        public virtual bool Remove(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            return _cache.TryRemove(name);
        }
    }

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
    }
}
