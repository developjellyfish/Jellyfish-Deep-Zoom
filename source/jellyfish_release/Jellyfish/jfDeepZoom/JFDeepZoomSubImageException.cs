using System;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// This class is to clarify programs having a problem of SubImages.
    /// </summary>
    public class JFDeepZoomSubImageException:Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The error message..</param>
        public JFDeepZoomSubImageException(string message)
            : base(message)
        {

        }
    }
}
