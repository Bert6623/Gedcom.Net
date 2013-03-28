using System;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_controls_edit_user_modal : System.Web.UI.UserControl
{
    #region Global Variables for membership user

    // declare global variables
    string username;
    MembershipUser user;

    #endregion

    #region On Page_Prerender

    private void Page_PreRender(object sender, EventArgs e)
    {
        // Load the User Roles into checkboxes.
        UserRoles.DataSource = Roles.GetAllRoles();
        UserRoles.DataBind();

        // if detailsview is not in edit mode, disable checkboxes
        if (UserInfo.CurrentMode != DetailsViewMode.Edit)
        {
            foreach (ListItem checkbox in UserRoles.Items)
            {
                checkbox.Enabled = false;
            }
        }

        // Bind checkboxes to the User's own set of roles.
        string[] userRoles = Roles.GetRolesForUser(username);
        foreach (string role in userRoles)
        {
            ListItem checkbox = UserRoles.Items.FindByValue(role);
            checkbox.Selected = true;
        }
    }

    #endregion

    #region On Page_Load

    private void Page_Load(object sender, EventArgs e)
    {
        // check if username exists in the query string
        username = Request.QueryString["username"];
        if (username == null || username == "")
        {
            Response.Redirect("users-a-z.aspx");
        }

        // get membership user account based on username sent in query string
        user = Membership.GetUser(username);
        UserUpdateMessage.Text = "";

        // get selected user's password
        try
        {
            if (user.IsLockedOut == true)
            {
                lblCurrentPassword.Text = "To see current Password, Unlock the User first.";
            }
            else
            {
                string password = Membership.Providers["dbSqlMemberShipProviderAdmin"].GetPassword(username, null);
                lblCurrentPassword.Text = password;
            }
        }
        catch (Exception ex)
        {
            UserUpdateMessage.Text = "OOps! This user has been deleted already! " + "Error: " + ex.Message;
        }

        // Get User's Profile
        if (!Page.IsPostBack)
        {
            // get country names from app_code folder
            // bind country names to the dropdown list
            ddlCountries.DataSource = CountryNames.CountryNames.GetCountries();
            ddlCountries.DataBind();
            
            // get state names from app_code folder
            // bind state names to the dropdown lists in address info and company info section
            ddlStates1.DataSource = UnitedStates.StateNames.GetStates();
            ddlStates1.DataBind();
            ddlStates2.DataSource = UnitedStates.StateNames.GetStates();
            ddlStates2.DataBind();

            // get the selected user's profile based on query string
            ProfileCommon profile = Profile;
            if (username.Length > 0)
                profile = Profile.GetProfile(username);

            // Personal Info
            txtFirstName.Text = profile.Personal.FirstName;
            txtLastName.Text = profile.Personal.LastName;
            ddlGenders.SelectedValue = profile.Personal.Gender;
            if (profile.Personal.BirthDate != DateTime.MinValue)
                txtBirthDate.Text = profile.Personal.BirthDate.ToShortDateString();
            ddlOccupations.SelectedValue = profile.Personal.Occupation;
            txtWebsite.Text = profile.Personal.Website;

            // Address Info
            ddlCountries.SelectedValue = profile.Address.Country;
            txtAddress.Text = profile.Address.Address;
            txtAptNumber.Text = profile.Address.AptNumber;
            txtCity.Text = profile.Address.City;
            ddlStates1.SelectedValue = profile.Address.State;
            txtPostalCode.Text = profile.Address.PostalCode;

            // Contact Info
            txtDayTimePhone.Text = profile.Contacts.DayPhone;
            txtDayTimePhoneExt.Text = profile.Contacts.DayPhoneExt;
            txtEveningPhone.Text = profile.Contacts.EveningPhone;
            txtEveningPhoneExt.Text = profile.Contacts.EveningPhoneExt;
            txtCellPhone.Text = profile.Contacts.CellPhone;
            txtHomeFax.Text = profile.Contacts.Fax;

            // Company Info
            txbCompanyName.Text = profile.Company.Company;
            txbCompanyAddress.Text = profile.Company.Address;
            txbCompanyCity.Text = profile.Company.City;
            ddlStates2.SelectedValue = profile.Company.State;
            txbCompanyZip.Text = profile.Company.PostalCode;
            txbCompanyPhone.Text = profile.Company.Phone;
            txbCompanyFax.Text = profile.Company.Fax;
            txbCompanyWebsite.Text = profile.Company.Website;

            // Subscriptions
            ddlNewsletter.SelectedValue = profile.Preferences.Newsletter;
        }
    }

    #endregion

    #region Update Profile Procedure

    public void SaveProfile()
    {
        // get the selected user's profile
        ProfileCommon profile = Profile;
        if (username.Length > 0)
            profile = Profile.GetProfile(username);

        // Personal Info
        profile.Personal.FirstName = txtFirstName.Text;
        profile.Personal.LastName = txtLastName.Text;
        profile.Personal.Gender = ddlGenders.SelectedValue;
        if (txtBirthDate.Text.Trim().Length > 0)
            profile.Personal.BirthDate = DateTime.Parse(txtBirthDate.Text);
        profile.Personal.Occupation = ddlOccupations.SelectedValue;
        profile.Personal.Website = txtWebsite.Text;

        // Address Info
        profile.Address.Country = ddlCountries.SelectedValue;
        profile.Address.Address = txtAddress.Text;
        profile.Address.AptNumber = txtAptNumber.Text;
        profile.Address.City = txtCity.Text;
        profile.Address.State = ddlStates1.Text;
        profile.Address.PostalCode = txtPostalCode.Text;

        // Contact Info
        profile.Contacts.DayPhone = txtDayTimePhone.Text;
        profile.Contacts.DayPhoneExt = txtDayTimePhoneExt.Text;
        profile.Contacts.EveningPhone = txtEveningPhone.Text;
        profile.Contacts.EveningPhoneExt = txtEveningPhoneExt.Text;
        profile.Contacts.CellPhone = txtCellPhone.Text;
        profile.Contacts.Fax = txtHomeFax.Text;

        // Company Info
        profile.Company.Company = txbCompanyName.Text;
        profile.Company.Address = txbCompanyAddress.Text;
        profile.Company.City = txbCompanyCity.Text;
        profile.Company.State = ddlStates2.SelectedValue;
        profile.Company.PostalCode = txbCompanyZip.Text;
        profile.Company.Phone = txbCompanyPhone.Text;
        profile.Company.Fax = txbCompanyFax.Text;
        profile.Company.Website = txbCompanyWebsite.Text;

        // Subscriptions
        profile.Preferences.Newsletter = ddlNewsletter.SelectedValue;

        // this is what we will call from the button click to save the user's profile
        profile.Save();
    }

    #endregion

    #region Update Profile Button Click

    protected void btnUpdateProfile_Click(object sender, EventArgs e)
    {
        SaveProfile();
        lblProfileMessage.Text = "Profile saved successfully!";
    }

    #endregion

    #region Delete Profile Button Click

    protected void btnDeleteProfile_Click(object sender, EventArgs e)
    {
        ProfileManager.DeleteProfile(username);
        lblProfileMessage.Text = "Profile deleted successfully!";

        // refresh the page to clear post back data from form fields
        Response.Redirect("edit_user_modal_success.aspx");
    }

    #endregion

    #region Update Membership User Info

    protected void UserInfo_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        // Need to handle the update manually because MembershipUser does not have a
        // parameterless constructor  

        user.Email = (string)e.NewValues[0];
        user.Comment = (string)e.NewValues[1];
        user.IsApproved = (bool)e.NewValues[2];

        try
        {
            // Update user info:
            Membership.UpdateUser(user);

            // Update user roles:
            UpdateUserRoles();

            UserUpdateMessage.Text = "Update Successful.";

            // make cancel button available
            e.Cancel = true;

            // make detailsview read only
            UserInfo.ChangeMode(DetailsViewMode.ReadOnly);
        }
        catch (Exception ex)
        {
            // if there is a problem
            UserUpdateMessage.Text = "Update Failed: " + ex.Message;

            e.Cancel = true;
            UserInfo.ChangeMode(DetailsViewMode.ReadOnly);
        }
    }

    #endregion

    #region Update User Roles

    private void UpdateUserRoles()
    {
        // add or remove user from role based on selection
        foreach (ListItem rolebox in UserRoles.Items)
        {
            if (rolebox.Selected)
            {
                if (!Roles.IsUserInRole(username, rolebox.Text))
                {
                    Roles.AddUserToRole(username, rolebox.Text);
                }
            }
            else
            {
                if (Roles.IsUserInRole(username, rolebox.Text))
                {
                    Roles.RemoveUserFromRole(username, rolebox.Text);
                }
            }
        }
    }

    #endregion

    #region Delete User

    public void DeleteUser(object sender, EventArgs e)
    {
        // Membership.DeleteUser(username, false);
        ProfileManager.DeleteProfile(username);
        Membership.DeleteUser(username, true);
        Response.Redirect("edit_user_modal_success.aspx");
    }

    #endregion

    #region Unlock User

    public void UnlockUser(object sender, EventArgs e)
    {

        // Unlock the user.
        user.UnlockUser();

        // DataBind the DetailsView to reflect changes.
        UserInfo.DataBind();
    }

    #endregion

    #region Add New Role

    public void AddRole(object sender, EventArgs e)
    {
        // create new roles
        try
        {
            Roles.CreateRole(NewRole.Text);
            ConfirmationMessage.InnerText = "The new role was added.";
        }
        catch (Exception ex)
        {
            ConfirmationMessage.InnerText = ex.Message;
        }
    }

    #endregion

    #region Change Password Button Click

    public void ChangePassword_OnClick(object sender, EventArgs args)
    {
        // check for user name in query string
        username = Request.QueryString["username"];
        if (username == null || username == "")
        {
            Response.Redirect("users.aspx");
        }

        // if user name exists in query string, get user from database
        MembershipUser u = Membership.GetUser(username);

        // try to update user password
        try
        {
            if (u.ChangePassword(OldPasswordTextbox.Text, PasswordTextbox.Text))
            {
                Msg.Text = "Password changed successfully and will show upon refresh.";
            }
            else
            {
                Msg.Text = "Password change failed. Please re-enter your values and try again.";
            }
        }
        catch (Exception e)
        {
            Msg.Text = "An exception occurred: " + Server.HtmlEncode(e.Message) + ". Please re-enter your values and try again.";
        }
    }

    #endregion

    #region Change Password Question and Answer

    protected void ChangePasswordQuestion_OnClick(object sender, EventArgs e)
    {
        try
        {
            // assign user name in query string to variable
            username = Request.QueryString["username"];

            // check if user exists in database
            MembershipUser mu = Membership.GetUser(username);

            // change password question and answer
            Boolean result = mu.ChangePasswordQuestionAndAnswer(qaCurrentPassword.Text, qaNewQuestion.Text, qaNewAnswer.Text);

            if (result)
            {
                Msg.Text = "Password Question and Answer changed.";
            }
            else
            {
                Msg.Text = "Password Question and Answer change failed.";
            }
        }
        catch (Exception ex)
        {
            Msg.Text = "Change failed. Please re-enter your values and try again. " + ex.Message; ;
        }
    }

    #endregion
}
