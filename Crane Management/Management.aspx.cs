using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Render();
    }
        
    private void Render()
    {
        try
        {
            CustomerID.Text = (string)Session["customerID"];
            date.Text = DateTime.Now.ToString();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LiveConnectionString"].ToString()))
            {
                conn.Open();
                string query = "SELECT * FROM [ProjectX].[dbo].[vw_ProjectX_CraneManagement] " +
                                        "where CustomerID = @CustomerID " +
                                        "AND Date = cast(GETDATE()-1 as Date) " +
                                        "order by Date desc, swiper_Description";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    var reader = cmd.ExecuteReader();

                    int i = 0;
                    while (reader.Read())
                    {
                        location.Text = reader["customerName"].ToString();

                        TableRow row = new TableRow();

                        TableCell cell1 = new TableCell();
                        Label lbl1 = new Label { ID = "GameID" + i, Text = reader["Game_Id"].ToString(), CssClass = "count" };
                        lbl1.Font.Size = 20;
                        cell1.Controls.Add(lbl1);

                        TableCell cell2 = new TableCell();
                        Label lbl2 = new Label { ID = "SwiperDescription" + i, Text = reader["Swiper_Description"].ToString(), CssClass = "form-control-plaintext" };
                        lbl2.Font.Size = 20;
                        cell2.Controls.Add(lbl2);

                        TableCell cell3 = new TableCell();
                        Label lbl3 = new Label { ID = "PrizeDescription" + i, Text = reader["PrizeDescription"].ToString(), CssClass = "form-control-plaintext" };
                        lbl3.Font.Size = 20;
                        cell3.Controls.Add(lbl3);

                        row.Cells.Add(cell1);
                        row.Cells.Add(cell2);
                        row.Cells.Add(cell3);
                        row.Cells.Add(cell3);
                        tbl.Rows.Add(row);
                        i++;
                    }
                }
            }
        }
        catch (Exception e) { Response.Write(e.Message); }
    }
}