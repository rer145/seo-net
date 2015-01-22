using System;
using System.Collections;
using System.Web;

using CultureInfo = System.Globalization.CultureInfo;
using Encoding = System.Text.Encoding;

namespace RonsHouse.SeoNet.UI
{
    public class SeoPageFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            string resource = context.Request.PathInfo.Length == 0 ? String.Empty :
                context.Request.Path.Substring(1).ToLower(CultureInfo.InvariantCulture);

            IHttpHandler handler = FindHandler(resource);

            if (handler == null)
                throw new HttpException(404, "Resource not found.");

            return handler;
        }

        private static IHttpHandler FindHandler(string name)
        {
            switch (name)
            {
                case "about":
                    return new AboutPage();
                //case "stylesheet":
                //    return new ManifestResourceHandler("ErrorLog.css", "text/css", Encoding.GetEncoding("Windows-1252"));
                default:
                    return name.Length == 0 ? new PageListPage() : null;
            }
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
