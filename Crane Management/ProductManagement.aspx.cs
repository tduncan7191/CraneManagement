using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class ProductManagement : System.Web.UI.Page
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
        CustomerIDs = (List<string>)Session["customerIDs"];
        Cranes = (List<string>)Session["cranesSelected"];

        if (CustomerIDs == null)
        {
            Response.Redirect("Default.aspx");
        }
        if (Cranes == null)
        {
            //Cranes = GetDBCranes(CustomerIDs);
        }

       // Render();
    }

    public void BtnSave_ServerClick(object sender, EventArgs e)
    {
        foreach (string customerID in CustomerIDs)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LiveConnectionString"].ToString()))
            {
                conn.Open();
                foreach (TableRow row in tbl.Rows)
                {
                    using (SqlCommand cmd = new SqlCommand("in_CraneManagement", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        bool flag = false;
                        foreach (TableCell cell in row.Cells)
                        {
                            foreach (Control ctrl in cell.Controls)
                            {
                                if (ctrl is Label)
                                {
                                    Label lbl = (Label)ctrl;
                                    cmd.Parameters.AddWithValue("@" + ctrl.ID.Substring(0, (lbl.ID.Length - 1)), lbl.Text.Trim());
                                }
                                if (ctrl is TextBox)
                                {
                                    TextBox txt = (TextBox)ctrl;
                                    cmd.Parameters.AddWithValue("@" + txt.ID.Substring(0, (txt.ID.Length - 1)), txt.Text.Trim());
                                }
                                if (ctrl is HtmlSelect)
                                {
                                    HtmlSelect select = (HtmlSelect)ctrl;
                                    cmd.Parameters.AddWithValue("@PrizeDescription", select.Value);
                                }
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            cmd.Parameters.AddWithValue("@PrizeType", string.Empty);
                            cmd.Parameters.AddWithValue("@CustomerID", customerID);
                            cmd.Parameters.AddWithValue("@Date", DateTime.Now.AddDays(-1).ToString("d"));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }

    protected void AddPrizeDescription_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (string customerID in CustomerIDs)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LiveConnectionString"].ToString()))
                {
                    conn.Open();
                    string strInsert = "Insert into [ProjectX].[dbo].[CraneManagement_PrizeDescriptions] (PrizeDescriptions, customerID) values (@PrizeDescriptions, @CustomerID)";
                    using (SqlCommand cmd = new SqlCommand(strInsert, conn))
                    {
                        cmd.Parameters.AddWithValue("@PrizeDescriptions", newPrizeDescription.Value);
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            foreach (TableRow row in tbl.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    foreach (Control ctrl in cell.Controls)
                    {
                        if (ctrl is HtmlSelect)
                        {
                            HtmlSelect select = (HtmlSelect)ctrl;
                            select.Items.Add(newPrizeDescription.Value);
                        }
                    }
                }
            }
        }
        catch (Exception ex) { Response.Write(ex.Message); }
    }
}