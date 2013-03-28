<%@ Page Title="Edit Admin CSS" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="admin-edit-css.aspx.cs" Inherits="admin_admin_edit_css" %>

<%@ Register Src="controls/admin-edit-css.ascx" TagName="admin" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:admin ID="admin1" runat="server" />
</asp:Content>
