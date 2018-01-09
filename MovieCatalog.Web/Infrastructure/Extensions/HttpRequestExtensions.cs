namespace MovieCatalog.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Http;

    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null || request.Headers == null)
            {
                return false;
            }

            return request.Headers[RequestedWithHeader] == XmlHttpRequest;
        }
    }
}
