<%@ Page Title="Contact The Software Developer" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="contact-developer.aspx.cs" Inherits="admin_about_contact_developer" %>

<%@ Register Src="controls/contact.ascx" TagName="contact" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:contact ID="contact1" runat="server" />
</asp:Content>
