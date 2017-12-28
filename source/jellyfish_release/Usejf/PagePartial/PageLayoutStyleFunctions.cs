using System.Windows;
using System.Windows.Controls;
using Jellyfish.jfDeepZoom;

namespace Usejf
{
    /// <summary>
    /// [LayoutStyle]
    /// </summary>
    public partial class Page : UserControl
    {
        /// <summary>
        /// flag that button is pressed.
        /// </summary>
        private bool layoutButtonPressed = false;

        private void InitLayoutFunction()
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
            // [Reset Zoom and Coordinates]
            ResetButton.Click += new RoutedEventHandler(ResetButton_Click);

            // initialize event
            jfd.JFDeepZoomChangeStarted += new JFDeepZoomEventHandler(jfd_JFDeepZoomChangeStarted);
            jfd.JFDeepZoomMotionFinished += new JFDeepZoomEventHandler(jfd_JFDeepZoomLayoutFinished);

            // previous, next
            jfd.PreviousButton.Click += new RoutedEventHandler(Button_Click);
            jfd.NextButton.Click += new RoutedEventHandler(Button_Click);

            // [tick slideshow]
            jfd.MultiScaleSubImageTick += new MultiScaleSubImageEventHandler(jfd_MultiScaleSubImageTick);
        }

        void jfd_MultiScaleSubImageTick(object sender, MultiScaleSubImageEventArgs e)
        {
            SwitchNextPreviousButtonEnable(false);
        }

        void jfd_JFDeepZoomChangeStarted(object sender, JFDeepZoomEventArgs e)
        {
            SwitchNextPreviousButtonEnable(false);
        }

        void Button_Click(object sender, RoutedEventArgs e)
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
            ResetButton.IsEnabled = flag;
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
                //jfd.CurrentLayoutStyle = LayoutStyle.FIT;
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
    }
}
