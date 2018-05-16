using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<string> cranes = (List<string>)Session["cranesSelected"];
        try
        {
            string url = "Dashboard.aspx?";
            foreach (string crane in cranes)
            {
                var li = new HtmlGenericControl("li");
                var anchor = new HtmlAnchor();
                anchor.HRef = url + crane;
                anchor.InnerHtml += "<i class='fa fa-desktop'></i>" + crane;
                li.Controls.Add(anchor);
                ulCranes.Controls.Add(li);

            }
            foreach (string crane in cranes)
            {
                url += crane + "&";
            }
            url = url.TrimEnd('&');
            var li2 = new HtmlGenericControl("li");
            var anchor2 = new HtmlAnchor();
            anchor2.InnerHtml += "<i class='fa fa-desktop'></i>All Selected Cranes";
            anchor2.HRef = url;
            li2.Controls.Add(anchor2);
            ulGeneral.Controls.Add(li2);
        }
        catch
        {
            Response.Redirect("Default.aspx");
        }
    }

    protected void BtnUpdateCraneSelection_Click(object sender, EventArgs e)
    {
        List<string> cranes = new List<string>();
        
        var selectedCranes = Request.Form["ctl00$masterCraneSelection"].Split(',');
        string url = HttpContext.Current.Request.Url.AbsolutePath + "?";
        foreach (string crane in selectedCranes)
        {
            cranes.Add(crane);
            url += crane + "&";
        }
        url = url.TrimEnd('&');
        Session["cranesSelected"] = cranes;
        Response.Redirect(url);
    }
}
