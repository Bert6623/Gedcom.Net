<%@ Page Title="Role Management" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="roles.aspx.cs" Inherits="admin_roles" MaintainScrollPositionOnPostback="true"%>

<%@ Register src="controls/roles.ascx" tagname="roles" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <uc1:roles ID="roles1" runat="server" />
</asp:Content>

