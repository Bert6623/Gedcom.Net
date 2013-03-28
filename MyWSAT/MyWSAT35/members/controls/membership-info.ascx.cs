using System;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class members_controls_membership_info : System.Web.UI.UserControl
{
    #region on page load get current profile

    protected void Page_Load(object sender, EventArgs e)
    {
        // Get Profile 
        if (!Page.IsPostBack)
        {
            if (Page.User.Identity.IsAuthenticated)
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

                // Subscriptions
                ddlNewsletter.SelectedValue = profile.Preferences.Newsletter;

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
                ddlStates1.SelectedValue = profile.Company.State;
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
    }

    #endregion

    #region Update current Profile Sub

    public void SaveProfile()
    {
        if (Page.User.Identity.IsAuthenticated)
        {
            // get the selected user's profile
            ProfileCommon profile = Profile;

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
            profile.Address.State = ddlStates1.SelectedValue;
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

            // this is what we will call from the button click
            // to save the user's profile
            profile.Save();
        }
    }

    #endregion

    #region update user info in detailsview

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        //Need to handle the update manually because MembershipUser does not have a
        //parameterless constructor  
        MembershipUser memUser = Membership.GetUser();

        memUser.Email = (string)e.NewValues[0];
        //memUser.Comment = (string)e.NewValues[1];
        //memUserIsApproved = (bool)e.NewValues[2];
        try
        {
            Membership.UpdateUser(memUser);

            e.Cancel = true;
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
        }
        catch (Exception ex)
        {
            Response.Write("<div>The following error occurred:<font color='red'> " + ex.Message + "</font></div>");
            e.Cancel = true;
        }
    }

    #endregion

    #region delete user account

    protected void btnDeleteCurrentUser_Click(object sender, EventArgs e)
    {
        if (Membership.DeleteUser(Page.User.Identity.Name))
        {
            FormsAuthentication.SignOut();
            Roles.DeleteCookie();
            Response.Redirect("~/account-removed.aspx");
        }
        else
        {
            lblResult.Visible = true;
            lblResult.Text = "The Membership user was not deleted.";
        }

    }

    #endregion

    #region Update current Profile Button Click

    protected void btnUpdateProfile_Click(object sender, EventArgs e)
    {
        SaveProfile();
        lblProfileMessage.Text = "Profile saved successfully!";
    }

    #endregion

    #region end all user sessions on logout

    protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();
    }

    #endregion
}
