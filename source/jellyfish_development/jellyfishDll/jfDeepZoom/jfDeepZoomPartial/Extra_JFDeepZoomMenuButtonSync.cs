using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// 
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        private void UpdateNextPrevButtonPosition()
        {
            try {
                if (PreviousButton != null && NextButton != null) {
                    double buttonTop = this.Height / 2 - NextButton.Height / 2;

                    Canvas.SetTop(NextButton, buttonTop);
                    Canvas.SetTop(PreviousButton, buttonTop);

                    Canvas.SetLeft(NextButton, this.Width - NextButton.Width);
                    Canvas.SetLeft(PreviousButton, 0);
                }
            }
            catch (Exception e) {
                // Button is Undefined
            }
        }

    }
}
