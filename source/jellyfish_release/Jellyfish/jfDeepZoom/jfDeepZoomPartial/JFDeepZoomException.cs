using System.Collections.Generic;
using System.Windows.Controls;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [Exception]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {
        /// <summary>
        /// return reference to specified MultiScaleSubImage.
        /// </summary>
        /// <param name="index">index</param>
        /// <exception cref="JFDeepZoomSubImageException">this exception is called if you specify index that is out of range.</exception>
        /// <returns>references to specified MultiScaleSubImage</returns>
        public MultiScaleSubImage GetMultiScaleSubImage(int index)
        {
            // decide there is subimage Index or not.
            judgeInRangeSubImage(index);

            return msi.SubImages[index];
        }

        /// <summary>
        /// return references to specified MultiScaleSubImage<br />
        /// [multiple]
        /// </summary>
        /// <param name="indecies">indecies</param>
        /// <exception cref="JFDeepZoomSubImageException">this exception is called if you specify index that is out of range.</exception>
        /// <returns>references to specified MultiScaleSubImage as List</returns>
        public List<MultiScaleSubImage> GetMultiScaleSubImageGroup(List<int> indecies)
        {
            List<MultiScaleSubImage> res = new List<MultiScaleSubImage>();

            for (int i = 0; i < Indices.Count; i++)
            {
                // deside there is subimage Index or not.
                judgeInRangeSubImage(indecies[i]);

                res.Add(msi.SubImages[i]);
            }

            return res;
        }

        /// <summary>
        /// return reference to specified MultiScaleSubImage by indecies.
        /// </summary>
        /// <param name="index">index</param>
        /// <exception cref="JFDeepZoomSubImageException">this exception is called if you specify index that is out of range.</exception>
        /// <returns>references to specified MultiScaleSubImage</returns>
        public MultiScaleSubImage GetMultiScaleSubImageIndex(int index)
        {
            // deside there is subimage Index or not.
            judgeInIndices(index);

            int indicesIndex = Indices.IndexOf(index);

            return msi.SubImages[indicesIndex];
        }

        /// <summary>
        /// return references to specified MultiScaleSubImage by indecies.<br />
        /// [multiple]
        /// </summary>
        /// <param name="indecies">indecies</param>
        /// <exception cref="JFDeepZoomSubImageException">this exception is called if you specify index that is out of range.</exception>
        /// <returns>references to specified MultiScaleSubImage as List</returns>
        public List<MultiScaleSubImage> GetMultiScaleSubImagesIndices(List<int> indecies)
        {
            List<MultiScaleSubImage> res = new List<MultiScaleSubImage>();

            int len = Indices.Count;
            for (int i = 0; i < Indices.Count; i++)
            {
                // deside there is subimage Index or not.
                judgeInIndices(indecies[i]);

                res.Add(msi.SubImages[Indices[i]]);
            }

            return res;
        }

        /// <summary>
        /// deside there is subimage Index or not.
        /// </summary>
        /// <param name="index">Index of SubImage</param>
        private void judgeInRangeSubImage(int index)
        {
            int msiCount = msi.SubImages.Count;
            if (index > msiCount || index < 0)
            {
                throw new JFDeepZoomSubImageException("specified index[" + index.ToString() + "] must not be more than the number of the SubImage. \r\n You can specify index from 0 to " + (msiCount - 1).ToString() + ".");
            }
            else if (msiCount <= 0)
            {
                throw new JFDeepZoomSubImageException("the number of the MultiScaleImage is 0.\r\n It may not set MultiScaleImage object.");
            }
        }

        /// <summary>
        /// Decide SubImage with specified index is include Indices or not.
        /// </summary>
        /// <param name="index">SubImageのIndex</param>
        private void judgeInIndices(int index)
        {
            int indicesIndex = Indices.IndexOf(index);
            int msiCount = msi.SubImages.Count;

            if (index > msiCount || index < 0)
            {
                throw new JFDeepZoomSubImageException("specified index[" + index.ToString() + "] must not be more than the number of the SubImage. \r\n You can specify index from 0 to " + (msiCount - 1).ToString() + ".");
            }
            else if (msiCount <= 0)
            {
                throw new JFDeepZoomSubImageException("the number of the MultiScaleImage is 0.\r\n It may not set MultiScaleImage object.");
            }
            else if (indicesIndex == -1)
            {
                throw new JFDeepZoomSubImageException("specified index[" + index.ToString() + "] must not be more than the number of the Indices. ");
            }
        }
    }
}
