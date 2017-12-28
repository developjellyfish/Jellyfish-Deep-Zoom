using System;
using System.Collections.Generic;
using System.Web;

namespace JellyfishAdmin.CollectionManager
{
    /// <summary>
    /// CollectionManagerLogic Class
    /// </summary>
    public class CollectionManagerLogic
    {
        /// <summary>
        /// COLLECTION XML FILE NAME(DZC XML FILE NAME)
        /// </summary>
        public const String COLLECTION_XML_NAME = "collection.xml";
        /// <summary>
        /// SOURCE IMAGE LIST FILE NAME
        /// </summary>
        public const String SOURCE_IMG_LIST_NAME = "source_image_list.txt";

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionManagerLogic"/> class.
        /// </summary>
        public CollectionManagerLogic()
        {

        }

        /// <summary>
        /// Creates the C id.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns>string</returns>
        public String CreateCId(String userID)
        {
            return userID
                    + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + (new Random()).Next(1, 999).ToString("000")
                    + "_collection";
        }

        /// <summary>
        /// Gets the collection XMLURL.
        /// </summary>
        /// <param name="cid">The cid.</param>
        /// <returns>string</returns>
        public String GetCollectionXMLURL(String cid)
        {
            return "out/collections/" + cid + "/" + COLLECTION_XML_NAME;
        }

        /// <summary>
        /// Gets the collection data dir.
        /// </summary>
        /// <param name="cid">The cid.</param>
        /// <returns>string</returns>
        public String GetCollectionDataDir(String cid)
        {
            return HttpContext.Current.Server.MapPath(".") + "../../sl/out/collections/" + cid + "/";
        }

        /// <summary>
        /// Gets the collection data URL base.
        /// </summary>
        /// <returns>string</returns>
        public String GetCollectionDataURLBase()
        {
            //-> mod by kazumichi 20090310
            //return GetUriBasePath() + "sl/out/";
            return HttpRuntime.AppDomainAppVirtualPath + "/sl/out/";
            //<- mod by kazumichi 20090310
        }

        /// <summary>
        /// Gets the URI base path.
        /// </summary>
        /// <returns>string</returns>
        public String GetUriBasePath()
        {
            Uri basePath = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
            Uri destPath = new Uri(basePath, "..");
            return destPath.AbsoluteUri;
        }

        /// <summary>
        /// Gets the physical path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>string</returns>
        public String GetPhysicalPath(String path)
        {
            Uri basePath = new Uri(HttpContext.Current.Request.PhysicalApplicationPath);
            Uri destPath = new Uri(basePath, ".");
            return destPath.LocalPath + path;
        }
    }
}
