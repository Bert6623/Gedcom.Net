<%@ Page Title="Membership Admin" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="admin_default" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="controls/membership-cpanel.ascx" TagName="membership" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:membership ID="membership1" runat="server" />
</asp:Content>
