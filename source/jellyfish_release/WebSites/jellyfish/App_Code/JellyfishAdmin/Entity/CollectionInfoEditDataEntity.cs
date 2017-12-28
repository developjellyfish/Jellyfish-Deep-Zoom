using System;
using System.Collections;
using System.Web;

namespace JellyfishAdmin.Entity
{
    /// <summary>
    /// CollectionInfoEditDataEntity
    /// </summary>
    public class CollectionInfoEditDataEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionInfoEditDataEntity"/> class.
        /// </summary>
        public CollectionInfoEditDataEntity()
        {
            UploadInfoList = new ArrayList();
        }

        private CollectionInfoEntity _collectionInfo;
        /// <summary>
        /// Gets or sets the collection info.
        /// </summary>
        /// <value>The collection info.</value>
        public CollectionInfoEntity CollectionInfo
        {
            get
            {
                return this._collectionInfo;
            }
            set
            {
                this._collectionInfo = value;
            }
        }

        private ArrayList _uploadInfoList;
        /// <summary>
        /// Gets or sets the upload info list.
        /// </summary>
        /// <value>The upload info list.</value>
        public ArrayList UploadInfoList
        {
            get
            {
                return this._uploadInfoList;
            }
            set
            {
                this._uploadInfoList = value;
            }
        }
    }
}
