using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

using HtmlAgilityPack;

using RonsHouse.SeoNet.Xml;

namespace RonsHouse.SeoNet
{
    internal class SeoResponseFilter : BaseFilter
    {
        private StringBuilder _html;

        public SeoResponseFilter()
        {
            _html = new StringBuilder();
        }
        
        public override void Write(byte[] buffer, int offset, int count)
        {
            string tempBuffer = Encoding.Default.GetString(buffer, offset, count);
            _html.Append(tempBuffer);

            //only do operation after the entire buffer has been exhausted
            Regex eof = new Regex("</html>", RegexOptions.IgnoreCase);
            if (eof.IsMatch(tempBuffer))
            {
                string finalHtml = _html.ToString();
                
                string pageTitle = String.Empty;
                string metaDescription = String.Empty;
                string metaKeywords = String.Empty;

                //get Xml values for the current page
                string url = HttpContext.Current.Request.RawUrl.ToString().ToLowerInvariant();
                string parameterlessUrl = url.IndexOf("?") >= 0 ? url.Substring(0, url.IndexOf("?")) : url;
                
                bool conditionMet = false;

                Page page = new Page(parameterlessUrl);
                if (page.Conditions.Count > 0)
                {
                    //TODO: add error handling to parameter retrieval
                    //TODO: update to allow for object comparisons
                    //TODO: add operators and parameter types

                    foreach (PageCondition condition in page.Conditions)
                    {
                        string currentValue = String.Empty;     //current parameter to compare to condition
                        switch (condition.ParameterType)
                        {
                            case ConditionParameterType.QueryString:
                                if (HttpContext.Current.Request.QueryString[condition.ParameterName] != null)
                                    currentValue = HttpContext.Current.Request.QueryString[condition.ParameterName].ToString();
                                break;
                            case ConditionParameterType.Session:
                                if (HttpContext.Current.Session[condition.ParameterName] != null)
                                    currentValue = HttpContext.Current.Session[condition.ParameterName].ToString();
                                break;
                            case ConditionParameterType.Cache:
                                if (HttpContext.Current.Cache[condition.ParameterName] != null)
                                    currentValue = HttpContext.Current.Cache[condition.ParameterName].ToString();
                                break;
                        }

                        switch (condition.Operator)
                        {
                            case ConditionOperator.Equals:
                                conditionMet = (currentValue == condition.Value);
                                break;
                        }

                        if (conditionMet)
                        {
                            pageTitle = condition.SeoValues.Title;
                            metaDescription = condition.SeoValues.Description;
                            metaKeywords = condition.SeoValues.Keywords;
                            break;
                        }
                    }
                }
                
                if (!conditionMet)
                {
                    //use defaults
                    pageTitle = page.DefaultSeoValues.Title;
                    metaDescription = page.DefaultSeoValues.Description;
                    metaKeywords = page.DefaultSeoValues.Keywords;
                }                

                //reformat html output
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(finalHtml);
                HtmlNode headNode = doc.DocumentNode.SelectSingleNode("/html/head");
                HtmlNode metaDescriptionNode = doc.DocumentNode.SelectSingleNode("/html/head/meta[@name=\"description\"]");
                HtmlNode metaKeywordsNode = doc.DocumentNode.SelectSingleNode("/html/head/meta[@name=\"keywords\"]");
                HtmlNode pageTitleNode = doc.DocumentNode.SelectSingleNode("/html/head/title");

                //set meta description
                if (metaDescriptionNode == null)
                {
                    metaDescriptionNode = headNode.AppendChild(doc.CreateElement("meta"));
                    metaDescriptionNode.Attributes.Add("name", "description");
                    metaDescriptionNode.Attributes.Add("content", metaDescription);
                }
                else
                {
                    metaDescriptionNode.Attributes["content"].Value = metaDescription;
                }

                //set meta keywords
                if (metaKeywordsNode == null)
                {
                    metaKeywordsNode = headNode.AppendChild(doc.CreateElement("meta"));
                    metaKeywordsNode.Attributes.Add("name", "keywords");
                    metaKeywordsNode.Attributes.Add("content", metaKeywords);
                }
                else
                {
                    metaKeywordsNode.Attributes["content"].Value = metaKeywords;
                }

                ////set page title
                ////if (pageTitleNode != null)
                ////{
                ////    doc.DocumentNode.RemoveChild(pageTitleNode);
                ////}
                ////pageTitleNode = headNode.AppendChild(doc.CreateElement("title"));
                //pageTitleNode.InnerText.Remove(0);
                //pageTitleNode.InnerText.Insert(0, pageTitle);
                
                //do other seo improvements (move viewstate, eventvalidator, etc.)

                //save the changes and output
                using (MemoryStream ms = new MemoryStream())
                {
                    StreamWriter writer = new StreamWriter(ms);
                    doc.Save(writer);
                    writer.Flush();

                    ms.Position = 0;
                    StreamReader reader = new StreamReader(ms);
                    string temp = reader.ReadToEnd();

                    byte[] output = Encoding.Default.GetBytes(temp);
                    this.Sink.Write(output, 0, output.GetLength(0));
                }
            }
        }
    }
}