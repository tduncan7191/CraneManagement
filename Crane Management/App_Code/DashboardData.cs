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
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<DateRangePlotChartData> GetDateRangePlotChartData(string startDate, string endDate, string crane)
    {        
        List<DateRangePlotChartData> returnObjects = new List<DateRangePlotChartData>();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Masserv02ConnectionString"].ConnectionString))
        {
            con.Open();
            string query = "select distinct " +
                "cast(Date as Date) as start_Date" +
                ",sum(cast(Transaction_Amount as money)) as Revenue " +
                ",sum(cast(CostOfGoodsSold as money)) as costOfGoods " +
                "from [DataWarehouse].[dbo].[vw_ProjectX_CraneDashboard] " +
                "where (cast(Date as Date) between cast(@startDate as Date) and cast(@endDate as Date)) " +
                "AND SwiperDescription = @crane " +
                "group by Date";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@crane", crane);

                var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    returnObjects.Add(new DateRangePlotChartData
                    {
                        Date = reader["start_Date"].ToString(),
                        CostOfGoods = reader["Revenue"].ToString(),
                        TotalRevenue = reader["costOfGoods"].ToString()
                    });
                }
            }
        }
        return returnObjects;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<DateRangePlotChartData> GetPayoutChartData(string dateType, string crane)
    {
        List<DateRangePlotChartData> returnObjects = new List<DateRangePlotChartData>();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Masserv02ConnectionString"].ConnectionString))
        {
            con.Open();
            string query = "select distinct date as date" +
                               ",SUM([AverageCost]/nullif([Transaction_Amount],0)) OVER(Partition by Date) as TotalMargin " +
                               "FROM[DataWarehouse].[dbo].[vw_ProjectX_CraneDashboard] " +
                               "where Date >= GETDATE()-365 " +
                               "and Transaction_Type = 'PLAY' " +
                               "and SwiperDescription = @crane"; 

            if (dateType == "month")
            {
                query = "select distinct " +
                               "concat(YEAR(GETDATE()), '-', month(date), '-', DAY(GETDATE())) as date " +
                               ",SUM([AverageCost]/nullif([Transaction_Amount],0)) OVER(Partition by Month(Date)) as TotalMargin " +
                               "FROM[DataWarehouse].[dbo].[vw_ProjectX_CraneDashboard] " +
                               "where Date >= GETDATE()-365 " +
                               "and Transaction_Type = 'PLAY' " +
                               "and SwiperDescription = @crane";
            }
            //if (dateType == "year")
            //{
            //    query = "select distinct" +
            //                   "concat(YEAR(date), '-', month(GETDATE()), '-', DAY(GETDATE())) as date " +
            //                   ",SUM([AverageCost]/nullif([Transaction_Amount],0)) OVER(Partition by year(Date)) as TotalMargin " +
            //                   "FROM[DataWarehouse].[dbo].[vw_ProjectX_CraneDashboard] " +
            //                   "where start_date >= GETDATE()-365 " +
            //                   "and Transaction_Type = 'PLAY' " +
            //                   "and SwiperDescription = @crane";
            //}
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@crane", crane);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnObjects.Add(new DateRangePlotChartData
                    {
                        Date = reader["date"].ToString(),
                        PayoutPercent = reader["TotalMargin"].ToString()
                    });
                }
            }
        }
        return returnObjects;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<TopTilesChartData> GetTopTilesData(string startDate, string endDate, string[] cranes)
    {
        List<TopTilesChartData> returnObjects = new List<TopTilesChartData>();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Masserv02ConnectionString"].ConnectionString))
        {
            con.Open();
            string query = "select " +
                            "sum(Prize_Count) as wins " +
                            ",sum(cast(CostOfGoodsSold as money)) as costOfGoods " +
                            ",1/(sum(cast(Prize_Count as money)) /a.plays) as HitRate " +
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
        }
        return returnObjects;
    }
    public class DateRangePlotChartData
    {
        public string Date { get; set; }
        public string CostOfGoods { get; set; }
        public string TotalRevenue { get; set; }
        public string PayoutPercent { get; set; }
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
