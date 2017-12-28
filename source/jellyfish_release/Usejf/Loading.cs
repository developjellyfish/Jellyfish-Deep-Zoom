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
using System.Diagnostics;
using Jellyfish;
using Jellyfish.jfDeepZoom;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace Usejf
{
    /// <summary>
    /// This class is a control that is for displaying 9 rectangles.
    /// </summary>
    public class Loading:Canvas
    {
        private List<Rectangle> rects;
        private Storyboard loadingStoryboard;
        private int currentType = 0;

        private double v = 0.02;

        private double fric = 0.08;

        private int col = 4;
        private int row = 4;

        private DispatcherTimer dt;

        public Loading()
        {
            InitLoading();

            StartLoading();
        }

        private void InitLoading()
        {
            dt = new DispatcherTimer();
            // shape is changed in 3 seconds.
            dt.Interval = TimeSpan.FromSeconds(3);
            dt.Tick += new EventHandler(dt_Tick);
            
            rects = new List<Rectangle>();
            for (int i = 0; i < (col*row); i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = 10;
                rect.Height = 10;
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(120, 255, 0, 0);
                rect.Fill = brush;
                rect.Opacity = 0.8;

                this.Children.Add(rect);
                Canvas.SetLeft(rect, ((i % col) * 12));
                Canvas.SetTop(rect, (Math.Floor(i / row) * 12));

                rects.Add(rect);
            }

            loadingStoryboard = new Storyboard();
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            currentType++;
            if (currentType > 3)
            {
                currentType = 0;
            }
            else if (currentType < 0)
            {
                currentType = 3;
            }

            v *= -1;
        }

        public void StartLoading()
        {
            for (int i = 0; i < (col * row); i++)
            {
                Rectangle rect = rects[i];

                Canvas.SetLeft(rect, ((i % col) * 12));
                Canvas.SetTop(rect, (Math.Floor(i / row) * 12));
            }

            loadingStoryboard.Duration = TimeSpan.FromSeconds(0);
            loadingStoryboard.Completed += new EventHandler(loadingStoryboard_Completed);
            loadingStoryboard.Begin();

            dt.Start();
        }

        private void loadingStoryboard_Completed(object sender, EventArgs e)
        {
            for (int i = 0; i < (col*row); i++)
            {
                Point dest = new Point();

                switch (currentType)
                {
                    case 0:
                        dest = SetSnowCrystal(i);
                        break;
                    case 1:
                        dest = SetTile(i);
                        break;
                    case 2:
                        dest = SetSpiral(i);
                        break;
                    case 3:
                        dest = SetTile(i);
                        break;
                    default:
                        dest = SetTile(i);
                        break;
                }

                Canvas.SetLeft(rects[i], dest.X);
                Canvas.SetTop(rects[i], dest.Y);
            }

            loadingStoryboard.Begin();
        }

        public void StopLoading()
        {
            dt.Stop();
        }

        private Point SetSnowCrystal(int i)
        {
            Point res = new Point();

            double radius = 60 * i;

            double scale = 2;
            double angle = radius * Math.PI / 180;

            double destX = ((Math.Cos(angle) + Math.Sin(angle)) * scale * i) + 20;
            double destY = ((Math.Sin(angle) - Math.Cos(angle)) * scale * i) + 20;

            Point currentPoint = new Point(Canvas.GetLeft(rects[i]), Canvas.GetTop(rects[i]));

            res.X = currentPoint.X + (destX - currentPoint.X) * fric;
            res.Y = currentPoint.Y + (destY - currentPoint.Y) * fric;

            return res;
        }

        private Point SetSpiral(int i)
        {
            Point res = new Point();

            double radius = 30 * i;

            double destX = (Math.Cos(radius * Math.PI / 180 * 5) * radius / 10) * -1 + 20;
            double destY = (Math.Sin(radius * Math.PI / 180 * 5) * radius / 10) + 20;

            Point currentPoint = new Point(Canvas.GetLeft(rects[i]), Canvas.GetTop(rects[i]));
            res.X = currentPoint.X + (destX - currentPoint.X) * fric;
            res.Y = currentPoint.Y + (destY - currentPoint.Y) * fric;

            return res;
        }

        private Point SetTile(int i)
        {
            Point res = new Point();

            double destX = (i % col) * 12;
            double destY = Math.Floor(i / row) * 12;

            Point currentPoint = new Point(Canvas.GetLeft(rects[i]), Canvas.GetTop(rects[i]));

            res.X = currentPoint.X + (destX - currentPoint.X) * fric;
            res.Y = currentPoint.Y + (destY - currentPoint.Y) * fric;

            return res;
        }
    }
}
