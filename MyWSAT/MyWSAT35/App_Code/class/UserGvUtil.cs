#region using references

using System;
using System.Data;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Net.Mail;

#endregion

/// <summary>
/// Code file for Membership User GridView and button functions to reduce code redundancy
/// </summary>
public class UserGvUtil
{
    #region global methods

    // exception error message used for try catch
    public static void ExceptionErrorMessage(HyperLink Msg, Exception ex)
    {
        Msg.Text = "Oops! " + ex.Message;
        Msg.Visible = true;
    }

    // refresh gridview to reflect changes
    public static void RefreshGridView(GridView GridView1)
    {
        GridView1.DataBind();
    }

    // refresh all dropdownlists
    public static void RefreshAllDropdownlists(params DropDownList[] dropDownLists)
    {
        foreach (DropDownList ddl in dropDownLists)
        {
            ddl.ClearSelection();
        }
    }

    // hide panel if gridview is empty
    public static void HidePanelIfGridviewIsEmpty(Panel pnlHideItems, GridView GridView1)
    {
        pnlHideItems.Visible = GridView1.Rows.Count > 0;
    }

    // show number of users online
    public static void ShowNumberOfUsersOnline(HyperLink totalRecordCount)
    {
        totalRecordCount.Text = Membership.GetNumberOfUsersOnline().ToString() + " Records";
    }

    // display message if no selection is made
    private static void DisplayMessageIfNoSelectionIsMade(HyperLink Msg, CheckBox cb)
    {
        if (cb == null || !cb.Checked)
        {
            Msg.Text = "You did <strong>NOT</strong> make a <strong>SELECTION</strong>!";
            Msg.Visible = true;
        }
    }

    #endregion

    #region ddl popup confirm messages

    // popup confirm - ADD the Selected USERS to the Selected ROLE
    public static void ddlAddUsersToRole(DropDownList ddlAddUsersToRole)
    {
        ddlAddUsersToRole.Attributes.Add("onchange", "if(!window.confirm('This will ADD the Selected USERS to the Selected ROLE.\\n\\rAre you sure?')){selectedIndex = '0'}");
    }

    // popup confirm - ADD ALL USERS to the Selected ROLE
    public static void ddlAddAllUsersToRole(DropDownList ddlAddAllUsersToRole)
    {
        ddlAddAllUsersToRole.Attributes.Add("onchange", "if(!window.confirm('This will ADD ALL USERS to the Selected ROLE.\\n\\rAre you sure?')){selectedIndex = '0'}");
    }

    // popup confirm - REMOVE ALL USERS from the Selected ROLE
    public static void ddlRemoveAllUsersFromRole(DropDownList ddlRemoveAllUsersFromRole)
    {
        ddlRemoveAllUsersFromRole.Attributes.Add("onchange", "if(!window.confirm('This will REMOVE ALL USERS from the Selected ROLE.\\n\\rAre you sure?')){selectedIndex = '0'}");
    }

    // popup confirm - REMOVE the SELECTED USERS from the SELECTED ROLE
    public static void ddlRemoveUsersFromRole(DropDownList ddlRemoveUsersFromRole)
    {
        ddlRemoveUsersFromRole.Attributes.Add("onchange", "if(!window.confirm('This will REMOVE the Selected USERS from the Selected ROLE.\\n\\rAre you sure?')){selectedIndex = '0'}");
    }

    // popup confirm - DELETE ALL USER ACCOUNTS present in the SELECTED ROLE
    public static void ddlDeleteAllUsersFromRole(DropDownList ddlDeleteAllUsersFromRole)
    {
        ddlDeleteAllUsersFromRole.Attributes.Add("onchange", "if(!window.confirm('This will DELETE ALL USER ACCOUNTS present in the Selected ROLE. Are you sure?')){selectedIndex = '0'}");
    }

    // popup confirm - SEND EMAIL to SELECTED ROLE
    public static void DdlSendEmailToSelectedRole(DropDownList ddlSendMailToSelectedRole)
    {
        ddlSendMailToSelectedRole.Attributes.Add("onchange", "if(!window.confirm('This will E-MAIL ALL USERS IN the Selected ROLE.\\n\\rAre you sure?')){selectedIndex = '0'}");
    }

    #region -- combined methods

