using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

using HtmlAgilityPack;

namespace RonsHouse.SeoNet.Xml
{
    public class Page
    {
        public string Url { get; set; }
        public SeoValues DefaultSeoValues { get; set; }
        public List<PageCondition> Conditions { get; set; }

        public SeoValues CurrentSeoValues { get; set; }

        public Page()
        {
            this.DefaultSeoValues = new SeoValues();
            this.Conditions = new List<PageCondition>();
            this.CurrentSeoValues = new SeoValues();
        }

        public Page(string url)
        {
            this.DefaultSeoValues = new SeoValues();
            this.Conditions = new List<PageCondition>();
            this.CurrentSeoValues = new SeoValues();
            
            //load up configuration values
            XmlDocument xml = new XmlDocument();
            xml.Load(System.Web.HttpContext.Current.Server.MapPath("/seo.xml"));
            XmlNode page = xml.SelectSingleNode("/pages/page[@url=\"" + url + "\"]");
            if (page != null)
            {
                this.Url = url;

                XmlNode defaultNode = page.SelectSingleNode("default");
                //TODO: add error checking
                this.DefaultSeoValues.Keywords = defaultNode.SelectSingleNode("keywords").InnerText;
                this.DefaultSeoValues.Description = defaultNode.SelectSingleNode("description").InnerText;
                this.DefaultSeoValues.Title = defaultNode.SelectSingleNode("title").InnerText;

                //load up conditions
                XmlNodeList conditions = page.SelectNodes("conditions/condition");
                if (conditions != null)
                {
                    foreach (XmlNode condition in conditions)
                    {
                        PageCondition pc = new PageCondition();

                        pc.ParameterName = condition.Attributes["parameter"].Value;
                        pc.Operator = (ConditionOperator)Enum.Parse(typeof(ConditionOperator), condition.Attributes["operator"].Value);
                        pc.ParameterType = (ConditionParameterType)Enum.Parse(typeof(ConditionParameterType), condition.Attributes["type"].Value);
                        pc.Value = condition.Attributes["value"].Value;

                        pc.SeoValues = new SeoValues();
                        pc.SeoValues.Description = condition.SelectSingleNode("description").InnerText;
                        pc.SeoValues.Keywords = condition.SelectSingleNode("keywords").InnerText;
                        pc.SeoValues.Title = condition.SelectSingleNode("title").InnerText;

                        this.Conditions.Add(pc);
                    }
                }
            }

            //load up current values
            //TODO: test for any performance issues
            //WebClient client = new WebClient();
            //string html = client.DownloadString(url);

            ////HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            ////HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            ////StreamReader reader = new StreamReader(webResponse.GetResponseStream());
            ////string html = reader.ReadToEnd();

            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(html);

            //HtmlNode headNode = doc.DocumentNode.SelectSingleNode("/html/head");
            //HtmlNode metaDescriptionNode = doc.DocumentNode.SelectSingleNode("/html/head/meta[@name=\"description\"]");
            //HtmlNode metaKeywordsNode = doc.DocumentNode.SelectSingleNode("/html/head/meta[@name=\"keywords\"]");
            //HtmlNode pageTitleNode = doc.DocumentNode.SelectSingleNode("/html/head/title");

            //if (metaDescriptionNode != null)
            //    this.CurrentSeoValues.Description = metaDescriptionNode.Attributes["content"].Value;

            //if (metaKeywordsNode != null)
            //    this.CurrentSeoValues.Keywords = metaKeywordsNode.Attributes["content"].Value;

            //if (pageTitleNode != null)
            //    this.CurrentSeoValues.Title = pageTitleNode.InnerText.Trim();

            
        }
    }
}
