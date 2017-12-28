using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [SubImage events]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        #region properties

        /// <summary>
        /// Disable List for Client Side Filtering
        /// </summary>
        private List<int> disappearList = new List<int>();

        /// <summary>
        /// Disable List for Client Side Filtering
        /// </summary>
        private List<int> appearList = new List<int>();

        /// <summary>
        /// Apply opacity process or not for Client Side Filtering
        /// </summary>
        private bool useOpacityAnimation = false;

        /// <summary>
        /// Object that get MouseWheelEvent
        /// </summary>
        protected MouseWheelHelper mw;

        /// <summary>
        /// The last axis with MouseDown
        /// </summary>
        private Point mouseDownPoint = new Point();

        private Point lastMouseActionPoint = new Point();

        #region detect event properties

        /// <summary>
        /// Decision valiable for which click or Drag.
        /// If mouseMove amount between 1st click and 2nd click, was less then define px, "click".
        /// Otherwise "Drag".
        /// </summary>
        private double delimMouseDown = 4;

        /// <summary>
        /// State of SubImage
        /// true -> MouseEnter
        /// false -> MouseLeave
        /// </summary>
        private List<bool> subImageState;

        /// <summary>
        /// Initial value of ViewportWidth in MultiScaleSubImage
        /// </summary>
        private List<double> multiScaleSubImageInitViewportWidth;

        #endregion

        #endregion

        #region Init mouseEvent

        /// <summary>
        /// Init the MouseEvent.
        /// </summary>
        private void InitMouseEvent()
        {
            AddDefaultMouseFunc();
            mw = new MouseWheelHelper(this);
            mw.Moved += new EventHandler<MouseWheelEventArgs>(JFDeepZoom_Moved);
        }

        /// <summary>
        /// Wrap Standard method of MultiScaleImage
        /// </summary>
        private void AddDefaultMouseFunc()
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(JFDeepZoom_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(JFDeepZoom_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(JFDeepZoom_MouseMove);
            this.MouseLeave += new MouseEventHandler(JFDeepZoom_MouseLeave);
            this.MouseEnter += new MouseEventHandler(JFDeepZoom_MouseEnter);

            NextButton.Click += new RoutedEventHandler(TopCanvasNextButton_Click);
            PreviousButton.Click += new RoutedEventHandler(TopCanvasPreviousButton_Click);
        }

        /// <summary>
        /// Unwrap Standard method of MultiScaleImage
        /// </summary>
        private void RemoveDefaultMouseFunc()
        {
            this.MouseLeftButtonDown -= new MouseButtonEventHandler(JFDeepZoom_MouseLeftButtonDown);
            this.MouseLeftButtonUp -= new MouseButtonEventHandler(JFDeepZoom_MouseLeftButtonUp);
            this.MouseMove -= new MouseEventHandler(JFDeepZoom_MouseMove);
            this.MouseLeave -= new MouseEventHandler(JFDeepZoom_MouseLeave);
            this.MouseEnter -= new MouseEventHandler(JFDeepZoom_MouseEnter);
        }

        #endregion

        #region define Custom event

        /// <summary>
        /// It calls when MouserEnter in paticular MultiScaleSubImage.
        /// </summary>
        public event MultiScaleSubImageEventHandler MultiScaleSubImageMouseEnter;

        /// <summary>
        /// It raises when MouserEnter in paticular MultiScaleSubImage.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.MultiScaleSubImageEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMultiScaleSubImageMouseEnter(MultiScaleSubImageEventArgs e)
        {
            MultiScaleSubImageEventHandler handler = MultiScaleSubImageMouseEnter;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// It calls when MouseMove in paticular MultiScaleSubImage.
        /// </summary>
        public event MultiScaleSubImageEventHandler MultiScaleSubImageMouseMove;

        /// <summary>
        /// It raises when MouseMove in paticular MultiScaleSubImage.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.MultiScaleSubImageEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMultiScaleSubImageMouseMove(MultiScaleSubImageEventArgs e)
        {
            MultiScaleSubImageEventHandler handler = MultiScaleSubImageMouseMove;

            if (handler != null)
            {
                handler(this, e);
            }

        }

        /// <summary>
        /// It calls when MouseLeftButtonDown in paticular MultiScaleSubImage.
        /// </summary>
        public event MultiScaleSubImageEventHandler MultiScaleSubImageMouseLeftButtonDown;

        /// <summary>
        /// It raises when MouseLeftButtonDown in paticular MultiScaleSubImage.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.MultiScaleSubImageEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMultiScaleSubImageMouseLeftButtonDown(MultiScaleSubImageEventArgs e)
        {
            MultiScaleSubImageEventHandler handler = MultiScaleSubImageMouseLeftButtonDown;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// It calls when MouseLeftButtonUp in paticular MultiScaleSubImage.
        /// </summary>
        public event MultiScaleSubImageEventHandler MultiScaleSubImageMouseLeftButtonUp;

        /// <summary>
        /// It raises when MouseLeftButtonUp in paticular MultiScaleSubImage.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.MultiScaleSubImageEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMultiScaleSubImageMouseLeftButtonUp(MultiScaleSubImageEventArgs e)
        {
            MultiScaleSubImageEventHandler handler = MultiScaleSubImageMouseLeftButtonUp;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// It calls when MouseLeave in paticular MultiScaleSubImage.
        /// </summary>
        public event MultiScaleSubImageEventHandler MultiScaleSubImageMouseLeave;

        /// <summary>
        /// It raises when MouseLeave in paticular MultiScaleSubImage.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.MultiScaleSubImageEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMultiScaleSubImageMouseLeave(MultiScaleSubImageEventArgs e)
        {
            MultiScaleSubImageEventHandler handler = MultiScaleSubImageMouseLeave;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// When it is auto playing slideshow, event is raised just before moving.
        /// </summary>
        public event MultiScaleSubImageEventHandler MultiScaleSubImageTick;

        /// <summary>
        /// The method is raised, When it is auto playing slideshow, event is raised just before moving.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.MultiScaleSubImageEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMultiScaleSubImageTick(MultiScaleSubImageEventArgs e)
        {
            MultiScaleSubImageEventHandler handler = MultiScaleSubImageTick;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Event that is raised when subImage is disable with opecity 0.
        /// </summary>
        internal protected event JFDeepZoomEventHandler Opacity0Completed;

        /// <summary>
        /// The method is raised, when subImage is disable with opecity 0, Event that is raised .
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFDeepZoomEventArgs"/> instance containing the event data.</param>
        protected virtual void OnOpacity0Completed(JFDeepZoomEventArgs e)
        {
            JFDeepZoomEventHandler handler = Opacity0Completed;
            if (handler != null)
            {
                IndicesToThumbnailList();

                useOpacityAnimation = false;
                handler(this, e);
            }
        }

        #endregion

        #region detect event method

        /// <summary>
        /// SubImage [MouseLeftButtonUp]
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void JFDeepZoom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // keep most front index that is satisfied condition.
            int SubImagehighestZIndex = -1;

            // If it is satisfied condition, Process as "click".
            if (Math.Abs(mouseDownPoint.X - e.GetPosition(msi).X) < delimMouseDown && Math.Abs(mouseDownPoint.Y - e.GetPosition(msi).Y) < delimMouseDown)
            {

                if (currentMouseEventType != MouseEventType.NONE && !isSlide)
                {
                    lastMouseActionPoint = e.GetPosition(msi);
                    //lastMousePos = e.GetPosition(msi);
                    int len = Indices.Count;

                    for (int i = 0; i < len; i++)
                    {
                        if (SubImageHitTest(e.GetPosition(msi), Indices[i]))
                        {
                            int id = (Indices[i]);

                            // check Zindex SubImage.
                            MultiScaleSubImage mssi = msi.SubImages[id];
                            if (SubImagehighestZIndex == -1) {
                                SubImagehighestZIndex = id;
                            }
                            else if (msi.SubImages[SubImagehighestZIndex].ZIndex <= mssi.ZIndex)
                            {
                                SubImagehighestZIndex = id;
                            }
                        }
                    }
                }

                // execute with Most front SubImage Index.
                if (SubImagehighestZIndex != -1)
                {
                    if (currentMouseEventType == MouseEventType.ZOOM)
                    {
                        this.ZoomSingleSubImage(SubImagehighestZIndex);
                    }

                    if (isSelectable)
                    {
                        if (ConvertSubImageIndexToSelectedIndex(SubImagehighestZIndex) == -1)
                        {
                            // Select itselef.
                            AddSelectedIndex(SubImagehighestZIndex);
                        }
                        else
                        {
                            // Deselect itself.
                            RemoveSelectedSubImageIndex(SubImagehighestZIndex);
                        }

                    }
                    // raise LeftButtonUp event of SubImage
                    msi.MotionFinished += new RoutedEventHandler(msi_MotionFinished_event);
                    MultiScaleSubImageEventArgs ev = new MultiScaleSubImageEventArgs(SubImagehighestZIndex, msi, msi.SubImages[SubImagehighestZIndex]);
                    OnMultiScaleSubImageMouseLeftButtonUp(ev);
                }
            }
            mouseIsDragging = false;
            mouseButtonPressed = false;
        }

        /// <summary>
        /// SubImage [MouseLeave]
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void JFDeepZoom_MouseLeave(object sender, MouseEventArgs e)
        {
            // keep most front index that is satisfied condition.
            int SubImagehighestZIndex = -1;

            if (currentMouseEventType != MouseEventType.NONE && !isSlide)
            {
                int len = Indices.Count;
                for(int i=0; i < len; i++ )
                {
                    if (SubImageHitTest(e.GetPosition(msi), Indices[i]))
                    {
                        int id = (Indices[i]);

                        // check Zindex SubImage.
                        MultiScaleSubImage mssi = msi.SubImages[id];
                        if (SubImagehighestZIndex == -1)
                        {
                            SubImagehighestZIndex = id;
                        }
                        else if (msi.SubImages[SubImagehighestZIndex].ZIndex <= mssi.ZIndex)
                        {
                            SubImagehighestZIndex = id;
                        }
                    }
                }

                if (SubImagehighestZIndex != -1)
                {
                    MultiScaleSubImageEventArgs ev = new MultiScaleSubImageEventArgs(SubImagehighestZIndex, msi, msi.SubImages[SubImagehighestZIndex]);
                    OnMultiScaleSubImageMouseLeave(ev);
                }
                mouseIsDragging = false;
            }
        }

        /// <summary>
        /// SubImage [MouseLeftButtonDown]
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void JFDeepZoom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // keep most front index that is satisfied condition.
            int SubImagehighestZIndex = -1;

            if (currentMouseEventType != MouseEventType.NONE && !isSlide)
            {
                //lastMousePos = e.GetPosition(msi);
                lastMouseActionPoint = e.GetPosition(msi);

                mouseDownPoint = e.GetPosition(msi);
                int len = Indices.Count;
                for (int i = 0; i < len; i++)
                {
                    if (SubImageHitTest(e.GetPosition(msi), Indices[i]))
                    {

                        int id = (Indices[i]);

                        // check Zindex SubImage.
                        MultiScaleSubImage mssi = msi.SubImages[id];
                        if (SubImagehighestZIndex == -1)
                        {
                            SubImagehighestZIndex = id;
                        }
                        else if (msi.SubImages[SubImagehighestZIndex].ZIndex <= mssi.ZIndex)
                        {
                            SubImagehighestZIndex = id;
                        }
                    }
                }

                if (SubImagehighestZIndex != -1)
                {
                    MultiScaleSubImageEventArgs ev = new MultiScaleSubImageEventArgs(SubImagehighestZIndex, msi, msi.SubImages[SubImagehighestZIndex]);
                    OnMultiScaleSubImageMouseLeftButtonDown(ev);
                }

                mouseButtonPressed = true;

                currentPosition = this.msi.ViewportOrigin;
                dragOffset = e.GetPosition(this);
            }
        }

        /// <summary>
        /// checking subimage events.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void JFDeepZoom_MouseEnter(object sender, MouseEventArgs e)
        {
            if (currentMouseEventType != MouseEventType.NONE && !isSlide)
            {
                mouseButtonPressed = false;
                mouseIsDragging = false;

                // keep most front index that is satisfied condition.
                int SubImagehighestZIndex = -1;

                int len = Indices.Count;
                for (int i = 0; i < len; i++)
                {
                    if (SubImageHitTest(e.GetPosition(msi), Indices[i]))
                    {
                        int id = (Indices[i]);

                        // check Zindex SubImage.
                        MultiScaleSubImage mssi = msi.SubImages[id];
                        if (SubImagehighestZIndex == -1)
                        {
                            SubImagehighestZIndex = id;
                        }
                        else if (msi.SubImages[SubImagehighestZIndex].ZIndex <= mssi.ZIndex)
                        {
                            SubImagehighestZIndex = id;
                        }
                    }
                }

                if (SubImagehighestZIndex != -1)
                {
                    MultiScaleSubImageEventArgs ev = new MultiScaleSubImageEventArgs(SubImagehighestZIndex, msi, msi.SubImages[SubImagehighestZIndex]);
                    OnMultiScaleSubImageMouseEnter(ev);
                }
            }
        }

        #endregion

        #region mouse event

        /// <summary>
        /// changing Zoom when mousewheel move.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.MouseWheelEventArgs"/> instance containing the event data.</param>
        protected virtual void JFDeepZoom_Moved(object sender, MouseWheelEventArgs e)
        {
            if (currentMouseEventType != MouseEventType.NONE && !isSlide)
            {
                lastMouseActionPoint = this.lastMousePos;

                e.Handled = true;
                double ZoomFactor = 0;
                if (e.Delta > 0)
                {
                    ZoomFactor = 1.2;
                }
                else
                {
                    ZoomFactor = 0.8;
                }
                Zoom(ZoomFactor, this.lastMousePos);
            }
        }

        /// <summary>
        /// Drag Deepzoom Object.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void JFDeepZoom_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentMouseEventType != MouseEventType.NONE && !isSlide)
            {
                /*

                //---- raise mousemove event
                int len = Indices.Count;
                for (int i = 0; i < len; i++)
                {
                    int id = Indices[i];
                    int useId = Indices[i];
                    if (SubImageHitTest(e.GetPosition(msi), useId))
                    {
                        MultiScaleSubImageEventArgs ev = new MultiScaleSubImageEventArgs(id, msi, msi.SubImages[useId]);
                        OnMultiScaleSubImageMouseMove(ev);
                    }
                    if (SubImageHitTest(e.GetPosition(msi), useId) == false && subImageState[useId]==true)
                    {
                        MultiScaleSubImageEventArgs ev = new MultiScaleSubImageEventArgs(id, msi, msi.SubImages[useId]);
                        OnMultiScaleSubImageMouseLeave(ev);
                        subImageState[useId] = false;
                    }
                    if (SubImageHitTest(e.GetPosition(msi), useId) && subImageState[useId] == false)
                    {
                        MultiScaleSubImageEventArgs ev = new MultiScaleSubImageEventArgs(id, msi, msi.SubImages[useId]);
                        OnMultiScaleSubImageMouseEnter(ev);
                        subImageState[useId] = true;
                    }
                }
                
                */
                 

                if (mouseButtonPressed)
                {
                    mouseIsDragging = true;
                }

                this.lastMousePos = e.GetPosition(this);

                double msiAspectRatio = getAspectRatio();

                if (mouseIsDragging)
                {
                    Point newOrigin = new Point();
                    newOrigin.X = currentPosition.X - (((e.GetPosition(msi).X - dragOffset.X) / msi.ActualWidth) * msi.ViewportWidth);
                    newOrigin.Y = currentPosition.Y - (((e.GetPosition(msi).Y - dragOffset.Y) / msi.ActualHeight) * (msi.ViewportWidth / msiAspectRatio));
                    msi.ViewportOrigin = newOrigin;
                }
            }
        }

        #endregion

        #region MultiScaleImage event

        /// <summary>
        /// When Movement like "DoFit" Done, process that is for raising event as same as 
        /// complete event in "DoLine"
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        protected virtual void msi_MotionFinished_event(object sender, RoutedEventArgs e)
        {
            if (isSlide)
            {
                isTweening = false;
            }

            isSlide = false;
            isTweening = false;
            isZooming = false;

            if (subImageAnimationTweening == false)
            {

                CurrentLayoutStyle = LayoutStyle.NONE;

                JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomMotionFinished(ev);
            }
        }

        /// <summary>
        /// Event handler for ImageOpenFailed event of the msi control.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Windows.ExceptionRoutedEventArgs"/> instance containing the event data.</param>
        protected virtual void msi_ImageOpenFailed(object sender, ExceptionRoutedEventArgs e)
        {
            string msiSrc = this.src;
            string excepMessage = "The Source file(" + this.src + ") of specified MultiScaleImage do not exist. ";

            throw new JFDeepZoomMultiScaleImageException(excepMessage);
        }

        /// <summary>
        /// Event handler for ImageOpenSucceeded event of the msi control.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        protected virtual void msi_ImageOpenSucceeded(object sender, RoutedEventArgs e)
        {
            if (msi.SubImages.Count <= 0)
            {
                string msiSrc = this.src;
                string excepMessage = "It cannot display MultiScaleImage, because the number of SubImages is 0.";

                throw new JFDeepZoomMultiScaleImageException(excepMessage);
            }

            currentMouseEventType = MouseEventType.ZOOM;
            isSelectable = false;

            // Reset
            newLayoutViewportList = new List<Dictionary<string, string>>();

            //---------------------------------------------- start >>>
            // 
            // You have to use "private", because the value must be set Indices.
            // It assume that every Image are selected.
            int len = 0;

            len = msi.SubImages.Count;
            for (int i = 0; i < len; i++)
            {
                indices.Add(i);
            }
            //---------------------------------------------- end <<<

            //--- Init data
            initViewportWidth = msi.ViewportWidth;
            initViewportOrigin = msi.ViewportOrigin;
            ZoomValue = 1;

            subImageState = new List<bool>();
            multiScaleSubImageInitViewportWidth = new List<double>();

            initSubImageViewportOrigin = msi.SubImages[0].ViewportOrigin;
            initSubImageViewportWidth = msi.SubImages[0].ViewportWidth;
            multiScaleSubImageInitPosition = new List<Point>();

            //---------------------------------- Initialize state list

            // for Initialization
            for (int i = 0; i < ThumbnailList.Count; i++)
            {
                indices.Add(ImageNameToIndex(ThumbnailList[i].ThumbnailPath));
            }
            
            //--------------------------------------

            if (Indices.Count == 0)
            {
                Indices = new IndicesList();
                for (int i = 0; i < msi.SubImages.Count; i++)
                {
                    Indices.Add(i);
                }
            }

            //--------------------------------------

            len = Math.Min( msi.SubImages.Count, Indices.Count);

            for (int i = 0; i < len; i++)
            {
                //
                if (i < Indices.Count)
                {
                    msi.SubImages[Indices[i]].Opacity = 1;

                }
                subImageState.Add(false);
                multiScaleSubImageInitViewportWidth.Add(msi.SubImages[Indices[i]].ViewportWidth);

                multiScaleSubImageInitPosition.Add(msi.SubImages[Indices[i]].ViewportOrigin);

            }

            msi.UseSprings = true;

            //-- EndThreshould
            EndThreshold = 0.00001;


            // configure event handler for filter
            this.Indices.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Indices_CollectionChanged);
            this.Opacity0Completed += new JFDeepZoomEventHandler(JFDeepZoom_Opacity0);

            
            // If it is specified for CurrentLayout property of XAML
            if (PreviousLayoutStyle != "" && CurrentApproachType == ApproachType.STATIC)
            {
                // Display with center Position.
                Fit();
                CurrentLayoutStyle = PreviousLayoutStyle;
            }

            //--------- 
            // With "INIT" Process of Semi-Dynamic Approach, if CUrl different from LUrl 
            // continue to layout.
            if (currSemiDynamicExecType == SemiDynamicExecType.INIT_CHANGE_ORDER)
            {
                // lUrl
                string lUrl = currentSemiDynamicCollection.LUrl;
                RetrieveLayoutChange(lUrl);
                msi.Opacity = 1;
            }

            // complete Event for Process like loading
            JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
            OnJFDeepZoomSrcLoaded(ev);
        }

        #region filtering Relation

        /// <summary>
        /// Event handler for opacity process completion for filtering
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFDeepZoomEventArgs"/> instance containing the event data.</param>
        private void JFDeepZoom_Opacity0(object sender, JFDeepZoomEventArgs e)
        {
            // blow far away
            GoFarAway();
        }

        /// <summary>
        /// blow far away
        /// </summary>
        private void GoFarAway()
        {
            // "far" means "-100,000"
            int count = disappearList.Count;
            for (int i = 0; i < disappearList.Count; i++)
            {
                msi.SubImages[disappearList[i]].ViewportOrigin = new Point(-100000, -100000);
            }
        }

        /// <summary>
        /// If Indices value is changing, execute filtering action.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void Indices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DoFiltering();
        }

        /// <summary>
        /// Opacity process
        /// </summary>
        private void AnimateOpacity()
        {
            int count = disappearList.Count;
            List<int> appearedList = new List<int>();
            List<int> disappearedList = new List<int>();

            for (int i = 0; i < count; i++)
            {
                msi.SubImages[disappearList[i]].Opacity -= 0.1;
                if (msi.SubImages[disappearList[i]].Opacity <= 0)
                {
                    disappearedList.Add(1);
                }
            }

            int appearCount = appearList.Count;
            for (int i = 0; i < appearCount; i++)
            {
                msi.SubImages[appearList[i]].Opacity += 0.1;
                if (msi.SubImages[appearList[i]].Opacity >= 1)
                {
                    appearedList.Add(1);
                }
            }

            // If opacity process is completed
            if (disappearedList.Count == disappearList.Count && appearedList.Count == appearList.Count)
            {
                isTweening = false;
                isLayoutTweening = false;
                isSlide = false;
                CurrentLayoutStyle = LayoutStyle.NONE;
                OnOpacity0Completed(new JFDeepZoomEventArgs(msi));
                OnFilterCompleted(new JFDeepZoomEventArgs(msi));
            }
        }

        /// <summary>
        /// Filtering action
        /// </summary>
        private void DoFiltering()
        {
            List<int> willDisappearList = new List<int>();
            appearList = new List<int>();
            int msiLen = msi.SubImages.Count;
            for (int i = 0; i < msiLen; i++)
            {
                if (! Indices.Contains(i))
                {
                    // Add it to "blow away" list
                    willDisappearList.Add(i);
                }
                else
                {
                    if( msi.SubImages[i].ViewportOrigin.X == -100000 && msi.SubImages[i].ViewportOrigin.Y == -100000)
                    {
                        // (display target) && (disappearList.Contains(i)) -> Display target list
                        appearList.Add(i);
                        // Move target SubImage to Center Position
                        Rect mssiRect = GetSubImageRect(i);
                        setSubImagePoint(i, 
                            new Point(this.ActualWidth / 2 - mssiRect.Width/2, this.ActualHeight / 2 - mssiRect.Height/2));
                    }
                }
                disappearList = willDisappearList;
                useOpacityAnimation = true;

                isSlide = true;
                isTweening = true;

            }
        }

#endregion

        #endregion

        /// <summary>
        /// Syncronizing Canvas and MultiScaleImage size.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void JFDeepZoom_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Syncronizing();
        }

    }
}
