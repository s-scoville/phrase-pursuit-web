using Microsoft.JSInterop;
using PhrasePursuitWeb.Core.Interfaces;
using System.Text.Json;

namespace PhrasePursuitWeb.Web.Services
{
    /// <summary>
    /// Provides browser-based storage operations using the browser's local storage.
    /// </summary>
    public class BrowserStorageService : IStorageService
    {
        /// <summary>
        /// Provides JavaScript interoperability for accessing browser local storage.
        /// </summary>
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserStorageService"/> class.
        /// </summary>
        /// <param name="jsRuntime">
        /// The JavaScript runtime used to interact with browser local storage.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="jsRuntime"/> is null.
        /// </exception>
        public BrowserStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime 
                ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        /// <summary>
        /// Serializes and saves a value to browser local storage using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of value to save.</typeparam>
        /// <param name="key">The key used to identify the stored value.</param>
        /// <param name="value">The value to serialize and save.</param>
        /// <returns>A task representing the asynchronous save operation.</returns>
        public async Task SaveAsync<T>(string key, T value)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            var json = JsonSerializer.Serialize(value, options);
            
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem", key, json);
        }

        /// <summary>
        /// Loads and deserializes a value from browser local storage using the specified key.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the stored value into.</typeparam>
        /// <param name="key">The key used to identify the stored value.</param>
        /// <returns>
        /// A task whose result contains the deserialized value, or the default value
        /// for <typeparamref name="T"/> if no value exists for the specified key.
        /// </returns>
        public async Task<T?> LoadAsync<T>(string key)
        {
            var value = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            
            if (value == null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(value);
        }

        /// <summary>
        /// Removes the value associated with the specified key from browser local storage.
        /// </summary>
        /// <param name="key">The key of the value to remove.</param>
        /// <returns>A task representing the asynchronous remove operation.</returns>
        public async Task RemoveAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
