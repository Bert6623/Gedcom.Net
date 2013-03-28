#region using references

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

#endregion

public partial class admin_controls_admin_edit_css_modal : System.Web.UI.UserControl
{
    // global variable
    string filePath = "";

    #region Page_Load - LOAD CSS file into TextBox

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // get cs file url from database
            GetCssFile();

            if (File.Exists(Server.MapPath(filePath)))
            {
                string FileText = Server.MapPath(filePath);
                StreamReader objStreamReader = File.OpenText(FileText);
                string Content = objStreamReader.ReadToEnd();
                TextBox1.Text = Content;
                objStreamReader.Close();
            }
            else
            {
                Response.Write("<script language='javascript'>window.alert('File not found...');</script>");
            }
        }
    }

    #endregion

    #region SAVE Button click

    protected void btnSave_Click(object sender, EventArgs e)
    {
        // get cs file url from database
        GetCssFile();
        
        try
        {
            string SaveFile = Server.MapPath(filePath);
            TextWriter objStreamWriter = new StreamWriter(SaveFile);
            objStreamWriter.Write(TextBox1.Text);
            objStreamWriter.Flush();
            objStreamWriter.Close();

            Msg.Text = "File saved successfully!...";
            Msg.Visible = true;
        }
        catch (Exception ex)
        {
            Msg.Text = "Oops! " + ex.Message;
            Msg.Visible = true;
        }
    }

    #endregion

    #region GetCssFile from database

    private void GetCssFile()
    {
        // get required elements
        int cssid = int.Parse(Request.QueryString["cssid"]);

        try
        {
            // connect to database
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbMyCMSConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_admin_SelectCss", con);
            cmd.CommandType = CommandType.StoredProcedure;

            // create parameters
            cmd.Parameters.Add("@CssId", SqlDbType.Int).Value = cssid;

            // open connection
            con.Open();

            // create reader
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader(CommandBehavior.SingleResult);

            if (myReader.Read())
            {
                filePath = myReader["CssUrl"].ToString();
                ltlCssUrl.Text = "CSS File: " + myReader["CssUrl"].ToString();

                con.Close();
                con.Dispose();
                myReader.Close();
            }
        }
        catch(Exception ex)
        {
            Msg.Text = "Oops! " + ex.Message;
            Msg.Visible = true;
        }
    }

    #endregion
}