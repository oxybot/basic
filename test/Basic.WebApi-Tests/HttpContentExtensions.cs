using Newtonsoft.Json;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpContentExtensions
    {
        public static async Task<object> ReadAsJsonAsync(this HttpContent content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            string text = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(text);
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            string text = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}
