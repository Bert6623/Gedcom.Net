<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit_user_modal.aspx.cs" Inherits="admin_Edit_user_modal" %>

<%@ Register Src="controls/edit-user-modal.ascx" TagName="edit" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>User Details</title>
  <link href="themes/default/default.css" rel="stylesheet" type="text/css" />
</head>
<body class="modalPG">
  <form id="form1" runat="server">
  <%-- Ajax script manager --%>
  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableViewState="False">
  </asp:ScriptManager>
  <uc1:edit ID="edit1" runat="server" />
  </form>
</body>
</html>
