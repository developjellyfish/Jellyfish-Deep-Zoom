using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [Init]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        #region init property

        /// <summary>
        /// loading JFDeepzoom or not
        /// </summary>
        private bool isLoaded = false;

        #endregion

        /// <summary>
        /// Initalize.
        /// </summary>
        private void Init()
        {
            //--------------- >>> data handling
            try
            {
                detailData = new XDocument();
                List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

                Dictionary<string, string> table1 = new Dictionary<string, string>();
                table1["condition1"] = "";
                dataList.Add(table1);

                Dictionary<string, string> table2 = new Dictionary<string, string>();
                table2["condition2"] = "";

                dataList.Add(table2);
            }
            catch (Exception)
            { }

            Syncronizing();
            this.SizeChanged += new SizeChangedEventHandler(JFDeepZoom_SizeChanged);

            int len = msi.SubImages.Count;

            topCanvas = new Canvas();
            if (LayoutRoot != null)
            {
                LayoutRoot.Children.Add(topCanvas);
            }
            setDispatcherTimer();
        }

        /// <summary>
        /// Initialize SubImage
        /// Before position "Default Position"
        /// </summary>
        private void InitSubImages()
        {
            uint ableToFinishTweenCount = 0;
            List<int> ableToFinishIndex = new List<int>();

            int useLen = Indices.Count;

            for( int i=0; i < useLen; i++ )
            {
                ableToFinishIndex.Add(0);
                int subIndex = Indices[i];

                MultiScaleSubImage mssi;
                if (subIndex >= msi.SubImages.Count)
                {
                    ableToFinishTweenCount++;
                    ableToFinishIndex[i] = 1;
                    continue;
                }
                mssi = msi.SubImages[subIndex];
                mssi.Opacity = 0;
                Point p = new Point();

                double destX = multiScaleSubImageInitPosition[subIndex].X;
                double destY = multiScaleSubImageInitPosition[subIndex].Y;

                p.X = destX;
                p.Y = destY;

                if (Math.Abs(p.X - destX) < 0.001 && Math.Abs(p.Y - destY) < 0.001)
                {
                    ableToFinishTweenCount++;
                    ableToFinishIndex[i] = 1;
                    p.X = destX;
                    p.Y = destY;
                }

                mssi.ViewportOrigin = p;
                mssi.ViewportWidth = multiScaleSubImageInitViewportWidth[subIndex];

                msi.ViewportWidth = 1;
                msi.ViewportOrigin = new Point(0, 0);

                // initViewportWidth;
                ZoomValue = 1;
            }

            if (ableToFinishIndex.Contains(0) == false)
            {
                isLayoutTweening = false;
            }
        }

        /// <summary>
        /// Event handler for Loaded event of the JFDeepZoom.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void JFDeepZoom_Loaded(object sender, RoutedEventArgs e)
        {
            if (Src != null && ! isLoaded)
            {
                CurrentApproachType = ApproachType.STATIC;
            }

            //--------------------- from init
            InitCanvas();
            Init();
            InitMouseEvent();

            if (CurrentApproachType != ApproachType.STATIC)
            {
                // Initialize events that is related with communication.
                InitCommunicationEvent();
            }
            //--------------------- from init

            isLoaded = true;

            #region Previous, Next Button

            NextButton.Width = NextButton.Height = 50;
            PreviousButton.Width = PreviousButton.Height = 50;
            NextButton.Content = ">";
            PreviousButton.Content = "<";
            topCanvas.Children.Add(NextButton);
            topCanvas.Children.Add(PreviousButton);
            Canvas.SetZIndex(NextButton, 20);
            Canvas.SetZIndex(PreviousButton, 21);

            double buttonTop = this.Height / 2 - NextButton.Height / 2;
            Canvas.SetTop(NextButton, buttonTop);
            Canvas.SetTop(PreviousButton, buttonTop);
            
            Canvas.SetLeft(NextButton, this.Width - NextButton.Width);
            Canvas.SetLeft(PreviousButton, 0);

            topCanvas.Visibility = Visibility.Collapsed;

            #endregion

            if (notSetValue.Count > 0)
            {
                for (int i = 0; i < notSetProperty.Count(); i++)
                {
                    if (notSetProperty[i] == "foreCanvas")
                    {
                        ForegroundCanvas = fCanvas;
                    }
                    else if (notSetProperty[i] == "backgroundCanvas")
                    {
                        BackgroundCanvas = bCanvas;
                    }
                    else if (notSetProperty[i] == "CurrentLayout")
                    {
                        // for static mode.
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for the Loaded event of the msi.
        /// </summary>
        /// <param name="sender">sender of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void msi_Loaded(object sender, RoutedEventArgs e)
        {
            // CurrentApproachType equal ApproachType.STATIC, then set Src again when msi.Loaded is raised.
            string path = "";
            if (! isMsiLoaded && CurrentApproachType == ApproachType.STATIC)
            {
                path = src;
                isMsiLoaded = true;
                Src = path;
            }
            
            if (PreviousLayoutStyle != "" && CurrentApproachType == ApproachType.STATIC)
            {
                Debug.WriteLine("static");
                CurrentLayoutStyle =  PreviousLayoutStyle;
            }
        }

        /// <summary>
        /// implementing JFDeepZoom_SizeChanged
        /// </summary>
        private void Syncronizing()
        {
            msi.Width = this.Width;
            msi.Height = this.Height;
            ForegroundCanvas.Width = this.Width;
            ForegroundCanvas.Height = this.Height;
            BackgroundCanvas.Width = this.Width;
            BackgroundCanvas.Height = this.Height;


            this.UpdateNextPrevButtonPosition();
        }
    }
}
