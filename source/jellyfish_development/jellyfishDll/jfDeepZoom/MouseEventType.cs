
namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// This class is to define several subImage's click behavior.
    /// </summary>
    public static class MouseEventType
    {
        /// <summary>
        /// NO execution
        /// </summary>
        public const string NONE = "none";

        /// <summary>
        /// only raise event 
        /// </summary>
        public const string EVENT_ONLY = "event_only";

        /// <summary>
        /// When Click SubImage, raise event and Zoom specified SubImage 
        /// </summary>
        public const string ZOOM = "zoom";
    }
}
