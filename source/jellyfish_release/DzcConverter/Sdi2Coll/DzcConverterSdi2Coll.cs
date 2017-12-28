using System;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Globalization;

namespace Sdi2Coll
{
    /// <summary>
    /// Sdi2Coll Class
    /// </summary>
    public class Sdi2Coll
    {
        /// <summary>
        /// SdiImage
        /// </summary>
        public class SdiImage
        {
            /// <summary>
            /// _sSdiPath
            /// </summary>
            public string _sSdiPath;
            internal RectangleF _rcfBounds;
            internal Point _ptActualSize;

            private SdiImage()
            { }

            /// <summary>
            /// Initializes a new instance of the <see cref="SdiImage"/> class.
            /// </summary>
            /// <param name="sSdiFolderPath">The s SDI folder path.</param>
            /// <param name="rcfBounds">The RCF bounds.</param>
            public SdiImage(string sSdiFolderPath, RectangleF rcfBounds, Point actualSize)
            {
                _sSdiPath = sSdiFolderPath;
                _rcfBounds = rcfBounds;
                _ptActualSize = actualSize;
            }
        }

        /// <summary>
        /// SdiCollectionImage
        /// </summary>
        public class SdiCollectionImage : SdiImage
        {
            Point _ptMorton;
            /// <summary>
            /// _nMorton
            /// </summary>
            public int _nMorton;
            internal PointF _ptfThumbSize;
            

            /// <summary>
            /// Initializes a new instance of the <see cref="SdiCollectionImage"/> class.
            /// </summary>
            /// <param name="sdiImage">The SDI image.</param>
            /// <param name="nMorton">The n morton.</param>
            public SdiCollectionImage(SdiImage sdiImage, int nMorton)
                : this(sdiImage._sSdiPath, sdiImage._rcfBounds, sdiImage._ptActualSize, nMorton)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SdiCollectionImage"/> class.
            /// </summary>
            /// <param name="sSdiFolderPath">The s SDI folder path.</param>
            /// <param name="rcfBounds">The RCF bounds.</param>
            /// <param name="nMorton">The n morton.</param>
            internal SdiCollectionImage(string sSdiFolderPath, RectangleF rcfBounds, Point actualSize, int nMorton)
                : base(sSdiFolderPath, rcfBounds, actualSize)
            {
                _rcfBounds = rcfBounds;

                _nMorton = nMorton;
                int xMorton = -1;
                int yMorton = -1;
                MortonToXY(nMorton, out xMorton, out yMorton);
                _ptMorton = new Point(xMorton, yMorton);
            }

            /// <summary>
            /// Gets the thumbnail_ beta1.
            /// </summary>
            /// <param name="iLevel">The i level.</param>
            /// <returns></returns>
            internal string GetThumbnail_Beta1(int iLevel)
            {
                return Path.GetDirectoryName(_sSdiPath)
                    + "/" + Path.GetFileNameWithoutExtension(_sSdiPath)
                    + "/" + iLevel.ToString() + "/0_0.jpg";
            }

            /// <summary>
            /// Gets the thumbnail_ beta2.
            /// </summary>
            /// <param name="iLevel">The i level.</param>
            /// <returns></returns>
            internal string GetThumbnail_Beta2(int iLevel)
            {
                return Path.GetDirectoryName(_sSdiPath)
                    + "/" + Path.GetFileNameWithoutExtension(_sSdiPath)
                    + "_files" + "/" + iLevel.ToString() + "/0_0.jpg";
            }
        }

        // *********************************************
        // Set the level of collection_files.
        // *********************************************
        const int nMaxLevel = 8;
        const int nMaxThumbSize = 0x1 << nMaxLevel;

        const int nCollPageSizeLog2 = 8;
        const int nCollPageSize = 0x1 << nCollPageSizeLog2;
        static Bitmap s_bmpScratch = new Bitmap(nCollPageSize, nCollPageSize);

        /// <summary>
        /// Pages the index from XY.
        /// </summary>
        /// <param name="xMorton">The x morton.</param>
        /// <param name="yMorton">The y morton.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        internal static Point PageIndexFromXY(int xMorton, int yMorton, int level)
        {
            int nItemsPerPageDim = 0x1 << (nCollPageSizeLog2 - level);
            return new Point(xMorton / nItemsPerPageDim, yMorton / nItemsPerPageDim);
        }

        /// <summary>
        /// Pages the position from XY.
        /// </summary>
        /// <param name="xMorton">The x morton.</param>
        /// <param name="yMorton">The y morton.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        internal static Point PagePositionFromXY(int xMorton, int yMorton, int level)
        {
            int nItemsPerPageDim = 0x1 << (nCollPageSizeLog2 - level);
            return new Point(xMorton % nItemsPerPageDim, yMorton % nItemsPerPageDim);
        }

