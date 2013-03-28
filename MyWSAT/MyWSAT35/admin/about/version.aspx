<%@ Page Title="Software Version" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="version.aspx.cs" Inherits="admin_about_version" %>

<%@ Register Src="controls/version.ascx" TagName="version" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:version ID="version1" runat="server" />
</asp:Content>
