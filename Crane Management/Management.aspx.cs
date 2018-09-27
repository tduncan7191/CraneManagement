using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    List<string> CustomerIDs
    {
        get { return ViewState["CustomerIDs"] as List<string>; }
        set { ViewState["CustomerIDs"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CustomerIDs = (List<string>)Session["customerIDs"];

        if (CustomerIDs == null)
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
            DsCrane.SelectParameters.Add("CustomerID", CustomerIDs[0]);
            DsPrize.SelectParameters.Add("CustomerID", CustomerIDs[0]);
            DsCustomer.SelectParameters.Add("CustomerID", CustomerIDs[0]);
        }
    }
    
    protected void GvCrane_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string prize = (GvCrane.Rows[e.RowIndex].FindControl("DDLPrize") as DropDownList).SelectedItem.Value;
        string CM_iID = GvCrane.DataKeys[e.RowIndex].Value.ToString();
        DsCrane.UpdateParameters.Add("Prize", prize);
        DsCrane.UpdateParameters.Add("CM_iID", CM_iID);
    }

}
 