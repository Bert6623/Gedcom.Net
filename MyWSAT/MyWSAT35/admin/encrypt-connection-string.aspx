<%@ Page Title="Connection String Security" Language="C#" MasterPageFile="~/admin/themes/default/default.master" AutoEventWireup="true" CodeFile="encrypt-connection-string.aspx.cs" Inherits="admin_encrypt_connection_string" MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

<%@ Register Src="controls/encrypt-connection-string.ascx" TagName="encrypt" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <uc1:encrypt ID="encrypt1" runat="server" />
</asp:Content>