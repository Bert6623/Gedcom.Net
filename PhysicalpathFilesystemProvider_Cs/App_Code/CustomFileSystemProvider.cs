using System;
using System.Collections.Generic;
using System.Web;
using Telerik.Web.UI.Widgets;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for CustomFileSistemProvider
/// </summary>
public class CustomFileSystemProvider : FileBrowserContentProvider
{
	private string _itemHandlerPath;
	protected string ItemHandlerPath
	{
		get
		{
			return this._itemHandlerPath;
		}
	}

	private Dictionary<string, string> mappedPathsInConfigFile;
	private string pathToConfigFile = "~/App_Code/MappingFile.mapping";

	/// <summary>
	/// Returns the mappings from the configuration file
	/// </summary>
	protected Dictionary<string, string> MappedPaths
	{
		get { return mappedPathsInConfigFile; }
	}

	public CustomFileSystemProvider(HttpContext context, string[] searchPatterns, string[] viewPaths, string[] uploadPaths, string[] deletePaths, string selectedUrl, string selectedItemTag)
		:
		base(context, searchPatterns, viewPaths, uploadPaths, deletePaths, selectedUrl, selectedItemTag)
	{
		// The 'viewPaths' contains values like "C:\Foder_1\Folder_2" or "C:\Foder_1\Folder_2\"


		this.Initialize();
	}

	private void Initialize()
	{
		XmlDocument configFile = new XmlDocument();
		string physicalPathToConfigFile = Context.Server.MapPath(this.pathToConfigFile);
		configFile.Load(physicalPathToConfigFile);// Load the configuration file
		XmlElement rootElement = configFile.DocumentElement;

		XmlNode handlerPathSection = rootElement.GetElementsByTagName("genericHandlerPath")[0]; // get all mappings ;
		this._itemHandlerPath = handlerPathSection.InnerText;

		this.mappedPathsInConfigFile = new Dictionary<string, string>();
		XmlNode mappingsSection = rootElement.GetElementsByTagName("Mappings")[0]; // get all mappings ;
		foreach (XmlNode mapping in mappingsSection.ChildNodes)
		{
			XmlNode virtualPathAsNode = mapping.SelectSingleNode("child::VirtualPath");
			XmlNode physicalPathAsNode = mapping.SelectSingleNode("child::PhysicalPath");
			this.mappedPathsInConfigFile.Add(PathHelper.RemoveEndingSlash(virtualPathAsNode.InnerText, '/'), PathHelper.RemoveEndingSlash(physicalPathAsNode.InnerText, '\\'));
		}
	}

	public override DirectoryItem ResolveRootDirectoryAsTree(string path)
	{
		string physicalPath;
		string virtualPath = string.Empty;

		if (PathHelper.IsSharedPath(path) || PathHelper.IsPhysicalPath(path))
		{// The path is a physical path
			physicalPath = path;

			foreach (KeyValuePair<string, string> mappedPath in MappedPaths)
			{
				// Checks whether a mapping exists for the current physical path
				// 'mappedPath.Value' does not end with trailing slash. It looks like : "C:\Path\Dir"
				if (physicalPath.StartsWith(mappedPath.Value, StringComparison.CurrentCultureIgnoreCase))
				{// Exists 

					// Get the part of the physical path which does not contain the mappeed part
					string restOfPhysicalPath = physicalPath.Substring(mappedPath.Value.Length);

					// 'mappedPath.Value' does not end with '\'
					// // The 'restOfVirtualPath' is something like Folder_1/SubFolder_2/ ==> convert it to Folder_1\SubFolder_2\
					virtualPath = mappedPath.Key + restOfPhysicalPath.Replace('\\', '/');


					virtualPath = PathHelper.AddEndingSlash(virtualPath, '/');
					break;// Exit the 'foreach' loop ;
				}
			}
		}
		else
		{// Virtual path ;
			virtualPath = PathHelper.AddEndingSlash(path, '/');
			physicalPath = this.GetPhysicalFromVirtualPath(path);
			if (physicalPath == null)
				return null;
		}

		DirectoryItem result = new DirectoryItem(PathHelper.GetDirectoryName(physicalPath),
													string.Empty,
													virtualPath,
													string.Empty,
													GetPermissions(physicalPath),
													new FileItem[] { }, // Files are added in the ResolveDirectory method
													GetDirectories(virtualPath)
												);

		return result;
	}

