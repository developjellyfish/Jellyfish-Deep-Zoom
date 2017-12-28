using Jellyfish.jfDeepZoom;

namespace Usejf
{
    public class ExtendedJFDeepZoom:JFDeepZoom
    {
        
        public ExtendedJFDeepZoom()
        {

        }

        protected override void JFDeepZoom_Moved(object sender, MouseWheelEventArgs e)
        {
            base.JFDeepZoom_Moved(sender, e);
        }
    }
}
