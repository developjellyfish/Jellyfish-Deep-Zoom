using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [Interaction Common]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        #region init properties

        /// <summary>
        /// set mouse Event type
        /// </summary>
        private string currentMouseEventType = MouseEventType.NONE;

        /// <summary>
        /// set mouse Event type
        /// </summary>
        public string CurrentMouseEventType
        {
            get
            {
                return currentMouseEventType;
            }
            set
            {
                currentMouseEventType = value;
            }
        }

        /// <summary>
        /// decide subImage is selectable or not
        /// </summary>
        private bool isSelectable = true;

        /// <summary>
        /// decide subImage is selectable or not
        /// </summary>
        public bool IsSelectable
        {
            get
            {
                return isSelectable;
            }
            set
            {
                isSelectable = value;
            }
        }

        /// <summary>
        /// number of total subimage including display and disable status.
        /// </summary>
        public int MsiLen
        {
            get
            {
                return msi.SubImages.Count;
            }
        }

        #region Zoom Properties

        /// <summary>
        /// minimum of zoom value.<br />
        /// default: 0(unlimited)
        /// </summary>
        private double minZoom = 0;

        /// <summary>
        /// minimum of zoom value.<br />
        /// default: 0(unlimited)
        /// </summary>
        public double MinZoom
        {
            get 
            { 
                return minZoom; 
            }
            set
            {
                if (value > 1)
                {
                    minZoom = 1;
                }
                else if (value < 0)
                {
                    minZoom = 0;
                }
                else
                {
                    minZoom = value;
                }

                if (ZoomValue < MinZoom)
                {
                    // adjust current zoom value when it exceed minZoom value
                    Point newCenter = new Point();
                    newCenter = msi.ElementToLogicalPoint(mouseDownPoint);
                    msi.ZoomAboutLogicalPoint((MinZoom / ZoomValue), newCenter.X, newCenter.Y);
                    ZoomValue = MinZoom;
                }
            }
        }

        /// <summary>
        /// maximum of zoom value.<br />
        /// default: Double.MaxValue(unlimited)
        /// </summary>
        private double maxZoom = Double.MaxValue;

        /// <summary>
        /// maximum of zoom value.<br />
        /// default: Double.MaxValue(unlimited)
        /// </summary>
        public double MaxZoom
        {
            get 
            {
                return maxZoom;
            }
            set
            {
                if (value < 1)
                {
                    maxZoom = 1;
                }
                else if (value < minZoom)
                {
                    maxZoom = minZoom;
                }
                else
                    maxZoom = value;

                if (ZoomValue > MaxZoom)
                {
                    // adjust current zoom value when it exceed MaxZoom value

                    Point newCenter = new Point();
                    newCenter = msi.ElementToLogicalPoint(mouseDownPoint);
                    msi.ZoomAboutLogicalPoint((MaxZoom / ZoomValue), newCenter.X, newCenter.Y);

                    ZoomValue = MaxZoom;
                }
            }
        }

        /// <summary>
        /// current zoom. basically 1/msi.ViewportWidth
        /// </summary>
        private double zoomValue = 1;

        /// <summary>
        /// current zoom. basically 1/msi.ViewportWidth
        /// </summary>
        public double ZoomValue
        {
            get 
            {
                return zoomValue;
            }
            set
            {
                if (value > MaxZoom)
                {
                    zoomValue = MaxZoom;
                }
                else if (value < MinZoom)
                {
                    zoomValue = MinZoom;
                }
                else
                {
                    zoomValue = value;
                }
                checkShowSlideButtonZoomValue();
            }
        }

        /// <summary>
        /// zoom level value for "Zoom" method
        /// </summary>
        private double zoomRatio = 2;

        /// <summary>
        /// zoom level value for "Zoom" method
        /// </summary>
        public double ZoomRatio
        {
            get
            {
                return zoomRatio;
            }
            set
            {
                zoomRatio = value;
            }
        }

        #endregion

        #endregion

        #region properties

        /// <summary>
        /// Index value that is using for slideshow
        /// </summary>
        private int currentSlideIndex = 0;

        /// <summary>
        /// Index value that is using for slideshow
        /// </summary>
        public int CurrentSlideIndex
        {
            get 
            {
                return currentSlideIndex;
            }
            set
            {

                if( value >= Indices.Count )
                {
                    currentSlideIndex = 0;
                }
                else if (value < 0)
                {
                    currentSlideIndex = Indices.Count - 1;
                }
                else
                {
                    currentSlideIndex = value;
                }
            }
        }

        #endregion

        #region Reset

        /// <summary>
        /// Reset axis and zoom level value
        /// </summary>
        public void Reset()
        {
            ResetCoorinates();
            ResetZoom();
        }

        /// <summary>
        /// Reset axis
        /// </summary>
        public void ResetCoorinates()
        {
            msi.ViewportOrigin = initViewportOrigin;
        }

        /// <summary>
        /// Reset zoom level value
        /// </summary>
        public void ResetZoom()
        {
            msi.ViewportWidth = initViewportWidth;
            zoomValue = 1 / initViewportWidth;
        }

        #endregion

        #region slideshow

        /// <summary>
        /// DispatcherTimer that execute periodically for using slideshow
        /// </summary>
        private DispatcherTimer dt;

        /// <summary>
        /// Toggles the slide button.
        /// </summary>
        private void toggleSlideButton()
        {
            Storyboard toggleStoryboard = new Storyboard();
            DoubleAnimation da = new DoubleAnimation();
            double durationSecond = 0.5;

            if (isShow)
            {
                topCanvas.Visibility = Visibility.Visible;
                da.From = topCanvas.Opacity;
                da.To = 1 / durationSecond;

            }
            else
            {
                topCanvas.Visibility = Visibility.Visible;
                da.From = topCanvas.Opacity;
                da.To = 1 / durationSecond;

                da.From = topCanvas.Opacity;
                da.To = topCanvas.Opacity * -1;
            }

            toggleStoryboard.Duration = TimeSpan.FromSeconds(durationSecond);
            Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Opacity)"));
            Storyboard.SetTarget(da, topCanvas);
            toggleStoryboard.Children.Add(da);
            toggleStoryboard.Completed += delegate(object sender, EventArgs e)
            {
                if (!isShow)
                {
                    topCanvas.Visibility = Visibility.Collapsed;
                }
            };
            toggleStoryboard.Begin();
        }

        /// <summary>
        /// start SlideShow.
        /// </summary>
        public void StartSlideShow()
        {
            CurrentLayoutStyle = LayoutStyle.NONE;
            isTweening = false;

            dt.Start();
        }

        /// <summary>
        /// Sets the dispatcher timer.
        /// </summary>
        private void setDispatcherTimer()
        {
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(3000);
            dt.Tick += new EventHandler(dt_Tick);
        }

        /// <summary>
        /// Handles the Tick event of the dt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void dt_Tick(object sender, EventArgs e)
        {
            if (!isTweening)
            {
                CurrentSlideIndex++;
                DoSlideShow();

                // tick
                MultiScaleSubImageEventArgs ev = new MultiScaleSubImageEventArgs(CurrentSlideIndex, msi, msi.SubImages[CurrentSlideIndex]);
                OnMultiScaleSubImageTick(ev);
            }
        }

        /// <summary>
        /// stop SlideShow.
        /// </summary>
        public void StopSlideShow()
        {
            isLayoutTweening = false;
            dt.Stop();
        }

        #endregion

        #region Zoom related 

        /// <summary>
        /// For showing prev / next button, check zoom level value,
        /// the button should be displayed
        /// </summary>
        private void checkShowSlideButtonZoomValue()
        {
            if (ZoomValue >= showSlideButtonZoomValue && isShow == false)
            {
                // button should be displayed
                isShow = true;
                toggleSlideButton();
            }
            else if (ZoomValue < showSlideButtonZoomValue && isShow == true)
            {
                // button should be disable
                isShow = false;
                toggleSlideButton();
            }
        }

        /// <summary>
        /// configure zoom value (zoom coefficient)
        /// </summary>
        /// <param name="zoom">Ratio of change with previous time</param>
        /// <param name="pointToZoom">standard axis for zoom execution</param>
        private void Zoom(double zoom, Point pointToZoom)
        {
            if (!isSlide)
            {
                isZooming = true;
                double prevZoom = ZoomValue;

                Point logicalPoint = msi.ElementToLogicalPoint(pointToZoom);

                //---- zoom ratio
                ZoomValue *= zoom;
                currentPosition = msi.ViewportOrigin;

                zoomTo = ZoomValue / prevZoom;

                msi.ZoomAboutLogicalPoint(zoomTo, logicalPoint.X, logicalPoint.Y);

                JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomChangeStarted(ev);
            }
        }

        /// <summary>
        /// configure zoom value(zoom value that would like to change)
        /// </summary>
        /// <param name="zoom">zoom value that would like to change</param>
        /// <param name="pointToZoom">standard axis for zoom execution</param>
        private void ZoomByRatio(double zoom, Point pointToZoom)
        {
            if (!isSlide)
            {
                isZooming = true;
                double prevZoom = ZoomValue;

                Point logicalPoint = msi.ElementToLogicalPoint(pointToZoom);

                //---- zoom ratio
                ZoomValue = zoom;
                currentPosition = msi.ViewportOrigin;

                zoomTo = ZoomValue / prevZoom;

                msi.ZoomAboutLogicalPoint(zoomTo, logicalPoint.X, logicalPoint.Y);
            }
        }

        /// <summary>
        ///　ZoomIn using mouse Down Point
        /// </summary>
        public void ZoomIn()
        {
            Zoom(ZoomRatio, mouseDownPoint);
        }

        /// <summary>
        ///　ZoomOut using mouse Down Point
        /// </summary>
        public void ZoomOut()
        {
            Zoom(1 / ZoomRatio, mouseDownPoint);
        }

        /// <summary>
        /// stretch specified subImage
        /// </summary>
        /// <param name="index">Index of SubImage</param>
        public void ZoomSingleSubImage(int index)
        {
            // deside there is subimage Index or not.
            judgeInRangeSubImage(index);

            isSlide = true;

            isLayoutTweening = true;
            SetZindex(index);

            CurrentLayoutStyle = LayoutStyle.NONE;
            Point newOrigin = new Point();

            MultiScaleSubImage mssi = msi.SubImages[index];

            // AspectRatio of MultiScaleImage
            double msiAsRatio = getAspectRatio();

            // ViewportWidth of subImage
            double mssiVpWidth = mssi.ViewportWidth;
            // Aspect ratio of subImage
            double mssiAspectRatio = mssi.AspectRatio;

            double mssiVpX = mssi.ViewportOrigin.X;
            double mssiVpY = mssi.ViewportOrigin.Y;

            // ViewportWidth of  multiScaleImage
            double msiVpWidth = msi.ViewportWidth;

            //-- If it is set move than 1, subimage is larger than display Area
            double adjustmentViewportWidth = 1.1;
            double destViewportWidth = 1 / mssiVpWidth / mssiAspectRatio * msiAsRatio;

            Point center = new Point();

            center.X = 0;
            center.Y = 0;

            double zeroVOriginX = -mssiVpX / mssiVpWidth;
            double zeroVOriginY = -mssiVpY / mssiVpWidth;

            double afterMsiVpWidth = 1 / mssiVpWidth / mssiAspectRatio * msiAsRatio * adjustmentViewportWidth;

            if ((1 / afterMsiVpWidth) >= MaxZoom && MaxZoom > 0)
            {
                afterMsiVpWidth = 1/MaxZoom;
            }
            else if ((1 / afterMsiVpWidth) <= MinZoom && MinZoom > 0)
            {
                afterMsiVpWidth = 1 / MinZoom;
            }

            newOrigin.X = zeroVOriginX + (1 / mssiVpWidth) / 2 - afterMsiVpWidth / 2;

            // Half of MultiScaleImage
            newOrigin.Y = zeroVOriginY + (1 / mssiVpWidth) / mssiAspectRatio / 2
                - afterMsiVpWidth / (msi.Width / msi.Height) / 2;

            msi.ViewportWidth = afterMsiVpWidth;

            CurrentLayoutStyle = LayoutStyle.NONE;

            ZoomValue = 1 / afterMsiVpWidth;

            msi.ViewportOrigin = newOrigin;
        }

        #endregion

        #region other layouts

        /// <summary>
        /// [Zoom] goNext
        /// </summary>
        public void Next()
        {
            if (!isTweening && !isZooming)
            {
                isTweening = true;
                CurrentSlideIndex++;
                DoSlideShow();
            }
        }

        /// <summary>
        /// [Zoom] goPrevious
        /// </summary>
        public void Previous()
        {
            if (!isTweening && !isZooming)
            {
                isTweening = true;
                CurrentSlideIndex--;
                DoSlideShow();
            }
        }

        #endregion

        #region LayoutStyle

        /// <summary>
        /// event handler for starting animation for DeepZoom
        /// </summary>
        public event JFDeepZoomEventHandler JFDeepZoomChangeStarted;

        /// <summary>
        /// Raises the DeepZoomChangeStarted event.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFDeepZoomEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFDeepZoomChangeStarted(JFDeepZoomEventArgs e)
        {
            JFDeepZoomEventHandler handler = JFDeepZoomChangeStarted;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// event handler for completing animation for DeepZoom
        /// </summary>
        public event JFDeepZoomEventHandler JFDeepZoomMotionFinished;

        /// <summary>
        /// raises the DeepZoomMotionFinished event.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFDeepZoomEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFDeepZoomMotionFinished(JFDeepZoomEventArgs e)
        {
            JFDeepZoomEventHandler handler = JFDeepZoomMotionFinished;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region constant executing

        /// <summary>
        /// storyboard of constant executing, sync layout modifying and check Image position
        /// </summary>
        private Storyboard sb;

        /// <summary>
        /// Initialize storyboard of constant executing, sync layout modifying and check Image position
        /// </summary>
        private void InitStoryboard()
        {
            sb = new Storyboard();
            sb.Duration = new Duration(TimeSpan.Parse("0:0:0"));
            sb.Completed += new EventHandler(storyboard_Completed);
            sb.Begin();
        }

        /// <summary>
        /// Begin Storyboard checking ViewportOrigin
        /// </summary>
        private void StartCheckViewportOrigin()
        {
            InitStoryboard();
        }

        /// <summary>
        /// Stop Storyboard checking ViewportOrigin
        /// </summary>
        private void StopCheckViewportOrigin()
        {
            sb.Stop();
        }

        #endregion
    }
}
