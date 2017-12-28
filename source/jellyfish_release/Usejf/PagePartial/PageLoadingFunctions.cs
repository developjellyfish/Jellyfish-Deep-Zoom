using System;
using System.Windows;
using System.Windows.Controls;
using Jellyfish.jfDeepZoom;

namespace Usejf
{
    /// <summary>
    /// [Loading]
    /// </summary>
    public partial class Page : UserControl
    {

        private void InitLoading()
        {

        }

        /// <summary>
        /// use it in begining loading.
        /// </summary>
        public void StartLoading()
        {
            jfd.JFDeepZoomSrcIsOverLimit += new JFCommunicationErrorEventHandler(jfd_JFDeepZoomSrcIsOverLimit);
            LoadStatusTb.Text = "Now Loading...";
            LoadCtrl.Visibility = Visibility.Visible;
            LoadingFadeIn.Begin();
        }

        /// <summary>
        /// this method is called if the number of search result is over 100.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void jfd_JFDeepZoomSrcIsOverLimit(object sender, JFCommunicationErrorEventArgs e)
        {
            LoadStatusTb.Text = "Error: Over 10 images.";
            additionalDataFadeOutByError.Completed += new EventHandler(LoadingFadeOut_Completed);
            additionalDataFadeOutByError.Begin();
        }

        /// <summary>
        /// use it in finishing loading.
        /// </summary>
        public void StopLoading()
        {
            LoadingFadeOut.Completed += new EventHandler(LoadingFadeOut_Completed);
            LoadingFadeOut.Begin();
        }

        void LoadingFadeOut_Completed(object sender, EventArgs e)
        {
            LoadCtrl.Visibility = Visibility.Collapsed;
        }


    }
}
