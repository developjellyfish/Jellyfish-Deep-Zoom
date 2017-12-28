using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;
using System.Configuration;

namespace JellyfishAdmin.Common.Web
{
    /// <summary>
    /// Admin site base web form
    /// </summary>
    public abstract class WebFormBase : System.Web.UI.Page
    {
        private UserSessionEntity _userSessionEntity;

        /// <summary>
        /// Gets or sets the user session entity.
        /// </summary>
        /// <value>The user session entity.</value>
        public UserSessionEntity userSessionEntity
        {
            get {
                return this._userSessionEntity;
            }
            set
            {
                this._userSessionEntity = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebFormBase"/> class.
        /// </summary>
        public WebFormBase()
        {
            
        }

        /// <summary>
        /// Sets the session data.
        /// </summary>
        public void SetSessionData()
        {
            userSessionEntity = (UserSessionEntity)Session[UserSessionEntity.SESSION_KEY_USER];

            if (userSessionEntity == null)
            {
                String url = ConfigurationManager.AppSettings["LoginURL"].ToString();
                Response.Redirect(url);
            }
        }

    }
}