        /// <summary>
        /// XYs to morton.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="uMorton">The u morton.</param>
        internal static void XYToMorton(int x, int y, out int uMorton)
        {
            const int BITS_PER_BYTE = 8;
            const int BIT_PAIRS = sizeof(int) * BITS_PER_BYTE / 2;

            uMorton = 0;
            for (int i = 0; i < BIT_PAIRS; i++)
            {
                uMorton |= (x & 1) << (i * 2);
                uMorton |= (y & 1) << (i * 2 + 1);
                x >>= 1;
                y >>= 1;
            }
        }

        /// <summary>
        /// Mortons to XY.
        /// </summary>
        /// <param name="uMorton">The u morton.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        internal static void MortonToXY(int uMorton, out int x, out int y)
        {
            const int BITS_PER_BYTE = 8;
            const int BIT_PAIRS = sizeof(int) * BITS_PER_BYTE / 2;

            x = y = 0;
            for (int i = 0; i < BIT_PAIRS; i++)
            {
                x |= ((uMorton & 1) >> 0) << i;
                y |= ((uMorton & 2) >> 1) << i;
                uMorton >>= 2;
            }
        }

        /// <summary>
        /// Writes the collection.
        /// </summary>
        /// <param name="rgSdiImages">The rg SDI images.</param>
        /// <param name="sCollOutputFolder">The s coll output folder.</param>
        /// <param name="collectionXmlFile">The collection XML file.</param>
        /// <param name="collectionImagesParentDirPath">The collection images parent dir path.</param>
        public static void WriteCollection(SdiImage[] rgSdiImages, string sCollOutputFolder, string collectionXmlFile, string collectionImagesParentDirPath)
        {
            SdiCollectionImage[] rgSdiCollImages = new SdiCollectionImage[rgSdiImages.Length];
            int iSdiImg = 0;
            foreach (SdiImage sdiImage in rgSdiImages)
            {
                rgSdiCollImages[iSdiImg] = new SdiCollectionImage(sdiImage, iSdiImg);
                iSdiImg++;
            }

            WriteCollection_Beta2(rgSdiCollImages, sCollOutputFolder + @"\" + collectionXmlFile, collectionImagesParentDirPath);
        }


        /// <summary>
        /// Write collection_files and collection.xml
        /// </summary>
        /// <param name="rgSdiCollImages"></param>
        /// <param name="sCollMetadataXmlPath"></param>
        /// <param name="collectionImagesParentDirPath"></param>
        public static void WriteCollection_Beta2(SdiCollectionImage[] rgSdiCollImages,
            string sCollMetadataXmlPath, string collectionImagesParentDirPath)
        {
            string sCollImagesFolderRel = Path.GetFileNameWithoutExtension(sCollMetadataXmlPath)
              + "_images";

            WriteCollectionThumbnailsOnly(rgSdiCollImages, sCollImagesFolderRel, sCollMetadataXmlPath, collectionImagesParentDirPath);
        }


        /// <summary>
        /// Writes the collection thumbnails only.
        /// </summary>
        /// <param name="rgSdiCollImages">The rg SDI coll images.</param>
        /// <param name="sCollImagesFolderRel">The s coll images folder rel.</param>
        /// <param name="sCollMetadataXmlPath">The s coll metadata XML path.</param>
        /// <param name="collectionImagesParentDirPath">The collection images parent dir path.</param>
        public static void WriteCollectionThumbnailsOnly(SdiCollectionImage[] rgSdiCollImages,
                                                            string sCollImagesFolderRel,
                                                            string sCollMetadataXmlPath, string collectionImagesParentDirPath)
        {
            string sCollThumbsFolder = Path.GetDirectoryName(sCollMetadataXmlPath) +
                @"\" + Path.GetFileNameWithoutExtension(sCollMetadataXmlPath) +
                "_files";

            Directory.CreateDirectory(sCollThumbsFolder);

            int xMortonMax = -1;
            int yMortonMax = -1;
            for (int iMorton = 0; iMorton < rgSdiCollImages.Length; iMorton++)
            {
                int xMortonCur = -1;
                int yMortonCur = -1;
                MortonToXY(iMorton, out xMortonCur, out yMortonCur);

                xMortonMax = Math.Max(xMortonMax, xMortonCur);
                yMortonMax = Math.Max(yMortonMax, yMortonCur);
            }

            Graphics g = Graphics.FromImage(s_bmpScratch);
            for (int iLevel = 0; iLevel <= nMaxLevel; iLevel++)
            {
                Point ptPageMax = PageIndexFromXY(xMortonMax, yMortonMax, iLevel);
                int nCollThumbsPerPageDim = 0x1 << (nCollPageSizeLog2 - iLevel);
                int nThumbSize = 0x1 << iLevel;

                for (int xPage = 0; xPage <= ptPageMax.X; xPage++)
                    for (int yPage = 0; yPage <= ptPageMax.Y; yPage++)
                    {
                        g.Clear(Color.Black);

                        for (int xPageThumb = 0; xPageThumb < nCollThumbsPerPageDim; xPageThumb++)
                            for (int yPageThumb = 0; yPageThumb < nCollThumbsPerPageDim; yPageThumb++)
                            {
                                int xMortonCur = xPage * nCollThumbsPerPageDim + xPageThumb;
                                int yMortonCur = yPage * nCollThumbsPerPageDim + yPageThumb;

                                int nMortonCur = -1;
                                XYToMorton(xMortonCur, yMortonCur, out nMortonCur);

                                if (nMortonCur < rgSdiCollImages.Length)
                                {
                                    SdiCollectionImage sdImgCur = rgSdiCollImages[nMortonCur];

                                    string sSdiThumbnailPath = sdImgCur.GetThumbnail_Beta2(iLevel);
                                    if (!System.IO.File.Exists(sSdiThumbnailPath))
                                        continue;

                                    using (System.IO.Stream stream = new System.IO.FileStream(sSdiThumbnailPath, FileMode.Open))
                                    {
                                        using (Image img = Bitmap.FromStream(stream))
                                        {
                                            g.DrawImage(img, new Point(xPageThumb * nThumbSize, yPageThumb * nThumbSize));
                                            sdImgCur._ptfThumbSize = new PointF(img.Width / (float)nThumbSize, img.Height / (float)nThumbSize);
                                        }
                                    }
                                }
                            }

                        string sThumbPagePath = sCollThumbsFolder + @"/"
                            + iLevel.ToString() + "/"
                            + xPage.ToString() + "_" + yPage.ToString() + ".jpg";

                        g.Flush();
                        Directory.CreateDirectory(Path.GetDirectoryName(sThumbPagePath));
                        s_bmpScratch.Save(sThumbPagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
            }

            WriteCollectionXml(rgSdiCollImages, sCollMetadataXmlPath, sCollImagesFolderRel, collectionImagesParentDirPath);
        }

        /// <summary>
        /// Write collection.xml
        /// </summary>
        /// <param name="rgSdiCollImages"></param>
        /// <param name="sCollXmlPath"></param>
        /// <param name="sCollImagesFolder"></param>
        /// <param name="collectionImagesParentDirPath"></param>
        internal static void WriteCollectionXml(
            SdiCollectionImage[] rgSdiCollImages,
            string sCollXmlPath,
            string sCollImagesFolder,
            string collectionImagesParentDirPath)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

            XmlElement eltColl = doc.CreateElement("Collection");
            eltColl.SetAttribute("MaxLevel", nMaxLevel.ToString());
            eltColl.SetAttribute("TileSize", nCollPageSize.ToString());
            eltColl.SetAttribute("Format", "jpg");
            eltColl.SetAttribute("Quality", "90");
            eltColl.SetAttribute("NextItemId", rgSdiCollImages.Length.ToString());

            XmlElement eltItems = doc.CreateElement("Items");
            int iItem = 0;

            foreach (SdiCollectionImage collItem in rgSdiCollImages)
            {
                XmlElement eltI = doc.CreateElement("I");
                eltI.SetAttribute("Source", collectionImagesParentDirPath + sCollImagesFolder + "/" + Path.GetFileName(collItem._sSdiPath));
                eltI.SetAttribute("Id", iItem.ToString());
                iItem++;
                eltI.SetAttribute("N", collItem._nMorton.ToString());
                eltI.SetAttribute("Type", "ImagePixelSource");

                XmlElement eltSize = doc.CreateElement("Size");
                eltSize.SetAttribute("Width",
                    (collItem._ptActualSize.X).ToString());
                eltSize.SetAttribute("Height",
                    (collItem._ptActualSize.Y).ToString());
#if DEBUG
                System.Diagnostics.Trace.WriteLine(string.Format("eltSize: {0}", eltSize));
                
#endif

                float flViewportWidth = 1.0f / collItem._rcfBounds.Width;
                float flViewportOriginX = -collItem._rcfBounds.X * flViewportWidth;
                float flViewportOriginY = -collItem._rcfBounds.Y * flViewportWidth;

                XmlElement eltViewport = doc.CreateElement("Viewport");
                eltViewport.SetAttribute("X", flViewportOriginX.ToString(CultureInfo.InvariantCulture));
                eltViewport.SetAttribute("Y", flViewportOriginY.ToString(CultureInfo.InvariantCulture));
                eltViewport.SetAttribute("Width", flViewportWidth.ToString(CultureInfo.InvariantCulture));

                eltI.AppendChild(eltSize);
                eltI.AppendChild(eltViewport);

                eltItems.AppendChild(eltI);
            }

            eltColl.AppendChild(eltItems);
            doc.AppendChild(eltColl);

            doc.Save(sCollXmlPath);
        }

    }
}
