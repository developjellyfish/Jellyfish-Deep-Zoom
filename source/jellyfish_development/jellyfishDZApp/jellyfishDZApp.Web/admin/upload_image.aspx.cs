using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using JellyfishAdmin.Common.Web;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;

/// <summary>
/// Administration API - Upload Image Class
/// </summary>
public partial class admin_upload_image : WebFormBase
{
    string userId = "";
    string titleTextBox = "";
    string descTextBox = "";
    string tagTextBox = "";
    int isShareCheckBox = 0;


    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // ********************************************
        //  Session
        // ********************************************
        SetSessionData();

        silverlightControlHost.Visible = false;
    }


    /// <summary>
    /// Handles the Click event of the Button_Upload control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Button_Upload_Click(object sender, EventArgs e)
    {
        // ********************************************
        //  Session
        // ********************************************
        SetSessionData();
        UserSessionEntity temp = userSessionEntity;
        Debug.WriteLine("UserSessionEntity : " + temp.UserID);

        silverlightControlHost.Visible = false;


        // ********************************************
        //  Setup
        // ********************************************
        Utility util = new Utility();

        // ********************************************
        // Get DB Config from web.config
        // * Read the connection string from web.config--regardless it is encrypted or clear
        // *********************************************
        string connString = ConfigurationManager.ConnectionStrings["LocalJFish"].ConnectionString;

        // ********************************************
        //  Get additional info from upload_image.aspx
        // ********************************************
        userId = userSessionEntity.UserID;
        titleTextBox = TitleTextBox.Text;
        descTextBox = DescTextBox.Text;
        tagTextBox = TagTextBox.Text;

        if (IsShareCheckBox.Checked)
        {
            isShareCheckBox = 1;
        }


        //****************************************************
        // Save the upload file on the server file system.
        //****************************************************
        string uploadFilesPath = GetPhysicalPath(@"sl\out\upload_files\" + userId + @"\image\");
        string DziUriPath = @"sl/out/collection_images/";
        string sourceFilesPath = GetPhysicalPath(@"sl\out\source_files\");
        string thumbnailFilesPath = GetPhysicalPath(@"sl\out\thumbs_files\");
        string collectionImagesOutputPath = GetPhysicalPath(@"sl\out\");
        string collectionImagesUriPath = "out/collection_images/";
        string thumbnailFilesUriPath = "out/thumbs_files/";

        HttpFileCollection uploadedFiles = Request.Files;

        for (int i = 0; i < uploadedFiles.Count; i++)
        {
            HttpPostedFile userPostedFile = uploadedFiles[i];

            try
            {
                if (userPostedFile.ContentLength > 0)
                {
                    Span1.InnerHtml = "File Size: " + userPostedFile.ContentLength + " Bytes<br>";
                    Span1.InnerHtml += "File Name: " + userPostedFile.FileName + "<br>";

                    // ***********************************************************
                    // Abort if the extesion & contenttype is not associated with JPEG.
                    // ***********************************************************
                    if (!(
                        ("image/jpeg".Equals(userPostedFile.ContentType) || "image/pjpeg".Equals(userPostedFile.ContentType))
                            && (".jpeg".Equals(Path.GetExtension(userPostedFile.FileName).ToLower()) || (".jpg".Equals(Path.GetExtension(userPostedFile.FileName).ToLower())))
                    ))
                    {
                        //throw new Exception("File Format Error");
                        Span1.InnerHtml += "<font color=\"red\">File Format Error</font><p><p>";
                        return;
                    }


                    // ***********************************************************
                    // Preparing Upload Directory.
                    // ***********************************************************
                    if (Directory.Exists(uploadFilesPath))
                    {
                        util.DeleteFiles(uploadFilesPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(uploadFilesPath);
                    }


                    string savedSourceFileName = GetSavedSourceFileName(userId, i);
                    string savedThumbnailFileName = GetSavedThumbnailFileName(savedSourceFileName);

                    // ***********************************************************
                    // Save the uploaded file.
                    // ***********************************************************
                    userPostedFile.SaveAs(uploadFilesPath + Path.GetFileName(savedSourceFileName));

                    System.Drawing.Image orig = System.Drawing.Image.FromFile(uploadFilesPath + Path.GetFileName(savedSourceFileName));

                    int sourceFileWidth = orig.Width;
                    int sourceFileHeight = orig.Height;


                    // ***********************************************************
                    // Create & Save the thumbnail file.
                    // ***********************************************************
                    System.Drawing.Image thumbnail = util.CreateThumbnail(orig, 120, 90);
                    thumbnail.Save(thumbnailFilesPath + Path.GetFileName(savedThumbnailFileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                    thumbnail.Dispose();
                    orig.Dispose();


                    // ***********************************************************
                    // Execute SDIConverter
                    // ***********************************************************
                    ProcessStartInfo startInfo = new ProcessStartInfo(GetPhysicalPath(@"Bin\DziConv\SDImageConverter.exe"));
                    startInfo.Arguments = uploadFilesPath + " " + collectionImagesOutputPath + " 256 90 5 5 0 " + userId;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    System.Diagnostics.Process _Process = Process.Start(startInfo);

                    // Waiting until this process will exit...
                    _Process.WaitForExit();
                    _Process.Close();
                    _Process.Dispose();

                    dzcPath.Attributes.Add("Source", DziUriPath + Path.GetFileNameWithoutExtension(savedSourceFileName) + ".xml");

                    silverlightControlHost.Visible = true;

                    if (!File.Exists(sourceFilesPath + Path.GetFileName(savedSourceFileName)))
                    {
                        File.Copy(uploadFilesPath + Path.GetFileName(savedSourceFileName), sourceFilesPath + Path.GetFileName(savedSourceFileName));
                    }

                    Span1.InnerHtml += "Location where saved: " +
                       uploadFilesPath + Path.GetFileName(savedSourceFileName) + "<p>";


                    // ***********************************************************
                    // Store the additional info into DB
                    // ***********************************************************
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        // Start a local transaction.
                        SqlTransaction sqlTran = conn.BeginTransaction();

                        // Enlist the command in the current transaction.
                        SqlCommand command = conn.CreateCommand();
                        command.Transaction = sqlTran;

                        try
                        {
                            // ***********************************************************
                            // Insert into upload_info
                            // ***********************************************************
                            string InsertUploadInfoCmd = "INSERT INTO  upload_info (UId, Url, Width, Height, Thumbnail, Title, Description, Tags, IsShare, Date, Owner) ";
                            InsertUploadInfoCmd += "VALUES (";
                            InsertUploadInfoCmd += " @pUId";
                            InsertUploadInfoCmd += ",@pUrl";
                            InsertUploadInfoCmd += ",@pWidth";
                            InsertUploadInfoCmd += ",@pHeight";
                            InsertUploadInfoCmd += ",@pThumbnail";
                            InsertUploadInfoCmd += ",@pTitle";
                            InsertUploadInfoCmd += ",@pDescription";
                            InsertUploadInfoCmd += ",@pTags";
                            InsertUploadInfoCmd += ",@pIsShare";
                            InsertUploadInfoCmd += ",GETDATE()";
                            InsertUploadInfoCmd += ",@pOwner";
                            InsertUploadInfoCmd += ")";

                            command.CommandText = InsertUploadInfoCmd;
                            command.Parameters.AddWithValue("@pUId", GetUId(savedSourceFileName));
                            command.Parameters.AddWithValue("@pUrl", collectionImagesUriPath + Path.GetFileNameWithoutExtension(savedSourceFileName) + ".xml");
                            command.Parameters.AddWithValue("@pWidth", sourceFileWidth);
                            command.Parameters.AddWithValue("@pHeight", sourceFileHeight);
                            command.Parameters.AddWithValue("@pThumbnail", thumbnailFilesUriPath + savedThumbnailFileName);
                            command.Parameters.AddWithValue("@pTitle", titleTextBox);
                            command.Parameters.AddWithValue("@pDescription", descTextBox);
                            command.Parameters.AddWithValue("@pTags", tagTextBox.Trim().ToUpper());
                            command.Parameters.AddWithValue("@pIsShare", isShareCheckBox);
                            command.Parameters.AddWithValue("@pOwner", userId);
                            command.ExecuteNonQuery();



                            // ***********************************************************
                            // Insert into tag_info
                            // ***********************************************************
                            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                            string[] list_tagTextBox = tagTextBox.Split(delimiterChars);

                            string InsertTagInfoCmd = "INSERT INTO  tag_info (UId, Tag, Date, Owner) ";
                            InsertTagInfoCmd += "VALUES (";
                            InsertTagInfoCmd += " @pUId";
                            InsertTagInfoCmd += ",@pTag";
                            InsertTagInfoCmd += ",GETDATE()";
                            InsertTagInfoCmd += ",@pOwner";
                            InsertTagInfoCmd += ")";

                            foreach (string strTag in list_tagTextBox)
                            {
                                command.Parameters.Clear();
                                command.CommandText = InsertTagInfoCmd;
                                command.Parameters.AddWithValue("@pUId", GetUId(savedSourceFileName));
                                command.Parameters.AddWithValue("@pTag", strTag.Trim().ToUpper());
                                command.Parameters.AddWithValue("@pOwner", userId);
                                command.ExecuteNonQuery();
                            } // End of foreach

                            // Commit this transaction
                            sqlTran.Commit();

                            Span1.InnerHtml += "&nbsp;&nbsp;Upload Image - <font color=\"green\">Completed</font><br>";
                        }
                        catch (SqlException ex)
                        {
                            // Error handling code goes here
                            Span1.InnerHtml += "InsertUploadInfo - SqlException" + ex.Message + "<br>";
                            sqlTran.Rollback();
                        }
                    } // End of SqlConnection
                } // End of if
            }
            catch (Exception Ex)
            {
                Span1.InnerHtml += "Error: " + Ex.Message + "<p>";
            }
        }
    }


    /// <summary>
    /// Get Physical path
    /// </summary>
    /// <remarks>
    /// Non
    /// </remarks>
    /// <param name="path">
    /// path
    /// </param>
    /// <returns>
    /// string
    /// </returns>
    public string GetPhysicalPath(string path)
    {
        Uri basePath = new Uri(Request.PhysicalApplicationPath);
        Uri destPath = new Uri(basePath, ".");
        return destPath.LocalPath + path;
    }

    /// <summary>
    /// Get the name when upload file is saved in the file system.
    /// </summary>
    /// <remarks>
    /// Non
    /// </remarks>
    /// <param name="userId">
    /// User ID
    /// </param>
    /// <param name="imageNum">
    /// Number of upload files
    /// </param>
    /// <returns>
    /// string
    /// </returns>
    private string GetSavedSourceFileName(string userId, int imageNum)
    {
        imageNum++;
        return userId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + imageNum.ToString("0000") + ".jpg";
    }

    /// <summary>
    /// Get UId(Unique Id)
    /// </summary>
    /// <remarks>
    /// Non
    /// </remarks>
    /// <param name="savedSourceFileName">
    /// Source File Name
    /// </param>
    /// <returns>
    /// string
    /// </returns>
    private string GetUId(string savedSourceFileName)
    {
        return Path.GetFileNameWithoutExtension(savedSourceFileName);
    }

    /// <summary>
    /// Get the file name when the thumbnail file from upload file is saved in the file system.
    /// </summary>
    /// <remarks>
    /// Non
    /// </remarks>
    /// <param name="savedSourceFileName">
    /// Source File Name
    /// </param>
    /// <returns>
    /// string
    /// </returns>
    private string GetSavedThumbnailFileName(string savedSourceFileName)
    {
        return Path.GetFileNameWithoutExtension(savedSourceFileName) + "_thumbnail.jpg";
    }
}
