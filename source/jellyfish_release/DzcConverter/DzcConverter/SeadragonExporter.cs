using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;


namespace DzcConverter
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
        /// imageActualWidth
        /// </summary>
        public int imageActualWidth;
        /// <summary>
        /// imageActualHeight
        /// </summary>
        public int imageActualHeight;

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

            imageActualWidth = width;
            imageActualHeight = height;
        }
    }


    /// <summary>
    /// SeadragonExporter Class
    /// </summary>
    /// <remarks>
    /// For converting input images to SeadragonImages.
    /// </remarks>
    public class SeadragonExporter
    {
        /// <summary>
        /// Exports the specified DZC output path.
        /// </summary>
        /// <param name="DzcOutputPath">The DZC output path.</param>
        /// <param name="images">The images.</param>
        /// <param name="viewPortWidth">Width of the view port.</param>
        /// <param name="viewPortHeight">Height of the view port.</param>
        /// <param name="tileSize">Size of the tile.</param>
        /// <param name="imageCompression">The image compression.</param>
        /// <param name="collectionXmlFile">The collection XML file.</param>
        /// <param name="collectionImagesParentDirPath">The collection images parent dir path.</param>
        /// <param name="collectionImagesDirPath">The collection images dir path.</param>
        static public void Export(String DzcOutputPath, List<SeadragonImage> images, Int32 viewPortWidth, Int32 viewPortHeight, Int32 tileSize, Int32 imageCompression, string collectionXmlFile, string collectionImagesParentDirPath, string collectionImagesDirPath)
        {
            // ********************************************************
            // Create Scratch folder. it will be deleted when convertion is completed.
            // ********************************************************
            //string sScratchDir = DzcOutputPath + @"\Scratch\";
            //System.IO.Directory.CreateDirectory(sScratchDir);


            // ********************************************************
            // Create Rectangle object from SeadragonImage object.
            // ********************************************************
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


            // ********************************************************
            // Create Rectangle object from SeadragonImage object.
            // ********************************************************

            // Define the size of the collection array of DZI 
            Sdi2Coll.Sdi2Coll.SdiImage[] rgSdiCollImages = new Sdi2Coll.Sdi2Coll.SdiImage[images.Count];
            int iSdImg = 0;

            foreach (SeadragonImage sdImg in images)
            {
                string sImageSourcePath = sdImg.imagePath;
                //        string sSdiFolder = @"C:\dev\WebSites\jellyfish\sl\out\collection_images\" + System.IO.Path.GetFileNameWithoutExtension(sImageSourcePath) + ".xml";
                string sSdiFolder = collectionImagesDirPath + System.IO.Path.GetFileNameWithoutExtension(sImageSourcePath) + ".xml";

                // -------------------------------------------------------
                // Create DZI
                // -------------------------------------------------------
                //        Img2Sdi.Img2Sdi.Image2Sdi_Beta1(sImageSourcePath, sSdiScratchFolder);
                //        DzcConverterImg2Sdi.DzcConverterImg2Sdi.Image2Sdi_Beta2(sImageSourcePath, sSdiFolder);

                float flUnit = rcfAll.Width;
                RectangleF rcfImgCurNormalized = new RectangleF(
                    sdImg.itemRect.left / flUnit,
                    sdImg.itemRect.top / flUnit,
                    (sdImg.itemRect.right - sdImg.itemRect.left) / flUnit,
                    (sdImg.itemRect.bottom - sdImg.itemRect.top) / flUnit
                    );

                Point actualSize = new Point(sdImg.imageActualWidth, sdImg.imageActualHeight);

                Sdi2Coll.Sdi2Coll.SdiImage sdiImage = new Sdi2Coll.Sdi2Coll.SdiImage(sSdiFolder, rcfImgCurNormalized, actualSize);
                rgSdiCollImages[iSdImg++] = sdiImage;
            }

            // ********************************************************
            // Create DZC (collection.xml & collection_files)
            // ********************************************************
            Sdi2Coll.Sdi2Coll.WriteCollection(rgSdiCollImages, DzcOutputPath, collectionXmlFile, collectionImagesParentDirPath);
            //      System.IO.Directory.Delete(sScratchDir, true);
        }
    }
}
