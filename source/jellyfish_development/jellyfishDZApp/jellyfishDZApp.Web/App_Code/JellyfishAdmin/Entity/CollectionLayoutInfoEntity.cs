using System;
using System.Collections.Generic;
using System.Web;

namespace JellyfishAdmin.Entity
{
    /// <summary>
    /// LayoutInfoEntity
    /// </summary>
    public class CollectionLayoutInfoEntity
    {
        public CollectionLayoutInfoEntity()
        {

        }

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

        private String _lid;
        /// <summary>
        /// Gets or sets the Lid.
        /// </summary>
        /// <value>The Lid.</value>
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

            str += " CId => " + CId;
            str += " LId => " + LId;
            str += " Date => " + Date;
            str += " Owner => " + Owner;

            return str;
        }
    }
}
