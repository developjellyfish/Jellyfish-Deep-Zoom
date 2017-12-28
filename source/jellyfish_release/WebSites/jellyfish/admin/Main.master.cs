using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using JellyfishAdmin.Common.Web;

/// <summary>
/// Administration API - Main Class
/// </summary>
public partial class admin_Main : System.Web.UI.MasterPage
{
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        UserSessionEntity entity = (UserSessionEntity)Session[UserSessionEntity.SESSION_KEY_USER];

        if (entity != null)
        {
            lblUserName.Text = entity.UserID;
        }
    }

    /// <summary>
    /// Handles the Click1 event of the LinkButton1 control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void LinkButton1_Click1(object sender, EventArgs e)
    {
        transferPage("HomeURL");
    }

    /// <summary>
    /// Handles the Click event of the LinkButton2 control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        transferPage("UploadImageURL");
    }

    /// <summary>
    /// Handles the Click event of the LinkButton3 control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        transferPage("ImageManagerListURL");
    }

    /// <summary>
    /// Handles the Click event of the LinkButton4 control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void LinkButton4_Click(object sender, EventArgs e)
    {
        transferPage("CollectionManagerListURL");
    }

    /// <summary>
    /// Handles the Click event of the LinkButton5 control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void LinkButton5_Click(object sender, EventArgs e)
    {
        transferPage("UploadLayoutURL");
    }

    /// <summary>
    /// Handles the Click event of the LinkButton6 control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void LinkButton6_Click(object sender, EventArgs e)
    {
        transferPage("LayoutManagerListURL");
    }

    /// <summary>
    /// Handles the Click event of the LinkButton7 control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void LinkButton7_Click(object sender, EventArgs e)
    {
        // Do your server-side stuff here getting the new window arguments.
        string newWindowUrl = ConfigurationManager.AppSettings["DeepZoomViewerURL"].ToString(); 
        string javaScript =
        "<script type='text/javascript'>\n" +
        "<!--\n" +
        "window.open('" + newWindowUrl + "');\n" +
        "// -->\n" +
        "</script>\n";
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", javaScript);
    }

    /// <summary>
    /// Handles the Click event of the btnLogout control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        transferPage("LoginURL");
    }

    /// <summary>
    /// Transfers the page.
    /// </summary>
    /// <param name="pageName">Name of the page.</param>
    private void transferPage(String pageName)
    {
        String url = ConfigurationManager.AppSettings[pageName].ToString();
        Response.Redirect(url);
    }
}
