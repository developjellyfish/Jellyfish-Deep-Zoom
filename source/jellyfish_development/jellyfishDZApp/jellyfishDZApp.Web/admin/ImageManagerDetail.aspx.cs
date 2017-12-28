using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Configuration;
using JellyfishAdmin.Common.Web;
using JellyfishAdmin.Entity;
using JellyfishAdmin.ImageManager;

/// <summary>
/// Administration API - Image Manager Detail Class
/// </summary>
public partial class admin_ImageManagerDetail : WebFormBase
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
        Debug.WriteLine("TargetUId : " +userSessionEntity.TargetUId);
    }

    /// <summary>
    /// Updates the detail form.
    /// </summary>
    private void updateDetailForm()
    {
        ImageManagerDao dao = new ImageManagerDao();
        UploadInfoEntity entity = dao.GetUploadInfoByUId(userSessionEntity.UserID,
                                                            userSessionEntity.TargetUId);

        lblUId.Text = "";
        txtTitle.Text = "";
        txtDescription.Text = "";
        txtTag.Text = "";
        imgThumbnail.ImageUrl = "";
        cbxShare.Checked = false;

        if (entity != null)
        {
            lblUId.Text = entity.UId;
            txtTitle.Text = entity.Title;
            txtDescription.Text = entity.Description;
            txtTag.Text = entity.Tags;
            imgThumbnail.ImageUrl = "../sl/" + entity.Thumbnail;
            if (entity.IsShare == 1)
            {
                cbxShare.Checked = true;
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the btnCommit control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnCommit_Click(object sender, EventArgs e)
    {
        ImageManagerDao dao = new ImageManagerDao();
        UploadInfoEntity entity = dao.GetUploadInfoByUId(userSessionEntity.UserID,
                                                            userSessionEntity.TargetUId);

        if (entity != null)
        {
            entity.Title = txtTitle.Text;
            entity.Description = txtDescription.Text;
            entity.Tags = txtTag.Text;
            entity.IsShare = cbxShare.Checked ? 1 : 0;

            dao.UpdateUploadInfoByEntity(entity);
        }

        //updateDetailForm();
        String url = ConfigurationManager.AppSettings["ImageManagerListURL"].ToString();
        Response.Redirect(url);
    }

    /// <summary>
    /// Handles the Click event of the btnCancel control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        String url = ConfigurationManager.AppSettings["ImageManagerListURL"].ToString();
        Response.Redirect(url);
    }
}
