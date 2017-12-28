using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Configuration;
using System.Data;
using JellyfishAdmin.Common.Web;
using JellyfishAdmin.Entity;
using JellyfishAdmin.CollectionManager;

/// <summary>
/// Administration API - Collection Manager List Class
/// </summary>
public partial class admin_CollectionManagerList : WebFormBase
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
            CollectionManagerDao dao = new CollectionManagerDao();
            ArrayList entityList = dao.GetCollectionInfoByOwner(userSessionEntity.UserID, null);
            if (entityList.Count > 0)
            {
                //Debug.WriteLine("Entity_List_Count : " +entityList.Count);
                InfoGrid.DataSource = CreateDataSourceByEntity(entityList);
                InfoGrid.DataBind();
            }
        }
    }

    /// <summary>
    /// Creates the data source by entity.
    /// </summary>
    /// <param name="entityList">The entity list.</param>
    /// <returns></returns>
    private ICollection CreateDataSourceByEntity(ArrayList entityList)
    {
        DataTable table = new DataTable();
        table.Columns.Add(new DataColumn("CId", typeof(string)));
        table.Columns.Add(new DataColumn("Title", typeof(string)));
        table.Columns.Add(new DataColumn("Date", typeof(string)));
        table.Columns.Add(new DataColumn("Owner", typeof(string)));

        DataColumn[] keys = new DataColumn[1];
        keys[0] = table.Columns[0];
        table.PrimaryKey = keys;

        DataRow record = null;
        foreach (CollectionInfoEntity entity in entityList)
        {
            record = table.NewRow();

            record[0] = entity.CId;
            record[1] = entity.Title;
            record[2] = entity.Date.ToString();
            record[3] = entity.Owner;

            table.Rows.Add(record);
        }

        DataView view = new DataView(table);
        return view;

    }

    /// <summary>
    /// Handles the OnItemCommand event of the InfoGrid control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
    protected void InfoGrid_OnItemCommand(object sender, DataGridCommandEventArgs e)
    {
        Debug.WriteLine("Call InfoGrid_OnItemCommand");
        if ("Edit".Equals(e.CommandName))
        {
            String targetCId = (String)InfoGrid.DataKeys[e.Item.ItemIndex];

            Debug.WriteLine("Grid Idx : " + e.Item.ItemIndex + ", TargetCId : " + targetCId);

            CollectionInfoEditDataEntity editData = new CollectionInfoEditDataEntity();
           
            CollectionManagerDao dao = new CollectionManagerDao();
            ArrayList entityList = dao.GetCollectionInfoByOwner(userSessionEntity.UserID, targetCId);
            if (entityList.Count > 0)
            {
                editData.CollectionInfo = (CollectionInfoEntity) entityList[0];
            }
            editData.UploadInfoList = dao.GetUploadInfoByCId(targetCId);
            

            userSessionEntity.CollectionInfoEditData = editData;

            String url = ConfigurationManager.AppSettings["CollectionManagerDetailURL"].ToString();
            Server.Transfer(url);
        }
    }

    /// <summary>
    /// Handles the Click event of the btnDelete control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ArrayList targetList = new ArrayList();

        foreach (DataGridItem item in InfoGrid.Items)
        {
            object ctrl = (object)item.FindControl("cbxSelect");

            if (ctrl is CheckBox)
            {
                CheckBox chk = (CheckBox)ctrl;
                if (chk.Checked)
                {
                    targetList.Add(InfoGrid.DataKeys[item.ItemIndex]);

                    Debug.WriteLine("Grid Idx : " + item.ItemIndex + ", TargetCId : " + InfoGrid.DataKeys[item.ItemIndex]);
                }
            }
        }


        if (targetList.Count > 0)
        {
            CollectionManagerDao dao = new CollectionManagerDao();

            // Execute Delete
            dao.DeleteCollectionInfoByCId(targetList);

            // Data Grid Refresh
            ArrayList entityList = dao.GetCollectionInfoByOwner(userSessionEntity.UserID, null);
            if (entityList != null)
            {
                //Debug.WriteLine("Entity_List_Count : " +entityList.Count);
                InfoGrid.DataSource = CreateDataSourceByEntity(entityList);
                InfoGrid.DataBind();
            }
        }
    }
}
