using System;
using System.Web.Security;
using System.Web.UI;
using System.Web;

public partial class admin_themes_default_default : System.Web.UI.MasterPage
{
    #region SHOW current YEAR in footer, ONLINE USERS and USER COUNT in header

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // get number of registred users and assign it to global variable
            MembershipUserCollection allRegisteredUsers = Membership.GetAllUsers();

            // For counting users online, using the membership API, uncomment the following line -
            // and comment out the lblOnlineUsers.Text = Application["OnlineUsers"].ToString(); below
            //lblOnlineUsers.Text = Membership.GetNumberOfUsersOnline().ToString();

            // For counting users online, using global.asax instead, uncomment the following line -
            // and comment out the lblOnlineUsers.Text = Membership.GetNumberOfUsersOnline().ToString(); above
            lblOnlineUsers.Text = Application["OnlineUsers"].ToString();
            lblTotalUsers.Text = allRegisteredUsers.Count.ToString();

            // display date in header
            ltlDateTime.Text = DateTime.Now.ToLongDateString();

            // display date in footer
            dateYear.Text = DateTime.Now.Year.ToString();
        }
    }

    #endregion

    #region Menu corrections

    protected void Menu1_MenuItemDataBound(object sender, System.Web.UI.WebControls.MenuEventArgs e)
    {
        System.Web.SiteMapNode menuNode = (SiteMapNode)e.Item.DataItem;
        if (menuNode["target"] != null)
            e.Item.Target = menuNode["target"];
    }

    #endregion
}