using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Windows.Interop;
using Jellyfish.jfDeepZoom;


namespace jellyfishDZApp
{
    public partial class Step09_ChangeSelectableSkin : UserControl
    {
        public Step09_ChangeSelectableSkin()
        {
            InitializeComponent();

            this.InitFullScreen();

            this.InitPanel();

            this.InitSemiDynamicList();

            this.InitMetaInfo();

            this.InitMetaPanel();

            this.InitThumbnailList();

            this.InitSelectableSkin();

        }



        #region Step 3 FullScreen

        private const double INIT_JFD_WIDTH = 800;
        private const double INIT_JFD_HEIGHT = 600;


        private void InitFullScreen()
        {
            this.jfd.Width = INIT_JFD_WIDTH;
            this.jfd.Height = INIT_JFD_HEIGHT;

            this.LayoutRoot.Width = INIT_JFD_WIDTH;
            this.LayoutRoot.Height = INIT_JFD_HEIGHT;



            this.FullScreen_Button.Click += new RoutedEventHandler(FullScreen_Button_Click);

            this.ZoomIn_Button.Click += new RoutedEventHandler(ZoomIn_Button_Click);

            this.ZoomOut_Button.Click += new RoutedEventHandler(ZoomOut_Button_Click);

            this.ZoomFit_Button.Click += new RoutedEventHandler(ZoomFit_Button_Click);

            Content contentObject = Application.Current.Host.Content;
            contentObject.FullScreenChanged += new EventHandler(contentObject_FullScreenChanged);

        }


        private void ZoomIn_Button_Click(object sender, RoutedEventArgs e)
        {
            jfd.ZoomIn();
        }

        private void ZoomOut_Button_Click(object sender, RoutedEventArgs e)
        {
            jfd.ZoomOut();
        }

        private void ZoomFit_Button_Click(object sender, RoutedEventArgs e)
        {
            jfd.DoFit();
        }

        private void FullScreen_Button_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchFullScreen();
        }

        private void contentObject_FullScreenChanged(object sender, EventArgs e)
        {
            this.SetMenuPosition();
        }



        private void SwitchFullScreen()
        {
            if (!Application.Current.Host.Content.IsFullScreen)
            {
                Application.Current.Host.Content.IsFullScreen = true;
            }
            else
            {
                Application.Current.Host.Content.IsFullScreen = false;
            }

        }

        private void SetMenuPosition()
        {
            double nextWidth = 0;
            double nextHeight = 0;

            Content contentObject = Application.Current.Host.Content;

            if (Application.Current.Host.Content.IsFullScreen)
            {
                nextWidth = contentObject.ActualWidth;
                nextHeight = contentObject.ActualHeight;

            }
            else
            {
                nextWidth = 800;
                nextHeight = 600;
            }


            #region Add Step6

            // Add  Step 6 (Meta Info) +++++++++++++++++++++++++++
            if (jfd.ForegroundCanvas != null)
            {
                SetXamlSyncMask(nextWidth, nextHeight);
            }
            // +++++++++++++++++++++++++++++++++++++++++++++++++++

            #endregion


            this.jfd.Width = nextWidth;
            this.jfd.Height = nextHeight;

            this.LayoutRoot.Width = nextWidth;
            this.LayoutRoot.Height = nextHeight;

        }

        #endregion



        #region Step4 Panel

        public void InitPanel()
        {
            this.Panel_Button.Click += new RoutedEventHandler(Panel_Button_Click);

            // Set method for ControlPanel
            this.InitControlPanel_Layout();
            this.InitControlPanel_SlideShow();
            this.InitControlPanel_Zoom();
            this.InitControlPanel_SetSubImage();


            this.jfd.LineTileSpace = 15;

        }


