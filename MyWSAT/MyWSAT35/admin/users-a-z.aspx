<%@ Page Title="Users A-Z" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="users-a-z.aspx.cs" Inherits="admin_users_a_z" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="controls/users-a-to-z.ascx" TagName="users" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:users ID="users1" runat="server" />
</asp:Content>
