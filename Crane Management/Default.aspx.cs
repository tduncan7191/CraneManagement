using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    private const string EncryptionKey = "MAKV2SPBNI99212";

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private string Encrypt(string clearText)
    {
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    //private string Decrypt(string cipherText)
    //{
    //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
    //    using (Aes encryptor = Aes.Create())
    //    {
    //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
    //        encryptor.Key = pdb.GetBytes(32);
    //        encryptor.IV = pdb.GetBytes(16);
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
    //            {
    //                cs.Write(cipherBytes, 0, cipherBytes.Length);
    //                cs.Close();
    //            }
    //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
    //        }
    //    }
    //    return cipherText;
    //}

    protected void BtnLogin_Click(object sender, EventArgs e)
    {
        List<string> customerIDs = new List<string>();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LiveConnectionString"].ToString()))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM [ProjectX].[dbo].[CustomerContact] WHERE username = @username and password = @password", conn))
            {
                cmd.Parameters.AddWithValue("@username", txtUserName.Value);
                cmd.Parameters.AddWithValue("@password", Encrypt(txtUserPassword.Value));
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Session["customerID"] = reader["customerID"].ToString();
                        }
                        Response.Redirect("Management.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Incorrect Usrname or password');</script>");
                    }
                }
            }
        }
    }

    //protected void BtnAddUser_Click(object sender, EventArgs e)
    //{
    //    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LiveConnectionString"].ToString()))
    //    {
    //        conn.Open();

    //        using (SqlCommand cmd = new SqlCommand("Insert into [ProjectX].[dbo].[CustomerContact] (customerID, username, password) Values (@customerID, @username, @password)", conn))
    //        {
    //            cmd.Parameters.AddWithValue("@customerID", txtCustomerID.Value);
    //            cmd.Parameters.AddWithValue("@username", txtUserName.Value);
    //            cmd.Parameters.AddWithValue("@password", Encrypt(txtUserPassword.Value));
    //            cmd.ExecuteReader();
    //            Response.Write("<script>alert('User added');</script>");
    //        }
    //    }
    //}
}