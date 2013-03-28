<%@ Page Title="User License Agreement" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="ula.aspx.cs" Inherits="admin_about_ula" %>

<%@ Register Src="controls/ula.ascx" TagName="ula" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:ula ID="ula1" runat="server" />
</asp:Content>
