using System;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;

using HtmlAgilityPack;

using RonsHouse.SeoNet.Xml;

namespace RonsHouse.SeoNet
{
    public class SeoHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //check if xml file exists
            //figure out path to render
            // render path and gather meta info

            string url = HttpContext.Current.Request.RawUrl.ToString().ToLowerInvariant().Replace("/seo.axd", "");

            HttpResponse Response = context.Response;
            Response.Write("url: " + url + "<br />");

            //XmlDocument seoXml = new XmlDocument();
            //seoXml.Load(HttpContext.Current.Server.MapPath("seo.xml"));
            //XmlNode pageSeoNode = seoXml.SelectSingleNode("/seo/page[@url=\"" + url + "\"]");
            Page page = new Page(url);
            if (page == null)
            {
                ////create node and append to file
                //XmlNode tempPageNode = seoXml.CreateElement("page");
                
                ////create page url attribute
                //XmlAttribute tempPageUrlAttr = seoXml.CreateAttribute("url");
                //tempPageUrlAttr.Value = url;
                //tempPageNode.Attributes.Append(tempPageUrlAttr);




                //seoXml.DocumentElement.AppendChild(tempPageNode);
                //seoXml.Save(HttpContext.Current.Server.MapPath("seo.xml"));

                //save xml file
                //reload object with updated xml?
            }


            Response.Write("seo description: " + page.DefaultSeoValues.Description + "<br />");
            Response.Write("seo keywords: " + page.DefaultSeoValues.Keywords + "<br />");
            Response.Write("seo title: " + page.DefaultSeoValues.Title + "<br />");

            Response.Write("current description: " + page.CurrentSeoValues.Description + "<br />");
            Response.Write("current keywords: " + page.CurrentSeoValues.Keywords + "<br />");
            Response.Write("current title: " + page.CurrentSeoValues.Title + "<br />");

            Response.Write("<br />");

            foreach (PageCondition pc in page.Conditions)
            {
                Response.Write("condition param: " + pc.ParameterName + "<br />");
                Response.Write("condition operator: " + pc.Operator.ToString() + "<br />");
                Response.Write("condition value: " + pc.Value + "<br />");
                Response.Write("condition type: " + pc.ParameterType.ToString() + "<br />");

                Response.Write("seo description: " + pc.SeoValues.Description + "<br />");
                Response.Write("seo keywords: " + pc.SeoValues.Keywords + "<br />");
                Response.Write("seo title: " + pc.SeoValues.Title + "<br />");
            }
        }
    }
}
