using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal static class DataHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<HttpResponseMessage> PostIcsAsync(string requestUri, string icsContent)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                throw new ArgumentException("Request URI must be provided.", nameof(requestUri));

            var content = new StringContent(icsContent ?? string.Empty, Encoding.UTF8, "text/calendar");
            return await _httpClient.PostAsync(requestUri, content);
        }

        public static async Task<HttpResponseMessage> PostIcsFileAsync(string requestUri, string filePath)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                throw new ArgumentException("Request URI must be provided.", nameof(requestUri));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path must be provided.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("ICS file not found.", filePath);

            var icsContent = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
            return await PostIcsAsync(requestUri, icsContent);
        }
    }
}
