<%@ Page Title="About This Software" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="about.aspx.cs" Inherits="admin_about_about" MaintainScrollPositionOnPostback="true"%>

<%@ Register Src="controls/about-cpanel.ascx" TagName="about" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:about ID="about1" runat="server" />
</asp:Content>
