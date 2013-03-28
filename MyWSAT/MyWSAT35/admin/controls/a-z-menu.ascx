<%@ Control Language="C#" AutoEventWireup="true" CodeFile="a-z-menu.ascx.cs" Inherits="admin_controls_a_z_menu" EnableViewState="false" %>
<%-- a-z repeater --%>
<div class="aToZWrap">
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="XmlDataSource1">
        <ItemTemplate>
            <div class="aTozNavigaion">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#XPath("value")%>'>
                    <asp:Label ID="Label1" runat="server" Text='<%#XPath("name")%>'></asp:Label>
                </asp:HyperLink>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="clearBoth"></div>
</div>
<%-- a-z repeater datasource --%>
<asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="AtoZRepeater.xml"></asp:XmlDataSource>