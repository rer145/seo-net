using System;
using System.Reflection;
using System.Web.UI;

namespace RonsHouse.SeoNet.UI
{
    internal sealed class AboutPage : SeoPageBase
    {
        public AboutPage()
        {
            this.PageTitle = "About SeoNET";
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

            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.Write("version: " + GetVersion());
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.Write("file version: " + GetFileVersion());
            writer.RenderEndTag();
        }

        private Version GetVersion()
        {
            return GetType().Assembly.GetName().Version;
        }

        private Version GetFileVersion()
        {
            AssemblyFileVersionAttribute version = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(GetType().Assembly, typeof(AssemblyFileVersionAttribute));
            return version != null ? new Version(version.Version) : new Version();
        }
    }
}
