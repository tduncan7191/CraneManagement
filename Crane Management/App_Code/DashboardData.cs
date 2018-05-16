using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
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

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
        {
            con.Open();
            string query = "select distinct cast(start_Date as Date) as start_Date, Transaction_Amount " +
                "from [DataWarehouse].[dbo].vw_ProjectX_Gameplay " +
                "where (cast(start_Date as Date) between cast(@startDate as Date) and cast(@endDate as Date)) " +
                "AND Swiper_Description = @crane " +
                "order by start_Date";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@crane", crane);

                var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    returnObjects.Add(new DateRangePlotChartData { Date = reader["start_Date"].ToString(), Data = reader["Transaction_Amount"].ToString() });
                }
            }
        }
        return returnObjects;
    }
    public class DateRangePlotChartData
    {
        public string Date { get; set; }
        public string Data { get; set; }        
    }
}
