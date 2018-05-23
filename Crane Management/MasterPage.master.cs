using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.HtmlControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    List<string> CustomerIDs
    {
        get { return ViewState["CustomerIDs"] as List<string>; }
        set { ViewState["CustomerIDs"] = value; }
    }

    List<string> Cranes
    {
        get { return ViewState["Cranes"] as List<string>; }
        set { ViewState["Cranes"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {        
        Cranes = (List<string>)Session["cranesSelected"];
        CustomerIDs = (List<string>)Session["customerIDs"];

        if (CustomerIDs == null)
        {
            Response.Redirect("Default.aspx");
        }
        if (Cranes == null)
        {
            Cranes = GetDBCranes(CustomerIDs);
        }
        else
        {
            GetDBCranes(CustomerIDs);
        }
        
        string url = "Dashboard.aspx?";
        foreach (string crane in Cranes)
        {
            var li = new HtmlGenericControl("li");
            var anchor = new HtmlAnchor();
            anchor.HRef = url + crane;
            anchor.InnerHtml += "<i class='fa fa-desktop'></i>" + crane;
            li.Controls.Add(anchor);
            ulCranes.Controls.Add(li);
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

    private List<string> GetDBCranes(List<string> customerIDs)
    {
        List<string> cranes = new List<string>();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                var ParameterList = new List<string>();
                var index = 0;
                foreach (var customerID in customerIDs)
                {
                    cmd.Parameters.AddWithValue("@Param" + index, customerID);
                    ParameterList.Add("@Param" + index);
                    index++;
                }
                string strCmd = String.Format("SELECT distinct swiperDescription FROM [ProjectX].[dbo].[CraneManagement] WHERE customerID in ({0})", string.Join(",", ParameterList));
                cmd.CommandText = strCmd;
                cmd.Connection = conn;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cranes.Add(reader["swiperDescription"].ToString());
                            masterCraneSelection.Items.Add(reader["swiperDescription"].ToString());
                        }
                    }
                }
            }
        }
        return cranes;
    }

    protected void BtnUpdateCraneSelection_Click(object sender, EventArgs e)
    {                
        var selectedCranes = Request.Form["ctl00$masterCraneSelection"].Split(',');
        List<string> cranes = new List<string>();
        List<string> lstSelectedCranes = new List<string>();
        foreach (var selectedCrane in selectedCranes)
        {
            lstSelectedCranes.Add(selectedCrane);
        }
        string url = HttpContext.Current.Request.Url.AbsolutePath + "?";
        foreach (string crane in selectedCranes)
        {
            cranes.Add(crane);
            url += crane + "&";
        }
        url = url.TrimEnd('&');
        Session["cranesSelected"] = lstSelectedCranes;
        Response.Redirect(url);
    }

    protected void BtnUpdateCustomerIDSelection_Click(object sender, EventArgs e)
    {
        var selectedCustomerIDs = Request.Form["ctl00$customerIDSelection"].Split(',');
        List<string> lstSelectedCustomerID = new List<string>();
        foreach (var selectedCustomerID in selectedCustomerIDs)
        {
            lstSelectedCustomerID.Add(selectedCustomerID);
        }
        Session["customerIDs"] = lstSelectedCustomerID;
        Response.Redirect(HttpContext.Current.Request.Url.AbsolutePath);
    }
}

