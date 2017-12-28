using System.Windows;
using System.Windows.Controls;

namespace Jellyfish.jfDeepZoom
{
    
    /// <summary>
    /// Event handler for MultiScaleSubImage
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    public delegate void MultiScaleSubImageEventHandler(object sender, MultiScaleSubImageEventArgs e);

    /// <summary>
    /// Event handler for JFDeepZoom
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    public delegate void JFDeepZoomEventHandler(object sender, JFDeepZoomEventArgs e);

    /// <summary>
    /// Event handler for Communication of JFDeepZoom
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    public delegate void JFCommunicationEventHandler(object sender, JFCommunicationEventArgs e);

    /// <summary>
    /// Event handler for Communication Error of JFDeepZoomJFDeepZoom
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    public delegate void JFCommunicationErrorEventHandler(object sender, JFCommunicationErrorEventArgs e);

    /// <summary>
    /// JFDeepZoom class
    /// </summary>
    public partial class JFDeepZoom:Canvas
    {
        /// <summary>
        /// LayoutRoot canvas.
        /// </summary>
        public Canvas LayoutRoot;

        /// <summary>
        /// Constructor.
        /// </summary>
        public JFDeepZoom()
        {
            this.LayoutRoot = new Canvas();
            this.Children.Add(LayoutRoot);

            this.Loaded += new RoutedEventHandler(JFDeepZoom_Loaded);
        }
    }
}
