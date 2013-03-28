<%@ Control Language="C#" AutoEventWireup="true" CodeFile="search-box.ascx.cs" Inherits="admin_controls_search_box" EnableViewState="false" %>
<span class="userSearchForm">User Name:</span>
<asp:TextBox ID="txbUserName" runat="server" Width="120px" EnableViewState="False"></asp:TextBox>
<span class="userSearchForm">Email Address:</span>
<asp:TextBox ID="txbEmail" runat="server" Width="120px" EnableViewState="False"></asp:TextBox>
<asp:Button ID="btnSearch" runat="server" Text="Find Users" OnClick="btnSearch_Click" /> 
<span class="userSearchMsg">
    <asp:HyperLink ID="Msg2" runat="server" Visible="False" EnableViewState="false"></asp:HyperLink>
</span>