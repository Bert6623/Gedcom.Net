using System;
using System.IO;
using System.Security.Permissions;
using System.Security;

/// <summary>
/// A helper class for working with physical and mapped shared directories
/// </summary>
public class FileSystem
{
	public FileSystem()
	{
	}

	public static string MoveDirectorty(string physicalSourcePath, string physicalDestPath, string virtualSourcePath, string virtualDestPath)
	{
		try
		{
			if ((PathHelper.IsSharedPath(physicalSourcePath) && PathHelper.IsSharedPath(physicalDestPath))
										||
				 (PathHelper.IsPhysicalPath(physicalSourcePath) && PathHelper.IsPhysicalPath(physicalDestPath))
				)
			{
				Directory.Move(physicalSourcePath, physicalDestPath);
			}
			else
			{
				// When the 'physicalSourcePath' is a shared path and the Directory.Move does not work, and thrown and 
				// exception like: "Source and destination path must have identical roots. Move will not work across volumes."

				// The solution:
				// 1. First copy the directory
				// 2. remove the directory from the old location

				string destinationDirectoryForCopy = Path.GetDirectoryName(physicalDestPath);
				string destVirtualDirForCopy = System.Web.VirtualPathUtility.GetDirectory(PathHelper.AddStartingSlash(virtualDestPath, '/'));
				destVirtualDirForCopy = PathHelper.RemoveStartingSlash(destVirtualDirForCopy, '/');
				destVirtualDirForCopy = PathHelper.RemoveEndingSlash(destVirtualDirForCopy, '/');
				FileSystem.CopyDirectory(physicalSourcePath.ToString(), destinationDirectoryForCopy, virtualSourcePath.ToString(), destVirtualDirForCopy);
				FileSystem.DeleteDirectory(physicalSourcePath, virtualSourcePath);
			}
		}
		catch (DirectoryNotFoundException ex)
		{
			string message = string.Format("One of the directories: '{0}' or '{1}' does not exist!", virtualSourcePath, virtualDestPath);
			return message;
		}
		catch (UnauthorizedAccessException ex)
		{
			string message = "You do not have enough permissions for this operation!";
			return message;
		}
		catch (Exception ex)
		{
			string message = "The operation cannot be compleated";
			return message;
		}

		return string.Empty;
	}

	public static string MoveFile(string physicalSourcePath, string physicalDestPath, string virtualSourcePath, string virtualDestPath)
	{
		try
		{
			File.Move(physicalSourcePath, physicalDestPath);
		}

		catch (FileNotFoundException)
		{
			string message = string.Format("File: '{0}' does not exist!", virtualSourcePath);
			return message;
		}
		catch (UnauthorizedAccessException)
		{
			string message = "FileSystem's restriction: You do not have enough permissions for this operation!";
			return message;
		}
		catch (IOException)
		{
			string message = "The operation cannot be compleated";
			return message;
		}

		return string.Empty;
	}

	public static string DeleteDirectory(string physicalTargetPath, string virtualTargetPath)
	{
		try
		{
			Directory.Delete(physicalTargetPath, true);
		}
		catch (DirectoryNotFoundException)
		{
			string message = string.Format("FileSystem restriction: Directory '{0}' is not found!", virtualTargetPath);
			return message;
		}
		catch (UnauthorizedAccessException)
		{
			string message = "FileSystem's restriction: You do not have enough permissions for this operation!";
			return message;
		}
		catch (IOException)
		{
			string message = string.Format("FileSystem restriction: The directory '{0}' cannot be deleted!", virtualTargetPath);
			return message;
		}

		return string.Empty;
	}

	public static string DeleteFile(string physicalTargetPath, string virtualTargetPath)
	{
		try
		{
			File.Delete(physicalTargetPath);
		}

		catch (FileNotFoundException)
		{
			string message = string.Format("File: '{0}' does not exist!", virtualTargetPath);
			return message;
		}
		catch (UnauthorizedAccessException)
		{
			string message = "FileSystem restriction: You do not have enough permissions for this operation!";
			return message;
		}
		catch (IOException)
		{
			string message = "The operation cannot be compleated";
			return message;
		}

		return string.Empty;
	}

	public static string CopyFile(string physicalSourcePath, string physicalDestPath, string virtualSourcePath, string virtualDestPath)
	{
		try
		{
			File.Copy(physicalSourcePath, physicalDestPath, true);
		}

		catch (FileNotFoundException)
		{
			string message = string.Format("File: '{0}' does not exist!", virtualSourcePath);
			return message;
		}
		catch (UnauthorizedAccessException)
		{
			string message = "FileSystem's restriction: You do not have enough permissions for this operation!";
			return message;
		}
		catch (IOException)
		{
			string message = "The operation cannot be compleated";
			return message;
		}

		return string.Empty;
	}

