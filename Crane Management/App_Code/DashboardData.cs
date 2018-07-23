using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class DashboardData : WebService
{    
    public DashboardData()
    {
    
    }
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<TopTilesChartData> GetTopTilesData(string startDate, string endDate, string[] cranes)
    {
        List<TopTilesChartData> returnObjects = new List<TopTilesChartData>();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LiveConnectionString"].ConnectionString))
        {
            con.Open();
            string query = "select " +
                            "sum(Prize_Count) as wins " +
                            ",sum(cast(CostOfGoodsSold as money)) as costOfGoods " +
                            ",1/case when (sum(cast(Prize_Count as money)) /a.plays) = 0 then 1 else (sum(cast(Prize_Count as money)) /a.plays) end as HitRate " +
                            ",cast((sum(cast(CostOfGoodsSold as money)) /a.Revenue)*100 as int) as PayoutPercent" +
                            ",a.plays " +
                            ",a.Revenue " +
                            ",a.RevenuePerPlay " +
                            "FROM [ProjectX].[dbo].[vw_ProjectX_CraneDashboard] " +
                            "cross join( " +
                            "    select count(Transaction_Id) as plays " +
                            "    , sum(cast(Transaction_Amount as money)) as Revenue " +
	                        "    ,sum(cast(Transaction_Amount as money)) / count(Transaction_Id) as RevenuePerPlay " +
                            "    FROM [ProjectX].[dbo].[vw_ProjectX_CraneDashboard] " +
                            "    where Transaction_Type = 'play' " +
                            "    and [Date] between @startDate and @endDate " +
                            "    and SwiperDescription in ({0}) " +
                            ") a " +
                            "where SwiperDescription in ({0}) " +
                            "and [Date] between @startDate and @endDate " +
                            "group by a.plays, a.Revenue, a.RevenuePerPlay";
            
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                //add parameters for each swiper description in cranes
                var ParameterList = new List<string>();
                var index = 0;
                foreach (var crane in cranes)
                {
                    cmd.Parameters.AddWithValue("@Param" + index, crane);
                    ParameterList.Add("@Param" + index);
                    index++;
                }
                string strCmd = String.Format(query, string.Join(",", ParameterList));
                cmd.CommandText = strCmd;
                cmd.Connection = con;

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
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
                        returnObjects.Add(new TopTilesChartData
                        {
                            Revenue = string.Format("{0:0.00}", revenue),
                            Plays = reader["Plays"].ToString(),
                            CostOfGoodsSold = string.Format("{0:0.00}", costOfGoods),
                            RevenuePerPlay = string.Format("{0:0.00}", revenuePerPlay),
                            PayoutPercent = reader["PayoutPercent"].ToString(),
                            HitRate = string.Format("{0:0}", Math.Round(hitRate)),
                            Wins = reader["wins"].ToString()
                        });
                    }
                }
                else
                {
                    returnObjects.Add(new TopTilesChartData
                    {
                        Revenue = string.Format("{0:0.00}", 0),
                        Plays = "0",
                        CostOfGoodsSold = string.Format("{0:0.00}", 0),
                        RevenuePerPlay = string.Format("{0:0.00}", 0),
                        PayoutPercent = "0",
                        HitRate = string.Format("{0:0}", 0),
                        Wins = "0"
                    });
                }
            }
        }
        return returnObjects;
    }

    public class TopTilesChartData
    {
        public string Revenue { get; set; }
        public string Plays { get; set; }
        public string CostOfGoodsSold { get; set; }
        public string RevenuePerPlay { get; set; }
        public string PayoutPercent { get; set; }
        public string HitRate { get; set; }
        public string Wins { get; set; }
    }
}
