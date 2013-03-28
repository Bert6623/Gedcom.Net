<%@ Control Language="C#" AutoEventWireup="true" CodeFile="admin-themes.ascx.cs" Inherits="admin_controls_admin_themes" %>
<%@ Register Src="js-include3.ascx" TagName="js" TagPrefix="uc3" %>
<%@ Register src="~/js/js/jquery.ascx" tagname="jquery" tagprefix="uc4" %>
<%-- current theme --%>
<div class="gvBanner">
  <span class="gvBannerThemes"><asp:Image ID="Image2" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/decoy-icon-36px.png" /></span>
  Current Admin Theme</div>
<asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSource2">
  <ItemTemplate>
    <div class="themeWrap">
      <div class="themeImageWrap">
        <a href="#" onclick="return alert('This Theme is currently selected...');"><asp:Image ID="Image1" runat="server" ToolTip='<%# Eval("ThemeName") %>' ImageUrl='<%# Eval("ThemeThumbUrl") %>' /></a>
      </div>
      <div class="clearBoth"></div>
      <div class="themeTextWrap">
          <span class="themeName"><%# Eval("ThemeName") %></span>
          <br />
          <span class="themeDescription"><%# Eval("ThemeDescription") %></span>
          <br />
          <span class="themeLocation">Location: <%# Eval("ThemeUrl") %></span>
      </div>
    </div>
  </ItemTemplate>
</asp:Repeater>

<div class="clearBoth padding"></div>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:dbMyCMSConnectionString %>" SelectCommand="sp_admin_CurrentTheme" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
<%-- available themes --%>
<div class="gvBanner">
  <span class="gvBannerThemes"><asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/decoy-icon-36px.png" /></span>
  Available Admin Themes</div>
<asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" onitemcommand="Repeater1_ItemCommand">
  <ItemTemplate>
    <div class="themeWrap">
      <div class="themeImageWrap">
        <asp:LinkButton ID="btnActivate0" runat="server" CommandName="activate" CommandArgument='<%# Eval("ThemeId") %>' OnClientClick="return confirm('This will ACTIVATE the selected Theme...\n\rContinue?');">
            <asp:Image ID="Image1" runat="server" ToolTip='<%# Eval("ThemeName") %>' ImageUrl='<%# Eval("ThemeThumbUrl") %>'/>
        </asp:LinkButton>
      </div>
      <div class="clearBoth"></div>
      <div class="themeTextWrap">
          <span class="themeName"><%# Eval("ThemeName") %></span>
          <br />
          <span class="themeDescription"><%# Eval("ThemeDescription") %></span>
          <br />
          <span class="themeLocation">Location: <%# Eval("ThemeUrl") %></span>
          <div class="clearBoth"></div>
          <span class="buttonCSS">
          <asp:LinkButton ID="btnActivate" runat="server" CommandName="activate" CommandArgument='<%# Eval("ThemeId") %>' OnClientClick="return confirm('This will ACTIVATE the selected Theme...\n\rContinue?');">ACTIVATE</asp:LinkButton>
          <asp:LinkButton ID="btnDelete" runat="server" CommandName="delete" CommandArgument='<%# Eval("ThemeId") %>' OnClientClick="return confirm('This will DELETE the selected Theme...\n\rContinue?');">DELETE</asp:LinkButton>
          </span>
      </div>
    </div>
  </ItemTemplate>
</asp:Repeater>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbMyCMSConnectionString %>" SelectCommand="sp_admin_AvailableThemes" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
<div class="clearBoth"></div>
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
        <asp:XmlDataSource ID="xmlHelp" runat="server" DataFile="~/admin/help/admin-themes.xml"></asp:XmlDataSource>
    </div>
</div>
<%-- sidebar help js --%>
<uc3:js ID="js3" runat="server" />
<%-- jquery js --%>
<uc4:jquery ID="jquery1" runat="server" />