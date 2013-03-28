using System;
using System.Web.Security;
using System.Web.UI;

public partial class admin_controls_number_of_users_online : System.Web.UI.UserControl
{
    #region count users online

    // get number of registred users and assign it to global variable
    private MembershipUserCollection allRegisteredUsers = Membership.GetAllUsers();

    protected void Page_Load(object sender, EventArgs e)
    {
        // get the number of registered and online users and bind them to the labels on the page
        if (!Page.IsPostBack)
        {
            // For counting users online, using the membership API, uncomment the following line -
            // and comment out the lblOnlineUsers.Text = Application["OnlineUsers"].ToString(); below
            //lblOnlineUsers.Text = Membership.GetNumberOfUsersOnline().ToString();

            // For counting users online, using global.asax instead, uncomment the following line -
            // and comment out the lblOnlineUsers.Text = Membership.GetNumberOfUsersOnline().ToString(); above
            lblOnlineUsers.Text = Application["OnlineUsers"].ToString();
        }
    }

    #endregion
}
