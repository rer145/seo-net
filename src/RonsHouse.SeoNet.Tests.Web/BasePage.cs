using System;
using System.Collections.Generic;
using System.Web;

namespace RonsHouse.SeoNet.Tests.Web
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.Page.Title = "test : " + base.Page.Title;
            base.OnLoad(e);
        }
    }
}