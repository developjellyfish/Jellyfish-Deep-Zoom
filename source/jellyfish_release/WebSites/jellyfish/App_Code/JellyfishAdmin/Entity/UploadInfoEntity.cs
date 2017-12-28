using System;
using System.Collections.Generic;
using System.Web;

namespace JellyfishAdmin.Entity
{
    /// <summary>
    /// UploadInfoEntity
    /// </summary>
    public class UploadInfoEntity
    {
        private String _uid;
        /// <summary>
        /// Gets or sets the UId.
        /// </summary>
        /// <value>The UId.</value>
        public String UId
        {
            get
            {
                return this._uid;
            }
            set
            {
                this._uid = value;
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

        private int _width;
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }

        private int _height;
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }

        private String _thumbnail;
        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        public String Thumbnail
        {
            get
            {
                return this._thumbnail;
            }
            set
            {
                this._thumbnail = value;
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

        private String _tags;
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public String Tags
        {
            get
            {
                return this._tags;
            }
            set
            {
                this._tags = value;
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
        /// Initializes a new instance of the <see cref="UploadInfoEntity"/> class.
        /// </summary>
        public UploadInfoEntity()
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

            str += " UId => " + UId;
            str += " Url => " + Url;
            str += " Width => " + Width;
            str += " Height => " + Height;
            str += " Thumbnail => " + Thumbnail;
            str += " Title => " + Title;
            str += " Description => " + Description;
            str += " Tags => " + Tags;
            str += " IsShare => " + IsShare;
            str += " Date => " + Date;
            str += " Owner => " + Owner;

            return str;
        }
    }
}
