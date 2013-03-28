#region using references
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Caching;
using System.Web.UI;
#endregion

/// <summary>
/// Gets the MasterPage for the user selected Admin Theme.
/// </summary>
public class GetAdminMasterPage : System.Web.UI.Page
{
    #region Get Admin MasterPage

    protected override void OnPreInit(EventArgs e)
    {
        if (Cache["cachedAdminMaster"] == null)
        {
            GetDefaultMasterPage();
        }
        else
        {
            string loadMasterFromCache = Cache["cachedAdminMaster"].ToString();
            Page.MasterPageFile = loadMasterFromCache;
        }
    }

    private void GetDefaultMasterPage()
    {
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbMyCMSConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_admin_SelectMasterPage", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.SingleResult);

            if (myReader.Read())
            {
                string masterPageFileName = myReader["ThemeUrl"] as string;
                Cache.Insert("cachedAdminMaster", masterPageFileName, null, DateTime.Now.AddSeconds(300), System.Web.Caching.Cache.NoSlidingExpiration);
                Page.MasterPageFile = masterPageFileName;
            }

            myReader.Close();
            con.Close();
            con.Dispose();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    #endregion
}