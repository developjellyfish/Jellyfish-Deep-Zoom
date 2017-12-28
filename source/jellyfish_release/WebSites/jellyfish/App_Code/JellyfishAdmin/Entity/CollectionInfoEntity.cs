using System;
using System.Collections.Generic;
using System.Web;

namespace JellyfishAdmin.Entity
{
    /// <summary>
    /// CollectionInfoEntity
    /// </summary>
    public class CollectionInfoEntity
    {
        private String _cid;
        /// <summary>
        /// Gets or sets the Cid.
        /// </summary>
        /// <value>The Cid.</value>
        public String CId
        {
            get
            {
                return this._cid;
            }
            set
            {
                this._cid = value;
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

        private DateTime _applyStartDate;
        /// <summary>
        /// Gets or sets the apply start date.
        /// </summary>
        /// <value>The apply start date.</value>
        public DateTime ApplyStartDate
        {
            get
            {
                return this._applyStartDate;
            }
            set
            {
                this._applyStartDate = value;
            }
        }

        private DateTime _applyEndDate;
        /// <summary>
        /// Gets or sets the apply end date.
        /// </summary>
        /// <value>The apply end date.</value>
        public DateTime ApplyEndDate
        {
            get
            {
                return this._applyEndDate;
            }
            set
            {
                this._applyEndDate = value;
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
        /// Initializes a new instance of the <see cref="CollectionInfoEntity"/> class.
        /// </summary>
        public CollectionInfoEntity()
        {

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

            str += " CId => " + CId;
            str += " Url => " + Url;
            str += " Title => " + Title;
            str += " Date => " + Date;
            str += " Owner => " + Owner;

            return str;
        }
    }
}
