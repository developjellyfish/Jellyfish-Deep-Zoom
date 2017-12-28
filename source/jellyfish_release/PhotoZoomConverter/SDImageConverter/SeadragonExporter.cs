using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;


namespace SDImageConverter
{
    /// <summary>
    /// SeadragonImage Class
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class SeadragonImage
    {
        /// <summary>
        /// RECT struct
        /// </summary>
        public struct RECT
        {
            /// <summary>
            /// left
            /// </summary>
            public Int32 left;
            /// <summary>
            /// top
            /// </summary>
            public Int32 top;
            /// <summary>
            /// right
            /// </summary>
            public Int32 right;
            /// <summary>
            /// bottom
            /// </summary>
            public Int32 bottom;
        };

        /// <summary>
        /// itemRect
        /// </summary>
        public RECT itemRect;
        /// <summary>
        /// imagePath
        /// </summary>
        public String imagePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeadragonImage"/> class.
        /// </summary>
        /// <param name="ptOrg">The pt org.</param>
        /// <param name="imagePath">The image path.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public SeadragonImage(Point ptOrg, string imagePath, int width, int height)
        {

            itemRect.left = (Int32)ptOrg.X;
            itemRect.top = (Int32)ptOrg.Y;

            itemRect.right = (Int32)(itemRect.left + width);
            itemRect.bottom = (Int32)(itemRect.top + height);
            this.imagePath = imagePath;
        }
    }

    /// <summary>
    /// Seadragon Exporter Class
    /// </summary>
    public class SeadragonExporter
    {
        /// <summary>
        /// Exports the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="images">The images.</param>
        /// <param name="viewPortWidth">Width of the view port.</param>
        /// <param name="viewPortHeight">Height of the view port.</param>
        /// <param name="tileSize">Size of the tile.</param>
        /// <param name="imageCompression">The image compression.</param>
        /// <param name="DzcOutputFlag">The DZC output flag.</param>
        /// <param name="Owner">The Owner Name.</param>
        static public void Export(String path, List<SeadragonImage> images, Int32 viewPortWidth, Int32 viewPortHeight, Int32 tileSize, Int32 imageCompression, Int32 DzcOutputFlag, string Owner)
        {
            string sScratchDir = path + @"\Scratch\" + Owner + @"\";
            System.IO.Directory.CreateDirectory(sScratchDir);

            RectangleF rcfAll = RectangleF.Empty;
            foreach (SeadragonImage sdImg in images)
            {
                RectangleF rcfImgCur = new RectangleF(
                    sdImg.itemRect.left, sdImg.itemRect.top,
                    sdImg.itemRect.right - sdImg.itemRect.left,
                    sdImg.itemRect.bottom - sdImg.itemRect.top
                    );

                rcfAll = RectangleF.Union(rcfAll, rcfImgCur);
            }

            Sdi2Coll.Sdi2Coll.SdiImage[] rgSdiCollImages = new Sdi2Coll.Sdi2Coll.SdiImage[images.Count];
            int iSdImg = 0;

            foreach (SeadragonImage sdImg in images)
            {
                string sImageSourcePath = sdImg.imagePath;
                string sSdiScratchFolder = sScratchDir +
                    System.IO.Path.GetFileNameWithoutExtension(sImageSourcePath)
                    + ".xml";

#if OUTPUT_BETA_1
                //->del kazumichi 20080704
                //        // Converting Logic for previous version of DeepZoom
                //        Img2Sdi.Img2Sdi.Image2Sdi_Beta1(sImageSourcePath, sSdiScratchFolder);
                //<-del kazumichi 20080704
#endif //OUTPUT_BETA_1
                Img2Sdi.Img2Sdi.Image2Sdi_Beta2(sImageSourcePath, sSdiScratchFolder);

                float flUnit = rcfAll.Width;
                RectangleF rcfImgCurNormalized = new RectangleF(
                    sdImg.itemRect.left / flUnit,
                    sdImg.itemRect.top / flUnit,
                    (sdImg.itemRect.right - sdImg.itemRect.left) / flUnit,
                    (sdImg.itemRect.bottom - sdImg.itemRect.top) / flUnit
                    );

                Sdi2Coll.Sdi2Coll.SdiImage sdiImage = new Sdi2Coll.Sdi2Coll.SdiImage(sSdiScratchFolder, rcfImgCurNormalized);
                rgSdiCollImages[iSdImg++] = sdiImage;
            }

            Sdi2Coll.Sdi2Coll.WriteCollection(rgSdiCollImages, path, DzcOutputFlag);
            System.IO.Directory.Delete(sScratchDir, true);
        }
    }
}
