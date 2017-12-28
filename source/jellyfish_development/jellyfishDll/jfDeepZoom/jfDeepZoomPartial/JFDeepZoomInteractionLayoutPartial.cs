using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [Interaction Layout]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        #region properties

        /// <summary>
        /// current mouse position
        /// </summary>
        private Point lastMousePos = new Point();

        /// <summary>
        /// current mouse position
        /// </summary>
        public Point LastMousePos
        {
            get
            {
                return lastMousePos;
            }
        }

        /// <summary>
        /// if mouse is dragging or not
        /// </summary>
        private bool mouseIsDragging = false;

        /// <summary>
        /// if mouse is dragging or not
        /// </summary>
        private bool mouseButtonPressed = false;

        /// <summary>
        /// Mouse position of JFDeepZoom_MouseLeftButtonDown
        /// </summary>
        private Point dragOffset = new Point();

        /// <summary>
        /// msi.ViewportOrigin for Mouse event raising time.
        /// </summary>
        private Point currentPosition = new Point();

       /// <summary>
       /// Default value of ViewportWidth
       /// </summary>
        private double initViewportWidth = 1;

        /// <summary>
        /// Default value of ViewportOrigin
        /// </summary>
        private Point initViewportOrigin = new Point(0, 0);
        
        /// <summary>
        /// Default value of ViewportWidth of SubImage
        /// </summary>
        private double initSubImageViewportWidth = 1;

        /// <summary>
        /// Default value of ViewportOrigin of SubImage
        /// </summary>
        private Point initSubImageViewportOrigin = new Point(0, 0);

        /// <summary>
        ///  Axis List of Initial SubImage
        /// </summary>
        private List<Point> multiScaleSubImageInitPosition = new List<Point>();

        /// <summary>
        /// the zoom value of before layout
        /// </summary>
        private double startZoom = 0;

        /// <summary>
        /// If it is tweening or not
        /// </summary>
        private bool isTweening = false;

        /// <summary>
        /// If it is zooming or not
        /// </summary>
        private bool isZooming = false;

        /// <summary>
        /// If it is middle of modifing or not
        /// </summary>
        private bool isLayoutTweening = false;

        /// <summary>
        /// If it is sliding or not
        /// </summary>
        private bool isSlide = false;

        /// <summary>
        /// Previous next button display or not
        /// </summary>
        private bool isShow = false;

        /// <summary>
        /// the threshold for layout modifying as completed
        /// </summary>
        private double endThreshold = 0;

        /// <summary>
        /// the threshold for layout modifying as completed
        /// </summary>
        public double EndThreshold
        {
            get 
            {
                return endThreshold; 
            }
            set
            {
                // 0.03 is coefficient
                int msiLen = msi.SubImages.Count;
                endThreshold = value * (Math.Ceiling(msiLen / 0.03) + 1);
            }
        }

        /// <summary>
        /// if animation of subimage is completed or not
        /// </summary>
        private bool subImageAnimationTweening = false;

        /// <summary>
        /// for zoom, rate of change previous and current zooming
        /// </summary>
        private double zoomTo = 0;

        #endregion

        #region "set" Do methods

        /// <summary>
        /// execution for Layout SHRINK
        /// </summary>
        private void SetLayoutSHRINK()
        {
            startZoom = ZoomValue;
            isLayoutTweening = true;
            subImageAnimationTweening = true;
        }

        /// <summary>
        /// execution for Layout LINE
        /// </summary>
        private void SetLayoutLINE()
        {
            startZoom = ZoomValue;
            isLayoutTweening = true;
            subImageAnimationTweening = true;
        }

        /// <summary>
        /// execution for Layout TILE
        /// </summary>
        private void SetLayoutTILE()
        {
            startZoom = ZoomValue;
            isLayoutTweening = true;
            subImageAnimationTweening = true;
        }

        /// <summary>
        /// execution for Layout DEFAULT_POSITION
        /// </summary>
        private void SetLayoutDEFAULT_POSITION()
        {
            startZoom = ZoomValue;
            isLayoutTweening = true;
            subImageAnimationTweening = true;
        }

        /// <summary>
        /// execution for Layout CUSTOM
        /// </summary>
        private void SetLayoutCUSTOM()
        {
            startZoom = ZoomValue;
            isLayoutTweening = true;
            subImageAnimationTweening = true;
        }

        /// <summary>
        /// execution for Layout SPIRAL
        /// </summary>
        private void SetLayoutSPIRAL()
        {
            startZoom = ZoomValue;
            isLayoutTweening = true;
            subImageAnimationTweening = true;
        }

        /// <summary>
        /// execution for Layout SNOWCRYSTAL
        /// </summary>
        private void SetLayoutSNOWCRYSTAL()
        {
            startZoom = ZoomValue;
            isLayoutTweening = true;
            subImageAnimationTweening = true;
        }

        /// <summary>
        /// execution for Layout SPREAD
        /// </summary>
        private void SetLayoutSPREAD()
        {
            startZoom = ZoomValue;

            isLayoutTweening = true;
            subImageAnimationTweening = true;

            radiusList = new List<int>();
            indexIntList = new List<int>();

            Random random = new Random();

            int len = Indices.Count;
            for (int i = 0; i < len; i++)
            {
                int valueInt = random.Next(1, 360);

                radiusList.Add(valueInt);
                indexIntList.Add(Indices[i]);
            }

            shuffleList(indexIntList);
        }
        #endregion

        #region other layouts

        /// <summary>move to Initioal position
        /// </summary>
        private void DefaultPosition()
        {
            startZoom = ZoomValue;
          
            isSlide = true;

            uint ableToFinishTweenCount = 0;
            List<int> ableToFinishIndex = new List<int>();

            int len = Indices.Count;
            for (int i = 0; i < len; i++)
            {
                int useId = Indices[i];
                ableToFinishIndex.Add(0);
                if (useId >= msi.SubImages.Count)
                {
                    ableToFinishTweenCount++;
                    ableToFinishIndex[i] = 1;
                    continue;
                }
                MultiScaleSubImage mssi = msi.SubImages[useId];
                Point p = new Point();
                
                int destId = i;
                double destX = multiScaleSubImageInitPosition[useId].X;
                double destY = multiScaleSubImageInitPosition[useId].Y;

                double fric = 0.05;

                p.X = destX - (destX - mssi.ViewportOrigin.X) * (1 - fric);
                p.Y = destY - (destY - mssi.ViewportOrigin.Y) * (1 - fric);

                double changeZoom = (startZoom - ZoomValue);
                if (changeZoom == 0) changeZoom = 1;
                if (Math.Abs(destX - mssi.ViewportOrigin.X) / changeZoom < EndThreshold && Math.Abs(destY - mssi.ViewportOrigin.Y) / changeZoom < EndThreshold && msi.ViewportWidth == 1)
                {
                    ableToFinishTweenCount++;
                    ableToFinishIndex[i] = 1;

                    //-- if exceed threshold, move to target position
                    p.X = destX;
                    p.Y = destY;
                }

                mssi.ViewportOrigin = p;
                mssi.ViewportWidth = multiScaleSubImageInitViewportWidth[useId];

                msi.ViewportWidth = 1;
                msi.ViewportOrigin = new Point(0, 0);
                // initViewportWidth;
                ZoomValue = 1;
            }

            if (ableToFinishIndex.Contains(0) == false)
            {
                isTweening = false;
                isSlide = false;
                subImageAnimationTweening = false;
                CurrentLayoutStyle = LayoutStyle.NONE;
                JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomMotionFinished(ev);
            }
        }

        #endregion

        #region LayoutStyle

        /// <summary>
        /// previous layout style
        /// line, tile, snowCrystal, spiral, spread, defaultPosition, custom, none
        /// </summary>
        private string PreviousLayoutStyle = "";

        /// <summary>
        /// current layout style
        /// line, tile, snowCrystal, spiral, spread, defaultPosition, custom, none
        /// </summary>
        private string currentLayoutStyle = "";

        /// <summary>
        /// current layout style
        /// line, tile, snowCrystal, spiral, spread, defaultPosition, custom, none
        /// </summary>
        public string CurrentLayoutStyle
        {
            get { return currentLayoutStyle; }
            set
            {
                if (isLoaded)
                {
                    if (!isSlide && !isZooming)
                    {
                        int msiLen = msi.SubImages.Count;
                        if (msiLen > 0)
                        {
                            currentLayoutStyle = value;

                            JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                            OnJFDeepZoomChangeStarted(ev);

                            if (currentLayoutStyle == LayoutStyle.LINE)
                            {
                                SetLayoutLINE();
                            }
                            else if (currentLayoutStyle == LayoutStyle.TILE)
                            {
                                SetLayoutTILE();
                            }
                            else if (currentLayoutStyle == LayoutStyle.SNOW_CRYSTAL)
                            {
                                SetLayoutSNOWCRYSTAL();
                            }
                            else if (currentLayoutStyle == LayoutStyle.SPIRAL)
                            {
                                SetLayoutSPIRAL();
                            }
                            else if (currentLayoutStyle == LayoutStyle.SPREAD)
                            {
                                SetLayoutSPREAD();
                            }
                            else if (currentLayoutStyle == LayoutStyle.DEFAULT_POSITION)
                            {
                                SetLayoutDEFAULT_POSITION();
                            }
                            else if (currentLayoutStyle == LayoutStyle.CUSTOM)
                            {
                                SetLayoutCUSTOM();
                            }
                            else if (currentLayoutStyle == LayoutStyle.SHRINK)
                            {
                                SetLayoutSHRINK();
                            }
                        }
                        else if (currentLayoutStyle == LayoutStyle.SPREAD && msiLen > 0)
                        {
                            SetLayoutSPREAD();
                        }

                        if ( currentLayoutStyle == "")
                        {
                            isTweening = true;
                        }

                        if (CurrentLayoutStyle != LayoutStyle.NONE && CurrentLayoutStyle != "" )
                        {
                            PreviousLayoutStyle = CurrentLayoutStyle;
                        }
                    }
                }
                else
                {
                    // If set value is existing in LayoutStyle
                    if (CheckLayoutStyleInXAML(value))
                    {
                        PreviousLayoutStyle = value;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// constant executing, sync layout modifying and check Image position
        /// 
        /// Flow
        ///  0. it takes position, from each parts
        ///  1. set appropriate viewportWidth fit into Display area using 0.
        ///  2. set actual allocation of each parts with 1.
        ///  3. animated/animate/ using storyboard
        /// 
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void storyboard_Completed(object sender, EventArgs e)
        {
            #region LayoutStyle

            if (isLayoutTweening && !isTweening)
            {
                if (CurrentLayoutStyle == LayoutStyle.LINE)
                {
                    DoLine();
                }
                else if (CurrentLayoutStyle == LayoutStyle.TILE)
                {
                    DoTile();
                }
                else if (CurrentLayoutStyle == LayoutStyle.SNOW_CRYSTAL)
                {
                    DoSnowCrystal();
                }
                else if (CurrentLayoutStyle == LayoutStyle.SPREAD)
                {
                    DoSpread();
                }
                else if (CurrentLayoutStyle == LayoutStyle.SPIRAL)
                {
                    DoSpiral();
                }
                else if (CurrentLayoutStyle == LayoutStyle.DEFAULT_POSITION)
                {
                    DefaultPosition();
                }
                else if (CurrentLayoutStyle == LayoutStyle.CUSTOM)
                {
                    DoCustomLayout();
                }
                else if (CurrentLayoutStyle == LayoutStyle.SHRINK)
                {
                    DoShrink();
                }
            }

            if (useVisiblitySubImageCheckMark)
            {
                // sync SubImage and check Image position
                syncSubImageCheckMark();
            }

            #endregion

            #region Filtering

            if (useOpacityAnimation)
            {
                AnimateOpacity();
            }

            #endregion

            zoomValue = 1 / msi.ViewportWidth;

            sb.Begin();
        }
    }
}
