<%@ Control Language="C#" AutoEventWireup="true" CodeFile="create-user-with-role.ascx.cs" Inherits="admin_controls_create_user_with_role" %>
<%@ Register Src="js-include3.ascx" TagName="js" TagPrefix="uc3" %>
<%@ Register src="~/js/js/jquery.ascx" tagname="jquery" tagprefix="uc4" %>
<div class="adminHelp">
    1.) Minimum Required Password Length = 7 char.<br />2.) Minimum Required Non-Alphanumeric char = 1.<br /> 3.) Passwords are NOT case sensitive.
</div>
<%-- gridview banner --%>
<div class="gvBanner">
  <span class="gvBannerUsers">
    <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/decoy-icon-36px.png" /></span> Create User With Role
</div>
<%-- create user wizard with roles --%>
<div class="cuwWrap">
  <asp:CreateUserWizard ID="RegisterUserWithRoles" runat="server" ContinueDestinationPageUrl="~/Admin/add-user.aspx" OnActiveStepChanged="RegisterUserWithRoles_ActiveStepChanged" LoginCreatedUser="False" CompleteSuccessText="The account has been successfully created." UnknownErrorMessage="The account was not created. Please try again." CreateUserButtonText="Continue - Step 2" OnCreatedUser="RegisterUserWithRoles_CreatedUser">
      <CreateUserButtonStyle CssClass="inputButton" />
    <TitleTextStyle CssClass="cuwTitle" />
    <WizardSteps>
      <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server" Title="Step 1 - Basic account details">
      </asp:CreateUserWizardStep>
      <asp:WizardStep ID="SpecifyRolesStep" runat="server" StepType="Step" Title="Step 2 -  Specify Roles" AllowReturn="False">
        <div class="checkboxList" style="width: 100px; overflow: auto;">
          <asp:CheckBoxList ID="RoleList" runat="server">
          </asp:CheckBoxList>
        </div>
      </asp:WizardStep>
      <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
      </asp:CompleteWizardStep>
    </WizardSteps>
      <FinishCompleteButtonStyle CssClass="inputButton" />
  </asp:CreateUserWizard>
</div>
<%-- help sidebar --%>
<div id="helpSidebarShow" class="helpSidebarShow">
    <a onclick="ShowHide(); return false;" href="#">
    H<br />
    I<br />
    N<br />
    T
    </a>
</div>
<div id="helpSidebar" class="helpSidebar" style="display: none;">
    <span class="helpSidebarClose">
        <a onclick="ShowHide(); return false;" href="#">CLOSE</a>
    </span>
    <div class="clearBoth2"></div>
    <div class="helpHintIcon"></div>
    <div>
        <asp:Repeater ID="rptHelp" runat="server" DataSourceID="xmlHelp">
            <ItemTemplate>
                <div class="helpTitle">
                    <asp:Literal ID="ltlTitle" runat="server" Text='<%#XPath("title")%>'></asp:Literal>
                </div>
                <div class="helpText">
                    <asp:Literal ID="ltlText" runat="server" Text='<%#XPath("text")%>'></asp:Literal>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:XmlDataSource ID="xmlHelp" runat="server" DataFile="~/admin/help/create-user-with-role.xml"></asp:XmlDataSource>
    </div>
</div>
<%-- sidebar help js --%>
<uc3:js ID="js3" runat="server" />
<%-- jquery js --%>
<uc4:jquery ID="jquery1" runat="server" />