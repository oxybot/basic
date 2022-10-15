using Newtonsoft.Json;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    /// Extensions methods for the <see cref="HttpContent"/> class.
    /// </summary>
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Reads the http content as a Json element.
        /// </summary>
        /// <param name="content">The reference.</param>
        /// <returns>The content deserialized as a json object.</returns>
        public static async Task<object> ReadAsJsonAsync(this HttpContent content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            string text = await content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject(text);
        }

        /// <summary>
        /// Reads the http content as a Json element.
        /// </summary>
        /// <typeparam name="T">The type of the json content.</typeparam>
        /// <param name="content">The reference.</param>
        /// <returns>The content deserialized as a json object.</returns>
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            string text = await content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}
