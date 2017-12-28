using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Configuration;
using JellyfishAdmin.Common.Web;
using JellyfishAdmin.Entity;
using JellyfishAdmin.LayoutManager;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

/// <summary>
/// Administration API - Collection Manager - Apply Layout Detail Class
/// </summary>
public partial class admin_CollectionManagerApplyLayoutDetail : WebFormBase
{
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        SetSessionData();
        if (!IsPostBack)
        {
            updateDetailForm();
            showDZPreview(userSessionEntity.TargetUId, userSessionEntity.TargetLId);
        }
        Debug.WriteLine("TargetLId : " + userSessionEntity.TargetLId);
    }


    /// <summary>
    /// Shows the DZ preview.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="lid">The lid.</param>
    private void showDZPreview(string userId, string lid)
    {
        string uploadFilesPath = GetPhysicalPath(@"sl\out\upload_files\" + userId + @"\layout\");
        string layoutFilesPath = GetPhysicalPath(@"sl\out\layouts\");
        string layoutFileUriPath = @"../sl/out/upload_files/" + userId + @"/layout/";

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.Load(layoutFilesPath + lid + ".xml");

        XmlNodeList nodes = xmlDoc.GetElementsByTagName("I");

        foreach (XmlNode node in nodes)
        {
            string strId = node.Attributes["Id"].Value;

            using (Bitmap bmp = new Bitmap(256, 256))
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

                Directory.CreateDirectory(uploadFilesPath + @"\source_files\");
                bmp.Save(uploadFilesPath + @"\source_files\" + strId + ".jpg", ImageFormat.Jpeg);
            }
        }


        ProcessStartInfo startInfo = new ProcessStartInfo(GetPhysicalPath(@"Bin\DziConv\SDImageConverter.exe"));
        startInfo.Arguments = uploadFilesPath + @"\source_files\" + " " + uploadFilesPath + " 256 90 5 5 1";
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        System.Diagnostics.Process _Process = Process.Start(startInfo);

        // Waiting until this process will exit...
        _Process.WaitForExit();
        _Process.Close();
        _Process.Dispose();

        // Rename the file name of collection.xml which has been created by SDIConverter.exe
        string collectionXmlFileName = GetCollectionXmlFileName();
        File.Copy(layoutFilesPath + lid + ".xml", uploadFilesPath + collectionXmlFileName);

        dzcPath.Attributes.Add("Source", layoutFileUriPath + collectionXmlFileName);

    }

    /// <summary>
    /// Gets the name of the collection XML file.
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
    /// Updates the detail form.
    /// </summary>
    private void updateDetailForm()
    {
        LayoutManagerDao dao = new LayoutManagerDao();
        LayoutInfoEntity entity = dao.GetLayoutInfoByLId(userSessionEntity.TargetUId,
                                                            userSessionEntity.TargetLId);

        lblLId.Text = "";
        txtTitle.Text = "";
        txtDescription.Text = "";
        cbxShare.Checked = false;

        if (entity != null)
        {
            lblLId.Text = entity.LId;
            txtTitle.Text = entity.Title;
            txtDescription.Text = entity.Description;
            if (entity.IsShare == 1)
            {
                cbxShare.Checked = true;
            }
        }
    }


    /// <summary>
    /// Handles the Click event of the btnCancel control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        String url = ConfigurationManager.AppSettings["CollectionManagerApplyLayoutURL"].ToString();
        Response.Redirect(url);
    }
}
