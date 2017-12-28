
namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// Stored Data of ThumbnailList
    /// </summary>
    public class JFThumbnailList
    {

        /// <summary>
        /// Thumbnail file name
        /// </summary>
        private string thumbnailPath = "";

        /// <summary>
        /// Thumbnail file name
        /// </summary>
        public string ThumbnailPath
        {
            get
            {
                return thumbnailPath;
            }
            set
            {
                thumbnailPath = value;
            }
        }

        /// <summary>
        /// Index of SubImage
        /// </summary>
        private int pathIndex = -1;

        /// <summary>
        /// Index of SubImage
        /// </summary>
        public int PathIndex
        {
            get
            {
                return pathIndex;
            }
            set
            {
                pathIndex = value;
            }
        }
    }
}
