using System;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		string[] viewPaths = new string[] { @"C:\PhysicalSource\ROOT", @"\\Telerik.com\Path\SharedDir" };
		string[] uploadPaths = new string[] { @"C:\PhysicalSource\ROOT\CanUpload", 
											  @"\\Telerik.com\Path\SharedDir\ROOT\CanUpload",
											  @"C:\PhysicalSource\ROOT\Folder_11\CanDelAndUpload", 
											  @"\\Telerik.com\Path\SharedDir\ROOT\Folder_11\CanDelAndUpload"
												};
		string[] deletePaths = new string[] { @"C:\PhysicalSource\ROOT\Folder_1\CanDelete", 
										      @"C:\PhysicalSource\ROOT\Folder_11\CanDelAndUpload", 
											  @"\\Telerik.com\Path\SharedDir\ROOT\Folder_1\CanDelete",
											  @"\\Telerik.com\Path\SharedDir\ROOT\Folder_11\CanDelAndUpload" };

        RadFileExplorer1.Configuration.ViewPaths = viewPaths;
        RadFileExplorer1.Configuration.UploadPaths = uploadPaths;
        RadFileExplorer1.Configuration.DeletePaths = deletePaths;
		RadFileExplorer1.Configuration.SearchPatterns = new []{"*.*"};
        RadFileExplorer1.Configuration.ContentProviderTypeName = typeof(CustomFileSystemProvider).AssemblyQualifiedName;
    }
}
