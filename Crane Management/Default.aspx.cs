using System;
using System.Collections.Generic;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BtnLogin_Click(object sender, EventArgs e)
    {
        List<string> cranes = new List<string>();
        IEnumerable<string> selectedCranes = Request.Form["craneSelection"].Split(',');
        string url = "Management.aspx?";
        foreach (var crane in selectedCranes)
        {
            cranes.Add(crane);
            url += crane + "&";
        }
        url = url.TrimEnd('&');
        Session["cranesSelected"] = cranes;
        Response.Redirect(url);
    }
}