
namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// This class is to define several SemiDynamicExecType.
    /// </summary>
    public static class SemiDynamicExecType
    {
        /// <summary>
        /// NO execution
        /// </summary>
        public const string NONE = "none";

        /// <summary>
        /// execution for LIST
        /// </summary>
        public const string LIST = "list";

        /// <summary>
        /// execution for INIT
        /// </summary>
        public const string INIT = "init";

        /// <summary>
        /// execution for INIT_CHANGE_ORDER
        /// (Change order ThumbnailList, ascending order in DZC)
        /// </summary>
        public const string INIT_CHANGE_ORDER = "init_change_order";

        /// <summary>
        /// execution for CHANGE_LAYOUT
        /// （Load dzcPath in XML as string, and layout by client side.）
        /// </summary>
        public const string CHANGE_LAYOUT = "change_layout";

        /// <summary>
        /// execution for LAYOUT
        /// </summary>
        public const string LAYOUT = "layout";
    }
}
