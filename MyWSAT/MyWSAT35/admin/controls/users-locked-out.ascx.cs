using System;
using System.Data;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class admin_controls_users_locked_out : System.Web.UI.UserControl
{
    #region PageLoad - HIDE following ITEMS when gridview is empty

    protected void Page_Load(object sender, EventArgs e)
    {
        UserGvUtil.HidePanelIfGridviewIsEmpty(pnlHideItems, GridView1);
        UserGvUtil.AssignConfirmMessagesToDdls(ddlAddUsersToRole, ddlAddAllUsersToRole, ddlRemoveAllUsersFromRole, ddlRemoveUsersFromRole, ddlDeleteAllUsersFromRole);
    }

    #endregion

    #region DELETE selected users - button

    protected void btnDeleteSelected_Click(object sender, EventArgs e)
    {
        UserGvUtil.DeleteSelectedUsers(GridView1, Msg);
    }

    #endregion

    #region DELETE ALL all users and ALL related DATA - button

    protected void deleteAllUsers_Click(object sender, EventArgs e)
    {
        UserGvUtil.DeleteAllUsersAndAllRelatedData(Msg, GridView1);
    }

    #endregion

    #region DELETE ALL users and ALL related DATA present in selected ROLE - dropdownlist

    protected void ddlDeleteAllUsersFromRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserGvUtil.DeleteAllUsersAndRelatedInfoPresentInSelectedRole(Msg, ddlDeleteAllUsersFromRole, ddlRemoveUsersFromRole, ddlAddUsersToRole, ddlAddAllUsersToRole, ddlRemoveAllUsersFromRole, GridView1);
    }

    #endregion

    #region SHOW TOTAL page count in label

    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        UserGvUtil.ShowPageCountInLabel(PagingInformation, GridView1);
    }

    #endregion

    #region SHOW TOTAL record count

    protected void ObjectDataSource1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        UserGvUtil.ShowTotalRecordCount(e, pnlHideItems, totalRecordCount);
    }

    #endregion

    #region NAVIGATION and PAGING - Gridview

    #region methods

    // gridview sending request
    private GridView GetGridView(object sender)
    {
        return UserGvUtil.GetGridView(sender);
    }

    #endregion

    // set page size to the dropdownlist selected value
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserGvUtil.SetPageSizeToDdlValue(sender, GridView1);
    }

    // go to page number typed into the textbox
    protected void GoToPage_TextChanged(object sender, EventArgs e)
    {
        UserGvUtil.GoToPageNumber(sender, GridView1);
    }

    // setup gridview sorting and row highlight to work with css
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        UserGvUtil.HighlightRowOnClick(e);
        UserGvUtil.HighlightSortedColumn(sender, e);
        UserGvUtil.DisplayAndRememberPageSize(sender, e);
    }

    #endregion

    #region ADD selected users TO selected ROLE - dropdownlist

    protected void ddlAddUsersToRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserGvUtil.AddSelectedUsersToSelectedRole2(GridView1, ddlAddUsersToRole, Msg, ddlRemoveUsersFromRole, ddlAddAllUsersToRole, ddlRemoveAllUsersFromRole, ddlDeleteAllUsersFromRole);
    }

    #endregion

    #region ADD ALL users TO selected ROLE - dropdownlist

    protected void ddlAddAllUsersToRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserGvUtil.AddAllUsresToSelectedRole2(Msg, ddlAddAllUsersToRole, ddlRemoveUsersFromRole, ddlAddUsersToRole, ddlRemoveAllUsersFromRole, ddlDeleteAllUsersFromRole);
    }

    #endregion

    #region REMOVE selected users FROM selected ROLE - dropdownlist

    protected void ddlRemoveUsersFromRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserGvUtil.RemoveSelectedUsersFromSelectedRole2(GridView1, Msg, ddlRemoveUsersFromRole, ddlAddUsersToRole, ddlAddAllUsersToRole, ddlRemoveAllUsersFromRole, ddlDeleteAllUsersFromRole);
    }

    #endregion

    #region REMOVE ALL users FROM selected ROLE - dropdownlist

    protected void ddlRemoveAllUsersFromRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserGvUtil.RemoveAllUsersFromSelectedRole2(Msg, ddlRemoveUsersFromRole, ddlAddUsersToRole, ddlAddAllUsersToRole, ddlRemoveAllUsersFromRole, ddlDeleteAllUsersFromRole);
    }

    #endregion

    #region REMOVE ALL users from ALL ROLES - button

    protected void btnRemoveAllUsersFromAllRoles_Click(object sender, EventArgs e)
    {
        UserGvUtil.RemoveAllUsersFromAllRoles2(Msg);
    }

    #endregion

    #region APPROVE selected users - button

    protected void btnApproveSelected_Click(object sender, EventArgs e)
    {
        UserGvUtil.ApproveSelectedUsers2(GridView1, Msg);
    }

    #endregion

    #region APPROVE ALL users - button

    protected void btnApproveAllUsers_Click(object sender, EventArgs e)
    {
        UserGvUtil.ApproveAllUsers2(Msg, GridView1);
    }

    #endregion

    #region UNAPPROVE selected users - button

    protected void btnUnApproveSelected_Click(object sender, EventArgs e)
    {
        UserGvUtil.UnapproveSelectedUsers2(GridView1, Msg);
    }

    #endregion

    #region UNAPPROVE ALL users -button

    protected void btnUnapproveAllUsers_Click(object sender, EventArgs e)
    {
        UserGvUtil.UnapproveAllUsers2(Msg, GridView1);
    }

    #endregion

    #region UNLOCK selected users - button

    protected void btnUnlockSelected_Click(object sender, EventArgs e)
    {
        UserGvUtil.UnlockSelectedUsers2(GridView1, Msg);
    }

    #endregion

    #region UNLOCK ALL users - button

    protected void btnUnlockAllUsers_Click(object sender, EventArgs e)
    {
        UserGvUtil.UnlockAllUsers2(Msg, GridView1);
    }

    #endregion
}