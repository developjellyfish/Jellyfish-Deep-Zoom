using System;
using System.Windows.Controls;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// EventArgs for MultiScaleSubImages
    /// </summary>
    public class MultiScaleSubImageEventArgs : EventArgs
    {
        /// <summary>
        /// EventArgs for MultiScaleSubImage
        /// </summary>
        /// <param name="_id">Index of SubImage</param>
        /// <param name="_msi">MultiScaleImage</param>
        /// <param name="_mssi">MultiScaleSubImage</param>
        public MultiScaleSubImageEventArgs(int _id, MultiScaleImage _msi, MultiScaleSubImage _mssi)
        {
            id = _id;
            msi = _msi;
            mssi = _mssi;
        }

        /// <summary>
        /// index of MultiScaleSubImage
        /// </summary>
        private int id = -1;

        /// <summary>
        /// index of MultiScaleSubImage
        /// </summary>
        public int Id
        {
            get 
            {
                return id; 
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// reference to MultiScaleImage
        /// </summary>
        private MultiScaleImage msi;

        /// <summary>
        /// reference to MultiScaleImage
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

        /// <summary>
        /// reference to MultiScaleSubImage (Event object)
        /// </summary>
        private MultiScaleSubImage mssi;

        /// <summary>
        /// reference to MultiScaleSubImage (Event object)
        /// 
        /// </summary>
        public MultiScaleSubImage Mssi
        {
            get 
            {
                return mssi; 
            }
            set
            {
                mssi = value;
            }
        }
    }
}
