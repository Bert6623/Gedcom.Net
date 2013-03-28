<%@ Control Language="C#" AutoEventWireup="true" CodeFile="admin-edit-themes.ascx.cs" Inherits="admin_controls_admin_edit_themes" %>
<%@ Register Src="js-include1.ascx" TagName="js" TagPrefix="uc1" %>
<%@ Register Src="js-include3.ascx" TagName="js" TagPrefix="uc3" %>
<%@ Register src="~/js/js/jquery.ascx" tagname="jquery" tagprefix="uc4" %>
<div class="gvBanner">
  <span class="gvBannerThemes">
    <asp:Image ID="Image2" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/decoy-icon-36px.png" /></span> Add / Edit Admin Themes</div>
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ThemeId" DataSourceID="ObjectDataSource1" CssClass="gv" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" ShowFooter="True" OnDataBound="GridView1_DataBound" OnRowDataBound="GridView1_RowDataBound">
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
        <asp:CheckBox ID="chkRows" runat="server" ToolTip="Select user in this row." />
      </ItemTemplate>
      <ItemStyle Width="25px" HorizontalAlign="Center" />
    </asp:TemplateField>
    <asp:TemplateField HeaderText="EDIT">
      <EditItemTemplate>
        <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
        &nbsp;<asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
      </EditItemTemplate>
      <FooterTemplate>
        <asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" Text="Insert" OnClick="btnInsert_Click" ValidationGroup="newTheme"></asp:LinkButton>
      </FooterTemplate>
      <ItemTemplate>
        <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
      </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="THEME URL" SortExpression="ThemeUrl">
      <EditItemTemplate>
        <asp:TextBox ID="txtThemeUrl" runat="server" Text='<%# Bind("ThemeUrl") %>' MaxLength="255"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvThemeUrl" runat="server" ControlToValidate="txtThemeUrl" Display="Dynamic" ErrorMessage="Theme Url is required!"></asp:RequiredFieldValidator>
      </EditItemTemplate>
      <FooterTemplate>
        <asp:TextBox ID="txtThemeUrlNew" runat="server" MaxLength="255"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvThemeUrl" runat="server" ControlToValidate="txtThemeUrlNew" Display="Dynamic" ErrorMessage="Theme Url is required!" ValidationGroup="newTheme"></asp:RequiredFieldValidator>
      </FooterTemplate>
      <ItemTemplate>
        <asp:Label ID="lblThemeUrl" runat="server" Text='<%# Bind("ThemeUrl") %>'></asp:Label>
      </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="THEME THUMB URL" SortExpression="ThemeThumbUrl">
      <EditItemTemplate>
        <asp:TextBox ID="txtThemeThumbUrl" runat="server" Text='<%# Bind("ThemeThumbUrl") %>' MaxLength="255"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvThemeThumb" runat="server" ControlToValidate="txtThemeThumbUrl" Display="Dynamic" ErrorMessage="Theme Thumbnail required!"></asp:RequiredFieldValidator>
      </EditItemTemplate>
      <FooterTemplate>
        <asp:TextBox ID="txtThemeThumbNew" runat="server" MaxLength="255"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvThemeThumb" runat="server" ControlToValidate="txtThemeThumbNew" Display="Dynamic" ErrorMessage="Theme Thumbnail required!" ValidationGroup="newTheme"></asp:RequiredFieldValidator>
      </FooterTemplate>
      <ItemTemplate>
        <asp:Label ID="lblThemeThumbUrl" runat="server" Text='<%# Bind("ThemeThumbUrl") %>'></asp:Label>
      </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="THEME NAME" SortExpression="ThemeName">
      <EditItemTemplate>
        <asp:TextBox ID="txtThemeName" runat="server" Text='<%# Bind("ThemeName") %>' MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvThemeName" runat="server" ControlToValidate="txtThemeName" Display="Dynamic" ErrorMessage="Please name your theme!"></asp:RequiredFieldValidator>
      </EditItemTemplate>
      <FooterTemplate>
        <asp:TextBox ID="txtThemeNameNew" runat="server" MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvThemeName" runat="server" ControlToValidate="txtThemeNameNew" Display="Dynamic" ErrorMessage="Please name your theme!" ValidationGroup="newTheme"></asp:RequiredFieldValidator>
      </FooterTemplate>
      <ItemTemplate>
        <asp:Label ID="lblThemeName" runat="server" Text='<%# Bind("ThemeName") %>'></asp:Label>
      </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="DESCRIPTION" SortExpression="ThemeDescription">
      <EditItemTemplate>
        <asp:TextBox ID="txtThemeDescription" runat="server" Text='<%# Bind("ThemeDescription") %>' MaxLength="255"></asp:TextBox>
      </EditItemTemplate>
      <FooterTemplate>
        <asp:TextBox ID="txtThemeDescriptionNew" runat="server" MaxLength="255"></asp:TextBox>
      </FooterTemplate>
      <ItemTemplate>
        <asp:Label ID="lblThemeDescription" runat="server" Text='<%# Bind("ThemeDescription") %>'></asp:Label>
      </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="CATEGORY" SortExpression="ThemeCategory">
      <EditItemTemplate>
        <asp:TextBox ID="txtThemeCategory" runat="server" Text='<%# Bind("ThemeCategory") %>' Enabled="False" MaxLength="100"></asp:TextBox>
      </EditItemTemplate>
      <FooterTemplate>
        <asp:TextBox ID="txtThemeCategoryNew" runat="server" Enabled="false" Text="admin" MaxLength="100"></asp:TextBox>
      </FooterTemplate>
      <ItemTemplate>
        <asp:Label ID="lblThemeCategory" runat="server" Text='<%# Bind("ThemeCategory") %>'></asp:Label>
      </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="ENABLED?" SortExpression="IsEnabled">
      <EditItemTemplate>
        <asp:CheckBox ID="cbxIsEnabled" runat="server" Checked='<%# Bind("IsEnabled") %>' />
      </EditItemTemplate>
      <FooterTemplate>
        <asp:CheckBox ID="cbxIsEnabledNew" runat="server" />
      </FooterTemplate>
      <ItemTemplate>
        <asp:CheckBox ID="cbxIsEnabled2" runat="server" Checked='<%# Bind("IsEnabled") %>' Enabled="false" />
      </ItemTemplate>
      <ItemStyle HorizontalAlign="Center" />
      <FooterStyle HorizontalAlign="Center" />
    </asp:TemplateField>
  </Columns>
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
      <asp:Button ID="btnFirst" runat="server" CommandName="Page" ToolTip="First" CommandArgument="First" Text="First" />
      <asp:Button ID="btnPrevious" runat="server" CommandName="Page" ToolTip="Previous Page" CommandArgument="Prev" Text="Prev" />
      <asp:Button ID="btnNext" runat="server" CommandName="Page" ToolTip="Next Page" CommandArgument="Next" Text="Next" />
      <asp:Button ID="btnLast" runat="server" CommandName="Page" ToolTip="Last" CommandArgument="Last" Text="Last" />
    </div>
  </PagerTemplate>
  <RowStyle CssClass="gvRowStyle" />
  <AlternatingRowStyle CssClass="gvAlternateRowStyle" />
  <SelectedRowStyle CssClass="gvSelected" />
  <HeaderStyle CssClass="gvHeader" />
  <EditRowStyle CssClass="gvEdit" />
  <FooterStyle CssClass="gvFooter" />