	public override DirectoryItem ResolveDirectory(string virtualPath)
	{
		string physicalPath;
		physicalPath = this.GetPhysicalFromVirtualPath(virtualPath);

		if (physicalPath == null)
			return null;

		DirectoryItem result = new DirectoryItem(PathHelper.GetDirectoryName(physicalPath),
													virtualPath,
													virtualPath,
													virtualPath,
													GetPermissions(physicalPath),
													GetFiles(virtualPath),
													new DirectoryItem[] { }// Directories are added in ResolveRootDirectoryAsTree method
												);

		return result;
	}

	public override string MoveDirectory(string virtualSourcePath, string virtualDestPath)
	{
		virtualSourcePath = PathHelper.AddEndingSlash(virtualSourcePath, '/');
		virtualDestPath = PathHelper.AddEndingSlash(virtualDestPath, '/');

		string physicalSourcePath;

		physicalSourcePath = this.GetPhysicalFromVirtualPath(virtualSourcePath);
		if (physicalSourcePath == null)
			return string.Format("The virtual path :'{0}' cannot be converted to a physical path", virtualSourcePath);

		string physicalDestinationPath;
		physicalDestinationPath = this.GetPhysicalFromVirtualPath(virtualDestPath);
		if (physicalDestinationPath == null)
			return string.Format("The virtual path :'{0}' cannot be converted to a physical path", virtualDestPath);

		string newFolderName = physicalDestinationPath;

		// Checks whether the folder already exists in the destination folder ;
		if (Directory.Exists(newFolderName))
		{// Yes the folder exists :
			string message = string.Format("The folder '{0}' already exists", virtualDestPath);
			return message;
		}

		// Checks whether the source directory is parent of the destination directory ;
		if (PathHelper.IsParentOf(virtualSourcePath, virtualDestPath))
		{
			string message = string.Format("The folder  '{0}' is parent of the '{1}' directory. Operation is canceled!", virtualSourcePath, virtualDestPath);
			return message;
		}

		// There is not a permission issue with the FileExplorer's permissions ==> Move can be performed
		// But, there can be some FileSystem permissions issue (file system's read/write permissions) ;
		string errorMessage = FileSystem.MoveDirectorty(physicalSourcePath, physicalDestinationPath, virtualSourcePath, virtualDestPath);
		return errorMessage;
	}

	public override string MoveFile(string virtualSourcePath, string virtualDestPath)
	{
		string physicalSourcePath;
		physicalSourcePath = this.GetPhysicalFromVirtualPath(virtualSourcePath);
		if (physicalSourcePath == null)
			return string.Format("The virtual path :'{0}' cannot be converted to a physical path", virtualSourcePath);

		string physicalDestinationPath = this.GetPhysicalFromVirtualPath(virtualDestPath);

		if (physicalDestinationPath == null)
			return string.Format("The virtual path :'{0}' cannot be converted to a physical path", virtualDestPath);

		// Check whether the file already exists in the destination folder
		if (File.Exists(physicalDestinationPath))
		{// Yes the file exists :
			string message = string.Format("The file '{0}' already exists", virtualDestPath);
			return message;
		}

		// There is not permission issue with the FileExplorer's permissions ==> Move can be performed
		// There can be some FileSystem error 
		string errorMessage = FileSystem.MoveFile(physicalSourcePath, physicalDestinationPath, virtualSourcePath, virtualDestPath);
		return errorMessage;
	}

	public override string DeleteDirectory(string virtualTargetPath)
	{
		string physicalTargetPath;
		physicalTargetPath = this.GetPhysicalFromVirtualPath(virtualTargetPath);
		if (physicalTargetPath == null)
			return string.Format("The virtual path : '{0}' cannot be converted to a physical path", virtualTargetPath);

		// There is not permission issue with the FileExplorer's permissions ==> Delete can be performed
		// but there can be some FileSystem restrictions 
		string errorMessage = FileSystem.DeleteDirectory(physicalTargetPath, virtualTargetPath);
		return errorMessage;
	}
	public override string DeleteFile(string virtualTargetPath)
	{
		string physicalTargetPath = this.GetPhysicalFromVirtualPath(virtualTargetPath);
		if (physicalTargetPath == null)
			return string.Format("The virtual path :'{0} cannot be converted to a physical path", virtualTargetPath);

		// There is not a permission issue with the FileExplorer's permissions ==> Delete can be performed,
		// but there can be some FileSystem restriction
		string errorMessage = FileSystem.DeleteFile(physicalTargetPath, virtualTargetPath);
		return errorMessage;
	}

