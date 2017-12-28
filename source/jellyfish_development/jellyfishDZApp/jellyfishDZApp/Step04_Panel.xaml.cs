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
    public partial class Step04_Panel : UserControl
    {
        public Step04_Panel()
        {
            InitializeComponent();

            this.InitFullScreen();

            this.InitPanel();
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




        #region Step 6 MetaInfo (skeleton Method)

        private void SetXamlSyncMask(double nextWidth, double nextHeight)
        {

        }

        #endregion

       
        
        

    }
}
