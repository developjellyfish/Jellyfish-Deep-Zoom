using System;
using System.Windows.Controls;

namespace Jellyfish.jfDeepZoom
{

    /// <summary>
    /// EventArgs of JFDeepZoom
    /// </summary>
    public class JFDeepZoomEventArgs : EventArgs
    {

        /// <summary>
        /// MultiScaleImage
        /// </summary>
        private MultiScaleImage msi;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_msi">MultiScaleImage</param>
        public JFDeepZoomEventArgs(MultiScaleImage _msi)
        {
            msi = _msi;
        }

        /// <summary>
        /// reference to MultiScaleImage object in JFDeepZoom
        /// </summary>
        public MultiScaleImage Msi
        {
            get
            {
                return msi; 
            }
            set
            {
                msi = value;
            }
        }
    }
}