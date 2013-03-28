using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class admin_controls_roles : System.Web.UI.UserControl
{
    #region globals

    private bool createRoleSuccess = true;

    #endregion

    #region Bind ROLES to Gridview

    private void Page_PreRender()
    {
        // Create a DataTable and define its columns
        DataTable RoleList = new DataTable();
        RoleList.Columns.Add("Role Name");
        RoleList.Columns.Add("User Count");

        string[] allRoles = Roles.GetAllRoles();

        // Get the list of roles in the system and how many users belong to each role
        foreach (string roleName in allRoles)
        {
            int numberOfUsersInRole = Roles.GetUsersInRole(roleName).Length;
            string[] roleRow = { roleName, numberOfUsersInRole.ToString() };
            RoleList.Rows.Add(roleRow);
        }

        // Bind the DataTable to the GridView
        UserRoles.DataSource = RoleList;
        UserRoles.DataBind();

        if (createRoleSuccess)
        {
            // Clears form field after a role was successfully added.
            NewRole.Text = "";
        }
    }

    #endregion

    #region CREATE new ROLE

    // create new role
    public void AddRole(object sender, EventArgs e)
    {
        try
        {
            Roles.CreateRole(NewRole.Text);
            Msg.Text = "The new role was added.";
            Msg.Visible = true;
            createRoleSuccess = true;
        }
        catch (Exception ex)
        {
            Msg.Text = ex.Message;
            Msg.Visible = true;
            createRoleSuccess = false;
        }
    }

    #endregion

    #region DELETE ROLE one by one

    // delete selected role
    public void DeleteRole(object sender, CommandEventArgs e)
    {
        try
        {
            // delete role only if no user exists in it (by adding the boolean value at the end)
            Roles.DeleteRole(e.CommandArgument.ToString(), true);
            Msg.Text = "Role '" + e.CommandArgument.ToString() + "' was DELETED.";
            Msg.Visible = true;
        }
        catch (Exception ex)
        {
            Msg.Text = "Oops! " + ex.Message;
            Msg.Visible = true;
        }
    }

    #endregion

    #region DELETE selected ROLES

    protected void btnDeleteSelected_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in UserRoles.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("chkRows");
                Label lbl = (Label)row.FindControl("RoleName");
                if (cb != null && cb.Checked)
                {
                    string userRole = lbl.Text.ToString();
                    Roles.DeleteRole(userRole, true);

                    this.UserRoles.DataBind();

                    Msg.Text = "ROLE(S) were sucessfully <b>DELETED</b>!";
                    Msg.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Msg.Text = ex.Message;
            Msg.Visible = true;
        }
    }

    #endregion

    #region highlight selected rows

    protected void UserRoles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //-----------------------------------------------------------------------------
        // highlight row on click - IE and FF
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onclick", "ChangeRowColor(this)");
        }
        //-----------------------------------------------------------------------------
    }

    #endregion
}