using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Supports features that are not implemented in the System.Web.VirtualPathUtility and SystemIO.Pat classes
/// </summary>
public class PathHelper
{
	/// <summary>
	/// Adds a symbol at the begining of a path if the symblol does not exist
	/// </summary>
	/// <param name="path"></param>
	/// <param name="symbolToAdd"></param>
	/// <returns>The modified path</returns>
	public static string AddStartingSlash(string path, char symbolToAdd)
	{
		if (path.StartsWith(symbolToAdd.ToString()))
		{
			return path;
		}
		else
		{
			return symbolToAdd + path;
		}
	}

	/// <summary>
	/// Removes a symblol from the begining of a path
	/// </summary>
	/// <param name="path"></param>
	/// <param name="symbolToRemove"></param>
	/// <returns></returns>
	/// 
	public static string RemoveStartingSlash(string path, char symbolToRemove)
	{
		if (path.StartsWith(symbolToRemove.ToString()))
		{
			return path.Substring(1);
		}
		else
		{
			return path;
		}
	}


	/// <summary>
	/// Adds a symbol at the end of a path if the symbol does not exist
	/// </summary>
	/// <param name="path"></param>
	/// <param name="symbolToRemove"></param>
	/// <returns>The modified path</returns>
	public static string AddEndingSlash(string path, char symbolToAdd)
	{
		if (path.EndsWith(symbolToAdd.ToString()))
		{
			return path;
		}
		else
		{
			return path + symbolToAdd;
		}
	}

	/// <summary>
	/// Removes a backslash from the end of a path
	/// </summary>
	/// <param name="path"></param>
	/// <returns>The modified path</returns>
	public static string RemoveEndingSlash(string path, char symbolToRemove)
	{
		if (path.EndsWith(symbolToRemove.ToString()))
		{
			return path.Substring(0, path.Length - 1);
		}
		else
		{
			return path;
		}
	}

	/// <summary>
	/// Gets the name of a directory.
	/// Example C:\Folder1\Folder2 ==> the function returns 'Folder2' 
	/// </summary>
	/// <param name="physicalPath"></param>
	/// <returns></returns>
	public static string GetDirectoryName(string physicalPath)
	{
		if (physicalPath.EndsWith("\\"))
		{
			int lastIndexOfSlash = physicalPath.Substring(0, physicalPath.Length - 1).LastIndexOf("\\");

			//if (lastIndexOfSlash == -1)
			//{// If the passsd path is C:\ for example
			//    return string.Empty;
			//}

			string name = physicalPath.Substring(lastIndexOfSlash + 1);
			return name.Replace("\\", "");
		}
		else
		{
			int lastIndexOfSlash = physicalPath.LastIndexOf("\\");
			string name = physicalPath.Substring(lastIndexOfSlash + 1);
			return name;
		}
	}

	/// <summary>
	/// Checks whether the passed path is a physical path 
	/// </summary>
	/// <param name="path">The paths looks likee: 'C:\Path\Dir' </param>
	/// <returns></returns>
	public static bool IsPhysicalPath(string path)
	{
		return path.Contains(@":\");
	}

	/// <summary>
	/// Checks whether the passed path is a shared folder's path 
	/// </summary>
	/// <param name="path">The path looks like: '\\Path\Dir'</param>
	/// <returns></returns>
	public static bool IsSharedPath(string path)
	{
		return path.StartsWith(@"\\"); ;
	}

	/// <summary>
	/// Checks whether a path is child of another path
	/// </summary>
	/// <param name="virtualParent">Should be the virtual parent directory's path</param>
	/// <param name="virtualChild">Should be the virtual child path. This parameter can be a path to file as well</param>
	/// <returns></returns>
	public static bool IsParentOf(string virtualParent, string virtualChild)
	{
		if (virtualChild.Equals(virtualParent, StringComparison.CurrentCultureIgnoreCase))
		{
			return false;
		}

		// else if
		if (virtualChild.StartsWith(virtualParent, StringComparison.CurrentCultureIgnoreCase))
		{
			return true;
		}

		// else
		return false;
	}
}
