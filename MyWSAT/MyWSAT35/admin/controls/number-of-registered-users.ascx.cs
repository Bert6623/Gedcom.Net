using System;
using System.Web.Security;
using System.Web.UI;

public partial class admin_controls_number_of_registered_users : System.Web.UI.UserControl
{
    #region count total users online

    // get number of registred users and assign it to global variable
    private MembershipUserCollection allRegisteredUsers = Membership.GetAllUsers();

    protected void Page_Load(object sender, EventArgs e)
    {
        // get the number of registered users and bind them to the label on the page
        if (!Page.IsPostBack)
        {
            lblTotalUsers.Text = allRegisteredUsers.Count.ToString();
        }
    }

    #endregion
}
