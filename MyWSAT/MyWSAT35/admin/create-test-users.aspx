<%@ Page Title="Create Test Users" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="create-test-users.aspx.cs" Inherits="admin_create_test_users" %>

<%@ Register Src="controls/create-test-users.ascx" TagName="create" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:create ID="create1" runat="server" />
</asp:Content>
