<%@ Control Language="C#" AutoEventWireup="true" CodeFile="users-by-role.ascx.cs" Inherits="admin_controls_users_by_role" %>
<%@ Register Src="js-include2.ascx" TagName="js" TagPrefix="uc1" %>
<%@ Register Src="js-include3.ascx" TagName="js" TagPrefix="uc3" %>
<%@ Register Src="~/js/js/jquery.ascx" TagName="jquery" TagPrefix="uc4" %>
<%@ Register src="search-box.ascx" tagname="search" tagprefix="uc2" %>
<%@ Register src="a-z-menu.ascx" tagname="a" tagprefix="uc5" %>
<%-- gridview banner --%>
<div class="gvBanner">
    <span class="gvBannerUsers">
        <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/decoy-icon-36px.png" />
    </span> Users By Role:
    <%-- roles dropdown list --%>
    <asp:DropDownList ID="ddlGridSelectRole" runat="server" AutoPostBack="True" DataSourceID="RolesDataSource" DataTextField="RoleName" DataValueField="RoleName" AppendDataBoundItems="True" ToolTip="Select a Role to display users within that Role.">
        <asp:ListItem Selected="True">-- Select a Role --</asp:ListItem>
    </asp:DropDownList>
    <%-- dropdown list datasource --%>
    <asp:ObjectDataSource ID="RolesDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="wsat_GetRoles" TypeName="sp_wsatTableAdapters.wsat_RolesTableAdapter"></asp:ObjectDataSource>
    <%-- search box --%>
    <uc2:search ID="search1" runat="server" />
</div>
<%-- a-z navigation --%>
<uc5:a ID="a1" runat="server" />
<%-- gridview users by role --%>
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" DataKeyNames="userName" EmptyDataText="No records found." OnDataBound="GridView1_DataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDataBound="GridView1_RowDataBound" CssClass="gv">
    <Columns>
        <asp:TemplateField>
            <HeaderStyle CssClass="gvHeader" Width="1px" />
            <ItemStyle CssClass="gvHeader" Width="1px" />
        </asp:TemplateField>
        <asp:BoundField DataField="RowNumber" HeaderText="#" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" />
        <asp:TemplateField HeaderText="DEL">
            <HeaderTemplate>
                <input id="chkAll" onclick="SelectAllCheckboxes('chkRows',this.checked);" runat="server" type="checkbox" title="Check all checkboxes" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkRows" runat="server" ToolTip="Select for deletion" />
            </ItemTemplate>
            <ItemStyle Width="25px" HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="USER NAME" SortExpression="UserName">
            <ItemTemplate>
                <span class="gvShortcut"><a href='edit_user_modal.aspx?username=<%# Eval("UserName") %>' rel="gb_page_center[525, 600]" title="Edit User Details">
                    <%# Eval("UserName") %></a></span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="EMAIL" SortExpression="Email">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                »: <a href='Mailto:<%# Eval("Email") %>' title="click to email from your computer">
                    <%#Eval("Email")%></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="createdate" HeaderText="ACCOUNT START" SortExpression="createdate" />
        <asp:BoundField DataField="lastlogindate" HeaderText="LAST LOGIN DATE" SortExpression="lastlogindate" />
        <asp:BoundField DataField="rolename" HeaderText="ROLE" SortExpression="rolename" ItemStyle-HorizontalAlign="Center">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:CheckBoxField DataField="IsApproved" HeaderText="APPROVED?" SortExpression="IsApproved" ItemStyle-HorizontalAlign="Center">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:CheckBoxField>
        <asp:CheckBoxField DataField="IsLockedOut" HeaderText="LOCKED OUT?" SortExpression="IsLockedOut" ItemStyle-HorizontalAlign="Center">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:CheckBoxField>
    </Columns>
    <RowStyle CssClass="gvRowStyle" />
    <AlternatingRowStyle CssClass="gvAlternateRowStyle" />
    <SelectedRowStyle CssClass="gvSelected" />
    <HeaderStyle CssClass="gvHeader" />
    <EditRowStyle CssClass="gvEdit" />
    <PagerTemplate>
        <div class="gvPagerFont">
            <asp:Label ID="Label2" runat="server" Text="Show rows:" />
            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" CssClass="gvPagerFont">
                <asp:ListItem Value="5" />
                <asp:ListItem Value="10" />
                <asp:ListItem Value="15" />
                <asp:ListItem Value="20" />
                <asp:ListItem Value="40" />
                <asp:ListItem Value="80" />
                <asp:ListItem Value="1000" />
            </asp:DropDownList>
            &nbsp; Page
            <asp:TextBox ID="txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="GoToPage_TextChanged" Width="40px" CssClass="gvPagerFont" />&nbsp;of
            <asp:Label ID="lblTotalNumberOfPages" runat="server" />&nbsp;
            <asp:Button ID="Button3" runat="server" CommandName="Page" ToolTip="First" CommandArgument="First" Text="First" />
            <asp:Button ID="Button1" runat="server" CommandName="Page" ToolTip="Previous Page" CommandArgument="Prev" Text="Prev" />
            <asp:Button ID="Button2" runat="server" CommandName="Page" ToolTip="Next Page" CommandArgument="Next" Text="Next" />
            <asp:Button ID="Button4" runat="server" CommandName="Page" ToolTip="Last" CommandArgument="Last" Text="Last" />
        </div>
    </PagerTemplate>
