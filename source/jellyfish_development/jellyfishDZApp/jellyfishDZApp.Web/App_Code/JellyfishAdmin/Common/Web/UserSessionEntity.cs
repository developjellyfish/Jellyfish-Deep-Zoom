using System;
using System.Collections;
using System.Web;
using JellyfishAdmin.Entity;

namespace JellyfishAdmin.Common.Web
{
    /// <summary>
    /// UserSessionEntity Class
    /// </summary>
    public class UserSessionEntity
    {
        /// <summary>
        /// SESSION_KEY_USER
        /// </summary>
        public const String SESSION_KEY_USER = "JellyfishAdmin.Common.Web.SESSION_KEY_USER";

        private String _userID;
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public String UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        private String _targetUId;
        /// <summary>
        /// Gets or sets the target U id.
        /// </summary>
        /// <value>The target U id.</value>
        public String TargetUId
        {
            get
            {
                return this._targetUId;
            }
            set
            {
                this._targetUId = value;
            }
        }

        private String _targetLId;
        /// <summary>
        /// Gets or sets the target Lid.
        /// </summary>
        /// <value>The target Lid.</value>
        public String TargetLId
        {
            get
            {
                return this._targetLId;
            }
            set
            {
                this._targetLId = value;
            }
        }

        private CollectionInfoEditDataEntity _collectionInfoEditData;
        /// <summary>
        /// Gets or sets the collection info edit data.
        /// </summary>
        /// <value>The collection info edit data.</value>
        public CollectionInfoEditDataEntity CollectionInfoEditData
        {
            get
            {
                return this._collectionInfoEditData;
            }
            set
            {
                this._collectionInfoEditData = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionEntity"/> class.
        /// </summary>
        public UserSessionEntity()
        {

        }
    }
}
