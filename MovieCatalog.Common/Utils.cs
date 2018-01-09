namespace MovieCatalog.Common
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public static class Utils
    {
        private static HttpClient httpClient = new HttpClient();

        public static string EncodeToValidUrl(this string text)
            => WebUtility.UrlEncode(text);

        public static void DownloadFile(this string url)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(new Uri(url), GlobalConstants.TempPosterFilePath);
            }
        }

        public static async Task DownloadAsync(this string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(url);
            }

            using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url)))
            {
                using (
                    Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(),
                    stream = new FileStream(GlobalConstants.TempPosterFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await contentStream.CopyToAsync(stream);
                }
            }
        }

        public static async Task<string> GetHtmlContentAsync(this string url)
        {
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:56.0) Gecko/20100101 Firefox/56.0");

            string htmlContent = await httpClient.GetStringAsync(new Uri(url));

            return WebUtility.HtmlDecode(htmlContent);
        }

        public static void WriteTempText(this string content)
        {
            File.WriteAllText(GlobalConstants.TempTextFilePath, content, Encoding.UTF8);
        }

        public static async Task WriteToFileAsync(this string content, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    await writer.WriteLineAsync(content);
                }
            }
        }
    }
}
