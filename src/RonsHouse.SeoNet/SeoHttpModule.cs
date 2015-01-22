using System;
using System.Web;

namespace RonsHouse.SeoNet
{
    public class SeoHttpModule : IHttpModule
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.PostRequestHandlerExecute += new EventHandler(InstallResponseFilter);
        }

        public void InstallResponseFilter(object sender, EventArgs e)
        {
            if (sender is HttpApplication)
			{
				using (HttpApplication app = sender as HttpApplication)
				{
					if (app.Context.CurrentHandler is System.Web.UI.Page)
					{
						app.Response.Filter = new SeoResponseFilter();
					}
				}
			}
		}
    }
}
