<%@ Page Title="Users Online" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="online-users.aspx.cs" Inherits="admin_online_users" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="controls/users-online.ascx" TagName="users" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <uc1:users ID="users1" runat="server" />
</asp:Content>