</asp:GridView>
<%-- gridview datasource --%>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="sp_wsat_GetUsersByRole" TypeName="sp_wsat_Users" EnablePaging="true" SelectCountMethod="TotalNumberOfUsersByRole" OnSelected="ObjectDataSource1_Selected">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlGridSelectRole" DefaultValue="RoleName" Name="RoleName" PropertyName="SelectedValue" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<%-- show hide panel --%>
<asp:Panel ID="pnlHideItems" runat="server" Visible="false">
    <%-- paging information label --%>
    <div class="messageWrap">
        <asp:HyperLink ID="PagingInformation" runat="server"></asp:HyperLink>
    </div>
    <%-- total record count --%>
    <div class="messageWrap">
        <asp:HyperLink ID="totalRecordCount" runat="server"></asp:HyperLink>
    </div>
    <%-- delete selected users button --%>
    <div class="buttonCSS">
        <asp:LinkButton ID="btnDeleteSelected" runat="server" OnClick="btnDeleteSelected_Click" OnClientClick="return confirm('DELETE selected users?');" ToolTip="DELETE the selected users.">Delete</asp:LinkButton>
    </div>
    <%-- delete all membership user acounts and all related data (profiles, users from roles etc) --%>
    <div class="buttonCSS">
        <asp:LinkButton ID="deleteAllUsers" runat="server" OnClick="deleteAllUsers_Click" ToolTip="DELETE ALL membership USERS and ALL related information." OnClientClick="return confirm('DELETE ALL membership USERS and ALL related information.? Warning! This action connot be reversed. If in doubt, backup your database first.');">Delete All</asp:LinkButton>
    </div>
    <%-- approve selected users --%>
    <div class="buttonCSS">
        <asp:LinkButton ID="btnApproveSelected" runat="server" OnClick="btnApproveSelected_Click" OnClientClick="return confirm('APPROVE the selected users?');" ToolTip="APPROVE the selected users." EnableViewState="False">Approve</asp:LinkButton>
    </div>
    <%-- approve ALL users --%>
    <div class="buttonCSS">
        <asp:LinkButton ID="btnApproveAllUsers" runat="server" OnClick="btnApproveAllUsers_Click" OnClientClick="return confirm('APPROVE ALL users?');" ToolTip="APPROVE ALL users." EnableViewState="False">Approve All</asp:LinkButton>
    </div>
    <%-- unapprove selected users --%>
    <div class="buttonCSS">
        <asp:LinkButton ID="btnUnApproveSelected" runat="server" OnClick="btnUnApproveSelected_Click" OnClientClick="return confirm('UNAPPROVE the selected users?');" ToolTip="UNAPPROVE the selected users." EnableViewState="False">Unapprove</asp:LinkButton>
    </div>
    <%-- unapprove ALL users --%>
    <div class="buttonCSS">
        <asp:LinkButton ID="btnUnapproveAllUsers" runat="server" OnClick="btnUnapproveAllUsers_Click" OnClientClick="return confirm('UNAPPROVE ALL users?');" ToolTip="UNAPPROVE ALL users." EnableViewState="False">Unapprove All</asp:LinkButton>
    </div>
    <%-- unlock selected users --%>
    <div class="buttonCSS">
        <asp:LinkButton ID="btnUnlockSelected" runat="server" OnClick="btnUnlockSelected_Click" OnClientClick="return confirm('UNLOCK the selected users?');" ToolTip="UNLOCK the selected users." EnableViewState="False">Unlock</asp:LinkButton>
    </div>
    <%-- unlock ALL users --%>
    <div class="buttonCSS">
        <asp:LinkButton ID="btnUnlockAllUsers" runat="server" OnClick="btnUnlockAllUsers_Click" OnClientClick="return confirm('UNLOCK ALL users?');" ToolTip="UNLOCK ALL users." EnableViewState="False">Unlock All</asp:LinkButton>
    </div>
    <%-- remove all users from all roles --%>
    <div class="buttonCSS">
        <asp:LinkButton ID="btnRemoveAllUsersFromAllRoles" runat="server" OnClick="btnRemoveAllUsersFromAllRoles_Click" OnClientClick="return confirm('REMOVE ALL USERS from ALL ROLES?');" ToolTip="REMOVE ALL users from ALL ROLES." EnableViewState="False">Remove All</asp:LinkButton>
    </div>
    <%-- add users to selected role --%>
    <div class="ddlWrap">
        <asp:DropDownList ID="ddlAddUsersToRole" runat="server" AutoPostBack="True" DataSourceID="RolesDataSource2" DataTextField="RoleName" DataValueField="RoleName" AppendDataBoundItems="True" EnableViewState="False" Font-Size="11px" OnSelectedIndexChanged="ddlAddUsersToRole_SelectedIndexChanged" ToolTip="ADD selected users to selected ROLE.">
            <asp:ListItem Selected="True">Add To</asp:ListItem>
        </asp:DropDownList>
    </div>
    <%-- add all users to selected role --%>
    <div class="ddlWrap">
        <asp:DropDownList ID="ddlAddAllUsersToRole" runat="server" AutoPostBack="True" DataSourceID="RolesDataSource2" DataTextField="RoleName" DataValueField="RoleName" AppendDataBoundItems="True" EnableViewState="False" Font-Size="11px" OnSelectedIndexChanged="ddlAddAllUsersToRole_SelectedIndexChanged" ToolTip="ADD ALL users to selected ROLE.">
            <asp:ListItem Selected="True">Add All To</asp:ListItem>
        </asp:DropDownList>
    </div>
    <%-- remove users from selected role --%>
    <div class="ddlWrap">
        <asp:DropDownList ID="ddlRemoveUsersFromRole" runat="server" AutoPostBack="True" DataSourceID="RolesDataSource2" DataTextField="RoleName" DataValueField="RoleName" AppendDataBoundItems="True" EnableViewState="False" Font-Size="11px" OnSelectedIndexChanged="ddlRemoveUsersFromRole_SelectedIndexChanged" ToolTip="REMOVE selected users from selected ROLE.">
            <asp:ListItem Selected="True">Remove From</asp:ListItem>
        </asp:DropDownList>
    </div>
    <%-- remove all users from selected role --%>
    <div class="ddlWrap">
        <asp:DropDownList ID="ddlRemoveAllUsersFromRole" runat="server" AutoPostBack="True" DataSourceID="RolesDataSource2" DataTextField="RoleName" DataValueField="RoleName" AppendDataBoundItems="True" EnableViewState="False" Font-Size="11px" OnSelectedIndexChanged="ddlRemoveAllUsersFromRole_SelectedIndexChanged" ToolTip="REMOVE ALL users from selected ROLE.">
            <asp:ListItem Selected="True">Remove All From</asp:ListItem>
        </asp:DropDownList>
    </div>
    <%-- delete all users from selected role --%>
    <div class="ddlWrap">
        <asp:DropDownList ID="ddlDeleteAllUsersFromRole" runat="server" AutoPostBack="True" DataSourceID="RolesDataSource" DataTextField="RoleName" DataValueField="RoleName" AppendDataBoundItems="True" EnableViewState="False" Font-Size="11px" OnSelectedIndexChanged="ddlDeleteAllUsersFromRole_SelectedIndexChanged" ToolTip="DELETE ALL user accounts present in the selected ROLE.">
            <asp:ListItem Selected="True">Delete All From</asp:ListItem>
        </asp:DropDownList>
    </div>
    <%-- dropdown list datasource --%>
    <asp:ObjectDataSource ID="RolesDataSource2" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="wsat_GetRoles" TypeName="sp_wsatTableAdapters.wsat_RolesTableAdapter"></asp:ObjectDataSource>
