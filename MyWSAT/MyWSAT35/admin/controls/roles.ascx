<%@ Control Language="C#" AutoEventWireup="true" CodeFile="roles.ascx.cs" Inherits="admin_controls_roles" %>
<%@ Register Src="js-include1.ascx" TagName="js" TagPrefix="uc1" %>
<%@ Register Src="js-include3.ascx" TagName="js" TagPrefix="uc3" %>
<%@ Register Src="~/js/js/jquery.ascx" TagName="jquery" TagPrefix="uc4" %>
<%@ Register src="search-box.ascx" tagname="search" tagprefix="uc2" %>
<%@ Register src="a-z-menu.ascx" tagname="a" tagprefix="uc5" %>
<%-- gridview banner --%>
<div class="gvBanner">
    <span class="gvBannerUsers">
        <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/decoy-icon-36px.png" />
        </span> Membership Roles:
    <%-- create new Role form elements --%>
    <asp:TextBox runat="server" ID="NewRole" MaxLength="50" Width="135px" ToolTip="Type the name of a new role you want to create."></asp:TextBox>
    <asp:Button ID="Button2" runat="server" OnClick="AddRole" Text="Add Role" ToolTip="Click to create new role." />
    <%-- search box --%>
    <uc2:search ID="search1" runat="server" />
</div>
<%-- a-z navigation --%>
<uc5:a ID="a1" runat="server" />
<%-- gridview to display membership users --%>
<asp:GridView ID="UserRoles" runat="server" AutoGenerateColumns="False" OnRowDataBound="UserRoles_RowDataBound" CssClass="gv">
    <Columns>
        <asp:TemplateField>
            <HeaderStyle CssClass="gvHeader" Width="1px" />
            <ItemStyle CssClass="gvHeader" Width="1px" />
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" HeaderText="#">
            <ItemTemplate>
                <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>.
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" Width="20px"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Del">
            <HeaderTemplate>
                <input id="chkAll" onclick="SelectAllCheckboxes('chkRows',this.checked);" runat="server" type="checkbox" title="Check all checkboxes" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkRows" runat="server" ToolTip="Select for deletion" />
            </ItemTemplate>
            <ItemStyle Width="25px" HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                ROLE NAME
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="RoleName" runat="server" Text='<%# Eval("Role Name") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                USER COUNT
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="UserCount" runat="server" Text='<%# Eval("User Count") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                DELETE ROLE
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="Button1" runat="server" CommandArgument='<%# Eval("Role Name") %>' CommandName="DeleteRole" OnClientClick="return confirm('Are you sure?')" OnCommand="DeleteRole" Text="Delete" ToolTip="Click to delete this role." />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
    </Columns>
    <RowStyle CssClass="gvRowStyle" />
    <AlternatingRowStyle CssClass="gvAlternateRowStyle" />
    <SelectedRowStyle CssClass="gvSelected" />
    <HeaderStyle CssClass="gvHeader" />
    <EditRowStyle CssClass="gvEdit" />
</asp:GridView>
<%-- delete checked users button --%>
<div class="buttonCSS">
    <asp:LinkButton ID="btnDeleteSelected" runat="server" OnClick="btnDeleteSelected_Click" OnClientClick="return confirm('DELETE selected ROLE(S)?');" ToolTip="DELETE selected ROLES.">Delete Selected</asp:LinkButton>
</div>
<%-- message label --%>
<div class="messageWrap">
    <asp:HyperLink ID="Msg" runat="server" Visible="False">[Msg]</asp:HyperLink>
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
        <asp:XmlDataSource ID="xmlHelp" runat="server" DataFile="~/admin/help/roles.xml"></asp:XmlDataSource>
    </div>
</div>
<%-- sidebar help js --%>
<uc3:js ID="js3" runat="server" />
<%-- jquery js --%>
<uc4:jquery ID="jquery1" runat="server" />
<%-- check all checkboxes javascript --%>
<uc1:js ID="js1" runat="server" />
