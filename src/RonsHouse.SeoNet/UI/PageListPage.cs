using System;
using System.Reflection;
using System.Web.UI;

namespace RonsHouse.SeoNet.UI
{
    internal sealed class PageListPage : SeoPageBase
    {
        public PageListPage()
        {
            this.PageTitle = "All SEO Pages managed";
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");


            writer.AddAttribute(HtmlTextWriterAttribute.Id, "PageTitle");
            writer.RenderBeginTag(HtmlTextWriterTag.H1);
            writer.Write(PageTitle);
            writer.RenderEndTag(); // </h1>
            writer.WriteLine();
        }
    }
}
