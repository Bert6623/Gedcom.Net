using System;
using System.Data;
using System.Net.Mail;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class admin_controls_email_broadcast : System.Web.UI.UserControl
{
    #region PAGE LOAD events

    protected void Page_Load(object sender, EventArgs e)
    {
        UserGvUtil.DdlSendEmailToSelectedRole(ddlSendMailToSelectedRole);
        UserGvUtil.AssignDefaultUserNameLetter(categoryID, ObjectDataSource1, GridView1, IsPostBack);

    }

    #endregion

    #region DELETE selected users - button

    protected void btnDeleteSelected_Click(object sender, EventArgs e)
    {
        UserGvUtil.DeleteSelectedUsers(GridView1, Msg);
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
        UserGvUtil.ShowTotalRecordCountEmailDataTable(e, pnlHideItems, totalRecordCount);
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

    #region EMAIL selected users - button

    protected void btnSendEmailToSelected_Click(object sender, EventArgs e)
    {
        UserGvUtil.SendEmailToSelectedUsers(GridView1, txbMailFrom, txb_Subject, WYSIWYGEditor_EmailBody, rbt_BodyTextType, rbt_Importance, Msg);
    }

    #endregion

    #region EMAIL ALL users - button

    protected void btnSendEmailToAll_Click(object sender, EventArgs e)
    {
        UserGvUtil.SendEmailToAllUsers(txbMailFrom, txb_Subject, WYSIWYGEditor_EmailBody, rbt_BodyTextType, rbt_Importance, Msg);
    }

    #endregion

    #region EMAIL users in SELECTED ROLE - dropdownlist

    protected void ddlSendMailToSelectedRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserGvUtil.SendEmailToSelectedRole(ddlSendMailToSelectedRole, txbMailFrom, txb_Subject, WYSIWYGEditor_EmailBody, rbt_BodyTextType, rbt_Importance, Msg);
    }

    #endregion
}