using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    private const string customer = "10001003";
    List<string> _lstPrizeDecriptions = new List<string>();
    //List<string> _lstSwiperDescriptions = new List<string> { "The Giant", "Ice Crane 1", "Key Master", "Big Choice Crane 1", "Big Choice Crane 2", "Big Choice Crane 3", "Big Choice Crane 4", "UFO Catcher #1", "UFO Catcher #2" };
    List<string> _lstSwiperDescriptions = new List<string>();

    string _CustomerID
    {
        get { return ViewState["CustomerID"] as string; }
        set { ViewState["CustomerID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _lstSwiperDescriptions = (List<string>)Session["cranesSelected"];
        _CustomerID = customer;
        Render();
    }

    public void BtnSave_ServerClick(object sender, EventArgs e)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
        {
            conn.Open();
            foreach (TableRow row in tbl.Rows)
            {
                using (SqlCommand cmd = new SqlCommand("in_CraneManagement", conn))
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            if (ctrl is Label)
                            {
                                Label lbl = (Label)ctrl;
                                cmd.Parameters.AddWithValue("@" + lbl.ID.Substring(0, (lbl.ID.Length - 1)), lbl.Text.Trim());
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
                        }
                    }
                    cmd.Parameters.AddWithValue("@CustomerID", _CustomerID);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.AddDays(-1).ToString("d"));
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    protected void AddPrizeDescription_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
            {
                conn.Open();
                string strInsert = "Insert into [ProjectX].[dbo].[CraneManagement_PrizeDescriptions] (PrizeDescriptions, customerID) values (@PrizeDescriptions, @CustomerID)";
                using (SqlCommand cmd = new SqlCommand(strInsert, conn))
                {
                    cmd.Parameters.AddWithValue("@PrizeDescriptions", newPrizeDescription.Value);
                    cmd.Parameters.AddWithValue("@CustomerID", _CustomerID);
                    cmd.ExecuteNonQuery();
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

    private void Render()
    {
        try
        {
            CustomerID.Text = _CustomerID;

            date.Text = DateTime.Now.ToString();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT PrizeDescriptions FROM [ProjectX].[dbo].[CraneManagement_PrizeDescriptions] WHERE CustomerID = @CustomerID", conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", _CustomerID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _lstPrizeDecriptions.Add(reader["PrizeDescriptions"].ToString());
                            }
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand())
                {
                    string strSelect = "SELECT * FROM [DataWarehouse].[dbo].[vw_ProjectX_CraneManagement] " +
                                        "where CustomerID = @CustomerID " +
                                        "AND Date = cast(GETDATE()-1 as Date)" +
                                        "AND Swiper_Description in ({0}) " +
                                        "order by Date desc, swiper_Description";

                    cmd.Parameters.AddWithValue("@CustomerID", _CustomerID);
                    var ParameterList = new List<string>();
                    var index = 0;
                    foreach (var swiperDescription in _lstSwiperDescriptions)
                    {
                        cmd.Parameters.AddWithValue("@Param" + index, swiperDescription);
                        ParameterList.Add("@Param" + index);
                        index++;
                    }
                    string strCmd = String.Format(strSelect, string.Join(",", ParameterList));
                    cmd.CommandText = strCmd;
                    cmd.Connection = conn;
                    var reader = cmd.ExecuteReader();

                    int i = 0;
                    while (reader.Read())
                    {
                        location.Text = reader["customerName"].ToString();

                        TableRow row = new TableRow();

                        TableCell cell1 = new TableCell();
                        cell1.Controls.Add(new Label { ID = "GameID" + i, Text = reader["Game_Id"].ToString(), CssClass = "form-control-plaintext" });

                        TableCell cell2 = new TableCell();
                        cell2.Controls.Add(new Label { ID = "SwiperDescription" + i, Text = reader["Swiper_Description"].ToString(), CssClass = "form-control-plaintext" });

                        TextBox txtAvgCost = new TextBox { ID = "AverageCost" + i, CssClass = "form-control", Text = reader["AverageCost"].ToString() };
                        txtAvgCost.Style.Add("width", "200px");

                        TableCell cell3 = new TableCell();
                        cell3.Controls.Add(txtAvgCost);

                        HtmlSelect select = new HtmlSelect { ID = "PrizeDescription" + i, Name = "PrizeDescription" };
                        select.Items.Add("None Selected");
                        foreach (string prizeDescription in _lstPrizeDecriptions)
                        {
                            select.Items.Add(prizeDescription);
                        }
                        try
                        {
                            select.Items.FindByText(reader["PrizeDescription"].ToString()).Selected = true;
                        }
                        catch { }

                        TableCell cell4 = new TableCell();
                        cell4.Controls.Add(select);

                        TextBox txtPrizeCost = new TextBox { ID = "PrizeType" + i, CssClass = "form-control", Text = reader["PrizeType"].ToString() };
                        txtPrizeCost.Style.Add("width", "200px");

                        TableCell cell5 = new TableCell();
                        cell5.Controls.Add(txtPrizeCost);

                        row.Cells.Add(cell1);
                        row.Cells.Add(cell2);
                        row.Cells.Add(cell3);
                        row.Cells.Add(cell4);
                        row.Cells.Add(cell5);
                        tbl.Rows.Add(row);
                        i++;
                    }
                    hdnNumberOfRows.Value = i.ToString();
                }
            }
        }
        catch (Exception e) { Response.Write(e.Message); }
    }
}