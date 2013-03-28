<%@ Control Language="C#" AutoEventWireup="true" CodeFile="admin-edit-css-modal.ascx.cs" Inherits="admin_controls_admin_edit_css_modal" %>
<%@ Register Src="js-include3.ascx" TagName="js" TagPrefix="uc1" %>
<%@ Register Src="~/js/js/jquery.ascx" TagName="jquery" TagPrefix="uc2" %>
<div class="cssEditBox">
    <asp:Literal ID="ltlCssUrl" runat="server"></asp:Literal>
    <asp:TextBox ID="TextBox1" runat="server" Height="380px" TextMode="MultiLine" Width="99%" Wrap="False"></asp:TextBox>
    <asp:Button ID="btnSave" runat="server" CssClass="inputButton" Text="Save Changes" OnClick="btnSave_Click" OnClientClick="return confirm('SAVE changes to CSS file?');" ToolTip="SAVE changes to CSS file?" />
    <div class="clearBoth">
    </div>
    <span class="messageWrap">
        <asp:HyperLink ID="Msg" runat="server" Visible="false" EnableViewState="false"></asp:HyperLink>
    </span>
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
        <asp:XmlDataSource ID="xmlHelp" runat="server" DataFile="~/admin/help/admin-edit-css-modal.xml"></asp:XmlDataSource>
    </div>
</div>
<%-- sidebar help js --%>
<uc1:js ID="js1" runat="server" />
<%-- jquery js --%>
<uc2:jquery ID="jquery1" runat="server" />
