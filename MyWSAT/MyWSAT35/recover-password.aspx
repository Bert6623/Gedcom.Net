<%@ Page Title="Password Recovery" Language="C#" MasterPageFile="~/themes/default/default.master" AutoEventWireup="true" CodeFile="recover-password.aspx.cs" Inherits="recover_password" EnableViewState="false" %>
<%@ Register Src="controls/recover-password.ascx" TagName="recover" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="robots" content="NOINDEX, NOFOLLOW" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:recover ID="recover1" runat="server" />
</asp:Content>
