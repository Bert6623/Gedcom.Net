using System;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using System.Web.UI;

public partial class admin_controls_encrypt_connection_string : System.Web.UI.UserControl
{
    //------------------------------------------------------------------------
    // Note: When running this code in a webhost environment
    // you enable write permissions to your application folder -
    // while encrypting the connection string. Then disable write permissions.
    //-------------------------------------------------------------------------

    #region SHOW web.config text in TEXT BOX

    protected void Page_Load(object sender, EventArgs e)
    {
        // On the first page visit, call DisplayWebConfig method 
        if (!Page.IsPostBack) DisplayWebConfig();

        Msg.Text = "Message: web.config file content displayed above.";
    }

    #endregion

    #region READ contents of web.config and BIND to TEXT BOX

    private void DisplayWebConfig()
    {
        // Reads in the contents of Web.config and displays them in the TextBox 
        StreamReader webConfigStream = File.OpenText(Path.Combine(Request.PhysicalApplicationPath, "Web.config"));
        string configContents = webConfigStream.ReadToEnd();
        webConfigStream.Close();
        WebConfigContents.Text = configContents;
    }

    #endregion

    #region ENCRYPT connection string - button

    protected void Button1_Click(object sender, EventArgs e)
    {
        // Get configuration information about Web.config 
        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

        // Let's work with the <connectionStrings> section 
        ConfigurationSection connectionStrings = config.GetSection("connectionStrings");
        if (connectionStrings != null)
        {
            // Only encrypt the section if it is not already protected 
            if (!connectionStrings.SectionInformation.IsProtected)
            {
                // Encrypt the <connectionStrings> section using the 
                // DataProtectionConfigurationProvider provider 
                connectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                config.Save();

                // Refresh the Web.config display 
                DisplayWebConfig();
                Msg.Text = "Message: Encryption successful!";
            }
        }
    }

    #endregion

    #region DECRYPT connection string - button

    protected void Button2_Click(object sender, EventArgs e)
    {
        // Get configuration information about Web.config 
        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

        // Let's work with the <connectionStrings> section 
        ConfigurationSection connectionStrings = config.GetSection("connectionStrings");

        if (connectionStrings != null)
        {
            // Only decrypt the section if it is protected 
            if (connectionStrings.SectionInformation.IsProtected)
            {
                // Decrypt the <connectionStrings> section 
                connectionStrings.SectionInformation.UnprotectSection();
                config.Save();

                // Refresh the Web.config display 
                DisplayWebConfig();
                Msg.Text = "Message: Decryption successful!";
            }
        }
    }

    #endregion
}
