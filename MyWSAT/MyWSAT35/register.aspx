<%@ Page Title="Create an Account" Language="C#" MasterPageFile="~/themes/default/default.master" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="register" %>
<%@ Register Src="controls/register-with-role.ascx" TagName="RegisterWithRole" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="robots" content="NOINDEX, NOFOLLOW" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:RegisterWithRole ID="RegisterWithRole1" runat="server" />
</asp:Content>
