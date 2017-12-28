using System;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// This class is to clarify programs having a problem of MultiScaleImage.
    /// </summary>
    public class JFDeepZoomMultiScaleImageException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The error message.</param>
        public JFDeepZoomMultiScaleImageException(string message)
            : base(message)
        {

        }
    }
}