        private void Panel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchControlVisibility();
        }



        private void SwitchControlVisibility()
        {
            if (ControlShowAreaGrid.Visibility == Visibility.Visible)
            {
                ControlShowAreaGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                ControlShowAreaGrid.Visibility = Visibility.Visible;
            }
        }





        #region ControlPanel:Layout


        /// <summary>
        /// flag that button is pressed.
        /// </summary>
        private bool layoutButtonPressed = false;

        private void InitControlPanel_Layout()
        {
            // [Line]
            LineButton.Click += new RoutedEventHandler(LineButton_Click);
            // [SnowCrystal]
            SnowCrystalButton.Click += new RoutedEventHandler(SnowCrystalButton_Click);
            // [Spiral]
            SpiralButton.Click += new RoutedEventHandler(SpiralButton_Click);
            // [Spread]
            SpreadButton.Click += new RoutedEventHandler(SpreadButton_Click);
            // [Tile]
            TileButton.Click += new RoutedEventHandler(TileButton_Click);
            // [Shrink -> Tile]
            ShrinkButton.Click += new RoutedEventHandler(ShrinkButton_Click);

            // [Default Position]: * Default "Tile" Position.
            DefaultPositionButton.Click += new RoutedEventHandler(DefaultPositionButton_Click);
            // [Fit]
            FitButton.Click += new RoutedEventHandler(FitButton_Click);

            // initialize event
            jfd.JFDeepZoomChangeStarted += new JFDeepZoomEventHandler(jfd_JFDeepZoomChangeStarted);
            jfd.JFDeepZoomMotionFinished += new JFDeepZoomEventHandler(jfd_JFDeepZoomLayoutFinished);

            // [tick slideshow]
            jfd.MultiScaleSubImageTick += new MultiScaleSubImageEventHandler(jfd_MultiScaleSubImageTick);
        }

        private void jfd_MultiScaleSubImageTick(object sender, MultiScaleSubImageEventArgs e)
        {
            SwitchNextPreviousButtonEnable(false);
        }

        private void jfd_JFDeepZoomChangeStarted(object sender, JFDeepZoomEventArgs e)
        {
            SwitchNextPreviousButtonEnable(false);
        }

        /// <summary>
        /// switch each function buttons state.
        /// </summary>
        /// <param name="flag"></param>
        private void SwitchNextPreviousButtonEnable(bool flag)
        {
            jfd.NextButton.IsEnabled = flag;
            jfd.PreviousButton.IsEnabled = flag;

            // on Usejf
            LineButton.IsEnabled = flag;
            SnowCrystalButton.IsEnabled = flag;
            SpiralButton.IsEnabled = flag;
            SpreadButton.IsEnabled = flag;
            TileButton.IsEnabled = flag;
            ShrinkButton.IsEnabled = flag;
            DefaultPositionButton.IsEnabled = flag;
            FitButton.IsEnabled = flag;
            FFButton.IsEnabled = flag;
            PrevButton.IsEnabled = flag;
            PlayButton.IsEnabled = flag;
        }


        private int shrinked = 0;
        private void jfd_JFDeepZoomLayoutFinished(object sender, JFDeepZoomEventArgs e)
        {
            if (shrinked == 1)
            {
                jfd.CurrentLayoutStyle = LayoutStyle.TILE;
                jfd.JFDeepZoomMotionFinished += new JFDeepZoomEventHandler(jfd_TileFinished);
                shrinked = 2;
            }
            else if (layoutButtonPressed)
            {
                // do fit when changing layout has done.
                jfd.DoFit();
                layoutButtonPressed = false;
            }
            else
            {
                SwitchNextPreviousButtonEnable(true);
            }

        }

        private void jfd_TileFinished(object sender, JFDeepZoomEventArgs e)
        {
            if (shrinked == 2)
            {
                jfd.DoFit();
                shrinked = 0;
            }
        }

        private void ShrinkButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.CurrentLayoutStyle = LayoutStyle.SHRINK;
            shrinked = 1;
            layoutButtonPressed = true;

            SwitchNextPreviousButtonEnable(false);
        }

        private void LineButton_Click(object sender, RoutedEventArgs e)
        {

            jfd.CurrentLayoutStyle = LayoutStyle.LINE;
            layoutButtonPressed = true;

            SwitchNextPreviousButtonEnable(false);
        }

        private void SnowCrystalButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.CurrentLayoutStyle = LayoutStyle.SNOW_CRYSTAL;
            layoutButtonPressed = true;

            SwitchNextPreviousButtonEnable(false);
        }

        private void SpiralButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.CurrentLayoutStyle = LayoutStyle.SPIRAL;
            layoutButtonPressed = true;

            SwitchNextPreviousButtonEnable(false);
        }

        private void SpreadButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.CurrentLayoutStyle = LayoutStyle.SPREAD;
            layoutButtonPressed = true;

            SwitchNextPreviousButtonEnable(false);
        }

        private void TileButton_Click(object sender, RoutedEventArgs e)
        {

            jfd.CurrentLayoutStyle = LayoutStyle.TILE;
            layoutButtonPressed = true;

            SwitchNextPreviousButtonEnable(false);
        }

        private void DefaultPositionButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.CurrentLayoutStyle = LayoutStyle.DEFAULT_POSITION;

            SwitchNextPreviousButtonEnable(false);
        }

        private void FitButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.DoFit();
            SwitchNextPreviousButtonEnable(false);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.Reset();
            SwitchNextPreviousButtonEnable(false);
        }

        #endregion

        #region ControlPanel:SildeShow

        private void InitControlPanel_SlideShow()
        {
            // [<<]
            PrevButton.Click += new RoutedEventHandler(PrevButton_Click);
            // [o]
            StopButton.Click += new RoutedEventHandler(StopButton_Click);
            // [>]
            PlayButton.Click += new RoutedEventHandler(PlayButton_Click);
            // [>>]
            FFButton.Click += new RoutedEventHandler(FFButton_Click);
        }

        private void FFButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.Next();
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.StartSlideShow();
            SwitchNextPreviousButtonEnable(false);
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.StopSlideShow();
        }
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.Previous();
        }

        #endregion

        #region ControlPanel:Zoom

        private void InitControlPanel_Zoom()
        {
            SetMaxZoomButton.Click += new RoutedEventHandler(SetMaxZoomButton_Click);
            SetMinZoomButton.Click += new RoutedEventHandler(SetMinZoomButton_Click);
            ResetZoomValueButton.Click += new RoutedEventHandler(ResetZoomValueButton_Click);
        }

        private void ResetZoomValueButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.MinZoom = 0;
            jfd.MaxZoom = Double.MaxValue;
            MinZoomTb.Text = "";
            MaxZoomTb.Text = "";
        }

        private void SetMinZoomButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.MinZoom = jfd.ZoomValue;
            MinZoomTb.Text = jfd.MinZoom.ToString();
        }

        private void SetMaxZoomButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.MaxZoom = jfd.ZoomValue;
            MaxZoomTb.Text = jfd.MaxZoom.ToString();
        }

        #endregion


        #region ControlPanel:SetSubImage


        private void InitControlPanel_SetSubImage()
        {
            SetWidthButton.Click += new RoutedEventHandler(SetWidthButton_Click);
            SetXYButton.Click += new RoutedEventHandler(SetXYButton_Click);
        }

        /// <summary>
        /// set x,y position of MultiScaleSubImages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetXYButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;

            if (SubImageIndex_NumericUpDown.Value >= 0 && SubImageX_NumericUpDown.Value >= 0 && SubImageY_NumericUpDown.Value >= 0)
            {
                index = Convert.ToInt32(SubImageIndex_NumericUpDown.Value);
                jfd.setSubImagePoint(index, new Point(SubImageX_NumericUpDown.Value, SubImageY_NumericUpDown.Value));
            }

        }

        /// <summary>
        /// set width of MultiScaleSubImages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetWidthButton_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;

            if (SubImageIndex_NumericUpDown.Value >= 0 && SubImageWidth_NumericUpDown.Value >= 0)
            {
                index = Convert.ToInt32(SubImageIndex_NumericUpDown.Value);
                jfd.setSubImageWidth(index, SubImageWidth_NumericUpDown.Value);
            }
        }

        #endregion





        #endregion



        #region Step5 Page05_SemiDynamicList


        private void InitSemiDynamicList()
        {
            this.List_Button.Click += new RoutedEventHandler(List_Button_Click);

            this.jfd.Loaded += new RoutedEventHandler(jfd_Loaded);


        }


        private void jfd_Loaded(object sender, RoutedEventArgs e)
        {
            this.SwitchSemiDynamicOrDynamic();
        }




        private void List_Button_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchSemiDynamicListVisibility();
        }





        /// <summary>
        /// Start SemiDynamic
        /// </summary>
        private void StartSemiDynamic()
        {
            this.jfd.CurrentApproachType = ApproachType.SEMI_DYNAMIC;

            this.jfd.JFDownloadSemiDynamicListCompleted += new JFCommunicationEventHandler(jfd_JFDownloadSemiDynamicListCompleted);
            this.ShowListListBox.SelectionChanged += new SelectionChangedEventHandler(ShowListListBox_SelectionChanged);
            this.jfd.RetrieveCollectionList("../semi-dynamic.aspx");
            this.SetXamlSyncMask(INIT_JFD_WIDTH, INIT_JFD_HEIGHT);

        }



        /// <summary>
        /// Handles the JFDownloadSemiDynamicListCompleted event of the jfd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationEventArgs"/> instance containing the event data.</param>
        private void jfd_JFDownloadSemiDynamicListCompleted(object sender, JFCommunicationEventArgs e)
        {
            this.SetSemiDynamicList();
        }


        private void SetSemiDynamicList()
        {
            this.ListSp.Visibility = Visibility.Visible;

            List<JFSemiDynamicColList> colList = jfd.SemiDynamicColList;
            int listCount = ShowListListBox.Items.Count;
            for (int i = 0; i < listCount; i++)
            {
                this.ShowListListBox.Items.RemoveAt(0);
            }
            for (int i = 0; i < colList.Count; i++)
            {
                string layoutTitle = colList[i].LTitle;
                string colTitle = colList[i].CTitle;
                string id = colList[i].CollectionId;

                string itemString = "[" + id + "]" + colTitle + ":" + layoutTitle;
                this.ShowListListBox.Items.Add(itemString);
            }


        }


        /// <summary>
        /// retrieve new collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowListListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // index selected in ShowListListBox.
            int selectListIndex = ShowListListBox.SelectedIndex;

            if (selectListIndex >= 0)
            {
                jfd.RetrieveCollectionInit("../semi-dynamic.aspx", selectListIndex);
            }
            ListSp.Visibility = Visibility.Collapsed;
        }

        private void SwitchSemiDynamicListVisibility()
        {
            if (this.ListSp.Visibility == Visibility.Visible)
            {
                this.ListSp.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ListSp.Visibility = Visibility.Visible;
            }
        }


        #endregion


        //#region Step 6 MetaInfo (skeleton Method)

        //private void SetXamlSyncMask(double nextWidth, double nextHeight)
        //{

        //}

        //#endregion


        #region Step6 MetaInfo



        /// <summary>
        /// this storyboard observes synchronizing MultiScaleSubImage and XAML outside Jellyfish.
        /// </summary>
        private Storyboard observerStoryboard;

        private int clickCount = 0;

        /// <summary>
        /// this property holds synchronizing index.
        /// </summary>
        private int syncIndex = -1;

        private void InitMetaInfo()
        {

            if (jfd.CurrentApproachType != ApproachType.STATIC)
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

                jfd.ForegroundCanvas = attachedCanvas;


            }
        }


        private void jfd_MultiScaleSubImageMouseLeftButtonUp(object sender, MultiScaleSubImageEventArgs e)
        {
            SwitchNextPreviousButtonEnable(false);

            if (jfd.CurrentApproachType != ApproachType.STATIC)
            {
                syncIndex = e.Id;
                SetAdditionalData(syncIndex);

                if (clickCount == 0)
                {
                    if (showInfo)
                    {
                        ShowAdditionalData();
                    }

                    clickCount++;
                }
                else
                {
                    if (!showInfo)
                    {
                        HideAdditionalData();
                    }
                    clickCount = 0;
                }
            }
        }

        private void jfd_MouseLeave(object sender, MouseEventArgs e)
        {
            this.HideAdditionalData();
            clickCount = 0;
        }


        private void ShowAdditionalData()
        {
            additionalData.Visibility = Visibility.Visible;
            observerStoryboard.Begin();
        }

        private void HideAdditionalData()
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
                //Canvas.SetTop(additionalData, np.Y - 60);
                //Canvas.SetLeft(additionalData, np.X - 20 + jfd.getSubImageWidth(syncIndex));

                Canvas.SetTop(additionalData, np.Y);
                Canvas.SetLeft(additionalData, np.X);

                ScaleTransform st = new ScaleTransform();

                //st.ScaleX = 1 / jfd.Msi.ViewportWidth ;
                //st.ScaleY = 1 / jfd.Msi.ViewportWidth;

                st.ScaleX = 1 / jfd.Msi.ViewportWidth / jfd.Msi.SubImages[syncIndex].ViewportWidth * 4;
                st.ScaleY = 1 / jfd.Msi.ViewportWidth / jfd.Msi.SubImages[syncIndex].ViewportWidth * 4;

                // additionalData.RenderTransform = st;
                additionalSubData.RenderTransform = st;

                // Set Width
                additionalData.Width = jfd.getSubImageWidth(syncIndex);
                additionalData.Height = jfd.getSubImageHeight(syncIndex) / 3;


                observerStoryboard.Begin();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        private void SetAdditionalData(int index)
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


            MetaInfo1_textBlock.Text = "Date:" + date;
            MetaInfo2_textBlock.Text = "Author:" + author;
            MetaInfo3_textBlock.Text = "Tags:" + tagdata;
            MetaInfo4_textBlock.Text = "Title:" + title;

        }

        private void resetAdditionalData()
        {
            MetaInfo1_textBlock.Text = "";
            MetaInfo2_textBlock.Text = "";
            MetaInfo3_textBlock.Text = "";
            MetaInfo4_textBlock.Text = "";
        }


        private void SetXamlSyncMask(double nextWidth, double nextHeight)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            rectangleGeometry.Rect = new Rect(0, 0, nextWidth, nextHeight);
            jfd.ForegroundCanvas.Clip = rectangleGeometry;
        }



        #endregion


        //#region Step 7 MetaPanel (Dummy Parameter)

        //private bool showInfo = true;

        //#endregion



        #region Step7 MetaPanel

        private bool showInfo = false;



        private void InitMetaPanel()
        {

            this.MetaPanel_Button.Click += new RoutedEventHandler(MetaPanel_Button_Click);

            InitSortFunction();
            InitFilterFunction();
            InitSelectableFunction();
            InitInfoFunction();
        }

        private void MetaPanel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchMetaPanelVisibility();
        }


        private void SwitchMetaPanelVisibility()
        {
            if (this.MetaPanelAreaGrid.Visibility == Visibility.Visible)
            {
                this.MetaPanelAreaGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.MetaPanelAreaGrid.Visibility = Visibility.Visible;
            }
        }


        #region Sort function

        private void InitSortFunction()
        {
            ExecSortButton.Click += new RoutedEventHandler(ExecSortButton_Click);
            // Reverse
            ReverseButton.Click += new RoutedEventHandler(ReverseButton_Click);
        }

        private string sortOrder = SortOrder.ASC;

        /// <summary>
        /// execute Sort function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecSortButton_Click(object sender, RoutedEventArgs e)
        {
            // 
            if (this.jfd.CurrentApproachType == ApproachType.STATIC)
            {
                if (sortOrder == SortOrder.ASC)
                    sortOrder = SortOrder.DESC;
                else
                {
                    sortOrder = SortOrder.ASC;
                }
                jfd.Sort(sortOrder);
            }
            else
            {
                string sortType = "";
                if ((bool)OrderDESCRb.IsChecked)
                {
                    //desc
                    sortType = SortOrder.DESC;
                }
                else
                {
                    //asc
                    sortType = SortOrder.ASC;
                }

                // 
                string sortBy = "";
                if ((bool)OrderByOwnerRb.IsChecked)
                {
                    sortBy = "Owner";
                }
                else
                {
                    sortBy = "Title";
                }
                jfd.Sort(sortType, sortBy);
            }
        }

        /// <summary>
        /// execute Reverse function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReverseButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.Reverse();
        }

        #endregion

        #region Filter function

        private bool isReset = false;

        private void InitFilterFunction()
        {
            ExecFilterButton.Click += new RoutedEventHandler(ExecFilterButton_Click);

            // Reset
            ResetFilterButton.Click += new RoutedEventHandler(ResetFilterButton_Click);
            jfd.FilterCompleted += new JFDeepZoomEventHandler(jfd_Opacity0);

        }

        /// <summary>
        /// [Semi-Dynamic] execute filter function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecFilterButton_Click(object sender, RoutedEventArgs e)
        {
            List<FilterObject> targetList = new List<FilterObject>();

            for (int i = 0; i <= 2; i++)
            {
                FilterObject fo = new FilterObject();

                if (i == 0)
                {
                    //----------------------- set up filtering information
                    fo.FilterKey = "Title";
                    fo.FilterValue = TitleFilterTb.Text;
                    fo.FilterOperation = FilterOperationType.OPERATION_CONTAIN;
                    targetList.Add(fo);
                }
                else if (i == 1)
                {
                    //----------------------- set up filtering information
                    fo.FilterKey = "Tag";
                    fo.FilterValue = TagFilterTb.Text;
                    // FilterOperationType has 4 types:
                    // EQUAL, CONTAIN, LESS_THAN, GREATER_THAN
                    fo.FilterOperation = FilterOperationType.OPERATION_EQUAL;
                    targetList.Add(fo);
                }
            }
            string stype = "";
            if ((bool)FilterConditionAndRb.IsChecked)
            {
                stype = SearchCondition.AND;
            }
            else
            {
                stype = SearchCondition.OR;
            }
            if (TagFilterTb.Text == "" && TitleFilterTb.Text == "")
            {
                jfd.ResetFilter();
                isReset = true;
            }
            else if (targetList.Count > 0)
            {
                jfd.Filter(targetList, stype);
            }
        }

        /// <summary>
        /// [Semi-Dynamic] execute reset filter function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.ResetFilter();
            isReset = true;
        }

        /// <summary>
        /// [Semi-Dynamic]  event handler 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jfd_Opacity0(object sender, JFDeepZoomEventArgs e)
        {
            if (isReset)
            {
                // if you want to layout subimages as specific layoutstyle, you can set new layoutstyle.
                jfd.CurrentLayoutStyle = LayoutStyle.LINE;
                jfd.JFDeepZoomMotionFinished += new JFDeepZoomEventHandler(jfd_JFDeepZoomMotionFinished);
                isReset = false;
            }

            // Add  Step 8 (ThumbnailList) +++++++++++++++++++++++++++
            this.UpdateThumbnailList();
            // +++++++++++++++++++++++++++++++++++++++++++++++++++
        }

        private void jfd_JFDeepZoomMotionFinished(object sender, JFDeepZoomEventArgs e)
        {
            jfd.DoFit();
            jfd.JFDeepZoomMotionFinished -= new JFDeepZoomEventHandler(jfd_JFDeepZoomMotionFinished);
        }


        #endregion

        #region info function



        private void InitInfoFunction()
        {
            this.InformationOffRb.Checked += new RoutedEventHandler(InformationRadioGroup_Change);
            this.InformationOnRb.Checked += new RoutedEventHandler(InformationRadioGroup_Change);
        }

        /// <summary>
        /// switch Information rect, "on, off".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InformationRadioGroup_Change(object sender, RoutedEventArgs e)
        {
            if ((bool)InformationOnRb.IsChecked)
            {
                showInfo = true;
            }
            else
            {
                showInfo = false;
            }
        }

        #endregion

        #region selectable function

        /// <summary>
        /// [Semi-Dynamic] [Dynamic]
        /// </summary>
        private void InitSelectableFunction()
        {
            this.SelectableOffRb.Checked += new RoutedEventHandler(SelectableRadioButton_Changed);
            this.SelectableOnRb.Checked += new RoutedEventHandler(SelectableRadioButton_Changed);
        }

        private void SelectableRadioButton_Changed(object sender, RoutedEventArgs e)
        {
            if ((bool)SelectableOnRb.IsChecked)
            {
                jfd.IsSelectable = true;
                jfd.SetVisiblitySubImageCheckMark(true);

                jfd.CurrentMouseEventType = MouseEventType.EVENT_ONLY;
            }
            else
            {
                jfd.IsSelectable = false;
                jfd.SetVisiblitySubImageCheckMark(false);

                jfd.CurrentMouseEventType = MouseEventType.ZOOM;
            }
        }

        #endregion



        #endregion


        //#region Step8 ThumbnailList (skeleton）

        //private void UpdateThumbnailList()
        //{

        //}

        //#endregion



        #region Step8 ThumbnailList



        private void InitThumbnailList()
        {
            Thumbnail_Grid.Visibility = Visibility.Collapsed;

            Thumbnail_ListBox.ItemsSource = null;

            this.jfd.JFDeepZoomSrcLoaded += new JFDeepZoomEventHandler(jfd_JFDeepZoomSrcLoaded);

            this.Thumbnail_Button.Click += new RoutedEventHandler(Thumbnail_Button_Click);

        }

        private void Thumbnail_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Thumbnail_Grid.Visibility == Visibility.Visible)
            {
                this.Thumbnail_Grid.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Thumbnail_Grid.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// set ThumbnailListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jfd_JFDeepZoomSrcLoaded(object sender, JFDeepZoomEventArgs e)
        {
            this.UpdateThumbnailList();

            // set SelectionChanged
            Thumbnail_ListBox.SelectionChanged += new SelectionChangedEventHandler(ThumbnailListBox_SelectionChanged);

            this.StopLoading();

        }

        private void UpdateThumbnailList()
        {
            Thumbnail_ListBox.ItemsSource = jfd.ThumbnailList;


        }


        private void ThumbnailListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SwitchNextPreviousButtonEnable(false);

            int index = Thumbnail_ListBox.SelectedIndex;
            if (index >= 0)
            {
                int jfd_index = jfd.ImageNameToIndex(jfd.ThumbnailList[index].ThumbnailPath);
                jfd.ZoomSingleSubImage(jfd_index);
                jfd.CurrentSlideIndex = jfd_index;
            }
        }



        #endregion


        #region Step9 ChangeSelectableSkin

        private void InitSelectableSkin()
        {
            this.jfd.SelectCheckImageUri = "Images/green_alpha50.png";
            this.jfd.CheckImageWidth = 20;
        }


        #endregion


        #region Step10 Dynamic (skeleton）

        private void StopLoading()
        {
        }

        #endregion


        #region SwitchSemiDynamicOrDynamic 

        /// <summary>
        /// true: SemiDynamic, false:Dynamic
        /// </summary>
        private const bool IS_SEMIDYNAMIC = true;

        private void SwitchSemiDynamicOrDynamic()
        {
            if (IS_SEMIDYNAMIC)
            {
                this.StartSemiDynamic();
                this.List_Button.Visibility = Visibility.Visible;
                //this.Search_Button.Visibility = Visibility.Collapsed;

            }
            //else
            //{
            //    this.InitDynamic();
            //this.List_Button.Visibility = Visibility.Collapsed;
            //this.Search_Button.Visibility = Visibility.Visible;
            //}
        }

        #endregion


    }
}
