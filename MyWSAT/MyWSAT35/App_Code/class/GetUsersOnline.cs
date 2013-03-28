#region using references
using System;
using System.Data;
using System.Web.Security;

#endregion

/// <summary>
/// GetUsersOnline - because we can't get the online column through a generated dataset,
/// we create one here. Tis will give us paging and sorting automatically as well. 
/// It does not however provide us with efficient paging. This issue will be figured 
/// out in future versions.
/// </summary>
public class GetUsersOnline
{
    #region get users online

    public GetUsersOnline()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DataSet CustomGetUsersOnline()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = ds.Tables.Add("Users");


        MembershipUserCollection muc = Membership.GetAllUsers();
        MembershipUserCollection OnlineUsers = new MembershipUserCollection();

        bool isOnline = true;

        dt.Columns.Add("UserName", Type.GetType("System.String"));
        dt.Columns.Add("Email", Type.GetType("System.String"));
        //dt.Columns.Add("PasswordQuestion", Type.GetType("System.String"));
        //dt.Columns.Add("Comment", Type.GetType("System.String"));
        //dt.Columns.Add("ProviderName", Type.GetType("System.String"));
        dt.Columns.Add("CreationDate", Type.GetType("System.DateTime"));
        dt.Columns.Add("LastLoginDate", Type.GetType("System.DateTime"));
        dt.Columns.Add("LastActivityDate", Type.GetType("System.DateTime"));
        //dt.Columns.Add("LastPasswordChangedDate", Type.GetType("System.DateTime"));
        //dt.Columns.Add("LastLockoutDate", Type.GetType("System.DateTime"));
        dt.Columns.Add("IsApproved", Type.GetType("System.Boolean"));
        dt.Columns.Add("IsLockedOut", Type.GetType("System.Boolean"));
        dt.Columns.Add("IsOnline", Type.GetType("System.Boolean"));


        foreach (MembershipUser mu in muc)
        {
            // if user is currently online, add to gridview list
            if (mu.IsOnline == isOnline)
            {
                OnlineUsers.Add(mu);

            DataRow dr;
            dr = dt.NewRow();
            dr["UserName"] = mu.UserName;
            dr["Email"] = mu.Email;
            dr["CreationDate"] = mu.CreationDate;
            dr["LastLoginDate"] = mu.LastLoginDate;
            dr["LastActivityDate"] = mu.LastActivityDate;
            dr["IsApproved"] = mu.IsApproved;
            dr["IsLockedOut"] = mu.IsLockedOut;
            dr["IsOnline"] = mu.IsOnline;
            // you can add the other columns that you want to include in your return value
            dt.Rows.Add(dr);

            }
        }
        return ds;
    }

    #endregion
}
