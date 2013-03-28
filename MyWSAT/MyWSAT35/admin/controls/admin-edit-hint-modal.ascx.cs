using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class admin_controls_admin_edit_hint_modal : System.Web.UI.UserControl
{
    // global variable
    string filePath = "";

    #region LOAD CSS file into TextBox

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // get cs file url from database
            GetHintFile();

            if (File.Exists(Server.MapPath(filePath)))
            {
                string FileText = Server.MapPath(filePath);
                StreamReader objStreamReader = File.OpenText(FileText);
                string Content = objStreamReader.ReadToEnd();
                txbHint.Text = Content;
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
        GetHintFile();

        try
        {
            string SaveFile = Server.MapPath(filePath);
            TextWriter objStreamWriter = new StreamWriter(SaveFile);
            objStreamWriter.Write(txbHint.Text);
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

    #region GetHintFile from database

    private void GetHintFile()
    {
        // get required elements
        int helpid = int.Parse(Request.QueryString["helpid"]);

        try
        {
            // connect to database
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbMyCMSConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_admin_SelectHint", con);
            cmd.CommandType = CommandType.StoredProcedure;

            // create parameters
            cmd.Parameters.Add("@helpId", SqlDbType.Int).Value = helpid;

            // open connection
            con.Open();

            // create reader
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader(CommandBehavior.SingleResult);

            if (myReader.Read())
            {
                filePath = myReader["HintUrl"].ToString();
                ltlHintUrl.Text = "Hint File: " + myReader["HintUrl"].ToString();

                con.Close();
                con.Dispose();
                myReader.Close();
            }
        }
        catch (Exception ex)
        {
            Msg.Text = "Oops! " + ex.Message;
            Msg.Visible = true;
        }
    }

    #endregion
}