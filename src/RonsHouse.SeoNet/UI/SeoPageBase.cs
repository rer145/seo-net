using System;
using System.Web.UI;

using CultureInfo = System.Globalization.CultureInfo;

namespace RonsHouse.SeoNet.UI
{
    internal abstract class SeoPageBase : Page
    {
        private string _title;

        protected string BasePageName
        {
            get { return this.Request.ServerVariables["URL"]; }
        }

        protected virtual string PageTitle
        {
            get { return _title; }
            set { _title = value; }
        }

        protected virtual void RenderDocumentStart(HtmlTextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");

            writer.AddAttribute("xmlns", "http://www.w3.org/1999/xhtml");
            writer.RenderBeginTag(HtmlTextWriterTag.Html);  // <html>

            writer.RenderBeginTag(HtmlTextWriterTag.Head);  // <head>
            RenderHead(writer);
            writer.RenderEndTag();                          // </head>
            writer.WriteLine();

            writer.RenderBeginTag(HtmlTextWriterTag.Body);  // <body>
        }

        protected virtual void RenderHead(HtmlTextWriter writer)
        {
            //
            // In IE 8 or later, mimic IE 7
            // http://msdn.microsoft.com/en-us/library/cc288325.aspx#DCModes
            //

            writer.AddAttribute("http-equiv", "X-UA-Compatible");
            writer.AddAttribute("content", "IE=EmulateIE7");
            writer.RenderBeginTag(HtmlTextWriterTag.Meta);
            writer.RenderEndTag();
            writer.WriteLine();

            //
            // Write the document title.
            //

            writer.RenderBeginTag(HtmlTextWriterTag.Title);
            Server.HtmlEncode(this.PageTitle, writer);
            writer.RenderEndTag();
            writer.WriteLine();

            //
            // Write a <link> tag to relate the style sheet.
            //

#if NET_1_0 || NET_1_1
            writer.AddAttribute("rel", "stylesheet");
#else
            writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
#endif
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, this.BasePageName + "/stylesheet");
            writer.RenderBeginTag(HtmlTextWriterTag.Link);
            writer.RenderEndTag();
            writer.WriteLine();
        }

        protected virtual void RenderDocumentEnd(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "Footer");
            writer.RenderBeginTag(HtmlTextWriterTag.P); // <p>

            //
            // Write out server date, time and time zone details.
            //

            DateTime now = DateTime.Now;

            writer.Write("Server date is ");
            this.Server.HtmlEncode(now.ToString("D", CultureInfo.InvariantCulture), writer);

            writer.Write(". Server time is ");
            this.Server.HtmlEncode(now.ToString("T", CultureInfo.InvariantCulture), writer);

            writer.Write(". All dates and times displayed are in the ");
            writer.Write(TimeZone.CurrentTimeZone.IsDaylightSavingTime(now) ?
                TimeZone.CurrentTimeZone.DaylightName : TimeZone.CurrentTimeZone.StandardName);
            writer.Write(" zone. ");

            writer.RenderEndTag(); // </p>

            writer.RenderEndTag(); // </body>
            writer.WriteLine();

            writer.RenderEndTag(); // </html>
            writer.WriteLine();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderDocumentStart(writer);
            RenderContents(writer);
            RenderDocumentEnd(writer);
        }

        protected virtual void RenderContents(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}
