using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [MultiSelect]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        #region properties

        /// <summary>
        /// check Image by select
        /// </summary>
        private List<Image> checkSubImageList = new List<Image>();

        //--- multiselect
        /// <summary>
        /// select use multiselect mode.
        /// </summary>
        private bool useMultiSelect = true;

        /// <summary>
        /// permit multi select or not
        /// </summary>
        public bool UseMultiSelect
        {
            get
            {
                return useMultiSelect;
            }
            set
            {
                useMultiSelect = value;

                // remain the last selected index. 
                if (! useMultiSelect)
                {
                    SelectedIndices.RemoveRange(0,SelectedIndices.Count - 1);

                    // remove all check Image by select
                    removeAllSubImageCheckMark();

                    // add "check" to last selected index SubImage
                    int lastSelectedIndex = SelectedIndices[0];

                    addSubCheckImageMark(lastSelectedIndex);

                    JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                    OnJFSelectionChanged(ev);
                }
            }
        }

        /// <summary>
        /// selected Indices
        /// </summary>
        private List<int> selectedIndices = new List<int>();

        /// <summary>
        /// selected Indices
        /// </summary>
        public List<int> SelectedIndices
        {
            get 
            {
                return selectedIndices;
            }
            set
            {
                if (UseMultiSelect)
                {
                    selectedIndices = value;

                    // remove all check Image by select
                    removeAllSubImageCheckMark();

                    int selectedIndicesCount = selectedIndices.Count;

                    for (int i = 0; i < selectedIndicesCount; i++) 
                    {
                        addSubCheckImageMark(selectedIndices[i]);
                    }

                    JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                    OnJFSelectionChanged(ev);
                }
                else
                {
                    SelectedIndices.RemoveRange(SelectedIndices.Count-1, 1);

                    JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                    OnJFSelectionChanged(ev);
                }
            }
        }

        /// <summary>
        /// reference of List of Selected SubImage
        /// </summary>
        public List<MultiScaleSubImage> SelectedItems
        {
            get
            {
                List<MultiScaleSubImage> items = new List<MultiScaleSubImage>();

                for (int i = 0; i < SelectedIndices.Count; i++)
                {
                    items.Add(msi.SubImages[SelectedIndices[i]]);
                }
                
                return items;
            }
        }

        /// <summary>
        /// width of check Image by select
        /// </summary>
        private double checkImageWidth = 15;

        /// <summary>
        /// width of check Image by select
        /// </summary>
        public double CheckImageWidth
        {
            get
            {
                return checkImageWidth;
            }
            set
            {
                if (checkImageWidth > 0)
                {
                    checkImageWidth = value;
                }
            }
        }

        /// <summary>
        /// URI of check Image by select
        /// </summary>
        private string selectCheckImageUri = "imageCheck.jpg";

        /// <summary>
        /// URI of check Image by select
        /// </summary>
        public string SelectCheckImageUri
        {
            get
            {
                return selectCheckImageUri;
            }
            set
            {
                selectCheckImageUri = value;
            }
        }

        /// <summary>
        /// set display/disable of all check Image by select
        /// </summary>
        private bool useVisiblitySubImageCheckMark = true;

        #endregion

        #region event 

        /// <summary>
        /// It calls when the Selection Changed
        /// </summary>
        public event JFDeepZoomEventHandler JFSelectionChanged;

        /// <summary>
        /// It raises when the Selection Changed
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFDeepZoomEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFSelectionChanged(JFDeepZoomEventArgs e)
        {
            JFDeepZoomEventHandler handler = JFSelectionChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region MultiSelect Method

        /// <summary>
        /// add to SelectedIndex
        /// </summary>
        /// <param name="subImageIndex">Index of the subImage.</param>
        public void AddSelectedIndex(int subImageIndex)
        {
            if (UseMultiSelect)
            {
                // In case of multi select
                if (!SelectedIndices.Contains(subImageIndex))
                {
                    SelectedIndices.Add(subImageIndex);

                    // Add check Image by select
                    addSubCheckImageMark(subImageIndex);

                    JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                    OnJFSelectionChanged(ev);
                }
            }
            else
            {
                // In case of single select
                if (SelectedIndices.Count > 0)
                {
                    SelectedIndices.RemoveRange(0, SelectedIndices.Count);
                    //  remove all check Image by select
                    removeAllSubImageCheckMark();
                }
                SelectedIndices.Add(subImageIndex);

                // Add check Image by select
                addSubCheckImageMark(subImageIndex);

                JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                OnJFSelectionChanged(ev);
            }
        }

        /// <summary>
        /// add to SelectedIndex(with the position set)
        /// </summary>
        /// <param name="subImageIndex">Index of the subImage.</param>
        /// <param name="position">The position</param>
        public void AddSelectedIndexAt(int subImageIndex, int position)
        {
            if (UseMultiSelect)
            {
                if (!SelectedIndices.Contains(subImageIndex))
                {
                    SelectedIndices.Add(subImageIndex);

                    /// Add check Image by select
                    addSubCheckImageMark(subImageIndex);

                    JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                    OnJFSelectionChanged(ev);
                }
            }
            else
            {
                // In case of single select
                if (SelectedIndices.Count > 0)
                {
                    SelectedIndices.RemoveRange(0, SelectedIndices.Count);
                    //  remove all check Image by select
                    removeAllSubImageCheckMark();
                }
                SelectedIndices.Add(subImageIndex);

                /// Add check Image by select
                addSubCheckImageMark(subImageIndex);

                JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                OnJFSelectionChanged(ev);
            }
        }

        /// <summary>
        /// remove index from SelectedIndices
        /// </summary>
        /// <param name="index">index of SelectedIndices </param>
        public void RemoveSelectedIndexAt(int index)
        {
            if (UseMultiSelect)
            {
                if ((SelectedIndices.Count - 1) >= index && index >= 0)
                {
                    // selected Indices
                    int subIndex = selectedIndices[index];

                    // remove check Image by select
                    deleteSubCheckImageMark(subIndex);

                    SelectedIndices.RemoveAt(index);

                    JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                    OnJFSelectionChanged(ev);
                }
            }
            else
            {
                // In case of single select
                if (SelectedIndices.Count > 0)
                {
                    SelectedIndices.RemoveRange(0, SelectedIndices.Count);
                }
                SelectedIndices.Add(index);

                JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                OnJFSelectionChanged(ev);
            }
        }

        /// <summary>
        /// remove index from SelectedIndices (specify Index of MultiScaleSubImage)
        /// </summary>
        /// <param name="subImageIndex">MutiScaleSubImageのindex</param>
        public void RemoveSelectedSubImageIndex(int subImageIndex)
        {
            int listIndex = ConvertSubImageIndexToSelectedIndex(subImageIndex);

            if (SelectedIndices.Contains(subImageIndex) && ((SelectedIndices.Count - 1) >= listIndex && listIndex >= 0))
            {
                // remove check Image by select
                deleteSubCheckImageMark(subImageIndex);

                SelectedIndices.RemoveAt(listIndex);

                JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                OnJFSelectionChanged(ev);
            }
        }

        /// <summary>
        /// De-Select all Images
        /// </summary>
        public void RemoveAllSelectedIndex()
        {
            // remove all check Image by select
            removeAllSubImageCheckMark();

            // remove all selected Indices
            SelectedIndices.RemoveRange(0, SelectedIndices.Count);

            JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
            OnJFSelectionChanged(ev);
        }

        /// <summary>
        /// Converts the index of the subImage index to selected Index.
        /// </summary>
        /// <param name="subImageIndex">Index of the subImage.</param>
        /// <returns></returns>
        public int ConvertSubImageIndexToSelectedIndex(int subImageIndex)
        {
            int res = -1;

            res = SelectedIndices.IndexOf(subImageIndex);

            return res;
        }

        /// <summary>
        /// Add check Image by select
        /// </summary>
        /// <param name="inIndex">Index of SubImage</param>
        protected virtual void addSubCheckImageMark(int inIndex)
        {
            // deside there is subimage Index or not.
            judgeInRangeSubImage(inIndex);

            // Make Image Dynamically
            Image img = new Image();

            BitmapImage checkBitmapImage = new BitmapImage();

            if (selectCheckImageUri.Contains("http://") || selectCheckImageUri.Contains("https://"))
            {
                try
                {
                    checkBitmapImage = new BitmapImage(new Uri(selectCheckImageUri, UriKind.Absolute));
                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    checkBitmapImage = new BitmapImage(new Uri(selectCheckImageUri, UriKind.Relative));
                }
                catch (Exception) { }
            }

            img.Source = checkBitmapImage;

            if (img != null && foregroundCanvas != null)
            {
                foregroundCanvas.Children.Add(img);

                checkSubImageList.Add(img);

                // Top left axis of selected SubImage
                Point subPxPoint = getSubImagePoint(inIndex);

                // width of selected subImage
                double subPxWidth = getSubImageWidth(inIndex);

                // display at Top right SubImage
                Canvas.SetLeft(img, subPxPoint.X + subPxWidth - checkImageWidth);
                Canvas.SetTop(img, subPxPoint.Y);
            }
        }

        /// <summary>
        /// remove check Image by select
        /// </summary>
        /// <param name="inIndex">Index of SubImage</param>
        protected virtual void deleteSubCheckImageMark(int inIndex)
        {
            // convert to SelectedIndex
            int subSelectIndex = ConvertSubImageIndexToSelectedIndex(inIndex);

            Image img = checkSubImageList[subSelectIndex];

            checkSubImageList.RemoveAt(subSelectIndex);

            foregroundCanvas.Children.Remove(img);
        }

        /// <summary>
        /// sync subImage and check Image by select
        /// </summary>
        protected virtual void syncSubImageCheckMark()
        {
            int selectedIndicesCount = selectedIndices.Count;
            
            for (int i = 0; i < selectedIndicesCount; i++)
            {
                // selectedIndices
                int subIndex = selectedIndices[i];

                Image img = checkSubImageList[i];

                // Top left axis of selected SubImage
                Point subPxPoint = getSubImagePoint(subIndex);

                // width of selected subImage
                double subPxWidth = getSubImageWidth(subIndex);

                // Top right axis of selected SubImage
                Point subImageCheckPoint = new Point();
                subImageCheckPoint.X = subPxPoint.X + subPxWidth - checkImageWidth;
                subImageCheckPoint.Y = subPxPoint.Y;

                // display at Top right SubImage
                Canvas.SetLeft(img, subImageCheckPoint.X);
                Canvas.SetTop(img, subImageCheckPoint.Y);

                if (isWithinRangeImage(img))
                {
                    img.Visibility = Visibility.Visible;
                }
                else
                {
                    img.Visibility = Visibility.Collapsed;
                }

            }
        }

        /// <summary>
        /// specified Image within MultiScaleImage or not
        /// </summary>
        /// <param name="inImg">SelectedImage</param>
        /// <returns>
        /// 	<c>true</c> if is within range MultiScaleImage; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool isWithinRangeImage(Image inImg)
        {
            bool outBol = true;

            double leftX = Canvas.GetLeft(inImg);
            double topY = Canvas.GetTop(inImg);

            double msiWidth = msi.ActualWidth;
            double msiHeight = msi.ActualHeight;

            if (leftX < 0 || leftX + checkImageWidth > msiWidth)
            {
                outBol = false;
            }

            if (topY < 0 || topY + checkImageWidth > msiHeight)
            {
                outBol = false;
            }

            return outBol;
        }

        /// <summary>
        /// set display/disable of all check Image by select
        /// </summary>
        public void SetVisiblitySubImageCheckMark(bool inBol)
        {
            useVisiblitySubImageCheckMark = inBol;

            int selectedIndicesCount = selectedIndices.Count;

            Visibility allVisibility = Visibility.Collapsed;

            if (inBol)
            {
                allVisibility = Visibility.Visible;
            }

            for (int i = 0; i < selectedIndicesCount; i++)
            {
                // selected Indices
                int subIndex = selectedIndices[i];

                Image img = checkSubImageList[i];

                img.Visibility = allVisibility;
            }
        }

        /// <summary>
        ///  remove all check Image by select
        /// </summary>
        protected virtual void removeAllSubImageCheckMark()
        {
            int checkSubImageCount = checkSubImageList.Count;

            for (int i = 0; i < checkSubImageCount; i++)
            {
                // Image of specified selected SubImage
                Image img = checkSubImageList[i];

                foregroundCanvas.Children.Remove(img);
            }

            // remove all checkSubImageList
            checkSubImageList.RemoveRange(0, checkSubImageList.Count);
        }

        #endregion
    }
}