	public override string CopyDirectory(string virtualSourcePath, string virtualDestPath)
	{
		string physicalSourcePath = this.GetPhysicalFromVirtualPath(virtualSourcePath);
		if (physicalSourcePath == null)
			return string.Format("The virtual path : '{0}' cannot be converted to a physical path", virtualSourcePath);

		string physicalDestinationPath = this.GetPhysicalFromVirtualPath(virtualDestPath);
		if (physicalDestinationPath == null)
			return string.Format("The virtual path : '{0}' cannot be converted to a physical path", virtualDestPath);

		string newFolderName = physicalDestinationPath + PathHelper.GetDirectoryName(physicalSourcePath);

		// Check whether the folder already exists in the destination folder
		if (Directory.Exists(newFolderName))
		{// Yes the folder exists:
			string message = string.Format("The folder: '{0}{1}' already exists", virtualDestPath, PathHelper.GetDirectoryName(physicalSourcePath));
			return message;
		}

		// A check whether the source directory is parent of the destination directory
		if (PathHelper.IsParentOf(virtualSourcePath, virtualDestPath))
		{
			string message = string.Format("The directory: '{0}' is parent of the '{1}' directory. Operation is canceled!", virtualSourcePath, virtualDestPath);
			return message;
		}

		// FileSystem.CopyDirectory returns a string that contains the error or an empty string
		string errorMessage = FileSystem.CopyDirectory(physicalSourcePath, physicalDestinationPath, virtualSourcePath, virtualDestPath);
		return errorMessage;
	}

	public override string CopyFile(string virtualSourcePath, string virtualDestPath)
	{
		string physicalSourcePath = this.GetPhysicalFromVirtualPath(virtualSourcePath);
		if (physicalSourcePath == null)
			return string.Format("The virtual path: '{0}' cannot be converted to a physical path", virtualSourcePath);

		string physicalDestinationPath = this.GetPhysicalFromVirtualPath(virtualDestPath);
		if (physicalDestinationPath == null)
			return string.Format("The virtual path: '{0}' cannot be converted to a physical path", virtualDestPath);

		// Checks whether the file already exists in the destination folder ;
		if (File.Exists(physicalDestinationPath))
		{// Yes the file exists :
			string message = string.Format("The file: '{0}' already exists. Operation IS canceled!", virtualDestPath);
			return message;
		}

		// There is not a permission issue with the FileExplorer's permissions ==> Copy can be performed,
		// but there can be some FileSystem restrictions 
		string errorMessage = FileSystem.CopyFile(physicalSourcePath, physicalDestinationPath, virtualSourcePath, virtualDestPath);
		return errorMessage;
	}

	public override string CreateDirectory(string virtualTargetPath, string name)
	{
		string physicalTargetPath = this.GetPhysicalFromVirtualPath(virtualTargetPath);
		if (physicalTargetPath == null)
			return string.Format("The virtual path: '{0}' cannot be converted to a physical path", virtualTargetPath);

		string virtualNewFolderPath = PathHelper.AddEndingSlash(virtualTargetPath, '/') + name;
		string physicalNewFolderPath = this.GetPhysicalFromVirtualPath(virtualNewFolderPath);
		if (physicalNewFolderPath == null)
			return string.Format("The virtual path: '{0}'  cannot be converted to a physical path", virtualNewFolderPath);

		if (Directory.Exists(physicalNewFolderPath))
		{
			string error = string.Format("The directory: '{0}' already exists", virtualNewFolderPath); ;
			return error;
		}

		// There is no restriction with the FileExplorer's permissions ==> Create can be performed
		// but there can be some FileSystems restrictions  
		string errorMessage = FileSystem.CreateDirectory(physicalTargetPath, name, virtualTargetPath);
		return errorMessage;
	}

	public override bool CheckWritePermissions(string virtualTargetPath)
	{
		string physicalTargetPath = this.GetPhysicalFromVirtualPath(virtualTargetPath);
		if (physicalTargetPath == null)
			return false;

		// The upload permission is not set ==> no write permission;
		// Also check whether the write is allowed by the filesystem 
		return CheckPermissions(physicalTargetPath, PathPermissions.Upload) && FileSystem.CheckWritePermission(physicalTargetPath, virtualTargetPath);
	}

	public override bool CheckDeletePermissions(string virtualTargetPath)
	{
		string physicalTargetPath = this.GetPhysicalFromVirtualPath(virtualTargetPath);
		if (physicalTargetPath == null)
			return false;

		// The Delete permission is not set ==> no Delete permission;
		// Also check whether the delete permission is allowed by the filesystem 
		return CheckPermissions(physicalTargetPath, PathPermissions.Delete) && FileSystem.CheckWritePermission(physicalTargetPath, virtualTargetPath);
	}