    // popup confirm - COMBINED METHODS
    public static void AssignConfirmMessagesToDdls(DropDownList ddlAddUsersToRole, DropDownList ddlAddAllUsersToRole, DropDownList ddlRemoveAllUsersFromRole, DropDownList ddlRemoveUsersFromRole, DropDownList ddlDeleteAllUsersFromRole)
    {
        // popup confirm - ADD the Selected USERS to the Selected ROLE
        UserGvUtil.ddlAddUsersToRole(ddlAddUsersToRole);

        // popup confirm - ADD ALL USERS to the Selected ROLE
        UserGvUtil.ddlAddAllUsersToRole(ddlAddAllUsersToRole);

        // popup confirm - REMOVE ALL USERS from the Selected ROLE
        UserGvUtil.ddlRemoveAllUsersFromRole(ddlRemoveAllUsersFromRole);

        // popup confirm - REMOVE the SELECTED USERS from the SELECTED ROLE
        UserGvUtil.ddlRemoveUsersFromRole(ddlRemoveUsersFromRole);

        // popup confirm - DELETE ALL USER ACCOUNTS present in the SELECTED ROLE
        UserGvUtil.ddlDeleteAllUsersFromRole(ddlDeleteAllUsersFromRole);
    }

    #endregion

    #endregion

    #region ASSIGN default user name letter to label and specify filter for db

    // assign default user name letter
    public static void AssignDefaultUserNameLetter(Literal categoryID, ObjectDataSource ObjectDataSource1, GridView GridView1, Boolean IsPostBack)
    {
        // if not a postback
        if (IsPostBack)
            return;

        // declare variable for filter query string
        string userFirstLetter = HttpContext.Current.Request.QueryString["az"];

        // check for category ID
        if (String.IsNullOrEmpty(userFirstLetter))
        {
            // display default category
            userFirstLetter = "%";
        }

        // display requested category
        categoryID.Text = string.Format(" ... ({0})", userFirstLetter);

        // specify filter for db search
        ObjectDataSource1.SelectParameters["UserName"].DefaultValue = userFirstLetter + "%";

        // call RefreshGridView method from above in global methods
        RefreshGridView(GridView1);
    }

    #endregion

    #region DELETE selected users button

    // delete selected users and profiles
    public static void DeleteSelectedUsersAndProfiles(GridView GridView1, HyperLink Msg)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            // find the checkboxes in gridview
            CheckBox cb = (CheckBox)row.FindControl("chkRows");

            // if at least one checkbox is selected
            if (cb != null && cb.Checked)
            {
                // get the row index values (DataKeyNames) and assign them to variable
                string userName = GridView1.DataKeys[row.RowIndex].Value.ToString();

                // delete selected users and their profiles
                ProfileManager.DeleteProfile(userName);
                Membership.DeleteUser(userName);

                // show success message
                Msg.Text = "User(s) were sucessfully <strong>DELETED</strong>!";
                Msg.Visible = true;
            }

