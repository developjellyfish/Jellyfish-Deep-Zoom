using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Drawing;

/// <summary>
/// Summary description for Utility
/// </summary>
public class Utility
{
    /// <summary>
    /// Constructor
    /// </summary>
	public Utility()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Delete files.
    /// </summary>
    /// <param name="dirPath">target directory path</param>
    public void DeleteFiles(string dirPath)
    {
        foreach (string childDirPath in Directory.GetDirectories(dirPath))
        {
            DeleteFiles(childDirPath);
        }

        string[] st;
        st = Directory.GetFiles(dirPath);
        int i;
        for (i = 0; i < st.Length; i++)
        {
            try
            {
                File.Delete(st.GetValue(i).ToString());
            }
            catch { }
        }
    }

    /// <summary>
    /// Delete directory.
    /// </summary>
    /// <param name="dir">target directory path</param>
    public static void DeleteDirectory(string dir)
    {
        DirectoryInfo di = new DirectoryInfo(dir);

        RemoveReadonlyAttribute(di);

        di.Delete(true);
    }

    /// <summary>
    /// Remove Read-Only attribute
    /// </summary>
    /// <param name="dirInfo">Directory Info</param>
    public static void RemoveReadonlyAttribute(DirectoryInfo dirInfo)
    {
        if ((dirInfo.Attributes & FileAttributes.ReadOnly) ==
            FileAttributes.ReadOnly)
            dirInfo.Attributes = FileAttributes.Normal;

        foreach (FileInfo fi in dirInfo.GetFiles())
            if ((fi.Attributes & FileAttributes.ReadOnly) ==
                FileAttributes.ReadOnly)
                fi.Attributes = FileAttributes.Normal;

        foreach (DirectoryInfo di in dirInfo.GetDirectories())
            RemoveReadonlyAttribute(di);
    }

    /// <summary>
    /// Copy directory.
    /// </summary>
    /// <param name="sourceDirectory">source directory path</param>
    /// <param name="targetDirectory">target directory path</param>
    public static void CopyDir(string sourceDirectory, string targetDirectory)
    {
        DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
        DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

        CopyAll(diSource, diTarget);
    }

    /// <summary>
    /// Copy All.
    /// </summary>
    /// <param name="source">source directory info</param>
    /// <param name="target">target directory info</param>
    public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        // Check if the target directory exists, if not, create it.
        if (Directory.Exists(target.FullName) == false)
        {
            Directory.CreateDirectory(target.FullName);
        }

        // Copy each file into it's new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }

    /// <summary>
    /// Create Thumbnail image file from source image.
    /// </summary>
    /// <remarks>
    /// Non
    /// </remarks>
    /// <param name="orig">
    /// source image
    /// </param>
    /// <param name="w">
    /// width of the thumbnail
    /// </param>
    /// <param name="h">
    /// height of the thumbnail
    /// </param>
    /// <returns>
    /// System.Drawing.Image Object
    /// </returns>
    public System.Drawing.Image CreateThumbnail(System.Drawing.Image orig, int w, int h)
    {
        return orig.GetThumbnailImage(
          w, h, delegate { return false; }, IntPtr.Zero);
    }

    /// <summary>
    /// Get max number of results for Dynamic Search.
    /// </summary>
    /// <returns>int</returns>
    public static int getMaxNumResultsForDynamicSearch()
    {
        return Int32.Parse(ConfigurationManager.AppSettings["MaxNumResultsForDynamicSearch"].ToString());
    }
}
