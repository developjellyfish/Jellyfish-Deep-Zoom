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
using JellyfishAdmin.LayoutManager;

/// <summary>
/// Administration API - Layout Manager List Class
/// </summary>
public partial class admin_LayoutManagerList : WebFormBase
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
            LayoutManagerDao dao = new LayoutManagerDao();
            ArrayList entityList = dao.GetLayoutInfoOnlyMineByKeyword(userSessionEntity.UserID, null);
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
        table.Columns.Add(new DataColumn("LId", typeof(string)));
        table.Columns.Add(new DataColumn("Owner", typeof(string)));
        table.Columns.Add(new DataColumn("Share", typeof(string)));
        table.Columns.Add(new DataColumn("Title", typeof(string)));
        table.Columns.Add(new DataColumn("Description", typeof(string)));


        DataColumn[] keys = new DataColumn[1];
        keys[0] = table.Columns[0];
        table.PrimaryKey = keys;

        DataRow record = null;
        foreach (LayoutInfoEntity entity in entityList)
        {
            record = table.NewRow();

            record[0] = entity.LId;
            record[1] = entity.Owner;
            record[2] = entity.IsShare == 1 ? "Shared" : "";
            record[3] = entity.Title;
            record[4] = entity.Description;

            table.Rows.Add(record);
        }

        DataView view = new DataView(table);
        return view;

    }

    /// <summary>
    /// Handles the OnItemCreated event of the InfoGrid control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
    protected void InfoGrid_OnItemCreated(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemIndex < 0)
        {
            return;
        }

        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (drv != null)
        {
            String owner = (String) drv.Row.ItemArray[1];
            if (!userSessionEntity.UserID.Equals(owner))
            {
                Button btn = (Button) e.Item.Cells[5].Controls[0];
                btn.Visible = false;
            }
        }
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
            String targetLId = (String)InfoGrid.DataKeys[e.Item.ItemIndex];

            Debug.WriteLine("Grid Idx : " + e.Item.ItemIndex + ", TargetLId : " +targetLId);

            LayoutManagerDao dao = new LayoutManagerDao();
            LayoutInfoEntity ownerInfo = dao.GetLayoutInfoByLId(userSessionEntity.UserID, targetLId);
            if (ownerInfo != null)
            {
                UserSessionEntity entity = (UserSessionEntity)Session[UserSessionEntity.SESSION_KEY_USER];
                entity.TargetLId = targetLId;


                String url = ConfigurationManager.AppSettings["LayoutManagerDetailURL"].ToString();
                Server.Transfer(url);
            }
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

                    Debug.WriteLine("Grid Idx : " + item.ItemIndex + ", TargetLId : " + InfoGrid.DataKeys[item.ItemIndex]);
                }
            }
        }


        if (targetList.Count > 0)
        {
            LayoutManagerDao dao = new LayoutManagerDao();

            // Execute Delete
            dao.DeleteLayoutInfoByLId(userSessionEntity.UserID, targetList);

            // Data Grid Refresh
            ArrayList entityList = dao.GetLayoutInfoOnlyMineByKeyword(userSessionEntity.UserID, null);
            if (entityList != null)
            {
                //Debug.WriteLine("Entity_List_Count : " +entityList.Count);
                InfoGrid.DataSource = CreateDataSourceByEntity(entityList);
                InfoGrid.DataBind();
            }
            txtTitle.Text = "";
        }
    }

    /// <summary>
    /// Handles the Click event of the btnSearch control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LayoutManagerDao dao = new LayoutManagerDao();

        String cndTitle = null;
        if (txtTitle.Text.Trim().Length > 0)
        {
            cndTitle = txtTitle.Text;
        }

        ArrayList entityList = dao.GetLayoutInfoOnlyMineByKeyword(userSessionEntity.UserID, cndTitle);
        if (entityList != null)
        {
            Debug.WriteLine("Entity_List_Count : " +entityList.Count);
            InfoGrid.DataSource = CreateDataSourceByEntity(entityList);
            InfoGrid.DataBind();
        }
    }
}