	public override bool CheckReadPermissions(string folderPath)
	{
		string physicalTargetPath = this.GetPhysicalFromVirtualPath(folderPath);
		if (physicalTargetPath == null)
			return false;

		var canRead = CheckPermissions(physicalTargetPath, PathPermissions.Read);

		return canRead;
	}

	private bool CheckPermissions(string folderPath, PathPermissions permToCheck)
	{
		//add a ending slash to the upload folder
		folderPath = folderPath.TrimEnd(PhysicalPathSeparator) + PhysicalPathSeparator;


		string[] pathsToCheck;
		if ((permToCheck & PathPermissions.Upload) != 0)
			pathsToCheck = UploadPaths;
		else if ((permToCheck & PathPermissions.Delete) != 0)
			pathsToCheck = DeletePaths;
		else
			pathsToCheck = ViewPaths;


		//Compare the 'folderPath' to all paths in the 'pathsToCheck' collection and check if it is a child or one of them.
		foreach (string pathToCheck in pathsToCheck)
		{
			if (!String.IsNullOrEmpty(pathToCheck) && folderPath.StartsWith(pathToCheck, StringComparison.OrdinalIgnoreCase))
			{
				// Remove trailing slash from the path
				string trimmedPath = pathToCheck.TrimEnd(PhysicalPathSeparator);
				//if (trimmedPath.Length == 0)
				//{
				//    //Path contains only the Path separator ==> give permissions everywhere
				//    return true;
				//}
				if (folderPath.Length > trimmedPath.Length && folderPath[trimmedPath.Length] == PhysicalPathSeparator)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override string StoreFile(Telerik.Web.UI.UploadedFile file, string path, string name, params string[] arguments)
	{
		string physicalPath = this.GetPhysicalFromVirtualPath(path);
		if (physicalPath == null)
			return string.Empty;

		physicalPath = PathHelper.AddEndingSlash(physicalPath, '\\') + name;
		file.SaveAs(physicalPath);


		// Returns the path to the newly created file
		return PathHelper.AddEndingSlash(path, '/') + name;
	}

	// This function is obsolete ;
	public override string StoreFile(HttpPostedFile file, string path, string name, params string[] arguments)
	{
		return base.StoreFile(file, path, name, arguments);
	}

	public override string StoreBitmap(System.Drawing.Bitmap bitmap, string url, System.Drawing.Imaging.ImageFormat format)
	{
		string virtualPath = RemoveProtocolNameAndServerName(url);
		string physicalPath = this.GetPhysicalFromVirtualPath(virtualPath);
		if (physicalPath == null)
			return string.Empty;

		StreamWriter bitmapWriter = StreamWriter.Null;

		try
		{
			bitmapWriter = new StreamWriter(physicalPath);
			bitmap.Save(bitmapWriter.BaseStream, format);
		}
		catch (IOException)
		{
			string errMessage = "The image cannot be stored!";
			return errMessage;
		}
		finally
		{
			bitmapWriter.Close();
		}
		return string.Empty;
	}

	public override string GetFileName(string url)
	{
		string fileName = Path.GetFileName(RemoveProtocolNameAndServerName(url));
		return fileName;
	}

	public override Stream GetFile(string url)
	{
		string virtualPath = RemoveProtocolNameAndServerName(url);
		string physicalPath = this.GetPhysicalFromVirtualPath(virtualPath);
		if (physicalPath == null)
			return null;

		if (!File.Exists(physicalPath))
		{
			return null;
		}

		return File.OpenRead(physicalPath);
	}

	public override bool CanCreateDirectory
	{
		get
		{
			return true;
		}
	}

	public override string GetPath(string path)
	{
		// First add the '~/' signs in order to use the VirtualPathUtility.GetDirectory() method ;
		string PathWithTilde = "~/" + path;
		string virtualPath = VirtualPathUtility.GetDirectory(PathWithTilde);
		virtualPath = virtualPath.Remove(0, 2);// remove the '~' signs

		return virtualPath;
	}

	public override char PathSeparator
	{
		get
		{
			return '/';
		}
	}

	private char PhysicalPathSeparator
	{
		get
		{
			return '\\';
		}
	}


	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//  The helper methods 
	//////////////////////////////////////////////////////////////////////////////////////////////////////////


	private PathPermissions GetPermissions(string physicalPath)
	{
		PathPermissions permission = PathPermissions.Read;
		permission = CheckPermissions(physicalPath, PathPermissions.Delete) ? permission | PathPermissions.Delete : permission;
		permission = CheckPermissions(physicalPath, PathPermissions.Upload) ? permission | PathPermissions.Upload : permission;

		return permission;
	}


	/// <summary>
	/// Gets the physical path from a virtual one by using the applied mappings and 
	/// returns null if no mappings are found
	/// </summary>
	/// <param name="virtualPath">A virtual path.</param>
	/// <returns> The converted physical path or 'null' if no mapping is found </returns>
	private string GetPhysicalFromVirtualPath(string virtualPath)
	{   // 'virtualPath' contains value similar to:  "/MyCusomRootDir/"

		virtualPath = PathHelper.RemoveEndingSlash(virtualPath, '/');
		string resultPhysicalPath;// Contains the result - physical path

		// Iterates through all mapped directories
		foreach (KeyValuePair<string, string> mappedPath in MappedPaths)
		{
			if (virtualPath.StartsWith(mappedPath.Key, StringComparison.CurrentCultureIgnoreCase))
			{// A mapping is found

				// Replase the virtual root directory with the physical one
				string restOfVirtualPath = virtualPath.Substring(mappedPath.Key.Length);
				restOfVirtualPath = restOfVirtualPath.Replace('/', '\\');
				restOfVirtualPath = PathHelper.AddStartingSlash(restOfVirtualPath, '\\');

				// 'mappedPath.Value' always ends with '\'
				// // The 'restOfVirtualPath' is something like Folder_1/SubFolder_2/ ==> convert it to Folder_1\SubFolder_2\
				resultPhysicalPath = mappedPath.Value + restOfVirtualPath;

				// Break the iteration - a physical path is found
				return resultPhysicalPath;
			}
		}
		// No mapping found
		return null;
	}

	/// <summary>
	/// Returns the files as an array of 'FileItem'
	/// </summary>
	/// <param name="virtualPath">Virtual path to the filder</param>
	/// <returns>An array of 'FileItem'</returns>
	private FileItem[] GetFiles(string virtualPath)
	{
		List<FileItem> filesItems = new List<FileItem>();
		string physicalPath = this.GetPhysicalFromVirtualPath(virtualPath);
		if (physicalPath == null)
			return null;
		List<string> files = new List<string>();// The files in this folder : 'physicalPath' ;

		try
		{
			foreach (string patern in this.SearchPatterns)
			{// Applied flters in the 'SearchPatterns' property;
				foreach (string filePath in Directory.GetFiles(physicalPath, patern))
				{
					if (!files.Contains(filePath))
						files.Add(filePath);
				}
			}

			foreach (string filePath in files)
			{
				FileInfo fileInfo = new FileInfo(filePath);
				string url = ItemHandlerPath + "?path=" + virtualPath + fileInfo.Name;
				FileItem fileItem = new FileItem(fileInfo.Name,
													fileInfo.Extension,
													fileInfo.Length,
													string.Empty,
													url,
													null,
													GetPermissions(filePath)
												);
				filesItems.Add(fileItem);
			}
		}
		catch (IOException)
		{// The parent directory is moved or deleted

		}

		return filesItems.ToArray();
	}

	/// <summary>
	/// Gets the folders that are contained in a specific virtual directory
	/// </summary>
	/// <param name="virtualPath">The virtual directory that contains the folders</param>
	/// <returns>Array of 'DirectoryItem'</returns>
	private DirectoryItem[] GetDirectories(string virtualPath)
	{
		List<DirectoryItem> directoryItems = new List<DirectoryItem>();
		string physicalPath = this.GetPhysicalFromVirtualPath(virtualPath);
		if (physicalPath == null)
			return null;
		string[] directories;

		try
		{
			directories = Directory.GetDirectories(physicalPath);// Can throw an exeption ;
			foreach (string dirPath in directories)
			{
				DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
				string newVirtualPath = PathHelper.AddEndingSlash(virtualPath, '/') + PathHelper.GetDirectoryName(dirPath) + "/";
				DirectoryItem dirItem = new DirectoryItem(PathHelper.GetDirectoryName(dirPath),
															string.Empty,
															newVirtualPath,
															PathHelper.AddEndingSlash(virtualPath, '/'),
															GetPermissions(dirPath),
                                                            new FileItem[] { }, // Files are added in the ResolveDirectory method
															null
														  );
				directoryItems.Add(dirItem);
			}
		}
		catch (IOException)
		{// The parent directory is moved or deleted

		}

		return directoryItems.ToArray();
	}
}

