using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_controls_create_user_with_role : System.Web.UI.UserControl
{
    #region page_load - get roles and databind it to role list

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // Reference the SpecifyRolesStep WizardStep
            WizardStep SpecifyRolesStep = RegisterUserWithRoles.FindControl("SpecifyRolesStep") as WizardStep;

            // Reference the RoleList CheckBoxList
            CheckBoxList RoleList = SpecifyRolesStep.FindControl("RoleList") as CheckBoxList;

            // Bind the set of roles to RoleList
            RoleList.DataSource = Roles.GetAllRoles();
            RoleList.DataBind();

            // find add focus to user name textbox
            TextBox UserName = CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName") as TextBox;
            UserName.Focus();
        }
    }

    #endregion

    #region add user to role

    protected void RegisterUserWithRoles_ActiveStepChanged(object sender, EventArgs e)
    {
        // Have we JUST reached the Complete step?
        if (RegisterUserWithRoles.ActiveStep.Title == "Complete")
        {
            // Reference the SpecifyRolesStep WizardStep
            WizardStep SpecifyRolesStep = RegisterUserWithRoles.FindControl("SpecifyRolesStep") as WizardStep;

            // Reference the RoleList CheckBoxList
            CheckBoxList RoleList = SpecifyRolesStep.FindControl("RoleList") as CheckBoxList;

            // Add the checked roles to the just-added user
            foreach (ListItem li in RoleList.Items)
            {
                if (li.Selected)
                {
                    Roles.AddUserToRole(RegisterUserWithRoles.UserName, li.Text);
                }
            }
        }
    }

    #endregion

    #region do not show newly created user as online

    // this code has been depricated as the number of online users now work 
    // with the global.asax file. It won't hurt anything though.
    protected void RegisterUserWithRoles_CreatedUser(object sender, EventArgs e)
    {
        // do not show newly created user as Online
        MembershipUser muser = Membership.GetUser(RegisterUserWithRoles.UserName);
        muser.LastActivityDate = DateTime.Now.AddDays(-1);
        Membership.UpdateUser(muser);
    }

    #endregion
}