</asp:Panel>
<%-- div padding --%>
<div class="padding">
</div>
<%-- message label --%>
<div class="messageWrap">
    <asp:HyperLink ID="Msg" runat="server" Visible="False" EnableViewState="false"></asp:HyperLink>
</div>
<%-- help sidebar --%>
<div id="helpSidebarShow" class="helpSidebarShow">
    <a onclick="ShowHide(); return false;" href="#">H<br />
        I<br />
        N<br />
        T </a>
</div>
<div id="helpSidebar" class="helpSidebar" style="display: none;">
    <span class="helpSidebarClose"><a onclick="ShowHide(); return false;" href="#">CLOSE</a> </span>
    <div class="clearBoth2">
    </div>
    <div class="helpHintIcon">
    </div>
    <div>
        <asp:Repeater ID="rptHelp" runat="server" DataSourceID="xmlHelp">
            <ItemTemplate>
                <div class="helpTitle">
                    <asp:Literal ID="ltlTitle" runat="server" Text='<%#XPath("title")%>'></asp:Literal>
                </div>
                <div class="helpText">
                    <asp:Literal ID="ltlText" runat="server" Text='<%#XPath("text")%>'></asp:Literal>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:XmlDataSource ID="xmlHelp" runat="server" DataFile="~/admin/help/users-by-role.xml"></asp:XmlDataSource>
    </div>
</div>
<%-- sidebar help js --%>
<uc3:js ID="js3" runat="server" />
<%-- jquery js --%>
<uc4:jquery ID="jquery1" runat="server" />
<%-- check allcheckboxes javascript --%>
<uc1:js ID="js1" runat="server" />
