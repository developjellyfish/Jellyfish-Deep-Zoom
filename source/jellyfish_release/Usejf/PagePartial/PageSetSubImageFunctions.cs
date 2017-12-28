using System;
using System.Windows;
using System.Windows.Controls;

namespace Usejf
{
    /// <summary>
    /// [SetSubImage]
    /// </summary>
    public partial class Page : UserControl
    {

        private void InitSetSubImageFunction()
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

            // return; if one of the TextBlocks have no string.
            if (SubImageXTb.Text == "" || SubImageYTb.Text == "" || SubImageIndexTb.Text == "")
                return;
            else
            {
                index = Int32.Parse(SubImageIndexTb.Text);
                jfd.setSubImagePoint(index, new Point(Double.Parse(SubImageXTb.Text), Double.Parse(SubImageYTb.Text)));
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
            if (SubImageWidthTb.Text == "" || SubImageIndexTb.Text == "")
                return;
            else
            {
                index = Int32.Parse(SubImageIndexTb.Text);
                jfd.setSubImageWidth(index, Double.Parse(SubImageWidthTb.Text));
            }
        }
    }
}
