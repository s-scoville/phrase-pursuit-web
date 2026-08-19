namespace PhrasePursuitWeb.Core.Interfaces
{
    /// <summary>
    /// Defines methods for saving, loading, and removing data from persistent storage.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Saves a value to persistent storage using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of value to save.</typeparam>
        /// <param name="key">The key used to identify the stored value.</param>
        /// <param name="value">The value to save.</param>
        Task SaveAsync<T>(string key, T value);

        
        /// <summary>
        /// Loads a value from persistent storage using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of value to load.</typeparam>
        /// <param name="key">The key used to identify the stored value.</param>
        /// <returns>
        /// The stored value if it exists; otherwise, the default value for the specified type.
        /// </returns>
        Task<T?> LoadAsync<T>(string key);


        /// <summary>
        /// Removes a value from persistent storage using the specified key.
        /// </summary>
        /// <param name="key">The key used to identify the stored value.</param>
        Task RemoveAsync(string key);
    }
}
