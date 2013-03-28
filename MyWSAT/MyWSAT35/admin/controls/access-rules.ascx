<%@ Control Language="C#" AutoEventWireup="true" CodeFile="access-rules.ascx.cs" Inherits="admin_controls_access_rules" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="js-include3.ascx" TagName="js" TagPrefix="uc1" %>
<%@ Register Src="~/js/js/jquery.ascx" TagName="jquery" TagPrefix="uc2" %>
<%-- help --%>
<div class="adminHelp">
    Use this page to manage access rules for your Web site. Rules are applied to folders, thus providing robust folder-level security enforced by the ASP.NET infrastructure. Rules are persisted as XML in each folder's Web.config file. Rules are applied in order. The first rule that matches applies, and the permission in each rule overrides the permissions in all following rules. Use the Move Up and Move Down buttons to change the order of the selected rule. Rules that appear dimmed are inherited from the parent and cannot be changed at this level.
</div>
<%-- top banner --%>
<div class="gvBanner">
    <span class="gvBannerSettings">
        <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/decoy-icon-36px.png" />
    </span>Directory Access Rules
</div>
<%-- ajax update panel start --%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div class="accessRulesWrap">
        <table width="100%">
            <tr>
                <td valign="top">
                    <table width="100%">
                        <tr>
                            <td valign="top" style="padding-right: 30px; width: 150px;">
                                <div class="treeView">
                                    <asp:TreeView runat="server" ID="FolderTree" OnSelectedNodeChanged="FolderTree_SelectedNodeChanged" ExpandDepth="1" ShowLines="True">
                                        <RootNodeStyle ImageUrl="~/admin/themes/default/images/treeview/folder-open.gif" HorizontalPadding="5" />
                                        <ParentNodeStyle ImageUrl="~/admin/themes/default/images/treeview/folder.gif" HorizontalPadding="5" />
                                        <LeafNodeStyle ImageUrl="~/admin/themes/default/images/treeview/folder.gif" HorizontalPadding="5" />
                                        <SelectedNodeStyle ImageUrl="~/admin/themes/default/images/treeview/folder-open.gif" HorizontalPadding="5" CssClass="tvSelectedNodeStyle" />
                                    </asp:TreeView>
                                </div>
                            </td>
                            <td valign="top" style="padding-left: 30px; border-left: 1px solid #F0F0F0;">
                                <asp:Panel runat="server" ID="SecurityInfoSection" Visible="false">
                                    <div runat="server" id="TitleOne" class="formSectionTitle">
                                    </div>
                                    <br />
                                    <asp:GridView runat="server" ID="GridView1" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound" CellPadding="4" CssClass="gv">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="gvHeader" Width="1px" />
                                                <ItemStyle CssClass="gvHeader" Width="1px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <%# GetAction((System.Web.Configuration.AuthorizationRule)Container.DataItem) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Roles">
                                                <ItemTemplate>
                                                    <%# GetRole((System.Web.Configuration.AuthorizationRule)Container.DataItem) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="User">
                                                <ItemTemplate>
                                                    <%# GetUser((System.Web.Configuration.AuthorizationRule)Container.DataItem) %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete Rule">
                                                <ItemTemplate>
                                                    <asp:Button ID="Button1" runat="server" Text="Delete Rule" CommandArgument="<%# (System.Web.Configuration.AuthorizationRule)Container.DataItem %>" OnClick="DeleteRule" OnClientClick="return confirm('Click OK to delete this rule.')" />
                                                </ItemTemplate>
                                                <ControlStyle Font-Size="10px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Move Rule">
                                                <ItemTemplate>
                                                    <asp:Button ID="Button2" runat="server" Text="  Up  " CommandArgument="<%# (System.Web.Configuration.AuthorizationRule)Container.DataItem %>" OnClick="MoveUp" />
                                                    <asp:Button ID="Button3" runat="server" Text="Down" CommandArgument="<%# (System.Web.Configuration.AuthorizationRule)Container.DataItem %>" OnClick="MoveDown" />
                                                </ItemTemplate>
                                                <ControlStyle Font-Size="10px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="gvRowStyle" />
                                        <AlternatingRowStyle CssClass="gvAlternateRowStyle" />
                                        <SelectedRowStyle CssClass="gvSelected" />
                                        <HeaderStyle CssClass="gvHeader" />
                                        <EditRowStyle CssClass="gvEdit" />
                                    </asp:GridView>
                                    <br />
                                    <br />
                                    <div runat="server" id="TitleTwo" class="formSectionTitle">
                                    </div>
                                    <br />
                                    <table cellpadding="3" cellspacing="0" style="width: 100%">
                                        <tr>
                                            <td style="width: 153px">
                                                <b>Action:</b>&nbsp;<asp:RadioButton ID="ActionDeny" runat="server" Checked="true" GroupName="action" Text="Deny" />
                                                <asp:RadioButton ID="ActionAllow" runat="server" GroupName="action" Text="Allow" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 153px">
                                                <b>Rule applies to:</b>&nbsp;
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 153px">
                                                <asp:RadioButton ID="ApplyRole" runat="server" Checked="true" GroupName="applyto" Text="This Role:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="UserRoles" runat="server" AppendDataBoundItems="true" Width="204px">
                                                    <asp:ListItem>Select Role</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 153px">
                                                <asp:RadioButton ID="ApplyUser" runat="server" GroupName="applyto" Text="This User:" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="SpecifyUser" runat="server" Width="204px" ToolTip="Enter a UserName.."></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="SpecifyUser_TextBoxWatermarkExtender" runat="server" TargetControlID="SpecifyUser" WatermarkText="type user name here...">
                                                </cc1:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 153px">
                                                <asp:RadioButton ID="ApplyAllUsers" runat="server" GroupName="applyto" Text="All Users (*)" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 153px">
                                                <asp:RadioButton ID="ApplyAnonUser" runat="server" GroupName="applyto" Text="Anonymous Users (?)" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 153px">
                                                <asp:Button ID="Button4" CssClass="inputButton" runat="server" OnClick="CreateRule" OnClientClick="return confirm('Click OK to create this rule.');" Text="Create Rule" />
                                            </td>
                                            <td>
                                                <asp:Literal ID="RuleCreationError" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%-- ajax update panel end --%>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
<%-- help sidebar --%>
<div id="helpSidebarShow" class="helpSidebarShow">
    <a onclick="ShowHide(); return false;" href="#">H<br />
        I<br />
        N<br />
        T </a>
</div>
<div id="helpSidebar" class="helpSidebar" style="display: none;">
    <span class="helpSidebarClose"><a onclick="ShowHide(); return false;" href="#">CLOSE</a> </span>
    <div class="clearBoth2">
    </div>
    <div class="helpHintIcon">
    </div>
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
        <asp:XmlDataSource ID="xmlHelp" runat="server" DataFile="~/admin/help/access-rules.xml"></asp:XmlDataSource>
    </div>
</div>
<%-- sidebar help js --%>
<uc1:js ID="js1" runat="server" />
<%-- jquery js --%>
<uc2:jquery ID="jquery1" runat="server" />
