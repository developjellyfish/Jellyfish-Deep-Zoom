using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Configuration;
using System.Data;
using System.IO;
using JellyfishAdmin.Common.Web;
using JellyfishAdmin.Entity;
using JellyfishAdmin.CollectionManager;

/// <summary>
/// Administration API - Collection Manager Detail Class
/// </summary>
public partial class admin_CollectionManagerDetail : WebFormBase
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
        }
    }

    /// <summary>
    /// Updates the detail form.
    /// </summary>
    private void updateDetailForm()
    {
        InfoGrid.DataSource = CreateDataSourceByEntity(userSessionEntity.CollectionInfoEditData.UploadInfoList);

        if (userSessionEntity.CollectionInfoEditData.CollectionInfo != null)
        {
            txtTitle.Text = userSessionEntity.CollectionInfoEditData.CollectionInfo.Title;
            txtApplyStartDate.Text = userSessionEntity.CollectionInfoEditData.CollectionInfo.ApplyStartDate.ToString("yyyy/MM/dd");
            txtApplyEndDate.Text = userSessionEntity.CollectionInfoEditData.CollectionInfo.ApplyEndDate.ToString("yyyy/MM/dd");
        }
        else
        {
            btnApplyLayout.Visible = false;
            txtApplyStartDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            txtApplyEndDate.Text = "9999/12/31";
        }

        InfoGrid.DataBind();
    }

    /// <summary>
    /// Creates the data source by entity.
    /// </summary>
    /// <param name="entityList">The entity list.</param>
    /// <returns></returns>
    private ICollection CreateDataSourceByEntity(ArrayList entityList)
    {
        DataTable table = new DataTable();
        table.Columns.Add(new DataColumn("UId", typeof(string)));
        table.Columns.Add(new DataColumn("Owner", typeof(string)));
        table.Columns.Add(new DataColumn("Share", typeof(string)));
        table.Columns.Add(new DataColumn("Thumbnail", typeof(string)));
        table.Columns.Add(new DataColumn("Title", typeof(string)));
        table.Columns.Add(new DataColumn("Description", typeof(string)));
        table.Columns.Add(new DataColumn("Tag", typeof(string)));


        DataColumn[] keys = new DataColumn[1];
        keys[0] = table.Columns[0];
        table.PrimaryKey = keys;

        DataRow record = null;
        foreach (UploadInfoEntity entity in entityList)
        {
            record = table.NewRow();

            record[0] = entity.UId;
            record[1] = entity.Owner;
            record[2] = entity.IsShare == 1 ? "Shared" : "";
            record[3] = "<IMG SRC=\"../sl/" + entity.Thumbnail + "\" />";
            record[4] = entity.Title;
            record[5] = entity.Description;
            record[6] = entity.Tags;


            table.Rows.Add(record);
        }

        DataView view = new DataView(table);
        return view;

    }

    /// <summary>
    /// Handles the Click event of the btnDelete control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Hashtable deleteTarget = new Hashtable();

        foreach (DataGridItem item in InfoGrid.Items)
        {
            object ctrl = (object)item.FindControl("cbxSelect");

            if (ctrl is CheckBox)
            {
                CheckBox chk = (CheckBox)ctrl;
                if (chk.Checked)
                {
                    if (!deleteTarget.ContainsKey(InfoGrid.DataKeys[item.ItemIndex]))
                    {
                        deleteTarget.Add(InfoGrid.DataKeys[item.ItemIndex], "");
                    }

                    Debug.WriteLine("Grid Idx : " + item.ItemIndex + ", TargetUId : " + InfoGrid.DataKeys[item.ItemIndex]);
                }
            }
        }

        ArrayList uploadInfoList = new ArrayList();
        foreach (UploadInfoEntity uploadInfo in userSessionEntity.CollectionInfoEditData.UploadInfoList)
        {
            if (!deleteTarget.ContainsKey(uploadInfo.UId))
            {
                uploadInfoList.Add(uploadInfo);
            }
        }
        userSessionEntity.CollectionInfoEditData.UploadInfoList = uploadInfoList;

        // Data Grid Refresh
        updateDetailForm();
    }

    /// <summary>
    /// Handles the Click event of the btnAdd control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        String url = ConfigurationManager.AppSettings["CollectionManagerAddSelectURL"].ToString();
        Response.Redirect(url);
    }

    /// <summary>
    /// Handles the Click event of the btnRegister control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        ArrayList targetUId = new ArrayList();
        String targetFileStr = "";
        foreach (UploadInfoEntity entity in userSessionEntity.CollectionInfoEditData.UploadInfoList)
        {
            targetUId.Add(entity.UId);
            targetFileStr += entity.UId + ".jpg\n";
        }

        if (targetUId.Count == 0)
        {
            return;
        }

        // Execute Generate Collection Data

        CollectionManagerLogic logic = new CollectionManagerLogic();

        String targetCId = null;
        if (userSessionEntity.CollectionInfoEditData.CollectionInfo == null)
        {
            // New Registration
            targetCId = logic.CreateCId(userSessionEntity.UserID);
        }
        else
        {
            // Update Target Info
            targetCId = userSessionEntity.CollectionInfoEditData.CollectionInfo.CId;   
        }

        // Collection Data Output Directory
        String dirPath = logic.GetCollectionDataDir(targetCId);

        Application.Lock();

        Utility util = new Utility();
        if (Directory.Exists(dirPath))
        {
            util.DeleteFiles(dirPath);
        }
        else
        {
            Directory.CreateDirectory(dirPath);
        }

        StreamWriter fs = new StreamWriter(dirPath + CollectionManagerLogic.SOURCE_IMG_LIST_NAME);
        fs.Write(targetFileStr);
        fs.Close();

        Application.UnLock();


        ProcessStartInfo startInfo = new ProcessStartInfo(logic.GetPhysicalPath(@"Bin\DzcConv\DzcConverter.exe"));
        startInfo.Arguments = Server.MapPath(".") + "../../sl/out/source_files/"
                                + " " + dirPath
                                + " 256 90 5 5"
                                + " " + CollectionManagerLogic.COLLECTION_XML_NAME
                                + " " + logic.GetCollectionDataURLBase()
                                + " " + dirPath + CollectionManagerLogic.SOURCE_IMG_LIST_NAME
                                + " " + logic.GetPhysicalPath(@"sl\out\collection_images\");

        Debug.WriteLine("CONVERTER ARGS => " + startInfo.Arguments);

        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        System.Diagnostics.Process _Process = Process.Start(startInfo);

        _Process.WaitForExit();
        _Process.Close();
        _Process.Dispose();


        // Execute Table Update

        CollectionManagerDao dao = new CollectionManagerDao();

        if (userSessionEntity.CollectionInfoEditData.CollectionInfo == null)
        {
            // New Registration
            CollectionInfoEntity collectionInfo = new CollectionInfoEntity();
            collectionInfo.CId = targetCId;
            collectionInfo.Url = logic.GetCollectionXMLURL(targetCId);
            collectionInfo.Title = txtTitle.Text;

            collectionInfo.ApplyStartDate = DateTime.ParseExact(txtApplyStartDate.Text, "yyyy/MM/dd", null);
            collectionInfo.ApplyEndDate = DateTime.ParseExact(txtApplyEndDate.Text, "yyyy/MM/dd", null);
            collectionInfo.Owner = userSessionEntity.UserID;
            dao.CreateCollectionInfo(collectionInfo, targetUId);
        }
        else
        {
            // Update Target Info
            userSessionEntity.CollectionInfoEditData.CollectionInfo.Title = txtTitle.Text;
            userSessionEntity.CollectionInfoEditData.CollectionInfo.ApplyStartDate = DateTime.ParseExact(txtApplyStartDate.Text, "yyyy/MM/dd", null);
            userSessionEntity.CollectionInfoEditData.CollectionInfo.ApplyEndDate = DateTime.ParseExact(txtApplyEndDate.Text, "yyyy/MM/dd", null);
            dao.UpdateCollectionInfo(userSessionEntity.CollectionInfoEditData.CollectionInfo, targetUId);
        }


        // Refresh Session Object
        CollectionInfoEditDataEntity editData = new CollectionInfoEditDataEntity();

        dao = new CollectionManagerDao();
        ArrayList entityList = dao.GetCollectionInfoByOwner(userSessionEntity.UserID, targetCId);
        if (entityList.Count > 0)
        {
            editData.CollectionInfo = (CollectionInfoEntity)entityList[0];
        }
        editData.UploadInfoList = dao.GetUploadInfoByCId(targetCId);

        userSessionEntity.CollectionInfoEditData = editData;
    }

    /// <summary>
    /// Handles the Click event of the btnApplyLayout control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnApplyLayout_Click(object sender, EventArgs e)
    {
        String url = ConfigurationManager.AppSettings["CollectionManagerApplyLayoutURL"].ToString();
        Response.Redirect(url);
    }
}
