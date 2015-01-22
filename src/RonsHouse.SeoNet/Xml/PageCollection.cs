using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RonsHouse.SeoNet.Xml
{
    public class PageCollection
    {
        public List<Page> Items { get; set; }

        public PageCollection()
        {
            //TODO: add caching
            
            this.Items = new List<Page>();
            
            XmlDocument xml = new XmlDocument();
            xml.Load(System.Web.HttpContext.Current.Server.MapPath("/seo.xml"));
            XmlNodeList pages = xml.SelectNodes("/pages/page");
            foreach (XmlNode page in pages)
            {
                Page p = new Page();
                p.Url = page.Attributes["url"].Value;

                SeoValues d = new SeoValues();
                XmlNode defaultNode = page.SelectSingleNode("default");
                //TODO: add error checking
                d.Keywords = defaultNode.SelectSingleNode("keywords").Value;
                d.Description = defaultNode.SelectSingleNode("description").Value;
                d.Title = defaultNode.SelectSingleNode("title").Value;
                p.DefaultSeoValues = d;
                
                this.Items.Add(p);
            }
        }
    }
}
