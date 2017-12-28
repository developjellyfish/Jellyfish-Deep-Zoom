using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

/// <summary>
/// Administration API - Default Class
/// </summary>
public partial class admin_Default : System.Web.UI.Page
{
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        String url = ConfigurationManager.AppSettings["LoginURL"].ToString();
        Server.Transfer(url);
    }
}
