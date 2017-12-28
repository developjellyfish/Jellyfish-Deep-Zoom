using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Jellyfish.jfDeepZoom;

namespace Usejf
{
    /// <summary>
    /// [SyncXAML]
    /// </summary>
    public partial class Page : UserControl
    {
        #region synchronizing with MultiScaleSubImage.

        #region properties
        /// <summary>
        /// this storyboard observes synchronizing MultiScaleSubImage and XAML outside Jellyfish.
        /// </summary>
        private Storyboard observerStoryboard;

        private int clickCount = 0;

        /// <summary>
        /// this property holds synchronizing index.
        /// </summary>
        private int syncIndex = -1;

        #endregion

        private void InitSyncXAML()
        {
            if (CurrentApproachType != ApproachType.STATIC)
            {
                // initializing observerStoryboard.
                observerStoryboard = new Storyboard();
                observerStoryboard.Duration = TimeSpan.FromMilliseconds(0);
                observerStoryboard.Completed += new EventHandler(observerStoryboard_Completed);

                // set event handler.
                jfd.MultiScaleSubImageMouseLeftButtonUp += new MultiScaleSubImageEventHandler(jfd_MultiScaleSubImageMouseLeftButtonUp);
                jfd.MouseLeave += new MouseEventHandler(jfd_MouseLeave);

                // attach parentInfoCanvas into jfd object.
                Canvas attachedCanvas = this.parentInfoCanvas;
                this.LayoutRoot.Children.Remove(parentInfoCanvas);

                // swap ForegroundCanvas 
                jfd.ForegroundCanvas = attachedCanvas;
            }
        }


        #region event handler.
        private void jfd_MultiScaleSubImageMouseLeftButtonUp(object sender, MultiScaleSubImageEventArgs e)
        {
            SwitchNextPreviousButtonEnable(false);

            if (CurrentApproachType != ApproachType.STATIC)
            {
                syncIndex = e.Id;
                showAdditionalData(e.Id);
                if (clickCount == 0)
                {
                    if (showInfo)
                    {
                        additionalData.Visibility = Visibility.Visible;
                        observerStoryboard.Begin();
                        additionalDataFadeIn.Begin();
                    }

                    clickCount++;
                }
                else
                {
                    if (!showInfo)
                    {
                        additionalDataFadeOut.Completed += new EventHandler(additionalDataFadeOut_Completed);
                        additionalDataFadeOut.Begin();
                    }
                    clickCount = 0;
                }
            }
        }

        private void jfd_MouseLeave(object sender, MouseEventArgs e)
        {
            additionalDataFadeOut.Completed += new EventHandler(additionalDataFadeOut_Completed);
            additionalDataFadeOut.Begin();
            clickCount = 0;
        }

        private void additionalDataFadeOut_Completed(object sender, EventArgs e)
        {
            additionalData.Visibility = Visibility.Collapsed;
            resetAdditionalData();
            observerStoryboard.Stop();
        }

        
        /// <summary>
        /// Synchronize.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void observerStoryboard_Completed(object sender, EventArgs e)
        {

            if (syncIndex >= 0)
            {
                //Debug.WriteLine("index:" + syncIndex);
                Point subImageRect = new Point(jfd.getSubImageWidth(syncIndex), jfd.getSubImageHeight(syncIndex));
                Point np = jfd.getSubImagePoint(syncIndex);

                //// set overlay XAML position.
                Canvas.SetTop(additionalData, np.Y-60 );
                Canvas.SetLeft(additionalData, np.X-20 + jfd.getSubImageWidth(syncIndex) );

                ScaleTransform st = new ScaleTransform();

                st.ScaleX = 1 / jfd.Msi.ViewportWidth;
                st.ScaleY = 1 / jfd.Msi.ViewportWidth;
                additionalData.RenderTransform = st;

                observerStoryboard.Begin();
            }
        }
        #endregion


        #region methods for additionalData control.

        private void showAdditionalData(int index)
        {

            /*
             * string jfd.GetSubImageData( int id, string tagname);
             * arguments：
             * id ID
             * tagname XML node
             * 
             * return
             * string result information
             */

            string date = jfd.GetSubImageData(index, "Date");
            string author = jfd.GetSubImageData(index, "Owner");
            string nodata = jfd.GetSubImageData(index, "Thumbnail");
            string tagdata = jfd.GetSubImageData(index, "Tag");
            string title = jfd.GetSubImageData(index, "Title");


           // additionalData.Visibility = Visibility.Visible;
            Point CurrentPoint = new Point(0, 0);

            CurrentPoint = jfd.getSubImagePoint(index);

            // set position of additionalData control.
            //Canvas.SetLeft(additionalData, CurrentPoint.X + jfd.getSubImageWidth(syncIndex));
            //Canvas.SetTop(additionalData, CurrentPoint.Y);

            // displaying information in TextBlocks on Overlay XAML.
            tb1.Text = date;
            tb2.Text = author;
            tb3.Text = tagdata;
            tb4.Text = title;

            // begin animation
            if (showInfo)
            {
                additionalData.Visibility = Visibility.Visible;
                additionalDataFadeIn.Begin();
            }
        }

        private void resetAdditionalData()
        {
            tb1.Text = "";
            tb2.Text = "";
            tb3.Text = "";
            tb4.Text = "";
        }
        #endregion

        #endregion
    }
}