	public static string CopyDirectory(string physycalSourcePath, string physicalDestPath, string virtualSourcePath, string virtualDestPath)
	{
		// Copy all files ;
		string newDirPhysicalFullPath;// Contains the physical path to the new directory ;
		DirectoryInfo dirInfoSource;
		try
		{
			dirInfoSource = new DirectoryInfo(physycalSourcePath);
			newDirPhysicalFullPath = string.Format("{0}{1}{2}", PathHelper.AddEndingSlash(physicalDestPath, '\\'), dirInfoSource.Name, "\\");

			// Else ;
			Directory.CreateDirectory(newDirPhysicalFullPath, dirInfoSource.GetAccessControl());
		}
		catch (UnauthorizedAccessException ex)
		{
			string message = "FileSystem's restriction: You do not have enough permissions for this operation!";
			return message;
		}

		// Directory is created ;

		foreach (string currentFilePath in Directory.GetFiles(physycalSourcePath))
		{
			FileInfo fileInfo = new FileInfo(currentFilePath);

			string newFilePath = newDirPhysicalFullPath + fileInfo.Name;

			try
			{
				File.Copy(currentFilePath, newFilePath);
			}

			catch (FileNotFoundException ex)
			{
				string message = string.Format("File: '{0}' does not exist!", virtualSourcePath);
				return message;
			}
			catch (UnauthorizedAccessException ex)
			{
				string message = "You do not have enough permissions for this operation!";
				return message;
			}
			catch (IOException ex)
			{
				string message = "The operation cannot be compleated";
				return message;
			}
		}

		// Copy all subdirectories ;
		foreach (string physicalCurrentSourcePath in Directory.GetDirectories(physycalSourcePath))
		{
			DirectoryInfo dirInfo = new DirectoryInfo(physicalCurrentSourcePath);
			string physicalCurrentDestPath = newDirPhysicalFullPath;// Change the name of the variable ;
			string virtualCurrentSourcePath = string.Format("{0}{1}{2}", PathHelper.AddEndingSlash(virtualSourcePath, '/'), dirInfo.Name, "/");
			string virtualCurrentDestPath = string.Format("{0}{1}{2}", PathHelper.AddEndingSlash(virtualDestPath, '/'), dirInfoSource.Name, "/");

			// Call recursively the Directory copy function ;
			string returnedError = CopyDirectory(physicalCurrentSourcePath, physicalCurrentDestPath, virtualCurrentSourcePath, virtualCurrentDestPath);
			if (returnedError != string.Empty)
			{// An error occured ;
				return returnedError;
			}
		}

		// No errors. 
		return string.Empty;
	}

	public static string CreateDirectory(string physicalTargetPath, string directoryName, string virtualTargetPath)
	{
		try
		{
			DirectoryInfo parentDir = new DirectoryInfo(physicalTargetPath);

			Directory.CreateDirectory(PathHelper.AddEndingSlash(physicalTargetPath, '\\') + directoryName, parentDir.GetAccessControl());
		}
		catch (DirectoryNotFoundException)
		{
			string message = string.Format("FileSystem restriction: Directory with name '{0}' is not found!", virtualTargetPath);
			return message;
		}
		catch (UnauthorizedAccessException)
		{
			string message = "FileSystem's restriction: You do not have enough permissions for this operation!";
			return message;
		}
		catch (IOException)
		{
			string message = string.Format("FileSystem restriction: The directory '{0}' cannot be created!", virtualTargetPath);
			return message;
		}

		return string.Empty;
	}

	public static byte[] GetFileContent(string physicalTargetPath, string virtualTargetPath)
	{
		FileStream fileStream = new FileStream(physicalTargetPath, FileMode.Open, FileAccess.Read);
		byte[] content = new byte[fileStream.Length];
		fileStream.Read(content, 0, (int)fileStream.Length);
		fileStream.Close();

		return content;
	}

	public static bool CheckWritePermission(string physicalTargetPath, string virtualTargetPath)
	{
		FileIOPermission f = new FileIOPermission(FileIOPermissionAccess.Write, physicalTargetPath);
		try
		{
			f.Demand();
			return true;
		}
		catch (SecurityException)
		{
			return false;
		}
	}
}
