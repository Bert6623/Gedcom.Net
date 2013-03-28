<%@ Control Language="C#" AutoEventWireup="true" CodeFile="encrypt-connection-string.ascx.cs" Inherits="admin_controls_encrypt_connection_string" %>
<%@ Register Src="js-include3.ascx" TagName="js" TagPrefix="uc3" %>
<%@ Register Src="~/js/js/jquery.ascx" TagName="jquery" TagPrefix="uc4" %>
<div class="adminHelp">
  Warning!: Use this tool after your files have been uploaded to your hosting provider. You can however test it locally 
    just don&#39;t upload you web.config file to the production server with the 
    connection string encrypted. Do the encryption after you files in place on the 
    production server. Use this page to encrypt or decrypt the connection string present in your web.config file. Encrypting the connection string applies security against malicious attacker who may hack their way into the configuration file and retrieve the database's user name and password. The textbox below displays the web.config file's content so you can have visual confirmation of the action taken. Simply click the appropriate button and you are done.
</div>
<div class="gvBanner">
  <span class="gvBannerConnection"><asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/decoy-icon-36px.png" /></span>
  Encrypt Connection String</div>
<%-- div to wrap form content and align to left by default --%>
<div>
  <%-- text box to display web.config file --%>
  <asp:TextBox ID="WebConfigContents" runat="server" Rows="20" TextMode="MultiLine" Width="100%" ReadOnly="True" CssClass="webConfigBox">
  </asp:TextBox>
  <%-- encrypt/decrypt buttons --%>
  <asp:Button ID="Button1" CssClass="inputButton" runat="server" Text="Encrypt Connection String" OnClick="Button1_Click" OnClientClick="return confirm('ENCRYPT connection string?');" ToolTip="ENCRYPT connection string." />
  <asp:Button ID="Button2" CssClass="inputButton" runat="server" Text="Decrypt Connection String" OnClick="Button2_Click" OnClientClick="return confirm('DECRYPT connection string?');" ToolTip="DECRYPT connection string." />
  <%-- label to displays message after encryption and decryption --%>
  <div class="messageWrap">
    <asp:HyperLink ID="Msg" runat="server">[Msg]</asp:HyperLink>
  </div>
</div>
<%-- help sidebar --%>
<div id="helpSidebarShow" class="helpSidebarShow">
    <a onclick="ShowHide(); return false;" href="#">H<br />
        I<br />
        N<br />
        T 
    </a>
</div>
<div id="helpSidebar" class="helpSidebar" style="display: none;">
    <span class="helpSidebarClose"><a onclick="ShowHide(); return false;" href="#">CLOSE</a> </span>
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
        <asp:XmlDataSource ID="xmlHelp" runat="server" DataFile="~/admin/help/encrypt-connection-string.xml"></asp:XmlDataSource>
    </div>
</div>
<%-- sidebar help js --%>
<uc3:js ID="js3" runat="server" />
<%-- jquery js --%>
<uc4:jquery ID="jquery1" runat="server" />