using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
    internal static class DataHelper
    {
        public static async Task<HttpResponseMessage> PostIcsAsync(string requestUri, string icsContent)
        {
            return await IcsExporter.PostIcsAsync(requestUri, icsContent);
        }

        public static async Task<HttpResponseMessage> PostIcsFileAsync(string requestUri, string filePath)
        {
            return await IcsExporter.PostIcsFileAsync(requestUri, filePath);
        }
    }
}
