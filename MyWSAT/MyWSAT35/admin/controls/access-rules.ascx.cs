#region using references

using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;

#endregion

public partial class admin_controls_access_rules : System.Web.UI.UserControl
{
    // --------------------------------------------------------------------------------------
    // Code from Dan Clam's tutorial http://aspnet.4guysfromrolla.com/articles/052307-1.aspx
    // replaced UserList dropdown with simple textbox for scalability. Admin can type in 
    // username rather than select from dropdown of perhaps thousands of users. 
    // Also rearranged and modified existing code base.
    // --------------------------------------------------------------------------------------

    #region global variables

    // the application path / site root
    private const string VirtualSiteRoot = "~/";
    string selectedFolderName;

    #endregion

    #region page_init - get roles and requests

    // get all roles and request - and the folder name only if it is not a postback
    protected void Page_Init(object sender, EventArgs e)
    {
        UserRoles.DataSource = Roles.GetAllRoles();
        UserRoles.DataBind();

        if(IsPostBack)
        {
            selectedFolderName = "";
        }
        else
        {
            selectedFolderName = Request.QueryString["selectedFolderName"];
        }
    }

    #endregion

    #region page_load - populate folder structure

    // populate folder structure on first page load, not on postback
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            PopulateTree();
        }
    }

    #endregion

    #region page_prerender - display access rules

    // on page prerender, call DisplayAccessRules
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if(FolderTree.SelectedNode != null)
        {
            DisplayAccessRules(FolderTree.SelectedValue);
            SecurityInfoSection.Visible = true;
        }
    }

    #endregion

    #region populate TreeView

    // Populate the tree based on the subfolders of the specified VirtualSiteRoot
    public void PopulateTree()
    {
        DirectoryInfo rootFolder = new DirectoryInfo(Server.MapPath(VirtualSiteRoot));
        TreeNode root = AddNodeAndDescendents(rootFolder, null);
        FolderTree.Nodes.Add(root);
        try
        {
            FolderTree.SelectedNode.ImageUrl = "~/admin/themes/default/images/treeview/folder-open.gif";
        }
        catch
        {
            // do nothing
        }
    }

    #endregion

    #region add treenode and descendants

    protected TreeNode AddNodeAndDescendents(DirectoryInfo folder, TreeNode parentNode)
    {
        // displaying the folder's name and storing the full path to the folder as the value
        string virtualFolderPath;
        if (parentNode == null)
        {
            virtualFolderPath = VirtualSiteRoot;
        }
        else
        {
            virtualFolderPath = parentNode.Value + folder.Name + "/";
        }

        TreeNode node = new TreeNode(folder.Name, virtualFolderPath);
        node.Selected = (folder.Name == selectedFolderName);

        // Recurse through this folder's subfolders
        DirectoryInfo[] subFolders = folder.GetDirectories();
        foreach (DirectoryInfo subFolder in subFolders)
        {
            if (subFolder.Name != "App_Data")
            {
                TreeNode child = AddNodeAndDescendents(subFolder, node);
                node.ChildNodes.Add(child);
            }
        }
        // Return the new TreeNode
        return node;
    }

    #endregion

    #region reset the add rule form fields

    // reset the Add Rule form field values whenever the user moves folders
    protected void FolderTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        ActionDeny.Checked = true;
        ActionAllow.Checked = false;
        ApplyRole.Checked = true;
        ApplyUser.Checked = false;
        ApplyAllUsers.Checked = false;
        ApplyAnonUser.Checked = false;
        UserRoles.SelectedIndex = 0;
        SpecifyUser.Text = "";

        RuleCreationError.Visible = false;

        // Restore previously selected folder's ImageUrl
        ResetFolderImageUrls(FolderTree.Nodes[0]);

        // Set the newly selected folder's ImageUrl
        FolderTree.SelectedNode.ImageUrl = "~/admin/themes/default/images/treeview/folder-open.gif";
    }

    #endregion

    #region reset folder image urls

    // Recurse through this node's child nodes
    protected void ResetFolderImageUrls(TreeNode parentNode)
    {
        parentNode.ImageUrl = "~/admin/themes/default/images/treeview/folder.gif";

        TreeNodeCollection nodes = parentNode.ChildNodes;
        foreach (TreeNode childNode in nodes)
        {
            ResetFolderImageUrls(childNode);
        }
    }

    #endregion

    #region throw exceptionif access outside this directory is detected

    // throw exception if access outside the directory is detected this isn't needed since IIS won't allow it anyway
    protected void DisplayAccessRules(string virtualFolderPath)
    {
        if (!virtualFolderPath.StartsWith(VirtualSiteRoot) || virtualFolderPath.IndexOf("..") >= 0)
        {
            throw new ApplicationException("An attempt to access a folder outside of the website directory has been detected and blocked.");
        }
        Configuration config = WebConfigurationManager.OpenWebConfiguration(virtualFolderPath);
        SystemWebSectionGroup systemWeb = (SystemWebSectionGroup)config.GetSectionGroup("system.web");
        AuthorizationRuleCollection authorizationRules = systemWeb.Authorization.Rules;
        GridView1.DataSource = authorizationRules;
        GridView1.DataBind();

        TitleOne.InnerText = "Rules applied to " + virtualFolderPath;
        TitleTwo.InnerText = "Create new rule for " + virtualFolderPath;
    }

    #endregion

    #region display message if no authorization rule present

    // if no authorization rule present, display message
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            AuthorizationRule rule = (AuthorizationRule)e.Row.DataItem;
            if (!rule.ElementInformation.IsPresent)
            {
                e.Row.Cells[3].Text = "Inherited from higher level";
                e.Row.Cells[4].Text = "Inherited from higher level";
            }
        }
    }

    #endregion

    #region authorization rules

    // authorization rules
    protected string GetAction(AuthorizationRule rule)
    {
        return rule.Action.ToString();
    }
    protected string GetRole(AuthorizationRule rule)
    {
        return rule.Roles.ToString();
    }
    protected string GetUser(AuthorizationRule rule)
    {
        return rule.Users.ToString();
    }

    #endregion

    #region delete or move rule

    // delete rule
    protected void DeleteRule(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        GridViewRow item = (GridViewRow)button.Parent.Parent;
        string virtualFolderPath = FolderTree.SelectedValue;
        Configuration config = WebConfigurationManager.OpenWebConfiguration(virtualFolderPath);
        SystemWebSectionGroup systemWeb = (SystemWebSectionGroup)config.GetSectionGroup("system.web");
        AuthorizationSection section = (AuthorizationSection)systemWeb.Sections["authorization"];
        section.Rules.RemoveAt(item.RowIndex);
        config.Save();
    }

    // move up rule
    protected void MoveUp(object sender, EventArgs e)
    {
        MoveRule(sender, e, "up");
    }

    // move down rule
    protected void MoveDown(object sender, EventArgs e)
    {
        MoveRule(sender, e, "down");
    }

    // move up or down rule
    protected void MoveRule(object sender, EventArgs e, string upOrDown)
    {
        upOrDown = upOrDown.ToLower();

        if (upOrDown == "up" || upOrDown == "down")
        {
            Button button = (Button)sender;
            GridViewRow item = (GridViewRow)button.Parent.Parent;
            int selectedIndex = item.RowIndex;
            if ((selectedIndex > 0 && upOrDown == "up") || (upOrDown == "down"))
            {
                string virtualFolderPath = FolderTree.SelectedValue;
                Configuration config = WebConfigurationManager.OpenWebConfiguration(virtualFolderPath);
                SystemWebSectionGroup systemWeb = (SystemWebSectionGroup)config.GetSectionGroup("system.web");
                AuthorizationSection section = (AuthorizationSection)systemWeb.Sections["authorization"];

                // Pull the local rules out of the authorization section, deleting them from same:
                ArrayList rulesArray = PullLocalRulesOutOfAuthorizationSection(section);
                if (upOrDown == "up")
                {
                    LoadRulesInNewOrder(section, rulesArray, selectedIndex, upOrDown);
                }
                else if (upOrDown == "down")
                {
                    if (selectedIndex < rulesArray.Count - 1)
                    {
                        LoadRulesInNewOrder(section, rulesArray, selectedIndex, upOrDown);
                    }
                    else
                    {
                        // DOWN button in last row was pressed. Load the rules array back in without resorting.
                        for (int x = 0; x < rulesArray.Count; x++)
                        {
                            section.Rules.Add((AuthorizationRule)rulesArray[x]);
                        }
                    }
                }
                // save configuration to file
                config.Save();
            }
        }
    }

    #endregion

    #region load rules in new order

    // load rules in new order
    protected void LoadRulesInNewOrder(AuthorizationSection section, ArrayList rulesArray, int selectedIndex, string upOrDown)
    {
        AddFirstGroupOfRules(section, rulesArray, selectedIndex, upOrDown);
        AddTheTwoSwappedRules(section, rulesArray, selectedIndex, upOrDown);
        AddFinalGroupOfRules(section, rulesArray, selectedIndex, upOrDown);
    }

    #endregion

    #region add first group of rules

    // add first group of rules
    protected void AddFirstGroupOfRules(AuthorizationSection section, ArrayList rulesArray, int selectedIndex, string upOrDown)
    {
        int adj;
        if (upOrDown == "up") adj = 1;
        else adj = 0;
        for (int x = 0; x < selectedIndex - adj; x++)
        {
            section.Rules.Add((AuthorizationRule)rulesArray[x]);
        }
    }

    #endregion

    #region add the two swapped rules

    // add two swapped rules
    protected void AddTheTwoSwappedRules(AuthorizationSection section, ArrayList rulesArray, int selectedIndex, string upOrDown)
    {
        if (upOrDown == "up")
        {
            section.Rules.Add((AuthorizationRule)rulesArray[selectedIndex]);
            section.Rules.Add((AuthorizationRule)rulesArray[selectedIndex - 1]);
        }
        else if (upOrDown == "down")
        {
            section.Rules.Add((AuthorizationRule)rulesArray[selectedIndex + 1]);
            section.Rules.Add((AuthorizationRule)rulesArray[selectedIndex]);
        }
    }

    #endregion

    #region add final group of rules

    // add final group of rules
    protected void AddFinalGroupOfRules(AuthorizationSection section, ArrayList rulesArray, int selectedIndex, string upOrDown)
    {
        int adj;
        if (upOrDown == "up") adj = 1;
        else adj = 2;
        for (int x = selectedIndex + adj; x < rulesArray.Count; x++)
        {
            section.Rules.Add((AuthorizationRule)rulesArray[x]);
        }
    }

    #endregion

    #region grab local rules from authorization section

    // use local rules out of authorization section
    protected ArrayList PullLocalRulesOutOfAuthorizationSection(AuthorizationSection section)
    {
        // First load the local rules into an ArrayList.
        ArrayList rulesArray = new ArrayList();
        foreach (AuthorizationRule rule in section.Rules)
        {
            if (rule.ElementInformation.IsPresent)
            {
                rulesArray.Add(rule);
            }
        }

        // Next delete the rules from the section.
        foreach (AuthorizationRule rule in rulesArray)
        {
            section.Rules.Remove(rule);
        }
        return rulesArray;
    }

    #endregion

    #region create rule

    // create rule
    protected void CreateRule(object sender, EventArgs e)
    {
        AuthorizationRule newRule;
        if (ActionAllow.Checked)
        {
            newRule = new AuthorizationRule(AuthorizationRuleAction.Allow);
        }
        else
        {
            newRule = new AuthorizationRule(AuthorizationRuleAction.Deny);
        }

        if (ApplyRole.Checked && UserRoles.SelectedIndex > 0)
        {
            newRule.Roles.Add(UserRoles.Text);
            AddRule(newRule);
        }
        else if (ApplyUser.Checked && SpecifyUser.Text != String.Empty)
        {
            newRule.Users.Add(SpecifyUser.Text);
            AddRule(newRule);
        }
        else if (ApplyAllUsers.Checked)
        {
            newRule.Users.Add("*");
            AddRule(newRule);
        }
        else if (ApplyAnonUser.Checked)
        {
            newRule.Users.Add("?");
            AddRule(newRule);
        }
    }

    #endregion

    #region add rule

    // add rule
    protected void AddRule(AuthorizationRule newRule)
    {
        string virtualFolderPath = FolderTree.SelectedValue;
        Configuration config = WebConfigurationManager.OpenWebConfiguration(virtualFolderPath);
        SystemWebSectionGroup systemWeb = (SystemWebSectionGroup)config.GetSectionGroup("system.web");
        AuthorizationSection section = (AuthorizationSection)systemWeb.Sections["authorization"];
        section.Rules.Add(newRule);
        try
        {
            config.Save();
            RuleCreationError.Visible = false;
        }
        catch (Exception ex)
        {
            RuleCreationError.Visible = true;
            RuleCreationError.Text = "<div class=\"alert\"><br />An error occurred and the rule was not added.<br /><br />Error message:<br /><br /><i>" + ex.Message + "</i></div>";
        }
    }

    #endregion
}
