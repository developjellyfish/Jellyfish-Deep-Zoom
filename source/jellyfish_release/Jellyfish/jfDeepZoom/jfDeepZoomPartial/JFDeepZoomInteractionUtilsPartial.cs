using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [Interaction Utils]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        # region Interaction Utils Properties

        /// <summary>
        /// Value of random angle
        /// </summary>
        private List<int> radiusList;

        /// <summary>
        /// value of random array Index
        /// </summary>
        private List<int> indexIntList;

        #endregion

        /// <summary>
        /// make sure about value of LayoutStyle is appropriate.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>existing LayoutStyle or not</returns>
        private bool CheckLayoutStyleInXAML(string style)
        {
            bool res = false;

            switch (style)
            {
                case "custom":
                case "line":
                case "spiral":
                case "spiral_archimedes":
                case "spread":
                case "tile":
                case "shrink":
                    res = true;
                    break;
                default:
                    break;
            }

            return res;
        }

        /// <summary>
        /// get x/y axis of subImage (based on parent MultiscaleImage)
        /// </summary>
        /// <param name="index">index of MultiScaleSubImage</param>
        /// <returns>Point (Unit of pixel)</returns>
        public Point getSubImagePoint(int index)
        {
            // deside there is subimage Index or not.
            judgeInRangeSubImage(index);

            Point outPoint = new Point();

            MultiScaleSubImage mssi = msi.SubImages[index];

            // Current SubImage Information
            double mssiVpX = mssi.ViewportOrigin.X;
            double mssiVpY = mssi.ViewportOrigin.Y;
            double mssiVpWidth = mssi.ViewportWidth;
            double mssiAspectRatio = mssi.AspectRatio;

            // ViewportWidth of multiScaleImage
            double msiVpWidth = msi.ViewportWidth;
            // Width of multiScaleImage(Unit of pixel)
            double msiPxWidth = msi.Width;
            // x axis of multiScaleImage
            double msiVpX = msi.ViewportOrigin.X;
            // y axis of multiScaleImage
            double msiVpY = msi.ViewportOrigin.Y;
            // Aspect ratio of multiScaleImage
            double msiAspectRatio = getAspectRatio();

            // x/y axis of subImage when it move to (0,0) Point of MultiScaleImage.
            double zeroVpX = -mssiVpX / mssiVpWidth;
            double zeroVpY = -mssiVpY / mssiVpWidth;

            // x axis difference between SubImage and (0,0) Point.
            double deltaVpX = msiVpX - zeroVpX;
            // x axis of subImage
            outPoint.X = (-1) * deltaVpX * msiPxWidth / msiVpWidth;

            // y axis difference between SubImage and (0,0) Point.
            double deltaVpY = msiVpY - zeroVpY;
            // y axis of subImage
            outPoint.Y = (-1) * deltaVpY * (msiPxWidth / msiAspectRatio) / (msiVpWidth / msiAspectRatio);

            return outPoint;
        }

        /// <summary>
        /// set Point of specified Index for SubImage
        /// </summary>
        /// <param name="index">index of MultiScaleSubImage</param>
        /// <param name="inPoint">Point (Unit of pixel)</param>
        public void setSubImagePoint(int index, Point inPoint)
        {
            // deside there is subimage Index or not.
            judgeInRangeSubImage(index);

            MultiScaleSubImage mssi = msi.SubImages[index];

            // Current SubImage Information
            double mssiVpX = mssi.ViewportOrigin.X;
            double mssiVpY = mssi.ViewportOrigin.Y;
            double mssiVpWidth = mssi.ViewportWidth;
            double mssiAspectRatio = mssi.AspectRatio;

            // ViewportWidth of multiScaleImage
            double msiVpWidth = msi.ViewportWidth;
            // Width of multiScaleImage(Unit of pixel)
            double msiPxWidth = msi.ActualWidth;

            // Height of multiScaleImage(Unit of pixel)
            double msiPxHeight = msi.ActualHeight;

            // x axis of multiScaleImage
            double msiVpX = msi.ViewportOrigin.X;
            // y axis of multiScaleImage
            double msiVpY = msi.ViewportOrigin.Y;
            // Aspect ratio of multiScaleImage
            double msiAspectRatio = getAspectRatio();
            
            Point nextVpPoint = new Point();

            // Zoom ratio of MultiScaleImage
            double zoomScale = msiVpWidth;
            // X axis of expected for modified Image (ViewportOrigin of MultiScaleImage)
            double nextVpX = -(inPoint.X / msiPxWidth) * zoomScale;
            // X axis of expected for modified Image (ViewportOrigin of specified subImage)
            nextVpPoint.X = nextVpX * mssiVpWidth - msiVpX * mssiVpWidth;

            // Y axis of expected for modified Image (ViewportOrigin of MultiScaleImage)
            double nextVpY = -(inPoint.Y / msiPxHeight) * (zoomScale / msiAspectRatio);
            // Y axis of expected for modified Image (ViewportOrigin of specified subImage)
            nextVpPoint.Y = nextVpY * (mssiVpWidth) - msiVpY * mssiVpWidth;

            mssi.ViewportOrigin = nextVpPoint;
        }

        /// <summary>
        /// get Width of Specified subImage
        /// </summary>
        /// <param name="index">Index of MultiScaleSubImage</param>
        /// <returns>width of subImage (Unit of pixel)</returns>
        public double getSubImageWidth(int index)
        {
            // deside there is subimage Index or not.
            judgeInRangeSubImage(index);

            double outMssiPxWidth = 1;

            MultiScaleSubImage mssi = msi.SubImages[index];

            // ViewportWidth of subImage
            double mssiVpWidth = mssi.ViewportWidth;

            // ViewportWidth of multiScaleImage
            double msiVpWidth = msi.ViewportWidth;

            // Width of multiScaleImage(Unit of pixel)
            double msiPxWidth = msi.Width;

            outMssiPxWidth = msiPxWidth / (mssiVpWidth * msiVpWidth);

            return outMssiPxWidth;
        }

        /// <summary>
        /// get Height of Specified subImage
        /// </summary>
        /// <param name="index">Index of MultiScaleSubImage</param>
        /// <returns>height of subImage (Unit of pixel)</returns>
        public double getSubImageHeight(int index)
        {
            // deside there is subimage Index or not.
            judgeInRangeSubImage(index);

            double outMssiPxHeight = 1;

            MultiScaleSubImage mssi = msi.SubImages[index];

            double mssiPxWidth = getSubImageWidth(index);

            // aspect ratio of subImage
            double mssiAspectRatio = mssi.AspectRatio;

            outMssiPxHeight = mssiPxWidth / mssiAspectRatio;

            return outMssiPxHeight;
        }

        /// <summary>
        /// set Width of SubImage by Pixel
        /// condition: x Axis of subImage must be maintained.
        /// </summary>
        /// <param name="index">index of MultiScaleSubImage</param>
        /// <param name="inPoint">Width (Unit of pixel)</param>
        public void setSubImageWidth(int index, double inPx)
        {
            // deside there is subimage Index or not.
            judgeInRangeSubImage(index);

            double mssiVpWidth = 1;

            MultiScaleSubImage mssi = msi.SubImages[index];

            // top left x/y axis of subImage before changing width
            Point prevSubImagePoint = getSubImagePoint(index);

            // Width of subImage(Unit of pixel)
            double mssiPxWidth = inPx;

            // ViewportWidth of multiScaleImage
            double msiVpWidth = msi.ViewportWidth;

            // Width of multiScaleImage(Unit of pixel)
            double msiPxWidth = msi.Width;

            mssiVpWidth = msiPxWidth / (mssiPxWidth * msiVpWidth);

            mssi.ViewportWidth = mssiVpWidth;

            // apply top left x/y axis of subImage before changing width
            setSubImagePoint(index, prevSubImagePoint);
        }

        /// <summary>
        /// set zIndex of subImage to most front
        /// </summary>
        /// <param name="topIndex">SubImageのIndex</param>
        private void SetZindex(int topIndex)
        {
            // deside there is subimage Index or not.
            judgeInRangeSubImage(topIndex);

            int msiLen = msi.SubImages.Count;
            int len = Indices.Count;
            for (int i = len-1; i >= 0; i-- )
            {
                MultiScaleSubImage mssi = msi.SubImages[Indices[i]];
                mssi.ZIndex = i - 1;
            }
            msi.SubImages[topIndex].ZIndex = msiLen;
        }

        /// <summary>
        /// get AspectRatio of MultiScaleImage
        /// </summary>
        /// <returns>AspectRatio of MultiScaleImage</returns>
        public double getAspectRatio()
        {
            double outRatio = msi.ActualWidth / msi.ActualHeight;

            return outRatio;
        }

        /// <summary>
        /// change height of all subImage to be same.
        /// </summary>
        /// <param name="indexSubImage">index of SubImage does set as standard</param>
        public void DoSameSizeSubImages(int indexSubImage)
        {
            // Standard rectangle
            Rect rect = GetSubImageRect(indexSubImage);

            int len = Indices.Count;
            for (int i = 0; i < len; i++)
            {
                MultiScaleSubImage mssi = msi.SubImages[Indices[i]];
                Rect mssiRect = GetSubImageRect(i);

                mssi.ViewportWidth = mssi.ViewportWidth / ((rect.Height) / (mssiRect.Height / mssi.AspectRatio));
            }
        }

        /// <summary>
        /// HitTest MultiScaleSubImage.
        /// </summary>
        /// <param name="p"> hittest point (element coordinates)</param>
        /// <param name="indexSubImage">Index of subImage as search target</param>
        /// <returns>is subImage in the Rectangle or not</returns>
        public bool SubImageHitTest(Point p, int indexSubImage)
        {
            Rect subImageRect = GetSubImageRect(indexSubImage);

            // convert hittest target into logical coordinates.
            if (subImageRect.Contains(msi.ElementToLogicalPoint(p)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return Rectangle of MultiScaleImage.SubImages.
        /// </summary>
        /// <param name="indexSubImage">search index of SubImage</param>
        /// <returns>Rectangle of MultiScaleImage.SubImages</returns>
        public Rect GetSubImageRect(int indexSubImage)
        {
            // if index of subImage is more than Maximum of Indices, return Rect.Empty
            if (indexSubImage < 0 || indexSubImage > Indices.Max())
            {
                return Rect.Empty;
            }

            // if index of SubImage is not exist, return Rect.Empty
            if (indexSubImage >= msi.SubImages.Count)
            {
                return Rect.Empty;
            }

            MultiScaleSubImage subImage = msi.SubImages[indexSubImage];

            double scaleBy = 1 / subImage.ViewportWidth;

            return new Rect(-subImage.ViewportOrigin.X * scaleBy, -subImage.ViewportOrigin.Y * scaleBy, 1 * scaleBy, (1 / subImage.AspectRatio) * scaleBy);
        }

        /// <summary>
        /// viewport width of each subImage when change height of all subImage tobe same as specified index subImage
        /// </summary>
        /// <param name="indexInt">index does set as standard</param>
        /// <returns>List of modified ViewPortWidth of SubImage</returns>
        private List<double> getViewPortWidthForUnifiedHeight(int indexInt)
        {
            // ViewPortWidth of expected for modified Image
            List<double> outVpList = new List<double>();

            // Width with viewportWidth is 1
            double basisWidth = 2062;

            // ViewportWidth of each subImage
            List<double> beforViewPortWList = new List<double>();

            // Width of each subImage (Unit of Pixel)
            List<double> beforeImageWList = new List<double>();
            // Height of each subImage (Unit of Pixel)
            List<double> beforeImageHList = new List<double>();

            // Width for modified Image (Unit of Pixel)
            List<double> afterImageWList = new List<double>();

           // for (int i = 0; i < msi.SubImages.Count; i++)
            int len = Indices.Count;
            for (int i = 0; i < len; i++)
            {
                MultiScaleSubImage msiSub = msi.SubImages[Indices[i]];
                // ViewportWidth of SubImage
                beforViewPortWList.Add(msiSub.ViewportWidth);

                // Width of subImage (Unit of Pixel)
                double beforeImageW = basisWidth / msiSub.ViewportWidth;
                beforeImageWList.Add(beforeImageW);

                // Height of subImage (Unit of Pixel)
                beforeImageHList.Add(beforeImageW / msiSub.AspectRatio);
            }

            // subImage width zoom in/out (Unit of Pixel)
            double afterImageBasisHeight = beforeImageHList[indexInt];

            for (int i = 0; i < len; i++)
            {
                // subImage zoom ratio before changing/ after changing
                double changeRatio = beforeImageHList[i] / afterImageBasisHeight;

                // ViewPortWidth of expected for modified Image
                outVpList.Add(changeRatio * beforViewPortWList[i]);
            }

            return outVpList;
        }

        /// <summary>
        /// Shuffles the list.
        /// </summary>
        /// <param name="inList">The in list.</param>
        private void shuffleList(List<int> inList)
        {
            int i = inList.Count;

            while (i-- > 0)
            {
                double aI = new Random().NextDouble() * (i + 1);

                // calcurate to random index
                int j = (int)Math.Floor(aI); 

                int t = inList[i];

                inList[i] = inList[j];

                inList[j] = t;
            }
        }

        /// <summary>
        /// Fit to MultiScaleImage space according to actual width and height (Instantaneously)
        /// </summary>
        public void Fit()
        {
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
            Point newOrigin = new Point(0, 0);

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

            // collection
            double adjustX = 0;
            double adjustY = 0;

            // center position of Occupied Area is calculated.
            adjustX = allSubImagePoint.X - allSubImagesWidth / 2;
            adjustY = allSubImagePoint.Y - allSubImagesHeight / 2;

            // target ViewportOrigin
            newOrigin.X = -adjustX - afterViewportWidth / 2;
            newOrigin.Y = -adjustY - afterViewportWidth / msiAspectRatio / 2;

            msi.UseSprings = false;

            // set ViewportWidth
            msi.ViewportWidth = afterViewportWidth;

            // move to center position
            msi.ViewportOrigin = newOrigin;

            msi.UseSprings = true;
        }
    }
}
