<%@ Control Language="C#" AutoEventWireup="true" CodeFile="create-test-users.ascx.cs" Inherits="admin_controls_create_test_users" %>
<div class="adminHelp">
   The create test user section is here only for your convenience. It allows you to create test user accounts so you can play with all the functionality the application has to offer before proceding to a production environment. It is recommended to remove this section before deploying your site. The interface, if you want to call it that is very simple and straight forward. (1) Enter the number of user accounts you'd like to create. (2) Set the number of days you want the accounts to be active (not important). (3) Select the roles you want the test users to be in. Consider creating a unique role for test accounts so you can easily remove them with a single click. (4) Check approved if you want the accounts to be active, functional logins.
</div>
<%-- banner --%>
<div class="gvBanner">
    <span class="gvBannerUsers">
        <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/decoy-icon-36px.png" />
    </span> Create Test Users
</div>
<%-- create test users form --%>
<div class="cuwWrap">
    Number of Users:
    <br />
    <asp:TextBox ID="txbUsersToCreate" Text="100" runat="server" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txbUsersToCreate" Display="Dynamic" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txbUsersToCreate" ErrorMessage="RegularExpressionValidator" ValidationExpression="\d+">*</asp:RegularExpressionValidator>
    <div class="clearBoth2"></div>
    Max Account Age (days):
    <br />
    <asp:TextBox ID="txbMaxAccountAge" Text="1" runat="server" />
    <div class="clearBoth2"></div>
    Roles:
    <div style="max-height: 60px; overflow: auto;">
        <asp:CheckBoxList ID="chkRolesList" DataSource="<%# Roles.GetAllRoles() %>" RepeatDirection="Vertical" RepeatLayout="Flow" runat="server" />
    </div>
    <div class="clearBoth2"></div>
    Approved:
    <asp:CheckBox ID="chkApproved" Checked="true" runat="server" />
    <%-- create test users button --%>
    <div class="buttonCSS">
      <asp:LinkButton ID="btnCreateTestUsers" runat="server" Text="Create Test Users" OnClick="btnCreateTestUsers_Click" ToolTip="CREATE test user data?" OnClientClick="return confirm('Do you want to CREATE the specified number of TEST USER data? This may take a while.. depending on the requested number. There is no progress indicator so be patient...');"></asp:LinkButton>
    </div>
    <%-- message label --%>
    <div class="messageWrap">
      <asp:HyperLink ID="Msg" runat="server" Visible="False" EnableViewState="false"></asp:HyperLink>
    </div>
    <div class="clearBoth"></div>
</div>