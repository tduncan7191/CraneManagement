using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

public partial class Dashboard : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    protected void BtnDownload_Click(object sender, EventArgs e)
    {
        string url = hdnURL.Value;
        string[] cranes = url.Replace("?", string.Empty).Replace("%20", " ").Split('&');

        List<TopTilesChartData> objects = new List<TopTilesChartData>();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LiveConnectionString"].ConnectionString))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("[dbo].[in_CraneManagement_Data]", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var crane in cranes)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@startDate", hdnStartDate.Value);
                    cmd.Parameters.AddWithValue("@endDate", hdnEndDate.Value);
                    cmd.Parameters.AddWithValue("@crane", crane);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            float revenue;
                            float costOfGoods;
                            float revenuePerPlay;
                            float hitRate;
                            float.TryParse(reader["Revenue"].ToString(), out revenue);
                            float.TryParse(reader["CostOfGoods"].ToString(), out costOfGoods);
                            float.TryParse(reader["RevenuePerPlay"].ToString(), out revenuePerPlay);
                            float.TryParse(reader["HitRate"].ToString(), out hitRate);
                            if (hitRate == 1)
                            {
                                hitRate = 0;
                            }
                            objects.Add(new TopTilesChartData
                            {
                                Crane = crane,
                                Revenue = string.Format("{0:0.00}", revenue),
                                Plays = reader["Plays"].ToString(),
                                CostOfGoodsSold = string.Format("{0:0.00}", costOfGoods),
                                RevenuePerPlay = string.Format("{0:0.00}", revenuePerPlay),
                                PayoutPercent = reader["PayoutPercent"].ToString(),
                                HitRate = string.Format("{0:0}", Math.Round(hitRate)),
                                Wins = reader["wins"].ToString(),
                                Prize = reader["Prize"].ToString()
                            });
                        }
                    }
                }
            }
        }
        string csv = "CostOfGoodsSold,Crane,HitRate,PayoutPercent,Plays,Prize,Revenue,RevenuePerPlay,Wins\r\n";

        foreach (TopTilesChartData obj in objects)
        {
            csv += obj.CostOfGoodsSold + ',';
            csv += obj.Crane + ',';
            csv += obj.HitRate + ',';
            csv += obj.PayoutPercent + ',';
            csv += obj.Plays + ',';
            csv += obj.Prize + ',';
            csv += obj.Revenue + ',';
            csv += obj.RevenuePerPlay + ',';
            csv += obj.Wins + ',';
            csv += "\r\n";
        }

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=CraneManagement.csv");
        Response.Charset = "";
        Response.ContentType = "application/text";
        Response.Output.Write(csv);
        Response.Flush();
        Response.End();
    }
}