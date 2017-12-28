using System.Windows;
using System.Windows.Controls;

namespace Usejf
{
    /// <summary>
    /// [SlideShow]
    /// </summary>
    public partial class Page : UserControl
    {
        private void InitSlideShowFunction()
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

        void FFButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.Next();
            SwitchNextPreviousButtonEnable(false);
        }
        void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.StartSlideShow();
            SwitchNextPreviousButtonEnable(false);
        }
        void StopButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.StopSlideShow();
        }
        void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.Previous();
            SwitchNextPreviousButtonEnable(false);
        }
    }
}
