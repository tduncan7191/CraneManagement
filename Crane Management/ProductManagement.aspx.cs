using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class ProductManagement : System.Web.UI.Page
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
    }

    protected void GvPrize_Insert(object sender, EventArgs e)
    {
        DsPrize.InsertParameters.Add("CustomerID", CustomerIDs[0]);

        DsPrize.InsertParameters.Add("CreateDate", DateTime.Now.ToString());

        TextBox txtItemNo = GvPrize.HeaderRow.FindControl("headerTxt_ItemNo") as TextBox;
        DsPrize.InsertParameters.Add("ItemNo", txtItemNo.Text);

        TextBox txtVendor = GvPrize.HeaderRow.FindControl("headerTxt_Vendor") as TextBox;
        DsPrize.InsertParameters.Add("Vendor", txtVendor.Text);

        TextBox txtPrize = GvPrize.HeaderRow.FindControl("headerTxt_Prize") as TextBox;
        DsPrize.InsertParameters.Add("Name", txtPrize.Text);

        TextBox txtCost = GvPrize.HeaderRow.FindControl("headerTxt_Cost") as TextBox;
        DsPrize.InsertParameters.Add("Cost", txtCost.Text);

        DsPrize.Insert();
        GvPrize.DataBind();
    }
}