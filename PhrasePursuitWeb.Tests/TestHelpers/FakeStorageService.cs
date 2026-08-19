using PhrasePursuitWeb.Core.Interfaces;

namespace PhrasePursuitWeb.Tests.TestHelpers
{
    /// <summary>
    /// Provides an in-memory implementation of <see cref="IStorageService"/>
    /// for use in unit tests.
    /// </summary>
    public class FakeStorageService : IStorageService
    {
        /// <summary>
        /// Represents the in-memory collection used to store values by their associated keys.
        /// </summary>
        private readonly Dictionary<string, object> _storage = new();

        /// <summary>
        /// Loads a value from the in-memory storage using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of value to load.</typeparam>
        /// <param name="key">The key used to identify the stored value.</param>
        /// <returns>The stored value if found and of the requested type; otherwise, the default value.</returns>
        public Task<T?> LoadAsync<T>(string key)
        {
            if (_storage.TryGetValue(key, out var value) && value is T typedValue)
            {
                return Task.FromResult<T?>(typedValue);
            }

            return Task.FromResult<T?>(default);
        }

        /// <summary>
        /// Removes a value from the in-memory storage using the specified key.
        /// </summary>
        /// <param name="key">The key used to identify the value to remove.</param>
        /// <returns>A completed task representing the removal operation.</returns>
        public Task RemoveAsync(string key)
        {
            _storage.Remove(key);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Saves a value to the in-memory storage using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of value to save.</typeparam>
        /// <param name="key">The key used to identify the stored value.</param>
        /// <param name="value">The value to save.</param>
        /// <returns>A completed task representing the save operation.</returns>
        public Task SaveAsync<T>(string key, T value)
        {
            _storage[key] = value!;

            return Task.CompletedTask;
        }
    }
}
