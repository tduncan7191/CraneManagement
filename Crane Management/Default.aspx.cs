using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BtnLogin_Click(object sender, EventArgs e)
    {
        List<string> customerIDs = new List<string>();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM [ProjectX].[dbo].[CustomerContact] WHERE username = @username and password = @password", conn))
            {
                cmd.Parameters.AddWithValue("@username", txtUserName.Value);
                cmd.Parameters.AddWithValue("@password", txtUserPassword.Value);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            customerIDs.Add(reader["customerID"].ToString());
                        }
                        Session["customerIDs"] = customerIDs;
                        Response.Redirect("Management.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Incorrect Uername or password');</script>");
                    }
                }
            }
        }
    }
}