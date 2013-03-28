using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class admin_controls_admin_themes : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #region Theme Activate - Preview - Delete

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if(e.CommandName == "activate")
        {
            // get required elements
            int ThemeId = Convert.ToInt32(e.CommandArgument);
            
            // ---------------------------------------
            // SET Admin Theme 
            // ---------------------------------------
            System.Data.SqlClient.SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbMyCMSConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_admin_SetMasterPage", con);
            cmd.CommandType = CommandType.StoredProcedure;

            // create parameters
            cmd.Parameters.Add("@ThemeId", SqlDbType.Int).Value = ThemeId;

            int updated = 0;
            try
            {
                con.Open();

                // execute update
                updated = cmd.ExecuteNonQuery();

                // rebind default repeater
                Repeater2.DataBind();

                // refresh cache to show new selection
                Cache.Remove("cachedAdminMaster");
                Response.Redirect("admin-themes.aspx");
            }
            catch
            {
                // display error mesage
                string strMessage = "Oops! A problem occured connecting to the database.\\r\\n Please try again later.";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script>alert('" + strMessage + "');</script>");
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else if (e.CommandName == "delete")
        {
            // get required elements
            int ThemeId = Convert.ToInt32(e.CommandArgument);

            // ---------------------------------------
            // DELETE Selected Theme 
            // ---------------------------------------
            System.Data.SqlClient.SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbMyCMSConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_admin_DeleteTheme", con);
            cmd.CommandType = CommandType.StoredProcedure;

            // create parameters
            cmd.Parameters.Add("@ThemeId", SqlDbType.Int).Value = ThemeId;

            int deleted = 0;
            try
            {
                con.Open();

                // execute update
                deleted = cmd.ExecuteNonQuery();

                // rebind default repeater
                Repeater1.DataBind();
                Repeater2.DataBind();
            }
            catch
            {
                // display error mesage
                string strMessage = "Oops! A problem occured connecting to the database.\\r\\n Please try again later.";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script>alert('" + strMessage + "');</script>");
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }

    #endregion
}