</asp:GridView>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetAdminThemes" TypeName="sp_cpanelTableAdapters.site_ThemesTableAdapter" UpdateMethod="Update" OnSelected="ObjectDataSource1_Selected">
  <DeleteParameters>
    <asp:Parameter Name="Original_ThemeId" Type="Int32" />
  </DeleteParameters>
  <UpdateParameters>
    <asp:Parameter Name="ThemeUrl" Type="String" />
    <asp:Parameter Name="ThemeThumbUrl" Type="String" />
    <asp:Parameter Name="ThemeName" Type="String" />
    <asp:Parameter Name="ThemeDescription" Type="String" />
    <asp:Parameter Name="ThemeCategory" Type="String" />
    <asp:Parameter Name="IsEnabled" Type="Boolean" />
    <asp:Parameter Name="Original_ThemeId" Type="Int32" />
  </UpdateParameters>
  <InsertParameters>
    <asp:Parameter Name="ThemeUrl" Type="String" />
    <asp:Parameter Name="ThemeThumbUrl" Type="String" />
    <asp:Parameter Name="ThemeName" Type="String" />
    <asp:Parameter Name="ThemeDescription" Type="String" />
    <asp:Parameter Name="ThemeCategory" Type="String" />
    <asp:Parameter Name="IsEnabled" Type="Boolean" />
  </InsertParameters>
</asp:ObjectDataSource>
<asp:Panel ID="pnlHideItems" runat="server" Visible="false">
  <div class="messageWrap">
    <asp:HyperLink ID="PagingInformation" runat="server"></asp:HyperLink>
  </div>
  <div class="messageWrap">
    <asp:HyperLink ID="totalRecordCount" runat="server"></asp:HyperLink>
  </div>
  <div class="buttonCSS">
    <asp:LinkButton ID="btnDeleteSelected" runat="server" OnClick="btnDeleteSelected_Click" OnClientClick="return confirm('DELETE selected Theme(s)?');" ToolTip="DELETE the selected Theme(s).">Delete</asp:LinkButton>
  </div>
</asp:Panel>
<div class="padding"></div>
<div class="messageWrap">
  <asp:HyperLink ID="Msg" runat="server" Visible="False" EnableViewState="false"></asp:HyperLink>
</div>
<%-- help sidebar --%>
<div id="helpSidebarShow" class="helpSidebarShow">
    <a onclick="ShowHide(); return false;" href="#">
    H<br />
    I<br />
    N<br />
    T
    </a>
</div>
<div id="helpSidebar" class="helpSidebar" style="display: none;">
    <span class="helpSidebarClose">
        <a onclick="ShowHide(); return false;" href="#">CLOSE</a>
    </span>
    <div class="clearBoth2"></div>
    <div class="helpHintIcon"></div>
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
        <asp:XmlDataSource ID="xmlHelp" runat="server" DataFile="~/admin/help/admin-edit-themes.xml"></asp:XmlDataSource>
    </div>
</div>
<%-- sidebar help js --%>
<uc3:js ID="js3" runat="server" />
<%-- jquery js --%>
<uc4:jquery ID="jquery1" runat="server" />
<uc1:js ID="js1" runat="server" />