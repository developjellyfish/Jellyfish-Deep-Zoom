using System;
using System.Windows;
using System.Windows.Controls;

namespace Usejf
{
    /// <summary>
    /// [Zoom]
    /// </summary>
    public partial class Page:UserControl
    {
        private void InitZoomFunction()
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
    }
}
