<%@ Control Language="C#" AutoEventWireup="true" CodeFile="membership-info.ascx.cs" Inherits="members_controls_membership_info" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div class="userInfoWrap">
    <%-- ajax update panel start --%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%-- ajax tab container start --%>
            <cc1:TabContainer ID="tcntUserInfo" runat="server" ActiveTabIndex="1" Width="100%" Font-Size="10px" CssClass="aTab1">
                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Change Password">
                    <ContentTemplate>
                        <div class="contentTemplate">
                            <div class="formSectionTitle2">
                                PASSWORD DETAILS
                            </div>
                            <asp:ChangePassword ID="ChangePassword1" runat="server" EnableViewState="False" ContinueDestinationPageUrl="~/login.aspx">
                                <MailDefinition BodyFileName="~/email_templates/change-password.htm" IsBodyHtml="True" Subject="Your password has been changed!">
                                </MailDefinition>
                            </asp:ChangePassword>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Change Email">
                    <ContentTemplate>
                        <div class="contentTemplate">
                            <div class="formSectionTitle2">
                                USER DETAILS
                            </div>
                            <asp:DetailsView AutoGenerateRows="False" DataSourceID="ObjectDataSource1" ID="DetailsView1" runat="server" OnItemUpdating="DetailsView1_ItemUpdating" CssClass="dv">
                                <Fields>
                                    <asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="True" SortExpression="UserName"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Email" SortExpression="Email">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBox1" Display="Dynamic" ErrorMessage="Email only!" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" Display="Dynamic" ErrorMessage="Email is required!" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CreationDate" HeaderText="Creation Date" SortExpression="CreationDate" ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="LastActivityDate" HeaderText="Last Activity Date" SortExpression="LastActivityDate" ReadOnly="True"></asp:BoundField>
                                    <asp:CheckBoxField DataField="IsOnline" HeaderText="Online?" SortExpression="IsOnline" ReadOnly="True"></asp:CheckBoxField>
                                    <asp:CheckBoxField DataField="IsApproved" HeaderText="Approved?" SortExpression="IsApproved" ReadOnly="True"></asp:CheckBoxField>
                                    <asp:CheckBoxField DataField="IsLockedOut" HeaderText="Locked Out?" SortExpression="IsLockedOut" ReadOnly="True"></asp:CheckBoxField>
                                    <asp:BoundField DataField="PasswordQuestion" HeaderText="Password Question" ReadOnly="True" SortExpression="PasswordQuestion"></asp:BoundField>
                                    <asp:BoundField DataField="LastLoginDate" HeaderText="Last Login Date" SortExpression="LastLoginDate" ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="LastLockoutDate" HeaderText="Last Lockout Date" ReadOnly="True" SortExpression="LastLockoutDate"></asp:BoundField>
                                    <asp:BoundField DataField="LastPasswordChangedDate" HeaderText="Last Password Changed Date" ReadOnly="True" SortExpression="LastPasswordChangedDate"></asp:BoundField>
                                    <asp:CommandField ButtonType="Button" ShowEditButton="True" EditText="Edit User Details">
                                        <ControlStyle Font-Size="11px" />
                                    </asp:CommandField>
                                </Fields>
                                <RowStyle CssClass="dvRowStyle" />
                                <FieldHeaderStyle CssClass="dvFieldHeader" />
                                <HeaderStyle CssClass="dvHeader" />
                                <AlternatingRowStyle CssClass="dvAlternateRowStyle" />
                            </asp:DetailsView>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DataObjectTypeName="System.Web.Security.MembershipUser" SelectMethod="GetUser" TypeName="System.Web.Security.Membership"></asp:ObjectDataSource>
                            <br />
                            <br />
                            <asp:Button ID="btnDeleteCurrentUser" runat="server" Text="Close and Delete my account" OnClick="btnDeleteCurrentUser_Click" OnClientClick="return confirm('Are you sure? This action will delete all your information and is unrecoverable.');" />
                            <br />
                            <asp:Label ID="lblResult" runat="server" Visible="False" BackColor="Red" />
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Edit Profile">
                    <ContentTemplate>
                        <div class="contentTemplate">
                            <div class="formSectionTitle2">
                                ABOUT YOU
                            </div>
                            <div class="formLabelsText">
                                First name:<br />
                                <asp:TextBox ID="txtFirstName" runat="server" Width="99%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="formLabelsText">
                                Last name:<br />
                                <asp:TextBox ID="txtLastName" runat="server" Width="99%" MaxLength="50" />
                            </div>
                            <div class="formLabelsText">
                                Gender:<br />
                                <asp:DropDownList runat="server" ID="ddlGenders">
                                    <asp:ListItem Text="Please select one..." Selected="True" />
                                    <asp:ListItem Text="Male" Value="M" />
                                    <asp:ListItem Text="Female" Value="F" />
                                </asp:DropDownList>
                            </div>
                            <div class="formLabelsText">
                                Birth date:<br />
                                <asp:TextBox ID="txtBirthDate" runat="server" Width="99%"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtBirthDate_CalendarExtender" runat="server" TargetControlID="txtBirthDate" Enabled="True">
                                </cc1:CalendarExtender>
                                <asp:CompareValidator runat="server" ID="valBirthDateFormat" ControlToValidate="txtBirthDate" SetFocusOnError="True" Display="Dynamic" Operator="DataTypeCheck" Type="Date" ErrorMessage="The format of the birth date is not valid." ValidationGroup="EditProfile">
                            <br />
                            The format of the birth date is not valid.
                                </asp:CompareValidator>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtBirthDate" Mask="99/99/9999" MaskType="Date" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtBirthDate" ControlExtender="MaskedEditExtender1" Display="Dynamic" TooltipMessage="Please enter a date" EmptyValueMessage="Date must be in correct format" InvalidValueMessage="This date is invalid" ErrorMessage="MaskedEditValidator1" />
                            </div>
                            <div class="formLabelsText">
                                Occupation:<br />
                                <asp:DropDownList ID="ddlOccupations" runat="server" Width="99%">
                                    <asp:ListItem Text="Please select one..." Selected="True" />
                                    <asp:ListItem Text="Academic" />
                                    <asp:ListItem Text="Accountant" />
                                    <asp:ListItem Text="Actor" />
                                    <asp:ListItem Text="Architect" />
                                    <asp:ListItem Text="Artist" />
                                    <asp:ListItem Text="Business Manager" />
                                    <asp:ListItem Text="Carpenter" />
                                    <asp:ListItem Text="Chief Executive" />
                                    <asp:ListItem Text="Cinematographer" />
                                    <asp:ListItem Text="Civil Servant" />
                                    <asp:ListItem Text="Coach" />
                                    <asp:ListItem Text="Composer" />
                                    <asp:ListItem Text="Computer programmer" />
                                    <asp:ListItem Text="Cook" />
                                    <asp:ListItem Text="Counsellor" />
                                    <asp:ListItem Text="Doctor" />
                                    <asp:ListItem Text="Driver" />
                                    <asp:ListItem Text="Economist" />
                                    <asp:ListItem Text="Editor" />
                                    <asp:ListItem Text="Electrician" />
                                    <asp:ListItem Text="Engineer" />
                                    <asp:ListItem Text="Executive Producer" />
                                    <asp:ListItem Text="Fixer" />
                                    <asp:ListItem Text="Graphic Designer" />
                                    <asp:ListItem Text="Hairdresser" />
                                    <asp:ListItem Text="Headhunter" />
                                    <asp:ListItem Text="HR - Recruitment" />
                                    <asp:ListItem Text="Information Officer" />
                                    <asp:ListItem Text="IT Consultant" />
                                    <asp:ListItem Text="Journalist" />
                                    <asp:ListItem Text="Lawyer / Solicitor" />
                                    <asp:ListItem Text="Lecturer" />
                                    <asp:ListItem Text="Librarian" />
                                    <asp:ListItem Text="Mechanic" />
                                    <asp:ListItem Text="Model" />
                                    <asp:ListItem Text="Musician" />
                                    <asp:ListItem Text="Office Worker" />
                                    <asp:ListItem Text="Performer" />
                                    <asp:ListItem Text="Photographer" />
                                    <asp:ListItem Text="Presenter" />
                                    <asp:ListItem Text="Producer / Director" />
                                    <asp:ListItem Text="Project Manager" />
                                    <asp:ListItem Text="Researcher" />
                                    <asp:ListItem Text="Salesman" />
                                    <asp:ListItem Text="Social Worker" />
                                    <asp:ListItem Text="Soldier" />
                                    <asp:ListItem Text="Sportsperson" />
                                    <asp:ListItem Text="Student" />
                                    <asp:ListItem Text="Teacher" />
                                    <asp:ListItem Text="Technical Crew" />
                                    <asp:ListItem Text="Technical Writer" />
                                    <asp:ListItem Text="Therapist" />
                                    <asp:ListItem Text="Translator" />
                                    <asp:ListItem Text="Waitress / Waiter" />
                                    <asp:ListItem Text="Web designer / author" />
                                    <asp:ListItem Text="Writer" />
                                    <asp:ListItem Text="Other" />
                                </asp:DropDownList>
                            </div>
                            <div class="formLabelsText">
                                Personal Website:<br />
                                <asp:TextBox ID="txtWebsite" runat="server" Width="99%" MaxLength="200" />
                            </div>
                            <div class="formSectionEnd">
                            </div>
                            <div class="formSectionTitle2">
                                ADDRESS
                            </div>
                            <div class="formLabelsText">
                                Country:<br />
                                <asp:DropDownList ID="ddlCountries" runat="server" AppendDataBoundItems="True" Width="99%">
                                    <asp:ListItem Selected="True" Text="Please select one..." />
                                </asp:DropDownList>
                            </div>
                            <div class="formLabelsText">
                                Address:<br />
                                <asp:TextBox runat="server" ID="txtAddress" Width="99%" MaxLength="100" />
                            </div>
                            <div class="formLabelsText">
                                Apartment Number:<br />
                                <asp:TextBox runat="server" ID="txtAptNumber" Width="99%" MaxLength="50" />
                            </div>
                            <div class="formLabelsText">
                                City:<br />
                                <asp:TextBox runat="server" ID="txtCity" Width="99%" MaxLength="100" />
                            </div>
                            <div class="formLabelsText">
                                State / Region:<br />
                                <asp:DropDownList ID="ddlStates1" runat="server" AppendDataBoundItems="True" Width="99%">
                                    <asp:ListItem Selected="True" Text="Please select one..." />
                                </asp:DropDownList>
                            </div>
                            <div class="formLabelsText">
                                Zip / Postal code:<br />
                                <asp:TextBox runat="server" ID="txtPostalCode" Width="99%" MaxLength="20" />
                            </div>
                            <div class="formSectionEnd">
                            </div>
                            <div class="formSectionTitle2">
                                CONTACT INFO
                            </div>
                            <div class="formLabelsText">
                                Day Time Phone:<br />
                                <asp:TextBox runat="server" ID="txtDayTimePhone" Width="99%" MaxLength="20" />
                            </div>
                            <div class="formLabelsText">
                                Day Time Phone Ext.:<br />
                                <asp:TextBox runat="server" ID="txtDayTimePhoneExt" Width="99%" MaxLength="10" />
                            </div>
                            <div class="formLabelsText">
                                Evening Phone:<br />
                                <asp:TextBox runat="server" ID="txtEveningPhone" Width="99%" MaxLength="20" />
                            </div>
                            <div class="formLabelsText">
                                Evening Phone Ext.:<br />
                                <asp:TextBox runat="server" ID="txtEveningPhoneExt" Width="99%" MaxLength="10" />
                            </div>
                            <div class="formLabelsText">
                                Cell Phone:<br />
                                <asp:TextBox runat="server" ID="txtCellPhone" Width="99%" MaxLength="20" />
                            </div>
                            <div class="formLabelsText">
                                Home Fax:<br />
                                <asp:TextBox runat="server" ID="txtHomeFax" Width="99%" MaxLength="20" />
                            </div>
                            <div class="formSectionEnd">
                            </div>
                            <div class="formSectionTitle2">
                                COMPANY DETAILS
                            </div>
                            <div class="formLabelsText">
                                Company Name:<br />
                                <asp:TextBox ID="txbCompanyName" runat="server" Width="99%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="formLabelsText">
                                Address:<br />
                                <asp:TextBox ID="txbCompanyAddress" runat="server" Width="99%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="formLabelsText">
                                City:<br />
                                <asp:TextBox ID="txbCompanyCity" runat="server" Width="99%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="formLabelsText">
                                State / Region:<br />
                                <asp:DropDownList ID="ddlStates2" runat="server" AppendDataBoundItems="True" Width="99%">
                                    <asp:ListItem Selected="True" Text="Please select one..." />
                                </asp:DropDownList>
                            </div>
                            <div class="formLabelsText">
                                Zip:<br />
                                <asp:TextBox ID="txbCompanyZip" runat="server" Width="99%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="formLabelsText">
                                Phone:<br />
                                <asp:TextBox ID="txbCompanyPhone" runat="server" Width="99%" MaxLength="20"></asp:TextBox>
                            </div>
                            <div class="formLabelsText">
                                Fax:<br />
                                <asp:TextBox ID="txbCompanyFax" runat="server" Width="99%" MaxLength="20"></asp:TextBox>
                            </div>
                            <div class="formLabelsText">
                                Fax:<br />
                                <asp:TextBox ID="txbCompanyWebsite" runat="server" Width="99%" MaxLength="200"></asp:TextBox>
                            </div>
                            <div class="formSectionEnd">
                            </div>
                            <div class="formSectionTitle2">
                                NEWSLETTER SUBSCRIPTION
                            </div>
                            <div class="formLabelsText">
                                Newsletter:<br />
                                <asp:DropDownList runat="server" ID="ddlNewsletter">
                                    <asp:ListItem Text="No subscription" Value="None" Selected="true" />
                                    <asp:ListItem Text="Subscribe to newsletter" Value="Html" />
                                </asp:DropDownList>
                            </div>
                            <div class="formSectionEnd">
                            </div>
                            <div class="formButton">
                                <asp:Button ID="btnUpdateProfile" runat="server" Text="Save Profile" ValidationGroup="EditProfile" OnClick="btnUpdateProfile_Click" OnClientClick="return confirm('This will Update your Profile information..\n\rClick OK to continue.')" />
                                &nbsp;
                                <asp:Label ID="lblProfileMessage" runat="server" />
                            </div>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
