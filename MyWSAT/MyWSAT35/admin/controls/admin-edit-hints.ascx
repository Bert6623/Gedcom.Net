<%@ Control Language="C#" AutoEventWireup="true" CodeFile="admin-edit-hints.ascx.cs" Inherits="admin_controls_admin_edit_hints" %>
<%@ Register Src="js-include1.ascx" TagName="js" TagPrefix="uc1" %>
<%@ Register src="js-include2.ascx" tagname="js" tagprefix="uc2" %>
<%@ Register Src="js-include3.ascx" TagName="js" TagPrefix="uc3" %>
<%@ Register src="~/js/js/jquery.ascx" tagname="jquery" tagprefix="uc4" %>
<div class="gvBanner">
    <span class="gvBannerThemes">
        <asp:Image ID="Image2" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/decoy-icon-36px.png" /></span> Add / Edit Admin Hints</div>
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="helpId" DataSourceID="ObjectDataSource1" EnableModelValidation="True" CssClass="gv" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" ShowFooter="True" OnDataBound="GridView1_DataBound" OnRowDataBound="GridView1_RowDataBound">
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
                <asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" Text="Insert" OnClick="btnInsert_Click" ValidationGroup="newHint"></asp:LinkButton>
            </FooterTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="HINT URL" SortExpression="HintUrl">
            <EditItemTemplate>
                <asp:TextBox ID="txtHintUrl" runat="server" Text='<%# Bind("HintUrl") %>' MaxLength="255"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvHintUrl" runat="server" ControlToValidate="txtHintUrl" Display="Dynamic" ErrorMessage="Hint Url is required!"></asp:RequiredFieldValidator>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:TextBox ID="txtHintUrlNew" runat="server" MaxLength="255"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvHintUrlNew" runat="server" ControlToValidate="txtHintUrlNew" Display="Dynamic" ErrorMessage="Hint Url is required!" ValidationGroup="newHint"></asp:RequiredFieldValidator>
            </FooterTemplate>
            <ItemTemplate>
                <span class="gvShortcut">
                    <a href='edit_hint_modal.aspx?helpid=<%# Eval("helpId") %>' rel="gb_page_center[800, 500]" title="Edit Hint File"><%# Eval("HintUrl") %></a>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="HINT NAME" SortExpression="HintName">
            <EditItemTemplate>
                <asp:TextBox ID="txtHintName" runat="server" Text='<%# Bind("HintName") %>' MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvHintName" runat="server" ControlToValidate="txtHintName" Display="Dynamic" ErrorMessage="Please name your Hint file!"></asp:RequiredFieldValidator>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:TextBox ID="txtHintNameNew" runat="server" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvHintName" runat="server" ControlToValidate="txtHintNameNew" Display="Dynamic" ErrorMessage="Please name your Hint!" ValidationGroup="newHint"></asp:RequiredFieldValidator>
            </FooterTemplate>
            <ItemTemplate>
                <asp:Label ID="lblHintName" runat="server" Text='<%# Bind("HintName") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="DESCRIPTION" SortExpression="HintDescription">
            <EditItemTemplate>
                <asp:TextBox ID="txtHintDescription" runat="server" Text='<%# Bind("HintDescription") %>' MaxLength="255"></asp:TextBox>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:TextBox ID="txtHintDescriptionNew" runat="server" MaxLength="255"></asp:TextBox>
            </FooterTemplate>
            <ItemTemplate>
                <asp:Label ID="lblHintDescription" runat="server" Text='<%# Bind("HintDescription") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="CATEGORY" SortExpression="HintCategory">
            <EditItemTemplate>
                <asp:TextBox ID="txtHintCategory" runat="server" Text='<%# Bind("HintCategory") %>' Enabled="False" MaxLength="100"></asp:TextBox>
            </EditItemTemplate>
            <FooterTemplate>
                <asp:TextBox ID="txtHintCategoryNew" runat="server" Enabled="false" Text="admin" MaxLength="100"></asp:TextBox>
            </FooterTemplate>
            <ItemTemplate>
                <asp:Label ID="lblHintCategory" runat="server" Text='<%# Bind("HintCategory") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <RowStyle CssClass="gvRowStyle" />
    <AlternatingRowStyle CssClass="gvAlternateRowStyle" />
    <SelectedRowStyle CssClass="gvSelected" />
    <HeaderStyle CssClass="gvHeader" />
    <EditRowStyle CssClass="gvEdit" />
    <FooterStyle CssClass="gvFooter" />
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
</asp:GridView>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetAdminHints" TypeName="sp_cpanelTableAdapters.admin_HintsTableAdapter" UpdateMethod="Update" OnSelected="ObjectDataSource1_Selected">
    <DeleteParameters>
        <asp:Parameter Name="Original_helpId" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="HintUrl" Type="String" />
        <asp:Parameter Name="HintName" Type="String" />
        <asp:Parameter Name="HintDescription" Type="String" />
        <asp:Parameter Name="HintCategory" Type="String" />
        <asp:Parameter Name="Original_helpId" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="HintUrl" Type="String" />
        <asp:Parameter Name="HintName" Type="String" />
        <asp:Parameter Name="HintDescription" Type="String" />
        <asp:Parameter Name="HintCategory" Type="String" />
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
        <asp:LinkButton ID="btnDeleteSelected" runat="server" OnClick="btnDeleteSelected_Click" OnClientClick="return confirm('DELETE selected Hint(s)?');" ToolTip="DELETE the selected Hint(s).">Delete</asp:LinkButton>
    </div>
</asp:Panel>
<div class="padding">
</div>
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
        <asp:XmlDataSource ID="xmlHelp" runat="server" DataFile="~/admin/help/admin-edit-hints.xml"></asp:XmlDataSource>
    </div>
</div>
<%-- sidebar help js --%>
<uc3:js ID="js3" runat="server" />
<%-- jquery js --%>
<uc4:jquery ID="jquery1" runat="server" />
<uc1:js ID="js1" runat="server" />
<uc2:js ID="js2" runat="server" />