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
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.SqlClient;

/// <summary>
/// Administration API - Upload Layout Class
/// </summary>
public partial class admin_upload_layout : WebFormBase
{
    string userId = "";
    string titleTextBox = "";
    string descTextBox = "";
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
        //  Get additional info from upload.aspx
        // ********************************************
        userId = userSessionEntity.UserID;
        titleTextBox = TitleTextBox.Text;
        descTextBox = DescTextBox.Text;

        if (IsShareCheckBox.Checked)
        {
            isShareCheckBox = 1;
        }


        //****************************************************
        // Save the upload file on the server file system.
        //****************************************************
        string uploadFilesPath = GetPhysicalPath(@"sl\out\upload_files\" + userId + @"\layout\");
        string layoutFilesPath = GetPhysicalPath(@"sl\out\layouts\");
        string layoutFilesUriPath = "out/layouts/";
        string layoutFileUriPath = @"sl/out/upload_files/" + userId + @"/layout/";

        HttpFileCollection uploadedFiles = Request.Files;

        for (int i = 0; i < uploadedFiles.Count; i++)
        {
            HttpPostedFile userPostedFile = uploadedFiles[i];

            try
            {
                if (userPostedFile.ContentLength > 0)
                {

                    //Span1.InnerHtml = "&nbsp;&nbsp;<u>File #" + (i + 1) + "</u><br>";
                    Span1.InnerHtml = "&nbsp;&nbsp;File Size: " + userPostedFile.ContentLength + " Bytes<br>";
                    Span1.InnerHtml += "&nbsp;&nbsp;File Name: " + userPostedFile.FileName + "<br>";

                    // ***********************************************************
                    // Abort if the extesion & contenttype is not associated with JPEG.
                    // ***********************************************************
                    if (!("text/xml".Equals(userPostedFile.ContentType) && ".xml".Equals(Path.GetExtension(userPostedFile.FileName))))
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


                    string savedLayoutFileName = GetSavedLayoutFileName(userId, i);


                    // ***********************************************************
                    // Save the uploaded file.
                    // ***********************************************************
                    userPostedFile.SaveAs(uploadFilesPath + Path.GetFileName(savedLayoutFileName));

                    XmlDocument xmlDoc = new XmlDocument();

                    xmlDoc.Load(uploadFilesPath + Path.GetFileName(savedLayoutFileName));

                    XmlNodeList nodes = xmlDoc.GetElementsByTagName("I");

                    foreach (XmlNode node in nodes)
                    {
                        string strId = node.Attributes["Id"].Value;
                        node.Attributes["Source"].Value = @"collection_images/" + strId + ".xml";

                        using (Bitmap bmp = new Bitmap(256, 256))
                        //using (Pen skyBluePen = new Pen(Brushes.DeepSkyBlue))
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            // Setting StringFormat
                            StringFormat strfmt = new StringFormat();
                            CharacterRange[] characterRanges = { new CharacterRange(0, strId.Length) };
                            strfmt.SetMeasurableCharacterRanges(characterRanges);
                            strfmt.LineAlignment = StringAlignment.Center;
                            strfmt.Alignment = StringAlignment.Center;

                            int intId = Convert.ToInt32(strId);

                            if ((intId % 2) == 0)
                            {
                                g.FillRectangle(Brushes.Black, 0, 0, 256, 256);
                            }
                            else
                            {
                                g.FillRectangle(Brushes.Red, 0, 0, 256, 256);
                            }

                            //skyBluePen.Width = 8.0F;
                            //g.DrawRectangle(skyBluePen, 0, 0, 256, 256);
                            //using (Font f = new Font("Arial", 100))
                            //{
                            //    string str = strId;
                            //    g.DrawString(str, f, Brushes.Yellow, 128, 128, strfmt);
                            //}
                            Directory.CreateDirectory(uploadFilesPath + @"\source_files\");
                            bmp.Save(uploadFilesPath + @"\source_files\" + strId + ".jpg", ImageFormat.Jpeg);
                        }
                    }

                    xmlDoc.Save(uploadFilesPath + Path.GetFileName(savedLayoutFileName));


                    ProcessStartInfo startInfo = new ProcessStartInfo(GetPhysicalPath(@"Bin\DziConv\SDImageConverter.exe"));
                    startInfo.Arguments = uploadFilesPath + @"\source_files\" + " " + uploadFilesPath + " 256 90 5 5 1 " + userId;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    System.Diagnostics.Process _Process = Process.Start(startInfo);

                    // Waiting until this process will exit...
                    _Process.WaitForExit();
                    _Process.Close();
                    _Process.Dispose();

                    // Rename the file name of collection.xml which has been created by SDIConverter.exe
                    string collectionXmlFileName = GetCollectionXmlFileName();
                    File.Copy(uploadFilesPath + Path.GetFileName(savedLayoutFileName), uploadFilesPath + collectionXmlFileName);

                    dzcPath.Attributes.Add("Source", layoutFileUriPath + collectionXmlFileName);

                    silverlightControlHost.Visible = true;

                    if (!File.Exists(layoutFilesPath + Path.GetFileName(savedLayoutFileName)))
                    {
                        File.Copy(uploadFilesPath + Path.GetFileName(savedLayoutFileName), layoutFilesPath + Path.GetFileName(savedLayoutFileName));
                    }

                    Span1.InnerHtml += "&nbsp;&nbsp;Location where saved: " + uploadFilesPath + Path.GetFileName(savedLayoutFileName) + "<p>";


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
                            string InsertLayoutInfoCmd = "INSERT INTO  layout_info (LId, Url, Title, Description, DefaultFlag, IsShare, Date, Owner) ";
                            InsertLayoutInfoCmd += "VALUES (";
                            InsertLayoutInfoCmd += " @pUId";
                            InsertLayoutInfoCmd += ",@pUrl";
                            InsertLayoutInfoCmd += ",@pTitle";
                            InsertLayoutInfoCmd += ",@pDescription";
                            InsertLayoutInfoCmd += ",@pDefaultFlag";
                            InsertLayoutInfoCmd += ",@pIsShare";
                            InsertLayoutInfoCmd += ",GETDATE()";
                            InsertLayoutInfoCmd += ",@pOwner";
                            InsertLayoutInfoCmd += ")";

                            command.CommandText = InsertLayoutInfoCmd;
                            command.Parameters.AddWithValue("@pUId", GetUId(savedLayoutFileName));
                            command.Parameters.AddWithValue("@pUrl", layoutFilesUriPath + Path.GetFileName(savedLayoutFileName));
                            command.Parameters.AddWithValue("@pTitle", titleTextBox);
                            command.Parameters.AddWithValue("@pDescription", descTextBox);
                            command.Parameters.AddWithValue("@pDefaultFlag", 0);
                            command.Parameters.AddWithValue("@pIsShare", isShareCheckBox);
                            command.Parameters.AddWithValue("@pOwner", userId);
                            command.ExecuteNonQuery();

                            // Commit this transaction
                            sqlTran.Commit();

                            Span1.InnerHtml += "&nbsp;&nbsp;Upload Layout - <font color=\"green\">Completed</font><br>";
                        }
                        catch (SqlException ex)
                        {
                            // Error handling code goes here
                            Span1.InnerHtml += "&nbsp;&nbsp;InsertLayoutInfo - SqlException" + ex.Message + "<br>";
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
    /// Get collection xml file name.
    /// </summary>
    /// <returns>string</returns>
    public string GetCollectionXmlFileName()
    {
        return "collection." + (new Random()).Next(1, 999).ToString("000");
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
    /// Get the name when layout file is saved in the file system.
    /// </summary>
    /// <remarks>
    /// Non
    /// </remarks>
    /// <param name="userId">
    /// User ID
    /// </param>
    /// <param name="xmlNum">
    /// Number of layout files
    /// </param>
    /// <returns>
    /// string
    /// </returns>
    private string GetSavedLayoutFileName(string userId, int xmlNum)
    {
        xmlNum++;
        return userId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_layout_" + xmlNum.ToString("0000") + ".xml";
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
}
