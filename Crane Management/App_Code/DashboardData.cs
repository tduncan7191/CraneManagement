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
            using (SqlCommand cmd = new SqlCommand("[dbo].[in_CraneManagement_Data]", con))
            {
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				
                foreach (var crane in cranes)
                {
                    if (string.IsNullOrEmpty(crane))
                    {
                        returnObjects.Add(DefaultTopTiles(crane));
                        return returnObjects;
                    }
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@swiperDescription", crane);

                    using (var reader = cmd.ExecuteReader())
                    {
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
                                    Crane = crane,
                                    Revenue = string.Format("{0:0.00}", revenue),
                                    Plays = reader["Plays"].ToString(),
                                    CostOfGoodsSold = string.Format("{0:0.00}", costOfGoods),
                                    RevenuePerPlay = string.Format("{0:0.00}", revenuePerPlay),
                                    PayoutPercent = reader["PayoutPercent"].ToString(),
                                    HitRate = string.Format("{0:0}", Math.Round(hitRate)),
                                    Wins = reader["wins"].ToString(),
                                    Prize = reader["PrizeDescription"].ToString()
                                });
                            }
                        }
                        else
                        {
                            returnObjects.Add(DefaultTopTiles(crane));
                            return returnObjects;
                        }
                    }
                }
            }            
        }
        return returnObjects;
    }

    protected TopTilesChartData DefaultTopTiles(string craneName)
    {
        return new TopTilesChartData
        {
            Crane = craneName,
            Revenue = string.Format("{0:0.00}", 0),
            Plays = "0",
            CostOfGoodsSold = string.Format("{0:0.00}", 0),
            RevenuePerPlay = string.Format("{0:0.00}", 0),
            PayoutPercent = "0",
            HitRate = string.Format("{0:0}", 0),
            Wins = "0",
            Prize = "None Selected"
        };
    }

    public class TopTilesChartData
    {
        public string Crane { get; set; }
        public string Revenue { get; set; }
        public string Plays { get; set; }
        public string CostOfGoodsSold { get; set; }
        public string RevenuePerPlay { get; set; }
        public string PayoutPercent { get; set; }
        public string HitRate { get; set; }
        public string Wins { get; set; }
        public string Prize { get; set; }
    }
}
