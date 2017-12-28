using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Configuration;
using System.Web.UI.WebControls;
using JellyfishAdmin.Common.Web;
using JellyfishAdmin.Entity;
using JellyfishAdmin.ImageManager;

/// <summary>
/// Administration API - Collection Manager - Add Select Class
/// </summary>
public partial class admin_CollectionManagerAddSelect : WebFormBase
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
            InfoGrid.DataSource = CreateDataSourceByEntity(GetDispList());
            InfoGrid.DataBind();
        }
    }

    /// <summary>
    /// Gets the disp list.
    /// </summary>
    /// <returns></returns>
    private ArrayList GetDispList()
    {
        Hashtable addedInfo = new Hashtable();
        foreach (UploadInfoEntity entity in userSessionEntity.CollectionInfoEditData.UploadInfoList)
        {
            addedInfo.Add(entity.UId, "");
        }

        ImageManagerDao dao = new ImageManagerDao();
        ArrayList entityList = dao.GetUploadInfoByKeyword(userSessionEntity.UserID, null, null);


        ArrayList dispList = new ArrayList();
        foreach (UploadInfoEntity entity in entityList)
        {
            if (!addedInfo.ContainsKey(entity.UId))
            {
                dispList.Add(entity);
            }
        }

        return dispList;
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
        table.Columns.Add(new DataColumn("Share", typeof(string)));
        table.Columns.Add(new DataColumn("Thumbnail", typeof(string)));
        table.Columns.Add(new DataColumn("Title", typeof(string)));
        table.Columns.Add(new DataColumn("Description", typeof(string)));
        table.Columns.Add(new DataColumn("Tag", typeof(string)));
        table.Columns.Add(new DataColumn("Owner", typeof(string)));

        DataColumn[] keys = new DataColumn[1];
        keys[0] = table.Columns[0];
        table.PrimaryKey = keys;

        DataRow record = null;
        foreach (UploadInfoEntity entity in entityList)
        {
            record = table.NewRow();

            record[0] = entity.UId;
            record[1] = entity.IsShare == 1 ? "Shared" : "";
            record[2] = "<IMG SRC=\"../sl/" + entity.Thumbnail + "\" />";
            //record[2] = entity.Thumbnail;
            record[3] = entity.Title;
            record[4] = entity.Description;
            record[5] = entity.Tags;
            record[6] = entity.Owner;

            table.Rows.Add(record);
        }

        DataView view = new DataView(table);
        return view;

    }

    /// <summary>
    /// Handles the Click event of the btnCancel control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        String url = ConfigurationManager.AppSettings["CollectionManagerDetailURL"].ToString();
        Response.Redirect(url);
    }

    /// <summary>
    /// Handles the Click event of the btnCommit control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnCommit_Click(object sender, EventArgs e)
    {
        Hashtable addTarget = new Hashtable();

        foreach (DataGridItem item in InfoGrid.Items)
        {
            object ctrl = (object)item.FindControl("cbxSelect");

            if (ctrl is CheckBox)
            {
                CheckBox chk = (CheckBox)ctrl;
                if (chk.Checked)
                {
                    if (!addTarget.ContainsKey(InfoGrid.DataKeys[item.ItemIndex]))
                    {
                        addTarget.Add(InfoGrid.DataKeys[item.ItemIndex], "");
                    }

                    Debug.WriteLine("Grid Idx : " + item.ItemIndex + ", TargetUId : " + InfoGrid.DataKeys[item.ItemIndex]);
                }
            }
        }

        ArrayList dispList = GetDispList();
        foreach (UploadInfoEntity uploadInfo in dispList)
        {
            if (addTarget.ContainsKey(uploadInfo.UId))
            {
                userSessionEntity.CollectionInfoEditData.UploadInfoList.Add(uploadInfo);
            }
        }

        String url = ConfigurationManager.AppSettings["CollectionManagerDetailURL"].ToString();
        Response.Redirect(url);
    }
}
