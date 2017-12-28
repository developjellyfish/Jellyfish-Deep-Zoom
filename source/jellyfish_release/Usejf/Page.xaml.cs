using System;
using System.Windows;
using System.Windows.Controls;


namespace Usejf
{
    /// <summary>
    /// This class consists of 6 parts:
    /// 
    /// 1. XAMLSync             [PageXAMLSyncFunctions.cs]
    /// 2. Communication        [PageCommunicationFunctions.cs]
    /// 3. Layout               [PageLayoutStyleFunctions.cs]
    /// 4. SlideShow            [PageSlideShowFunctions.cs]
    /// 5. Zoom                 [PageZoomFunctions.cs]
    /// 6. Set SubImage         [PageSetSubImageFunctions.cs]
    /// 7. ApproarchType        [PageApproachTypeFunctions.cs]
    /// 8. Loading              [PageLoadingFunctions.cs]
    /// </summary>
    public partial class Page : UserControl
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        public Page()
        {
            // Required to initialize variables
            InitializeComponent();

            // 1. synchronizing xaml
            InitSyncXAML();

            // 2. each input area is appeared when you click each button.
            InitTabs();

            // 3. change Layout style
            InitLayoutFunction();

            // 4. slideshow
            InitSlideShowFunction();

            // 5. zoomvalue
            InitZoomFunction();

            // 6. set subimage
            InitSetSubImageFunction();

            // 7. change Mode: static, semi-dynamic, dynamic
            InitApproachType();
            
            // 8. Dynamic loading ux
            InitLoading();
        }

        private void EntranceFadeOut_Completed(object sender, EventArgs e)
        {
            mainCanvas.Visibility = Visibility.Visible;
        }
    }
}
