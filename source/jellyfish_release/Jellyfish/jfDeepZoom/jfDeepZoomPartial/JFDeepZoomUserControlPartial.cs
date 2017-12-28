using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [UserControl]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        #region properties

        /// <summary>
        /// Root of JFDeepZoom
        /// </summary>
        private Canvas topCanvas;

        /// <summary>
        /// foreground Canvas of JFDeepZoom
        /// </summary>
        private Canvas foregroundCanvas;

        /// <summary>
        /// foreground Canvas of JFDeepZoom
        /// </summary>
        public Canvas ForegroundCanvas
        {
            get
            {
                return foregroundCanvas;
            }
            set
            {

                if (value != null)
                {
                    Canvas c = value;
                    Canvas parentCanvas = value.Parent as Canvas;
                    parentCanvas.Children.Remove(value);
                    if (foregroundCanvas != null)
                    {
                        foregroundCanvas.Children.Add(c);
                    }
                    else
                    {
                        preFCanvas = c;
                    }
                }
            }
        }

        private Canvas preBCanvas = new Canvas();
        private Canvas preFCanvas = new Canvas();

        /// <summary>
        /// background Canvas of JFDeepZoom
        /// </summary>
        private Canvas backgroundCanvas;

        /// <summary>
        /// background Canvas of JFDeepZoom
        /// </summary>
        public Canvas BackgroundCanvas
        {
            get
            {
                return backgroundCanvas;
            }
            set
            {
                if (value != null)
                {
                    Canvas c = value;
                    Canvas parentCanvas = value.Parent as Canvas;
                    parentCanvas.Children.Remove(value);
                    if (backgroundCanvas != null)
                    {
                        backgroundCanvas.Children.Add(c);
                    }
                    else
                    {
                        preBCanvas = c;
                    }
                }
            }
        }

        /// <summary>
        /// temp variable for configuring ForeGround Canvas before JFDeepZoom loading.
        /// </summary>
        private Canvas fCanvas;

        /// <summary>
        /// temp variable for configuring BackGround Canvas before JFDeepZoom loading.
        /// </summary>
        private Canvas bCanvas;

        private List<string> notSetProperty = new List<string>();
        private List<string> notSetValue = new List<string>();

        /// <summary>
        /// reference of MultiScaleImage for JFDeepZoom
        /// </summary>
        private MultiScaleImage msi;

        /// <summary>
        /// reference of MultiScaleImage for JFDeepZoom
        /// </summary>
        public MultiScaleImage Msi
        {
            get
            {
                return msi;
            }
        }

        #endregion

        /// <summary>
        /// init of Canvas
        /// </summary>
        private void InitCanvas()
        {
            foregroundCanvas = new Canvas();
            backgroundCanvas = new Canvas();

            msi = new MultiScaleImage();
            msi.ImageOpenSucceeded += new RoutedEventHandler(msi_ImageOpenSucceeded);
            msi.ImageOpenFailed += new EventHandler<ExceptionRoutedEventArgs>(msi_ImageOpenFailed);
            msi.MotionFinished += new RoutedEventHandler(msi_MotionFinished_event);
            msi.Loaded += new RoutedEventHandler(msi_Loaded);

            //----- back >>> front
            if (this.LayoutRoot != null)
            {
                this.LayoutRoot.Children.Add(BackgroundCanvas);
                this.LayoutRoot.Children.Add(msi);
                this.LayoutRoot.Children.Add(ForegroundCanvas);

                preFCanvas.Visibility = Visibility.Visible;
                preBCanvas.Visibility = Visibility.Visible;

                foregroundCanvas.Children.Add(preFCanvas);
                backgroundCanvas.Children.Add(preBCanvas);
            }
            StartCheckViewportOrigin();
        }

        #region zoom ratio of Next, Previous

        /// <summary>
        ///  the threshold for display Next/Previous button in JFDeepZoom
        /// </summary>
        private double showSlideButtonZoomValue = 1;

        /// <summary>
        ///  the threshold for display Next/Previous button in JFDeepZoom
        /// </summary>
        public double ShowSlideButtonZoomValue
        {
            get
            {
                return showSlideButtonZoomValue;
            }
            set
            {
                if (value > MinZoom && value < MaxZoom)
                {
                    showSlideButtonZoomValue = value;

                    checkShowSlideButtonZoomValue();
                }
            }
        }

        #endregion

        #region object of Next, Previous


        /// <summary>
        /// Previous Button in JFDeepZoom
        /// </summary>
        private Button previousButton = new Button();

        /// <summary>
        /// Previous Button in JFDeepZoom
        /// </summary>
        public Button PreviousButton
        {
            get
            {
                return previousButton;
            }
            set
            {
                previousButton = value;
            }
        }

        /// <summary>
        /// Next Button in JFDeepZoom
        /// </summary>
        private Button nextButton = new Button();

        /// <summary>
        /// Next Button in JFDeepZoom
        /// </summary>
        public Button NextButton
        {
            get
            {
                return nextButton;
            }
            set
            {
                nextButton = value;
            }
        }

        #endregion

        #region Template of Next, Previous

        /// <summary>
        /// Template of Previous Button in JFDeepZoom
        /// </summary>
        private ControlTemplate previousButtonTemplate;

        /// <summary>
        /// Template of Previous Button in JFDeepZoom
        /// </summary>
        public ControlTemplate PreviousButtonTemplate
        {
            get
            {
                return previousButtonTemplate;
            }
            set
            {
                previousButtonTemplate = value;
                PreviousButton.Template = previousButtonTemplate;
            }
        }

        /// <summary>
        /// Template of Next Button in JFDeepZoom
        /// </summary>
        private ControlTemplate nextButtonTemplate;

        /// <summary>
        /// Template of Next Button in JFDeepZoom
        /// </summary>
        public ControlTemplate NextButtonTemplate
        {
            get
            {
                return nextButtonTemplate;
            }
            set
            {
                nextButtonTemplate = value;
                NextButton.Template = nextButtonTemplate;
            }
        }

        #endregion

        #region event of Next, Previous


        /// <summary>
        /// Event Handles for Click event of the TopCanvasPreviousButton.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void TopCanvasPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isSlide)
            {
                Previous();
                JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomPrevious(ev);
            }
        }

        /// <summary>
        /// Event Handles for Click event of the TopCanvasNextButton.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void TopCanvasNextButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isSlide)
            {
                Next();
                JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomNext(ev);
            }
        }

        #endregion

        #region event of Next/Previous button in JFDeepZoom

        /// <summary>
        /// It calls when click the Previous Button.
        /// </summary>
        internal event JFDeepZoomEventHandler JFDeepZoomPrevious;

        /// <summary>
        /// It raises when click the Previous Button.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnJFDeepZoomPrevious(JFDeepZoomEventArgs e)
        {
            JFDeepZoomEventHandler handler = JFDeepZoomPrevious;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// It calls when click the Next Button.
        /// </summary>
        internal event JFDeepZoomEventHandler JFDeepZoomNext;

        /// <summary>
        /// It calls when click the Next Button.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnJFDeepZoomNext(JFDeepZoomEventArgs e)
        {
            JFDeepZoomEventHandler handler = JFDeepZoomNext;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}
