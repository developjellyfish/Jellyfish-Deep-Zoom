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

namespace jellyfishDZApp
{
    public partial class Page03_FullScreen : UserControl
    {
        public Page03_FullScreen()
        {
            InitializeComponent();

            this.InitFullScreen();
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


        #region Step 6 MetaInfo (skeleton Method)

        private void SetXamlSyncMask(double nextWidth, double nextHeight)
        {

        }

        #endregion

       
        
        

    }
}
