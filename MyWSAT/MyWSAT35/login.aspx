<%@ Page Title="Login" Language="C#" MasterPageFile="~/themes/default/default.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" EnableViewState="false" %>
<%@ Register Src="controls/login-with-captcha.ascx" TagName="LoginWithCaptcha" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<meta name="robots" content="NOINDEX, NOFOLLOW"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <uc1:LoginWithCaptcha ID="LoginWithCaptcha1" runat="server" />
</asp:Content>