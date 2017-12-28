
namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// Stored Data of One of List for SemiDynamic Approach
    /// </summary>
    public class JFSemiDynamicColList
    {

        /// <summary>
        /// collection id of List
        /// </summary>
        private string collectionId = "";

        /// <summary>
        /// collection id of List
        /// </summary>
        public string CollectionId
        {
            get
            {
                return collectionId;
            }
            set
            {
                collectionId = value;
            }
        }

        /// <summary>
        /// collection id of dzc
        /// </summary>
        /// <value>The CId.</value>
        private string cId = "";

        /// <summary>
        /// collection id of dzc
        /// </summary>
        /// <value>The CId.</value>
        public string CId
        {
            get
            {
                return cId;
            }
            set
            {
                cId = value;
            }
        }

        /// <summary>
        /// Collection URL.
        /// </summary>
        /// <value>The Collection URL.</value>
        private string cUrl = "";

        /// <summary>
        /// Collection URL.
        /// </summary>
        /// <value>The Collection URL.</value>
        public string CUrl
        {
            get
            {
                return cUrl;
            }
            set
            {
                cUrl = value;
            }
        }

        /// <summary>
        /// Collection title.
        /// </summary>
        /// <value>The Collection title.</value>
        private string cTitle = "";

        /// <summary>
        /// Collection title.
        /// </summary>
        /// <value>The Collection title.</value>
        public string CTitle
        {
            get
            {
                return cTitle;
            }
            set
            {
                cTitle = value;
            }
        }

        /// <summary>
        /// Layout ID.
        /// </summary>
        private string lId = "";

        /// <summary>
        /// Layout ID.
        /// </summary>
        public string LId
        {
            get
            {
                return lId;
            }
            set
            {
                lId = value;
            }
        }

        /// <summary>
        /// Layout URL.
        /// </summary>
        private string lUrl = "";

        /// <summary>
        /// Layout URL.
        /// </summary>
        public string LUrl
        {
            get
            {
                return lUrl;
            }
            set
            {
                lUrl = value;
            }
        }

        /// <summary>
        /// Layout title.
        /// </summary>
        /// <value>title.</value>
        private string lTitle = "";

        /// <summary>
        /// Layout title.
        /// </summary>
        /// <value>title.</value>
        public string LTitle
        {
            get
            {
                return lTitle;
            }
            set
            {
                lTitle = value;
            }
        }

        /// <summary>
        /// Layout is "Default Layout" or not
        /// </summary>
        private bool isDefault = true;

        /// <summary>
        /// Layout is "Default Layout" or not
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return isDefault;
            }
            set
            {
                isDefault = value;
            }
        }
    }
}
