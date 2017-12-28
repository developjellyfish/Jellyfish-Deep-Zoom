using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using JellyfishAdmin.Common.Web;
using JellyfishAdmin.Entity;


/// <summary>
/// Administration API - Login Class
/// </summary>
public partial class admin_Login : System.Web.UI.Page
{
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Login1_LoggedIn(object sender, EventArgs e)
    {
        UserSessionEntity entity = new UserSessionEntity();
        //entity.UserID = HttpContext.Current.User.Identity.Name;
        entity.UserID = Login1.UserName;
        Session[UserSessionEntity.SESSION_KEY_USER] = entity;

        String url = ConfigurationManager.AppSettings["StartURL"].ToString();
        Response.Redirect(url);
    }
}
