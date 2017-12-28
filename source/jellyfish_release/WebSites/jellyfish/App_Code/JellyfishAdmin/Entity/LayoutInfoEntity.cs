using System;
using System.Collections.Generic;
using System.Web;

namespace JellyfishAdmin.Entity
{
    /// <summary>
    /// LayoutInfoEntity
    /// </summary>
    public class LayoutInfoEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInfoEntity"/> class.
        /// </summary>
        public LayoutInfoEntity()
        {

        }

        private String _lid;
        /// <summary>
        /// Gets or sets the L id.
        /// </summary>
        /// <value>The L id.</value>
        public String LId
        {
            get
            {
                return this._lid;
            }
            set
            {
                this._lid = value;
            }
        }

        private String _url;
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public String Url
        {
            get
            {
                return this._url;
            }
            set
            {
                this._url = value;
            }
        }

        private String _title;
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public String Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        private String _description;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public String Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        private Int32 _defaultFlag;
        /// <summary>
        /// Gets or sets the default flag.
        /// </summary>
        /// <value>The default flag.</value>
        public Int32 DefaultFlag
        {
            get
            {
                return this._defaultFlag;
            }
            set
            {
                this._defaultFlag = value;
            }
        }

        private int _isShare;
        /// <summary>
        /// Gets or sets the is share.
        /// </summary>
        /// <value>The is share.</value>
        public int IsShare
        {
            get
            {
                return this._isShare;
            }
            set
            {
                this._isShare = value;
            }
        }

        private DateTime _date;
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date
        {
            get
            {
                return this._date;
            }
            set
            {
                this._date = value;
            }
        }

        private String _owner;
        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public String Owner
        {
            get
            {
                return this._owner;
            }
            set
            {
                this._owner = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public String ToDebugString()
        {
            String str = "";

            str += " LId => " + LId;
            str += " Url => " + Url;
            str += " Title => " + Title;
            str += " Description => " + Description;
            str += " DefaultFlag => " + DefaultFlag;
            str += " IsShare => " + IsShare;
            str += " Date => " + Date;
            str += " Owner => " + Owner;

            return str;
        }
    }
}
