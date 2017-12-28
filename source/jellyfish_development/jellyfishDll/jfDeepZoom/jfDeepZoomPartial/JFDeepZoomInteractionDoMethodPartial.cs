using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [Interaction Domethod]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        # region "Do" Properties

        /// <summary>
        /// Margin that is used "Line" Layout and "Tile" Layout
        /// </summary>
        private double lineTileSpace = 0;

        /// <summary>
        /// Margin that is used "Line" Layout and "Tile" Layout
        /// </summary>
        public double LineTileSpace
        {
            get
            {
                return lineTileSpace;
            }
            set
            {
                lineTileSpace = value;
            }
        }

        /// <summary>
        /// lineTileBasisIndex Default value is 0
        /// </summary>
        private int lineTileBasisIndex = 0;

        /// <summary>
        /// Decide which index does set as standard for "Line" Layout and "Tile" Layout
        /// </summary>
        public int LineTileBasisIndex
        {
            get
            {
                return lineTileBasisIndex;
            }
            set
            {
                if (lineTileBasisIndex >= 0 && lineTileBasisIndex < msi.SubImages.Count)
                {
                    lineTileBasisIndex = value;
                }
            }
        }

        /// <summary>
        /// the Default value of tileBasisRatio is 4
        /// </summary>
        private double tileBasisRatio = 4;

        /// <summary>
        /// With "Tile" Layout, decide what time to wrap with standard image.
        /// </summary>
        public double TileBasisRatio
        {
            get
            {
                return tileBasisRatio;
            }
            set
            {
                if (tileBasisRatio > 0)
                {
                    tileBasisRatio = value;
                }
            }
        }

        #endregion

        #region "Do" methods

        /// <summary>
        /// Execution of line layout 
        /// </summary>
        protected virtual void DoLine()
        {
            if (!isSlide)
            {
                // total width
                double totalW = 0;

                // total width with all Image ( with already deployed only) 
                double totalLimitW = 0;

                // Margin
                double spaceW = lineTileSpace;

                // Width with viewportWidth is 1
                double basisWidth = 2062;

                // Spring Coefficient
                double fric = 0.95;

                // Decide which index does set as standard
                int basisIndex = lineTileBasisIndex;

                // List of modified ViewportList
                List<double> afterVpWList = getViewPortWidthForUnifiedHeight(basisIndex);

                // ViewportWidth of Image
                double viewportW = 1;

                // Width of Image
                double pixelW = 1;

                // X axis of expected for modified Image
                double pixelX = 1;

                uint ableToFinishTweenCount = 0;
                List<int> ableToFinishIndex = new List<int>();
                int len = Indices.Count;

                for (int i = 0; i < len; i++)
                {
                    int indiciesIndexOf = Indices.IndexOf(Indices[i]);
                    if (indiciesIndexOf == -1)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex.Add(0);
                        ableToFinishIndex[i] = 1;
                        continue;
                    }

                    ableToFinishIndex.Add(0);
                    int useId = Indices[i];
                    MultiScaleSubImage mssi = msi.SubImages[useId];

                    // ViewportWidth of Image
                    viewportW = afterVpWList[i];

                    // Width of Image
                    pixelW = basisWidth / viewportW;

                    // X axis of expected for modified Image
                    totalW += pixelW;
                }

                for (int i = 0; i < len; i++)
                {
                    int indiciesIndexOf = Indices.IndexOf(Indices[i]);
                    if (indiciesIndexOf == -1)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex.Add(0);
                        ableToFinishIndex[i] = 1;
                        continue;
                    }

                    int useId = Indices[i];
                    MultiScaleSubImage mssi = msi.SubImages[useId];

                    // ViewportWidth of Image
                    viewportW = afterVpWList[i];

                    // Width of Image
                    pixelW = basisWidth / viewportW;

                    // X axis of expected for modified Image
                    // (for deploying center position, it is set half value with Width of Image)
                    pixelX = totalLimitW - totalW / 2;

                    // get X axis of ViewportOrigin of Image
                    double afterViewportOriginX = -pixelX / pixelW;

                    Point p = new Point();

                    // --- (target + distance) * Spring Coefficient
                    p.X = afterViewportOriginX - (afterViewportOriginX - mssi.ViewportOrigin.X) * fric;

                    // get Y axis of ViewportOrigin of Image
                    double afterViewportOriginY = 0.5 / mssi.AspectRatio;
                    p.Y = afterViewportOriginY - (afterViewportOriginY - mssi.ViewportOrigin.Y) * fric;

                    if ( (Math.Abs(afterViewportOriginX - mssi.ViewportOrigin.X) < EndThreshold) && (Math.Abs(afterViewportOriginY - mssi.ViewportOrigin.Y) < EndThreshold))
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex[i] = 1;

                        // Move to the target place
                        p.X = afterViewportOriginX;
                        p.Y = afterViewportOriginY;
                    }

                    mssi.ViewportWidth = mssi.ViewportWidth + (afterVpWList[i] - mssi.ViewportWidth) * fric;

                    // configure ViewportOrigin of Image
                    mssi.ViewportOrigin = p;

                    // for deploying next Image, Add current Image width to Total Image
                    totalLimitW += pixelW + spaceW;
                }
                if (ableToFinishIndex.Contains(0) == false)
                {
                    DoFinishLayoutCommon();
                }
            }
        }

        /// <summary>
        /// Execution of tile layout 
        /// </summary>
        protected virtual void DoTile()
        {
            if (!isSlide)
            {
                // the limited width for deploying tile, if it exceeded with this width, then wrap.
                double limitW = 1;

                // total width
                double totalW = 0;
                // total height
                double totalH = 0;
                // total width with all Image ( with already deployed only) 
                double totalLimitW = 0;
                // total height with all Image ( with already deployed only) 
                double totalLimitH = 0;

                // Width with viewportWidth is 1
                double basisWidth = 2062;
                // Margin (horizontal)
                double spaceW = lineTileSpace;
                // Margin (vertical)
                double spaceH = lineTileSpace;

                // Decide which index does set as standard
                int basisIndex = lineTileBasisIndex;

                // decide what time to wrap with standard image
                double basisRatio = tileBasisRatio;

                // List of modified ViewportList
                List<double> afterVpWList = getViewPortWidthForUnifiedHeight(basisIndex);

                // the limited width for deploying tile, if it exceeded with this width, then wrap.
                limitW = basisWidth / afterVpWList[basisIndex] * basisRatio;

                // The standard of all Image
                double basisHeight = (basisWidth / msi.SubImages[basisIndex].ViewportWidth) / msi.SubImages[basisIndex].AspectRatio + spaceH;

                // ViewportWidth of Image
                double viewportW = 1;

                // Width of Image
                double pixelW = 1;
                // X axis of expected for modified Image
                double pixelX = 1;
                // Height of Image
                double pixelH = 1;
                // Y axis of expected for modified Image
                double pixelY = 1;

                uint ableToFinishTweenCount = 0;
                List<int> ableToFinishIndex = new List<int>();
                int len = Indices.Count;

                for (int i = 0; i < len; i++)
                {
                    ableToFinishIndex.Add(0);
                }

                for (int i = 0; i < len; i++)
                {
                    int indiciesIndexOf = Indices.IndexOf(Indices[i]);
                    if (indiciesIndexOf == -1)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex.Add(0);
                        ableToFinishIndex[i] = 1;
                        continue;
                    }

                    int useId = Indices[i];
                    MultiScaleSubImage mssi = msi.SubImages[useId];

                    // ViewportWidth of Image
                    viewportW = afterVpWList[i];

                    // Width of Image
                    pixelW = basisWidth / viewportW;

                    // if (totalW + pixelW + spaceW > limitW), move to next line
                    if (totalW + pixelW + spaceW > limitW)
                    {
                        totalW = 0;
                        totalH += basisHeight;
                    }

                    // for deploying next Image, Add current Image width to Total Image
                    totalW += pixelW + spaceW;
                }

                for (int i = 0; i < len; i++)
                {
                    int useId = Indices[i];
                    MultiScaleSubImage mssi = msi.SubImages[useId];

                    // ViewportWidth of Image
                    viewportW = afterVpWList[i];

                    // Width of Image
                    pixelW = basisWidth / viewportW;

                    // if (totalLimitW + pixelW + spaceW > limitW), move to next line
                    if (totalLimitW + pixelW + spaceW > limitW)
                    {
                        totalLimitW = 0;
                        totalLimitH += basisHeight;
                    }

                    // X axis of expected for modified Image
                    // (for deploying center position, it is set half value with Width of Image)
                    pixelX = totalLimitW - limitW / 2;

                    // get X axis of ViewportOrigin of Image
                    double afterViewportOriginX = -pixelX / pixelW;

                    // Height of Image
                    pixelH = pixelW;

                    // Y axis of expected for modified Image
                    pixelY = totalLimitH - (totalH) / 2;

                    // get Y axis of ViewportOrigin of Image
                    double afterViewportOriginY = -pixelY / pixelH + 0.5 / mssi.AspectRatio;

                    Point p = new Point();
                    double fric = 0.95;

                    // --- (target + distance) * Spring Coefficient
                    p.X = afterViewportOriginX - (afterViewportOriginX - mssi.ViewportOrigin.X) * fric;

                    p.Y = afterViewportOriginY - (afterViewportOriginY - mssi.ViewportOrigin.Y) * fric;

                    if ( Math.Abs(afterViewportOriginX - mssi.ViewportOrigin.X) < EndThreshold && Math.Abs(afterViewportOriginY - mssi.ViewportOrigin.Y) < EndThreshold)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex[i] = 1;

                        // Move to the target place
                        p.X = afterViewportOriginX;
                        p.Y = afterViewportOriginY;
                    }

                    mssi.ViewportWidth = mssi.ViewportWidth + (afterVpWList[i] - mssi.ViewportWidth) * fric;

                    // configure ViewportOrigin of Image
                    mssi.ViewportOrigin = p;

                    // for deploying next Image, Add current Image width to Total Image
                    totalLimitW += pixelW + spaceW;
                }

                if (ableToFinishIndex.Contains(0) == false)
                {
                    DoFinishLayoutCommon();
                }
            }
        }

        /// <summary>
        /// Execution of snow cristal layout 
        /// </summary>
        protected virtual void DoSnowCrystal()
        {
            if (!isSlide)
            {
                double destWidth = 4;
                double destViewportWidth = destWidth;

                uint ableToFinishTweenCount = 0;

                List<int> ableToFinishIndex = new List<int>();
                int len = Indices.Count;
               
                for (int i = 0; i < len; i++)
                {

                    int indiciesIndexOf = Indices.IndexOf(Indices[i]);
                    if (indiciesIndexOf == -1)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex.Add(0);
                        ableToFinishIndex[i] = 1;
                        continue;
                    }

                    int useId = Indices[i];
                    ableToFinishIndex.Add(0);
                    MultiScaleSubImage mssi = msi.SubImages[useId];
                    Point p = new Point();

                    //------------------------------------------------ Destination >>>
                    double radius = 60 * i;

                    //-- making spiral
                    double scale = 0.25;
                    double angle = radius * Math.PI / 180;

                    double destX = ((Math.Cos(angle) + Math.Sin(angle)) * scale * i);
                    double destY = ((Math.Sin(angle) - Math.Cos(angle)) * scale * i);

                    //------------------------------------------------ Destination <<<

                    double fric = 0.05;

                    p.X = mssi.ViewportOrigin.X + (destX - mssi.ViewportOrigin.X) * fric;
                    p.Y = mssi.ViewportOrigin.Y + (destY - mssi.ViewportOrigin.Y) * fric;

                    mssi.ViewportOrigin = p;

                    if ( Math.Abs(destX - mssi.ViewportOrigin.X) < EndThreshold && Math.Abs(destY - mssi.ViewportOrigin.Y) < EndThreshold)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex[i] = 1;

                        // Move to the target place
                        p.X = destX;
                        p.Y = destY;
                    }
                }

                if (ableToFinishIndex.Contains(0) == false)
                {
                    DoFinishLayoutCommon();
                }
            }
        }

        /// <summary>
        /// Execution of spiral layout 
        /// </summary>
        protected virtual void DoSpiral()
        {
            if (!isSlide)
            {

                double destWidth = 10;
                double destViewportWidth = destWidth;

                uint ableToFinishTweenCount = 0;
                List<int> ableToFinishIndex = new List<int>();

                int len = Indices.Count;
                for (int i = 0; i < len; i++)
                {
                    int indiciesIndexOf = Indices.IndexOf(Indices[i]);
                    if (indiciesIndexOf == -1)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex.Add(0);
                        ableToFinishIndex[i] = 1;
                        continue;
                    }

                    int useId = Indices[i];
                    ableToFinishIndex.Add(0);

                    MultiScaleSubImage mssi = msi.SubImages[useId];
                    Point p = new Point();

                    //------------------------------------------------ Destination >>>
                    double radius = 5 * i;

                    //-- spiral
                    double destX = (Math.Cos(radius * Math.PI / 180 * 5) * radius / 20) * -1;
                    double destY = (Math.Sin(radius * Math.PI / 180 * 5) * radius / 20);

                    //------------------------------------------------ Destination <<<

                    double fric = 0.05;

                    p.X = mssi.ViewportOrigin.X + (destX - mssi.ViewportOrigin.X) * fric;
                    p.Y = mssi.ViewportOrigin.Y + (destY - mssi.ViewportOrigin.Y) * fric;

                    mssi.ViewportOrigin = p;

                    if ( Math.Abs(destX - mssi.ViewportOrigin.X) < EndThreshold && Math.Abs(destY - mssi.ViewportOrigin.Y) < EndThreshold)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex[i] = 1;

                        // Move to the target place
                        p.X = destX;
                        p.Y = destY;
                    }
                }

                if (ableToFinishIndex.Contains(0) == false)
                {
                    DoFinishLayoutCommon();
                }
            }
        }

        /// <summary>
        /// Execution of random spread layout 
        /// </summary>
        protected virtual void DoSpread()
        {
            if (!isSlide)
            {
                double destWidth = 5;
                double destViewportWidth = destWidth;
                uint ableToFinishTweenCount = 0;
                List<int> ableToFinishIndex = new List<int>();

                Point p = new Point();
                int len = Indices.Count;
                for (int i = 0; i < len; i++)
                {
                    int indiciesIndexOf = Indices.IndexOf(i);
                    if (indiciesIndexOf == -1)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex.Add(1);
                        continue;
                    }

                    ableToFinishIndex.Add(0);
                    int randomI = indexIntList[i];

                    MultiScaleSubImage mssi = msi.SubImages[randomI];
                    p = new Point();

                    //------------------------------------------------ Destination >>>
                    double radius = radiusList[i];

                    double scale = 0.25;

                    double angle = radius * Math.PI / 180;
                    double destX = ((Math.Cos(angle) + Math.Sin(angle)) * scale * i);
                    double destY = ((Math.Sin(angle) - Math.Cos(angle)) * scale * i);

                    //------------------------------------------------ Destination <<<

                    double fric = 0.05;

                    p.X = mssi.ViewportOrigin.X + (destX - mssi.ViewportOrigin.X) * fric;
                    p.Y = mssi.ViewportOrigin.Y + (destY - mssi.ViewportOrigin.Y) * fric;

                    double changeZoom = (startZoom-ZoomValue);
                    if (changeZoom == 0) changeZoom = 1;

                    if (Math.Abs(destX - mssi.ViewportOrigin.X) / changeZoom < EndThreshold && Math.Abs(destY - mssi.ViewportOrigin.Y) / changeZoom < EndThreshold)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex[i] = 1;

                        // Move to the goal
                        p.X = destX;
                        p.Y = destY;

                    }
                    mssi.ViewportOrigin = p;
                }
                
                if (ableToFinishIndex.Contains(0) == false)
                {
                    isTweening = false;
                    isSlide = false;
                    subImageAnimationTweening = false;
                    CurrentLayoutStyle = LayoutStyle.NONE;
                    JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
                    OnJFDeepZoomMotionFinished(ev);
                }
            }
        }

        /// <summary>
        /// Fit to MultiScaleImage space according to actual width and height.
        /// </summary>
        public virtual void DoFit()
        {
            isTweening = true;
            isSlide = true;

            // for fitting Image, maximum accupied width / height are calculated.
            double maxX = 0;
            double maxY = 0;

            double minX = 0;
            double minY = 0;

            int len = Indices.Count;

            for (int i = 0; i < len; i++)
            {
                int useId = Indices[i];
                MultiScaleSubImage mssi = msi.SubImages[useId];

                Rect subRect = GetSubImageRect(useId);

                minX = Math.Min(minX, subRect.X);
                minY = Math.Min(minY, subRect.Y);
                maxX = Math.Max(maxX, subRect.X + subRect.Width);
                maxY = Math.Max(maxY, subRect.Y + subRect.Height);
            }

            // for fitting Image, maximum accupied width / height are calculated.
            double allSubImagesWidth = Math.Abs(maxX - minX);
            double allSubImagesHeight = Math.Abs(maxY - minY);

            // Top left axis of occupied area of SubImages is calculated
            Point allSubImagePoint = new Point(-minX, -minY);


            // ViewportWidth of All SubImage 
            double allSubImageAspectRatio = allSubImagesWidth / allSubImagesHeight;

            double afterViewportWidth = 1;

            double msiAspectRatio = getAspectRatio();

            // Correct viewportWidth to fit
            if (msiAspectRatio > allSubImageAspectRatio)
            {
                afterViewportWidth = allSubImagesHeight * msiAspectRatio;
            }
            else
            {
                afterViewportWidth = allSubImagesWidth;
            }

            // Move center Position of MultiScaleImage Space
            // Target ViewportOrigin
            Point newOrigin = new Point(0,0);

            // x axis of center position of MultiScaleImage Space
            double centerElementX = -0.5;
            // y axis of center position of MultiScaleImage Space
            double centerElementY = centerElementX * msi.ActualHeight / msi.ActualWidth;

            if ((1 / afterViewportWidth) <= MinZoom && MinZoom > 0)
            {
                afterViewportWidth = 1 / MinZoom;
            }
            else if ((1 / afterViewportWidth) >= MaxZoom && MaxZoom > 0)
            {
                afterViewportWidth = 1 / MaxZoom;

            }
            ZoomValue = 1 / afterViewportWidth;

            // correspond ViewportWidth
            msi.ViewportWidth = afterViewportWidth;

            // collection
            double adjustX = 0;
            double adjustY = 0;

            // center position of Occupied Area is calculated.
            adjustX = allSubImagePoint.X - allSubImagesWidth / 2;
            adjustY = allSubImagePoint.Y - allSubImagesHeight / 2;

            // target iewportOrigin
            newOrigin.X = - adjustX - afterViewportWidth / 2;
            newOrigin.Y = - adjustY - afterViewportWidth / msiAspectRatio / 2;

            // move to center position
            msi.ViewportOrigin = newOrigin;
        }

        /// <summary>
        /// Execution of custom layout (use only Semi-Dynamic)
        /// </summary>
        protected virtual void DoCustomLayout()
        {
            if (!isSlide)
            {
                double destWidth = 10;
                double destViewportWidth = destWidth;

                uint ableToFinishTweenCount = 0;
                List<int> ableToFinishIndex = new List<int>();

                int hitCount = -1;

                int msiLen = msi.SubImages.Count;
                int len = newLayoutViewportList.Count;
                for (int i = 0; i < len; i++)
                {
                    int indiciesIndexOf = Indices.IndexOf(i);
                    if ( (indiciesIndexOf == -1 || indiciesIndexOf >= len) || indiciesIndexOf >= msiLen )
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex.Add(0);
                        ableToFinishIndex[i] = 1;
                        continue;
                    }
                    hitCount++;
                    int useId = Indices[indiciesIndexOf];

                    ableToFinishIndex.Add(0);

                    MultiScaleSubImage mssi = msi.SubImages[useId];
                    int destId = indiciesIndexOf;//(Indices.Count-1) - indiciesIndexOf;
                    Point p = new Point();

                    //------------------------------------------------ Destination >>>

                    double destX = Double.Parse(newLayoutViewportList[destId]["X"]);
                    double destY = Double.Parse(newLayoutViewportList[destId]["Y"]);

                    //------------------------------------------------ Destination <<<

                    double fric = 0.05;

                    p.X = mssi.ViewportOrigin.X + (destX - mssi.ViewportOrigin.X)*fric;
                    p.Y = mssi.ViewportOrigin.Y + (destY - mssi.ViewportOrigin.Y)*fric;

                    double currentWidth = Double.Parse(newLayoutViewportList[destId]["Width"]) * 1;

                    mssi.ViewportOrigin = p;
                    mssi.ViewportWidth = currentWidth;

                    if (Math.Abs(destX - mssi.ViewportOrigin.X) < EndThreshold && Math.Abs(destY - mssi.ViewportOrigin.Y) < EndThreshold)
                    {
                        ableToFinishTweenCount++;
                        ableToFinishIndex[i] = 1;

                        // Move to target
                        p.X = destX;
                        p.Y = destY;
                    }
                }

                if (ableToFinishIndex.Contains(0) == false)
                {
                    DoFinishLayoutCommon();
                }
            }
        }

        /// <summary>
        /// Execution of shrink layout 
        /// </summary>
        protected virtual void DoShrink()
        {
            if (!isSlide)
            {
                if (subImageAnimationTweening)
                {
                    Point centerPoint = new Point(this.ActualWidth / 2, this.ActualHeight / 2);
                    ZoomByRatio(0.005, centerPoint);
                    subImageAnimationTweening = false;
                }
            }
        }

        /// <summary>
        /// it called after the layout processing is completed
        /// </summary>
        protected virtual void DoFinishLayoutCommon()
        {
            isTweening = false;
            isSlide = false;
            subImageAnimationTweening = false;
            CurrentLayoutStyle = LayoutStyle.NONE;
            JFDeepZoomEventArgs ev = new JFDeepZoomEventArgs(msi);
            OnJFDeepZoomMotionFinished(ev);
        }

        /// <summary>
        /// Execution of slide show 
        /// </summary>
        protected virtual void DoSlideShow()
        {
            isSlide = true;

            // Input the corresponding index number
            ZoomSingleSubImage(Indices[CurrentSlideIndex]);

            CurrentLayoutStyle = LayoutStyle.NONE;
        }

        #endregion
    }
}
