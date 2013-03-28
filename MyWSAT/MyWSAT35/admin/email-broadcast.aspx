<%@ Page Title="Email Broadcast" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="email-broadcast.aspx.cs" Inherits="admin_email_broadcast" ValidateRequest="false" %>

<%@ Register Src="controls/email-broadcast.ascx" TagName="email" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <uc1:email ID="email1" runat="server" />
</asp:Content>