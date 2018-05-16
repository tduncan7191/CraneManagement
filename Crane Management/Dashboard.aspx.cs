using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

public partial class Dashboard : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string Swiper_Description = Request.QueryString[0];
        //using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
        //{
        //    con.Open();
        //    using (SqlCommand cmd = new SqlCommand("SELECT COUNT([Prize_Count]) as Prize_Count FROM [DataWarehouse].[dbo].[vw_ProjectX_Gameplay] where Swiper_Description = @Swiper_Description and Prize_Count != 0", con))
        //    {
        //        cmd.Parameters.AddWithValue("@Swiper_Description", Swiper_Description);
        //        var reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            prizeCount.InnerHtml += reader["Prize_Count"].ToString();
        //        }
        //    }
        //}
    }
}