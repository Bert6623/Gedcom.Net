<%@ Page Title="Home" Language="C#" MasterPageFile="~/themes/default/default.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- temporary content --%>
    <div class="tempLoginWrap">
        <div class="buttonCSS">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Admin/Default.aspx" Target="_blank">Login as Admin</asp:HyperLink>
        </div>
        <div class="buttonCSS">
            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Members/Default.aspx" Target="_blank">Login as Member</asp:HyperLink>
        </div>
        <div class="clearBoth3">
        </div>
        <div style="text-align: left;">
            <b><span style="font-size: medium">Admin Login:</span></b><br />
            UserName: admin<br />
            Password: nimda123*<br />
            <br />
            <b><span style="font-size: medium">Member Login:</span></b><br />
            UserName: member<br />
            Password: nimda123*
            <br />
            <br />
            This is the default page and what you see is just a temporary content for testing. You can delete this from ~/default.aspx and then modify the master page located at ~/themes/default/default.master</div>
    </div>
</asp:Content>
