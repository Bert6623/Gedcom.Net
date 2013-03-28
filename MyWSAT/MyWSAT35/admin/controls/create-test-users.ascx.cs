using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_controls_create_test_users : System.Web.UI.UserControl
{
    #region databind ROLES to checkboxes on prerender

    protected override void OnPreRender(EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBind();
        }
        base.OnPreRender(e);
    }

    #endregion

    #region put selected ROLES into an array for later

    private string[] GetSelectedRoles()
    {
        List<string> userCheckedRoles = new List<string>();
        foreach (ListItem item in chkRolesList.Items)
        {
            if (item.Selected)
                userCheckedRoles.Add(item.Text);
        }
        return userCheckedRoles.ToArray();
    }

    #endregion

    #region create TEST-USER - button click

    protected void btnCreateTestUsers_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            // declare and assign some variables
            int userCount = int.Parse(txbUsersToCreate.Text);
            int maxDays = int.Parse(txbMaxAccountAge.Text);
            string[] selectedRoles = GetSelectedRoles();

            // get connection string from web.config file
            string connectionString = ConfigurationManager.ConnectionStrings["dbMyCMSConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // create sql for database update
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE aspnet_Membership SET CreateDate=DATEADD(minute,@CreateOffset,CreateDate), LastLoginDate=DATEADD(minute,@LoginOffset,LastLoginDate), IsApproved=@Approved WHERE UserID=@UserID", con);
                cmd.Parameters.Add("@UserID", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters.Add("@CreateOffset", SqlDbType.Int);
                cmd.Parameters.Add("@LoginOffset", SqlDbType.Int);
                cmd.Parameters.Add("@Approved", SqlDbType.Bit);

                // create random guid
                Random randomGuid = new Random(Guid.NewGuid().GetHashCode());

                // keep repeating the following process until we reach the submitted number
                for (int i = 0; i < userCount; i++)
                {
                    bool created = false;
                    while (!created)
                    {
                        string userName = GetRandomName(1, 4, 0);
                        string email = GetRandomEmail();
                        string password = GetRandomName(4, 6, Membership.MinRequiredNonAlphanumericCharacters);
                        string question = Membership.RequiresQuestionAndAnswer ? GetRandomName(4, 6, 0) : null;
                        string answer = Membership.RequiresQuestionAndAnswer ? GetRandomName(4, 6, 0) : null;

                        if (Membership.GetUserNameByEmail(email) == null && Membership.GetUser(userName) == null)
                        {
                            created = true;

                            MembershipCreateStatus status;
                            MembershipUser user = Membership.CreateUser(userName, password, email, question, answer, true, out status);
                            if (status == MembershipCreateStatus.Success)
                            {
                                int createOffset = randomGuid.Next(60 * 24 * maxDays);
                                int loginOffset = randomGuid.Next(createOffset);

                                cmd.Parameters["@UserID"].Value = user.ProviderUserKey;
                                cmd.Parameters["@CreateOffset"].Value = -createOffset;
                                cmd.Parameters["@LoginOffset"].Value = -loginOffset;
                                cmd.Parameters["@Approved"].Value = chkApproved.Checked;

                                cmd.ExecuteNonQuery();
                            }

                            if (selectedRoles.Length > 0)
                            {
                                Roles.AddUserToRoles(userName, selectedRoles);
                            }
                        }
                    }
                }
            }

            // show success label
            Msg.Text = "<b>" + txbUsersToCreate.Text.ToString() + "</b> Test-Users were sucessfully <b>CREATED</b>!";
            Msg.Visible = true;
        }
    }

    #endregion

    #region EMAIL string builder

    public static string GetRandomEmail()
    {
        StringBuilder email = new StringBuilder(GetRandomName(1, 4, 0));
        email.Append("@");
        email.Append(GetRandomDomainName());
        return email.ToString();
    }

    #endregion

    #region DOMAIN NAME string builder

    public static string GetRandomDomainName()
    {
        StringBuilder domain = new StringBuilder();
        domain.Append(GetRandomName(1, 3, 0));
        domain.Append(_mix105[_randomGuid.Next(_mix105.Length)]);
        return domain.ToString();
    }

    #endregion

    #region USER NAME/PASSWORD string builder

    public static string GetRandomName(int minSyllables, int maxSyllables, int minNonAlphaNumeric)
    {
        int syll = _randomGuid.Next(maxSyllables - minSyllables) + minSyllables + 1;

        StringBuilder name = new StringBuilder(maxSyllables * 4);

        name.Append(_mix101[_randomGuid.Next(_mix101.Length)]);
        name.Append(_mix103[_randomGuid.Next(_mix103.Length)]);

        for (int i = 0; i < syll - 1; i++)
        {
            name.Append(_mix102[_randomGuid.Next(_mix102.Length)]);
            name.Append(_mix103[_randomGuid.Next(_mix103.Length)]);
        }

        for (int i = 0; i < minNonAlphaNumeric; i++)
        {
            char c = _mix104[_randomGuid.Next(_mix104.Length)];
            name.Insert(_randomGuid.Next(name.Length), c);
        }

        return name.ToString();
    }

    static string[] _mix101 = new string[] { "a", "b", "d", "f", "ph", "g", "gr", "h", "j", "k", "cr", "cl", "l", "m", "n", "p", "r", "s", "sh", "st", "t", "v", "w" };
    static string[] _mix102 = new string[] { "b", "br", "d", "dr", "f", "fl", "ph", "g", "gr", "h", "j", "k", "cr", "ck", "l", "m", "n", "p", "q", "r", "s", "sh", "st", "t", "v", "w", "z" };
    static string[] _mix103 = new string[] { "a", "e", "i", "ee", "o", "oe", "u", "ay", "ey" };
    static char[] _mix104 = new char[] { '!', '@', '#', '$', '%', '^', '*', '-', '+', '~' };
    static string[] _mix105 = new string[] { ".com", ".net", ".org", ".us", ".tv" };
    static Random _randomGuid = new Random();

    #endregion
}