            // display message if no selection is made
            DisplayMessageIfNoSelectionIsMade(Msg, cb);
        }
    }

    #region -- combined methods

    // delete selected users with try catch
    public static void DeleteSelectedUsers(GridView GridView1, HyperLink Msg)
    {
        try
        {
            DeleteSelectedUsersAndProfiles(GridView1, Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshGridView(GridView1);
    }

    #endregion

    #endregion

    #region DELETE ALL users and ALL related DATA - button

    // delete all users and associated data
    public static void DeleteAllUsersAndAssociatedData(HyperLink Msg)
    {
        foreach (MembershipUser usr in Membership.GetAllUsers())
        {
            Membership.DeleteUser(usr.UserName);

            Msg.Text = "<strong>ALL</strong> Users were sucessfully <strong>DELETED</strong>!";
            Msg.Visible = true;
        }
    }

    #region -- combined methods

    // delete all users and all related data
    public static void DeleteAllUsersAndAllRelatedData(HyperLink Msg, GridView GridView1)
    {
        try
        {
            DeleteAllUsersAndAssociatedData(Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshGridView(GridView1);
    }

    #endregion

    #endregion

    #region DELETE ALL users and ALL related DATA present in selected ROLE - dropdownlist

    // delete all users from selected role
    public static void DeleteAllUsersFromSelectedRole(DropDownList ddlDeleteAllUsersFromRole, HyperLink Msg)
    {
        foreach (MembershipUser usr in Membership.GetAllUsers())
        {
            // if selected user exist in the selected role
            if (Roles.IsUserInRole(usr.UserName, ddlDeleteAllUsersFromRole.SelectedItem.Text))
            {
                // delete users and their profiles present in the selected role
                ProfileManager.DeleteProfile(usr.UserName);
                Membership.DeleteUser(usr.UserName);

                Msg.Text = "<strong>ALL</strong> Users were sucessfully <strong>DELETED</strong> from <strong>" + ddlDeleteAllUsersFromRole.SelectedItem.Text.ToUpper() + "</strong> Role!";
                Msg.Visible = true;
            }
        }
    }

    #region -- combined methods

    // delete all users and related data present in selected role
    public static void DeleteAllUsersAndRelatedInfoPresentInSelectedRole(HyperLink Msg, DropDownList ddlDeleteAllUsersFromRole, DropDownList ddlRemoveUsersFromRole, DropDownList ddlAddUsersToRole, DropDownList ddlAddAllUsersToRole, DropDownList ddlRemoveAllUsersFromRole, GridView GridView1)
    {
        try
        {
            DeleteAllUsersFromSelectedRole(ddlDeleteAllUsersFromRole, Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshAllDropdownlists(ddlRemoveUsersFromRole, ddlAddUsersToRole, ddlAddAllUsersToRole, ddlRemoveAllUsersFromRole, ddlDeleteAllUsersFromRole);
        RefreshGridView(GridView1);

    }

    #endregion

    #endregion

    #region SHOW TOTAL page count in label

    // show page count in label
    public static void ShowPageCountInLabel(HyperLink PagingInformation, GridView GridView1)
    {
        PagingInformation.Text = string.Format("Page {0} of {1}...", GridView1.PageIndex + 1, GridView1.PageCount);
    }

    #endregion

    #region SHOW TOTAL record count

    // show total record count
    public static void ShowTotalRecordCount(ObjectDataSourceStatusEventArgs e, Panel pnlHideItems, HyperLink totalRecordCount)
    {
        if (e.Exception != null)
        {
            return;
        }

        object result = e.ReturnValue;
        if (result is DataTable)
        {
            //sp_wsat_GetUsersByNameSorted
            DataTable dt = (DataTable)result;

            // hide following items if gridview is empty
            pnlHideItems.Visible = dt.Rows.Count > 0;
        }
        else
        {
            // TotalNumberOfUsersByName
            int totalRowCount = Convert.ToInt32(result);
            totalRecordCount.Text = totalRowCount.ToString() + " Records";
        }
    }

    // show total record count - data table
    public static void ShowTotalRecordCountDataTable(ObjectDataSourceStatusEventArgs e, HyperLink totalRecordCount)
    {
        DataTable dt = (DataTable)e.ReturnValue;
        totalRecordCount.Text = dt.Rows.Count.ToString() + " Records";
    }

    // show total record count - EMAIL data table
    public static void ShowTotalRecordCountEmailDataTable(ObjectDataSourceStatusEventArgs e, Panel pnlHideItems, HyperLink totalRecordCount)
    {
        if (e.Exception != null)
        {
            return;
        }

        object result = e.ReturnValue;
        if (result is DataTable)
        {
            //sp_wsat_GetUsersByNameSorted
            DataTable dt = (DataTable)result;

            // hide following items if gridview is empty
            pnlHideItems.Visible = dt.Rows.Count > 0;
        }
        else
        {
            // TotalNumberOfUsersByName
            int totalRowCount = Convert.ToInt32(result);
            totalRecordCount.Text = totalRowCount.ToString() + " Records";
        }
    }

    #endregion

    #region NAVIGATION and PAGING - Gridview

    // set page size to ddl value
    public static void SetPageSizeToDdlValue(object sender, GridView GridView1)
    {
        DropDownList dropDown = (DropDownList)sender;
        GridView1.PageSize = int.Parse(dropDown.SelectedValue);
    }

    // go to page number
    public static void GoToPageNumber(object sender, GridView GridView1)
    {
        TextBox txtGoToPage = (TextBox)sender;

        int pageNumber;

        if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= GridView1.PageCount)
        {
            GridView1.PageIndex = pageNumber - 1;
        }
        else
        {
            GridView1.PageIndex = 0;
        }
    }

    // highlight row on click - IE and FF
    public static void HighlightRowOnClick(GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onclick", "ChangeRowColor(this)");
        }
    }

    // gridview sending request
    public static GridView GetGridView(object sender)
    {
        return (GridView)sender;
    }

    // highlight sorted column
    public static void HighlightSortedColumn(object sender, GridViewRowEventArgs e)
    {
        if (GetGridView(sender).SortExpression.Length > 0)
        {
            int cellIndex = -1;
            foreach (DataControlField field in GetGridView(sender).Columns)
            {
                if (field.SortExpression == GetGridView(sender).SortExpression)
                {
                    cellIndex = GetGridView(sender).Columns.IndexOf(field);
                    break;
                }
            }

            if (cellIndex > -1)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //  this is a header row, - set the sort css style
                    e.Row.Cells[cellIndex].CssClass = GetGridView(sender).SortDirection == SortDirection.Ascending ? "gvSortAscHeaderStyle" : "gvSortDescHeaderStyle";
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //  this is an alternating row - set css style
                    e.Row.Cells[cellIndex].CssClass = e.Row.RowIndex % 2 == 0 ? "gvSortAlternatingRowstyle" : "gvSortRowStyle";
                }
            }
        }
    }

    // display and remember page size
    public static void DisplayAndRememberPageSize(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager)
        {
            // display total number of pages in gridview pager
            Label lblTotalNumberOfPages = (Label)e.Row.FindControl("lblTotalNumberOfPages");
            lblTotalNumberOfPages.Text = GetGridView(sender).PageCount.ToString();

            // display and hold selected page number in gotopage textbox
            TextBox txtGoToPage = (TextBox)e.Row.FindControl("txtGoToPage");
            txtGoToPage.Text = (GetGridView(sender).PageIndex + 1).ToString();

            // display and hold selected page size in dropdownlist
            DropDownList ddlPageSize = (DropDownList)e.Row.FindControl("ddlPageSize");
            ddlPageSize.SelectedValue = GetGridView(sender).PageSize.ToString();
        }
    }

    #endregion

    #region ADD selected users TO selected ROLE - dropdownlist

    // add selected users to selected role
    public static void AddSelectedUsersToSelectedRole(GridView GridView1, DropDownList ddlAddUsersToRole, HyperLink Msg)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("chkRows");
            if (cb != null && cb.Checked)
            {
                // assign selected user names from gridview to variable
                string userName;
                userName = GridView1.DataKeys[row.RowIndex].Value.ToString();

                // if user already exist in the selected role, skip onto the others
                if (!Roles.IsUserInRole(userName, ddlAddUsersToRole.SelectedItem.Text))
                {
                    // the magic happens here!
                    Roles.AddUserToRole(userName, ddlAddUsersToRole.SelectedItem.Text);
                }

                Msg.Text = "User(s) were sucessfully <strong>ADDED</strong> to <strong>" + ddlAddUsersToRole.SelectedItem.Text.ToUpper() + "</strong> Role!";
                Msg.Visible = true;
            }

            // display message if no selection is made
            DisplayMessageIfNoSelectionIsMade(Msg, cb);
        }
    }

    #region -- combined methods

    // add selected users to selected role
    public static void AddSelectedUsersToSelectedRole2(GridView GridView1, DropDownList  ddlAddUsersToRole, HyperLink Msg, DropDownList ddlRemoveUsersFromRole, DropDownList ddlAddAllUsersToRole, DropDownList ddlRemoveAllUsersFromRole, DropDownList ddlDeleteAllUsersFromRole)
    {
        try
        {
            AddSelectedUsersToSelectedRole(GridView1, ddlAddUsersToRole, Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshAllDropdownlists(ddlRemoveUsersFromRole, ddlAddUsersToRole, ddlAddAllUsersToRole, ddlRemoveAllUsersFromRole, ddlDeleteAllUsersFromRole);
    }

    #endregion

    #endregion

    #region ADD ALL users TO selected ROLE - dropdownlist

    // add all users to the selected role
    public static void AddAllUsersToSelectedRole(DropDownList ddlAddAllUsersToRole, HyperLink Msg)
    {
        foreach (MembershipUser usr in Membership.GetAllUsers())
        {

            // if user already exist in the selected role, skip onto the others
            if (!Roles.IsUserInRole(usr.UserName, ddlAddAllUsersToRole.SelectedItem.Text))
            {
                // add users to the selected role
                Roles.AddUserToRole(usr.UserName, ddlAddAllUsersToRole.SelectedItem.Text);

                Msg.Visible = true;
                Msg.Text = "<strong>ALL</strong> Users were sucessfully <strong>ADDED</strong> to <strong>" + ddlAddAllUsersToRole.SelectedItem.Text.ToUpper() + "</strong> Role!";
            }
        }
    }

    #region -- combined methods

    // add all users to the selected role
    public static void AddAllUsresToSelectedRole2(HyperLink Msg, DropDownList ddlAddAllUsersToRole, DropDownList ddlRemoveUsersFromRole, DropDownList ddlAddUsersToRole, DropDownList ddlRemoveAllUsersFromRole, DropDownList ddlDeleteAllUsersFromRole)
    {
        try
        {
            AddAllUsersToSelectedRole(ddlAddAllUsersToRole, Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshAllDropdownlists(ddlRemoveUsersFromRole, ddlAddUsersToRole, ddlAddAllUsersToRole, ddlRemoveAllUsersFromRole, ddlDeleteAllUsersFromRole);
    }

    #endregion

    #endregion

    #region REMOVE selected users FROM selected ROLE - dropdownlist

    // remove selected users from selected role
    public static void RemoveSelectedUsersFromSelectedRole(GridView GridView1, DropDownList ddlRemoveUsersFromRole, HyperLink Msg)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("chkRows");
            if (cb != null && cb.Checked)
            {
                // assign selected user names from gridview to variable
                string userName;
                userName = GridView1.DataKeys[row.RowIndex].Value.ToString();

                // if the selected users exist in the selected role, remove, else skip over
                if (Roles.IsUserInRole(userName, ddlRemoveUsersFromRole.SelectedItem.Text))
                {
                    // the magic happens here again!
                    Roles.RemoveUserFromRole(userName, ddlRemoveUsersFromRole.SelectedItem.Text);
                }

                Msg.Visible = true;
                Msg.Text = "User(s) were sucessfully <strong>REMOVED</strong> from <strong>" + ddlRemoveUsersFromRole.SelectedItem.Text.ToUpper() + "</strong> Role!";
            }

            // display message if no selection is made
            DisplayMessageIfNoSelectionIsMade(Msg, cb);
        }
    }

    #region -- combined methods

    // remove selected users from selected role
    public static void RemoveSelectedUsersFromSelectedRole2(GridView GridView1, HyperLink Msg, DropDownList ddlRemoveUsersFromRole, DropDownList ddlAddUsersToRole, DropDownList ddlAddAllUsersToRole, DropDownList ddlRemoveAllUsersFromRole, DropDownList ddlDeleteAllUsersFromRole)
    {
        try
        {
            RemoveSelectedUsersFromSelectedRole(GridView1, ddlRemoveUsersFromRole, Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshAllDropdownlists(ddlRemoveUsersFromRole, ddlAddUsersToRole, ddlAddAllUsersToRole, ddlRemoveAllUsersFromRole, ddlDeleteAllUsersFromRole);
    }

    #endregion

    #endregion

    #region REMOVE ALL users FROM selected ROLE - dropdownlist

    // remove all users from selected role
    public static void RemoveAllUsersFromSelectedRole(DropDownList ddlRemoveAllUsersFromRole, HyperLink Msg)
    {
        foreach (MembershipUser usr in Membership.GetAllUsers())
        {
            // if selected user already exist in the selected role, skip onto the others
            if (Roles.IsUserInRole(usr.UserName, ddlRemoveAllUsersFromRole.SelectedItem.Text))
            {
                // remove users from the selected role
                Roles.RemoveUserFromRole(usr.UserName, ddlRemoveAllUsersFromRole.SelectedItem.Text);

                Msg.Visible = true;
                Msg.Text = "<strong>ALL</strong> Users were sucessfully <strong>REMOVED</strong> from <strong>" + ddlRemoveAllUsersFromRole.SelectedItem.Text.ToUpper() + "</strong> Role!";
            }
        }
    }

    #region -- combined methods

    // remove all users from selected role
    public static void RemoveAllUsersFromSelectedRole2(HyperLink Msg, DropDownList ddlRemoveUsersFromRole, DropDownList ddlAddUsersToRole, DropDownList ddlAddAllUsersToRole, DropDownList ddlRemoveAllUsersFromRole, DropDownList ddlDeleteAllUsersFromRole)
    {
        try
        {
            RemoveAllUsersFromSelectedRole(ddlRemoveAllUsersFromRole, Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshAllDropdownlists(ddlRemoveUsersFromRole, ddlAddUsersToRole, ddlAddAllUsersToRole, ddlRemoveAllUsersFromRole, ddlDeleteAllUsersFromRole);
    }

    #endregion

    #endregion

    #region REMOVE ALL users from ALL ROLES - button

    // remove all users from all roles
    public static void RemoveAllUsersFromAllRoles(HyperLink Msg)
    {
        foreach (var role in Roles.GetAllRoles())
        {
            string[] users = Roles.GetUsersInRole(role);

            // check if some users are not in any role
            if (users.Length > 0)
            {
                // then remove all "roled" users from all roles
                Roles.RemoveUsersFromRole(users, role);

                Msg.Text = "<strong>ALL</strong> Users were sucessfully <strong>REMOVED</strong> from <strong>ALL</strong> <strong>ROLES</strong> !";
                Msg.Visible = true;
            }
        }
    }

    #region -- combined methods

    // remove all users from all roles
    public static void RemoveAllUsersFromAllRoles2(HyperLink Msg)
    {
        try
        {
            UserGvUtil.RemoveAllUsersFromAllRoles(Msg);
        }
        catch (Exception ex)
        {
            UserGvUtil.ExceptionErrorMessage(Msg, ex);
        }
    }

    #endregion

    #endregion

    #region APPROVE selected users - button

    // approve selected users
    public static void ApproveSelectedUsers(GridView GridView1, HyperLink Msg)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("chkRows");
            if (cb != null && cb.Checked)
            {
                // set membership usernames equal to selected users
                MembershipUser userName = Membership.GetUser(GridView1.DataKeys[row.RowIndex].Value.ToString());
                if (userName != null)
                {
                    // the magic happens here! approve and update
                    userName.IsApproved = true;
                    Membership.UpdateUser(userName);
                }

                Msg.Text = "User(s) were successfully <strong>APPROVED</strong>!";
                Msg.Visible = true;
            }

            // display message if no selection is made
            DisplayMessageIfNoSelectionIsMade(Msg, cb);
        }
    }

    #region -- combined methods

    // approve selected users
    public static void ApproveSelectedUsers2(GridView GridView1, HyperLink Msg)
    {
        try
        {
            ApproveSelectedUsers(GridView1, Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshGridView(GridView1);
    }

    #endregion

    #endregion

    #region APPROVE ALL users - button

    // approve all users
    public static void ApproveAllUsers(HyperLink Msg)
    {
        foreach (MembershipUser userName in Membership.GetAllUsers())
        {
            if (userName.IsApproved == false)
            {
                // the magic happens here! approve and update
                userName.IsApproved = true;
                Membership.UpdateUser(userName);

                Msg.Text = "<strong>ALL</strong> Users were successfully <strong>APPROVED</strong>!";
                Msg.Visible = true;
            }
        }
    }

    #region -- combined methods

    // approve all users
    public static void ApproveAllUsers2(HyperLink Msg, GridView GridView1)
    {
        try
        {
            ApproveAllUsers(Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshGridView(GridView1);
    }

    #endregion

    #endregion

    #region UNAPPROVE selected users - button

    // unapprove selected users
    public static void UnapproveSelectedUsers(GridView GridView1, HyperLink Msg)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("chkRows");
            if (cb != null && cb.Checked)
            {
                MembershipUser userName = Membership.GetUser(GridView1.DataKeys[row.RowIndex].Value.ToString());
                if (userName != null)
                {
                    // unapprove all selected users
                    userName.IsApproved = false;
                    Membership.UpdateUser(userName);
                }

                Msg.Text = "User(s) were successfully <strong>UNAPPROVED</strong> !";
                Msg.Visible = true;
            }

            // display message if no selection is made
            DisplayMessageIfNoSelectionIsMade(Msg, cb);
        }
    }

    #region -- combined methods

    // unapprove selected users
    public static void UnapproveSelectedUsers2(GridView GridView1, HyperLink Msg)
    {
        try
        {
            UnapproveSelectedUsers(GridView1, Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshGridView(GridView1);
    }

    #endregion

    #endregion

    #region UNAPPROVE ALL users - button

    // unapprove all users
    public static void UnapproveAllUsers(HyperLink Msg)
    {
        foreach (MembershipUser userName in Membership.GetAllUsers())
        {
            if (userName.IsApproved == true)
            {
                // the magic happens here! approve and update
                userName.IsApproved = false;
                Membership.UpdateUser(userName);

                Msg.Text = "<strong>ALL</strong> Users were successfully <strong>UNAPPROVED</strong>!";
                Msg.Visible = true;
            }
        }
    }

    #region -- combined methods

    // unapprove all users
    public static void UnapproveAllUsers2(HyperLink Msg, GridView GridView1)
    {
        try
        {
            UnapproveAllUsers(Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshGridView(GridView1);
    }

    #endregion

    #endregion

    #region UNLOCK selected users - button

    // unlock selected users
    public static void UnlockSelectedUsers(GridView GridView1, HyperLink Msg)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("chkRows");
            if (cb != null && cb.Checked)
            {
                MembershipUser userName = Membership.GetUser(GridView1.DataKeys[row.RowIndex].Value.ToString());
                if (userName != null)
                {
                    // unlock selected users
                    userName.UnlockUser();
                    Membership.UpdateUser(userName);
                }

                Msg.Text = "User(s) were successfully <strong>UNLOCKED</strong> !";
                Msg.Visible = true;
            }

            // display message if no selection is made
            DisplayMessageIfNoSelectionIsMade(Msg, cb);
        }
    }

    #region -- combined methods

    // unlock selected users
    public static void UnlockSelectedUsers2(GridView GridView1, HyperLink Msg)
    {
        try
        {
            UnlockSelectedUsers(GridView1, Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshGridView(GridView1);
    }

    #endregion

    #endregion

    #region UNLOCK ALL users - button

    // unlock all users
    public static void UnlockAllUsers(HyperLink Msg)
    {
        foreach (MembershipUser userName in Membership.GetAllUsers())
        {
            if (userName.IsLockedOut == true)
            {
                // the magic happens here! approve and update
                userName.UnlockUser();
                Membership.UpdateUser(userName);

                Msg.Text = "<strong>ALL</strong> Users were successfully <strong>UNLOCKED</strong>!";
                Msg.Visible = true;
            }
        }
    }

    #region -- combined methods

    // unlock all users
    public static void UnlockAllUsers2(HyperLink Msg, GridView GridView1)
    {
        try
        {
            UnlockAllUsers(Msg);
        }
        catch (Exception ex)
        {
            ExceptionErrorMessage(Msg, ex);
        }

        RefreshGridView(GridView1);
    }

    #endregion

    #endregion

    #region EMAIL to selected users - button

    // email selected users
    public static void SendEmailToSelectedUsers(GridView GridView1, TextBox txbMailFrom, TextBox txb_Subject, FredCK.FCKeditorV2.FCKeditor WYSIWYGEditor_EmailBody, RadioButtonList rbt_BodyTextType, RadioButtonList rbt_Importance, HyperLink Msg)
    {
        // for each row in the gridview
        foreach (GridViewRow row in GridView1.Rows)
        {
            // find checked checkboxes in gridview
            CheckBox cb = (CheckBox)row.FindControl("chkRows");

            // if checkboxes are nut null and at least one of them are checked
            if (cb == null || !cb.Checked)
                continue;

            try
            {
                // declare new mailer
                System.Net.Mail.MailMessage MyMailer = new System.Net.Mail.MailMessage();

                // grab the email address of the selected checkboxes
                string Email = GridView1.DataKeys[row.RowIndex].Values["Email"].ToString();

                // grab the user name of the selected checkboxes
                string Name = GridView1.DataKeys[row.RowIndex].Values["userName"].ToString();

                //MyMailer.To.Add(Email);
                MyMailer.Bcc.Add(Email);
                MyMailer.From = new MailAddress(txbMailFrom.Text);
                MyMailer.Subject = txb_Subject.Text;
                MyMailer.Body = "Dear " + Name + ", <br/><br/>" + WYSIWYGEditor_EmailBody.Value;
                MyMailer.IsBodyHtml = Convert.ToBoolean(rbt_BodyTextType.SelectedValue);

                // grab the selected priority level
                switch (rbt_Importance.SelectedValue)
                {
                    case "Low":
                        MyMailer.Priority = MailPriority.Low;
                        break;
                    case "Normal":
                        MyMailer.Priority = MailPriority.Normal;
                        break;
                    case "High":
                        MyMailer.Priority = MailPriority.High;
                        break;
                }

                // create new smtp client
                SmtpClient client = new SmtpClient();

                // send the email
                client.Send(MyMailer);

                // display success message
                Msg.Text = "<strong>E-MAIL</strong> message <strong>SENT</strong> successfully!...";
                Msg.Visible = true;
            }
            catch (Exception ex)
            {
                // display error sending message
                Msg.Text = "ERROR: Problem sending email! " + ex.Message;
                Msg.Visible = true;
            }
        }
    }

    #endregion

    #region EMAIL to ALL users - button

    public static void SendEmailToAllUsers(TextBox txbMailFrom, TextBox txb_Subject, FredCK.FCKeditorV2.FCKeditor WYSIWYGEditor_EmailBody, RadioButtonList rbt_BodyTextType, RadioButtonList rbt_Importance, HyperLink Msg)
    {
        try
        {
            // declare new mailer
            System.Net.Mail.MailMessage MyMailer = new System.Net.Mail.MailMessage();

            // for each user in the membership database
            foreach (MembershipUser mu in Membership.GetAllUsers())
            {

                // assign particulars
                MyMailer.Bcc.Add(mu.Email);
                MyMailer.From = new MailAddress(txbMailFrom.Text);
                MyMailer.Subject = txb_Subject.Text;
                MyMailer.Body = WYSIWYGEditor_EmailBody.Value;
                MyMailer.IsBodyHtml = Convert.ToBoolean(rbt_BodyTextType.SelectedValue);

                // grab the selected priority level
                switch (rbt_Importance.SelectedValue)
                {
                    case "Low":
                        MyMailer.Priority = MailPriority.Low;
                        break;
                    case "Normal":
                        MyMailer.Priority = MailPriority.Normal;
                        break;
                    case "High":
                        MyMailer.Priority = MailPriority.High;
                        break;
                }

                // create new smtp client
                SmtpClient client = new SmtpClient();

                // send the email
                client.Send(MyMailer);

                // display success message
                Msg.Text = "<strong>E-MAIL</strong> message <strong>SENT</strong> successfully!...";
                Msg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            // display error sending message
            Msg.Text = "ERROR: Problem sending email! " + ex.Message;
            Msg.Visible = true;
        }
    }

    #endregion

    #region EMAIL selected ROLE

    public static void SendEmailToSelectedRole(DropDownList ddlSendMailToSelectedRole, TextBox txbMailFrom, TextBox txb_Subject, FredCK.FCKeditorV2.FCKeditor WYSIWYGEditor_EmailBody, RadioButtonList rbt_BodyTextType, RadioButtonList rbt_Importance, HyperLink Msg)
    {
        try
        {
            // for each user in the membership database
            foreach (MembershipUser mu in Membership.GetAllUsers())
            {
                // if a user in database is in the selected role
                if (Roles.IsUserInRole(mu.UserName, ddlSendMailToSelectedRole.SelectedItem.Text))
                {
                    // declare new mailer
                    System.Net.Mail.MailMessage MyMailer = new System.Net.Mail.MailMessage();

                    //MyMailer.To.Add(mu.Email);
                    MyMailer.Bcc.Add(mu.Email);
                    MyMailer.From = new MailAddress(txbMailFrom.Text);
                    MyMailer.Subject = txb_Subject.Text;
                    MyMailer.Body = WYSIWYGEditor_EmailBody.Value;
                    MyMailer.IsBodyHtml = Convert.ToBoolean(rbt_BodyTextType.SelectedValue);

                    // grab the selected priority level
                    switch (rbt_Importance.SelectedValue)
                    {
                        case "Low":
                            MyMailer.Priority = MailPriority.Low;
                            break;
                        case "Normal":
                            MyMailer.Priority = MailPriority.Normal;
                            break;
                        case "High":
                            MyMailer.Priority = MailPriority.High;
                            break;
                    }

                    // create new smtp client
                    SmtpClient client = new SmtpClient();

                    // send the email
                    client.Send(MyMailer);

                    // display success message
                    Msg.Text = "<strong>E-MAIL</strong> message <strong>SENT</strong> successfully!...";
                    Msg.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            // display error sending message
            Msg.Text = "ERROR: Problem sending email! " + ex.Message;
            Msg.Visible = true;
        }
    }

    #endregion
}