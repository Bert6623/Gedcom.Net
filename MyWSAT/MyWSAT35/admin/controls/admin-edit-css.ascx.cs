using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class admin_controls_admin_edit_css : System.Web.UI.UserControl
{
    #region NAVIGATION and PAGING - Gridview

    // set page size to the dropdownlist selected value
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dropDown = (DropDownList)sender;
        this.GridView1.PageSize = int.Parse(dropDown.SelectedValue);
    }

    // go to page number typed into the textbox
    protected void GoToPage_TextChanged(object sender, EventArgs e)
    {
        TextBox txtGoToPage = (TextBox)sender;

        int pageNumber;

        if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.GridView1.PageCount)
        {
            this.GridView1.PageIndex = pageNumber - 1;
        }
        else
        {
            this.GridView1.PageIndex = 0;
        }
    }

    // setup gridview sorting and row highlight to work with css
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView gridView = (GridView)sender;

        // highlight row on click - IE and FF
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onclick", "ChangeRowColor(this)");
        }

        if (gridView.SortExpression.Length > 0)
        {
            int cellIndex = -1;
            foreach (DataControlField field in gridView.Columns)
            {
                if (field.SortExpression == gridView.SortExpression)
                {
                    cellIndex = gridView.Columns.IndexOf(field);
                    break;
                }
            }

            if (cellIndex > -1)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //  this is a header row, - set the sort css style
                    e.Row.Cells[cellIndex].CssClass = gridView.SortDirection == SortDirection.Ascending ? "gvSortAscHeaderStyle" : "gvSortDescHeaderStyle";
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //  this is an alternating row - set css style
                    e.Row.Cells[cellIndex].CssClass = e.Row.RowIndex % 2 == 0 ? "gvSortAlternatingRowstyle" : "gvSortRowStyle";
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Pager)
        {
            // display total number of pages in gridview pager
            Label lblTotalNumberOfPages = (Label)e.Row.FindControl("lblTotalNumberOfPages");
            lblTotalNumberOfPages.Text = gridView.PageCount.ToString();

            // display and hold selected page number in gotopage textbox
            TextBox txtGoToPage = (TextBox)e.Row.FindControl("txtGoToPage");
            txtGoToPage.Text = (gridView.PageIndex + 1).ToString();

            // display and hold selected page size in dropdownlist
            DropDownList ddlPageSize = (DropDownList)e.Row.FindControl("ddlPageSize");
            ddlPageSize.SelectedValue = gridView.PageSize.ToString();
        }
    }

    #endregion

    #region SHOW TOTAL page count in label

    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        // show page count in label
        PagingInformation.Text = string.Format("Page {0} of {1}...", GridView1.PageIndex + 1, GridView1.PageCount);
    }

    #endregion

    #region SHOW TOTAL record count

    protected void ObjectDataSource1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        DataTable dt = (DataTable)e.ReturnValue;
        totalRecordCount.Text = dt.Rows.Count.ToString() + " Records";

        // hide following items if gridview is empty
        pnlHideItems.Visible = dt.Rows.Count > 0;
    }

    #endregion

    #region DELETE selected Modules

    protected void btnDeleteSelected_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("chkRows");
            if (cb != null && cb.Checked)
            {
                try
                {
                    int deleteSelected = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                    ObjectDataSource1.DeleteParameters["Original_CssId"].DefaultValue = deleteSelected.ToString();
                    ObjectDataSource1.Delete();
                    Msg.Text = "DELETE was successful.";
                    Msg.Visible = true;
                }
                catch (Exception ex)
                {
                    Msg.Text = "Oops! " + ex.Message;
                    Msg.Visible = true;
                }
            }
        }
        GridView1.DataBind();
    }

    #endregion

    #region Register CSS

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        TextBox txtCssUrlNew = GridView1.FooterRow.FindControl("txtCssUrlNew") as TextBox;
        TextBox txtCssNameNew = GridView1.FooterRow.FindControl("txtCssNameNew") as TextBox;
        TextBox txtCssDescriptionNew = GridView1.FooterRow.FindControl("txtCssDescriptionNew") as TextBox;
        TextBox txtThemeCategoryNew = GridView1.FooterRow.FindControl("txtThemeCategoryNew") as TextBox;

        ObjectDataSource1.InsertParameters["CssUrl"].DefaultValue = txtCssUrlNew.Text.Trim();
        ObjectDataSource1.InsertParameters["CssName"].DefaultValue = txtCssNameNew.Text.Trim();
        ObjectDataSource1.InsertParameters["CssDescription"].DefaultValue = txtCssDescriptionNew.Text.Trim();
        ObjectDataSource1.InsertParameters["ThemeCategory"].DefaultValue = txtThemeCategoryNew.Text.Trim();

        try
        {
            ObjectDataSource1.Insert();
            GridView1.DataBind();

            Msg.Text = "CSS REGISTRATION was successful.";
            Msg.Visible = true;
        }
        catch (Exception ex)
        {
            Msg.Text = "Oops! " + ex.Message;
            Msg.Visible = true;
        }
    }

    #endregion
}