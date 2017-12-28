using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.IO;
using System.Xml;


namespace Sdi2Coll
{
    /// <summary>
    /// Sdi2Coll Class
    /// </summary>
    public class Sdi2Coll
    {
        /// <summary>
        /// SdiImage Class
        /// </summary>
        public class SdiImage
        {
            /// <summary>
            /// _sSdiPath
            /// </summary>
            public string _sSdiPath;
            internal RectangleF _rcfBounds;

            private SdiImage()
            { }

            /// <summary>
            /// Initializes a new instance of the <see cref="SdiImage"/> class.
            /// </summary>
            /// <param name="sSdiFolderPath">The s SDI folder path.</param>
            /// <param name="rcfBounds">The RCF bounds.</param>
            public SdiImage(string sSdiFolderPath, RectangleF rcfBounds)
            {
                _sSdiPath = sSdiFolderPath;
                _rcfBounds = rcfBounds;
            }
        }

        /// <summary>
        /// SdiCollectionImage Class
        /// </summary>
        public class SdiCollectionImage : SdiImage
        {
            Point _ptMorton;
            /// <summary>
            /// _nMorton
            /// </summary>
            public int _nMorton;
            internal PointF _ptfThumbSize;
            internal PointF _ptActualSize;

            /// <summary>
            /// Initializes a new instance of the <see cref="SdiCollectionImage"/> class.
            /// </summary>
            /// <param name="sdiImage">The SDI image.</param>
            /// <param name="nMorton">The n morton.</param>
            public SdiCollectionImage(SdiImage sdiImage, int nMorton)
                : this(sdiImage._sSdiPath, sdiImage._rcfBounds, nMorton)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SdiCollectionImage"/> class.
            /// </summary>
            /// <param name="sSdiFolderPath">The s SDI folder path.</param>
            /// <param name="rcfBounds">The RCF bounds.</param>
            /// <param name="nMorton">The n morton.</param>
            internal SdiCollectionImage(string sSdiFolderPath, RectangleF rcfBounds, int nMorton)
                : base(sSdiFolderPath, rcfBounds)
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
        /// <param name="DzcOutputFlag">The DZC output flag.</param>
        public static void WriteCollection(SdiImage[] rgSdiImages, string sCollOutputFolder, Int32 DzcOutputFlag)
        {
            SdiCollectionImage[] rgSdiCollImages = new SdiCollectionImage[rgSdiImages.Length];
            int iSdiImg = 0;
            foreach (SdiImage sdiImage in rgSdiImages)
            {
                rgSdiCollImages[iSdiImg] = new SdiCollectionImage(sdiImage, iSdiImg);
                iSdiImg++;
            }

#if OUTPUT_BETA_1
            WriteCollection_Beta1(rgSdiCollImages, sCollOutputFolder);
#endif //OUTPUT_BETA_1
#if OUTPUT_BETA_2
            WriteCollection_Beta2(rgSdiCollImages, sCollOutputFolder + @"\collection.xml", DzcOutputFlag);
#endif //OUTPUT_BETA_2
        }

#if OUTPUT_BETA_1
        public static void WriteCollection_Beta1(SdiCollectionImage[] rgSdiCollImages, string sCollOutputFolder)
        {
            Directory.CreateDirectory(sCollOutputFolder);

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

                                    string sSdiThumbnailPath = sdImgCur.GetThumbnail_Beta1(iLevel);
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

                        string sDestDir = sCollOutputFolder + @"/thumbs/"
                            + iLevel.ToString() + "/"
                            + xPage.ToString() + "_" + yPage.ToString()
                            + "/" + nCollPageSizeLog2.ToString();

                        g.Flush();
                        Directory.CreateDirectory(sDestDir);
                        s_bmpScratch.Save(sDestDir + "/0_0.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
            }

            WriteCollectionMetadata(rgSdiCollImages, sCollOutputFolder);
            WriteCollectionLayoutMetadata(rgSdiCollImages, sCollOutputFolder);

            string sImagesFolder = sCollOutputFolder + @"\images";
            CopyImages_Beta1(rgSdiCollImages, sImagesFolder);
        }

        internal static void WriteCollectionMetadata(SdiCollectionImage[] rgSdiCollImages, string sCollOutputFolder)
        {
            ItemsBinReadWrite.ItemsBin items = new ItemsBinReadWrite.ItemsBin();

            ItemsBinReadWrite.InfoBinHeader header = new ItemsBinReadWrite.InfoBinHeader();
            header._collFormat = 0x2;
            header._bUseStringsFile = 0;
            header._uThumbnailMinLevel = 0;
            header._dThumbnailPageCompressionQuality = 1.0;
            header._nItems = (uint)rgSdiCollImages.Length;
            header._uItemIdNext = (uint)rgSdiCollImages.Length;
            header._uThumbnailMaxLevel = 8;
            header._uThumbnailPageSizeLog2 = 8;
            header._uFormat = 1;

            items._header = header;
            items._rgItemRecords = new ItemsBinReadWrite.ItemData[rgSdiCollImages.Length];

            for (int iSdi = 0; iSdi < rgSdiCollImages.Length; iSdi++)
            {
                ItemsBinReadWrite.ItemRecord itemRecord = new ItemsBinReadWrite.ItemRecord();
                itemRecord.SetPath("images/"
                    + Path.GetFileNameWithoutExtension(rgSdiCollImages[iSdi]._sSdiPath)
                    + ".sdi");
                itemRecord._uItemId = itemRecord._uThumbnailId = (uint)iSdi;

                ItemsBinReadWrite.ThumbRecord thumbRecord = new ItemsBinReadWrite.ThumbRecord();
                thumbRecord._flSizeX = rgSdiCollImages[iSdi]._ptfThumbSize.X;
                thumbRecord._flSizeY = rgSdiCollImages[iSdi]._ptfThumbSize.Y;
                thumbRecord._uMinLevel = 0;
                thumbRecord._uMaxLevel = 8;
                thumbRecord._uThumbnailId = (uint)iSdi;

                ItemsBinReadWrite.ItemData itemData = new ItemsBinReadWrite.ItemData();
                itemData._itemRecord = itemRecord;
                itemData._thumbRecord = thumbRecord;
                items._rgItemRecords[iSdi] = itemData;
            }

            items.Save(sCollOutputFolder + @"\items.bin");
        }

        internal static void WriteCollectionLayoutMetadata(SdiCollectionImage[] rgSdiCollImages, string sCollOutputFolder)
        {
            using (System.IO.FileStream fs = new FileStream(sCollOutputFolder + @"\layout.bin", FileMode.Create))
            {
                ItemsBinReadWrite.ItemsBinReadWrite.WriteUInt32((uint)rgSdiCollImages.Length, fs, 0, 0);

                uint iSdiImgCur = 0;

                foreach (SdiCollectionImage sdiCollImg in rgSdiCollImages)
                {
                    float flViewportWidth = 1.0f / sdiCollImg._rcfBounds.Width;
                    float flViewportOriginX = -sdiCollImg._rcfBounds.X * flViewportWidth;
                    float flViewportOriginY = -sdiCollImg._rcfBounds.Y * flViewportWidth;

                    ItemsBinReadWrite.ItemsBinReadWrite.WriteUInt32(iSdiImgCur, fs, 0, 0);
                    ItemsBinReadWrite.ItemsBinReadWrite.WriteFloat(flViewportOriginX, fs, 0, 0);
                    ItemsBinReadWrite.ItemsBinReadWrite.WriteFloat(flViewportOriginY, fs, 0, 0);
                    ItemsBinReadWrite.ItemsBinReadWrite.WriteFloat(flViewportWidth, fs, 0, 0);

                    iSdiImgCur++;
                }
            }
        }

        private static void CopyImages_Beta1(SdiCollectionImage[] rgSdiCollImages, string sImagesFolder)
        {
            System.IO.Directory.CreateDirectory(sImagesFolder);
            foreach (SdiCollectionImage sdiCollImage in rgSdiCollImages)
            {
                string sImageFolderSrc = Path.GetDirectoryName(sdiCollImage._sSdiPath)
                  + @"\" + Path.GetFileNameWithoutExtension(sdiCollImage._sSdiPath);

                string sImageFolderDest = sImagesFolder + @"\"
                  + Path.GetFileNameWithoutExtension(sImageFolderSrc);

                CopyDirectory(sImageFolderSrc, sImageFolderDest);
            }
        }
#endif //OUTPUT_BETA_1

        /// <summary>
        /// Writes the collection thumbnails only.
        /// </summary>
        /// <param name="rgSdiCollImages">The rg SDI coll images.</param>
        /// <param name="sCollImagesFolderRel">The s coll images folder rel.</param>
        /// <param name="sCollMetadataXmlPath">The s coll metadata XML path.</param>
        public static void WriteCollectionThumbnailsOnly(SdiCollectionImage[] rgSdiCollImages,
            string sCollImagesFolderRel,
            string sCollMetadataXmlPath)
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
                                            sdImgCur._ptActualSize = new PointF((float)img.Width, (float)img.Height);
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

            WriteCollectionXml(rgSdiCollImages, sCollMetadataXmlPath, sCollImagesFolderRel);
        }

        /// <summary>
        /// Writes the collection_ beta2.
        /// </summary>
        /// <param name="rgSdiCollImages">The rg SDI coll images.</param>
        /// <param name="sCollMetadataXmlPath">The s coll metadata XML path.</param>
        /// <param name="DzcOutputFlag">The DZC output flag.</param>
        public static void WriteCollection_Beta2(SdiCollectionImage[] rgSdiCollImages,
            string sCollMetadataXmlPath, Int32 DzcOutputFlag)
        {
            string sCollImagesFolderRel = Path.GetFileNameWithoutExtension(sCollMetadataXmlPath)
              + "_images";

            if (DzcOutputFlag == 1)
            {
                WriteCollectionThumbnailsOnly(rgSdiCollImages, sCollImagesFolderRel, sCollMetadataXmlPath);
            }

            string sImagesFolder = Path.GetDirectoryName(sCollMetadataXmlPath)
                + @"\" + sCollImagesFolderRel;
            CopyImages_Beta2(rgSdiCollImages, sImagesFolder);
        }

        /// <summary>
        /// Copies the images_ beta2.
        /// </summary>
        /// <param name="rgSdiCollImages">The rg SDI coll images.</param>
        /// <param name="sImagesFolder">The s images folder.</param>
        private static void CopyImages_Beta2(SdiCollectionImage[] rgSdiCollImages, string sImagesFolder)
        {
            System.IO.Directory.CreateDirectory(sImagesFolder);
            foreach (SdiCollectionImage sdiCollImage in rgSdiCollImages)
            {
                string sImageFolderSrc = Path.GetDirectoryName(sdiCollImage._sSdiPath)
                      + @"\" + Path.GetFileNameWithoutExtension(sdiCollImage._sSdiPath)
                      + "_files";

                File.Copy(sdiCollImage._sSdiPath,
                  sImagesFolder + @"\" + Path.GetFileNameWithoutExtension(sdiCollImage._sSdiPath) + ".xml",
                  true);

                string sImageFolderDest = sImagesFolder + @"\"
                  + Path.GetFileNameWithoutExtension(sImageFolderSrc);

                CopyDirectory(sImageFolderSrc, sImageFolderDest);
            }
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dest">The dest.</param>
        private static void CopyDirectory(string source, string dest)
        {
            Directory.CreateDirectory(dest);

            string[] rgFiles = Directory.GetFiles(source);
            foreach (string sFileName in rgFiles)
                File.Copy(sFileName, dest + @"\" + Path.GetFileName(sFileName), true);

            string[] rgDirectories = Directory.GetDirectories(source);
            foreach (string sDir in rgDirectories)
            {
                string sDirName = Path.GetFileName(sDir);
                CopyDirectory(source + @"\" + sDirName, dest + @"\" + sDirName);
            }
        }

        /// <summary>
        /// Writes the collection XML.
        /// </summary>
        /// <param name="rgSdiCollImages">The rg SDI coll images.</param>
        /// <param name="sCollXmlPath">The s coll XML path.</param>
        /// <param name="sCollImagesFolder">The s coll images folder.</param>
        internal static void WriteCollectionXml(
            SdiCollectionImage[] rgSdiCollImages,
            string sCollXmlPath,
            string sCollImagesFolder)
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
                eltI.SetAttribute("Source", sCollImagesFolder + "/" + Path.GetFileNameWithoutExtension(collItem._sSdiPath) + ".xml"
                    );
                eltI.SetAttribute("Id", iItem.ToString());
                iItem++;
                eltI.SetAttribute("N", collItem._nMorton.ToString());
                eltI.SetAttribute("Type", "ImagePixelSource");

                XmlElement eltSize = doc.CreateElement("Size");
                eltSize.SetAttribute("Width",
                    //((int)(nMaxThumbSize * collItem._ptfThumbSize.X)).ToString());
                    collItem._ptActualSize.X.ToString());
                eltSize.SetAttribute("Height",
                    //((int)(nMaxThumbSize * collItem._ptfThumbSize.Y)).ToString());
                    collItem._ptActualSize.Y.ToString());

                float flViewportWidth = 1.0f / collItem._rcfBounds.Width;
                float flViewportOriginX = -collItem._rcfBounds.X * flViewportWidth;
                float flViewportOriginY = -collItem._rcfBounds.Y * flViewportWidth;

                XmlElement eltViewport = doc.CreateElement("Viewport");
                eltViewport.SetAttribute("X", flViewportOriginX.ToString());
                eltViewport.SetAttribute("Y", flViewportOriginY.ToString());
                eltViewport.SetAttribute("Width", flViewportWidth.ToString());

                eltI.AppendChild(eltSize);
                eltI.AppendChild(eltViewport);

                eltItems.AppendChild(eltI);
            }

            eltColl.AppendChild(eltItems);
            doc.AppendChild(eltColl);

            doc.Save(sCollXmlPath);
        }

        /// <summary>
        /// Collection Incremental Build Class
        /// </summary>
        public class CollectionIncrementalBuild
        {
            /// <summary>
            /// Gets the collection files folder.
            /// </summary>
            /// <param name="sCollMetadataXmlPath">The s coll metadata XML path.</param>
            /// <returns></returns>
            private static string GetCollectionFilesFolder(string sCollMetadataXmlPath)
            {
                return Path.GetDirectoryName(sCollMetadataXmlPath) +
                    @"\" + Path.GetFileNameWithoutExtension(sCollMetadataXmlPath) +
                    "_files";
            }

            /// <summary>
            /// Adds the image.
            /// </summary>
            /// <param name="sdiCollImage">The SDI coll image.</param>
            /// <param name="nItemMortonNumber">The n item morton number.</param>
            /// <param name="sImageXmlRelativeLocation">The s image XML relative location.</param>
            /// <param name="sCollMetadataXmlPath">The s coll metadata XML path.</param>
            public static void AddImage(
                SdiCollectionImage sdiCollImage,
                int nItemMortonNumber,
                string sImageXmlRelativeLocation,
                string sCollMetadataXmlPath
                )
            {
                AddImageThumbnails(sdiCollImage, nItemMortonNumber, sCollMetadataXmlPath);
                AddImageXmlRecord(sdiCollImage, nItemMortonNumber, sImageXmlRelativeLocation, sCollMetadataXmlPath);
            }

            /// <summary>
            /// Adds the image thumbnails.
            /// </summary>
            /// <param name="sdiCollImage">The SDI coll image.</param>
            /// <param name="nItemMortonNumber">The n item morton number.</param>
            /// <param name="sCollMetadataXmlPath">The s coll metadata XML path.</param>
            internal static void AddImageThumbnails(
                SdiCollectionImage sdiCollImage,
                int nItemMortonNumber,
                string sCollMetadataXmlPath
                )
            {
                string sCollThumbsFolder = GetCollectionFilesFolder(sCollMetadataXmlPath);
                if (!Directory.Exists(sCollThumbsFolder))
                    Directory.CreateDirectory(sCollThumbsFolder);

                int xMortonCur = -1;
                int yMortonCur = -1;
                MortonToXY(nItemMortonNumber, out xMortonCur, out yMortonCur);

                for (int iLevel = 0; iLevel <= nMaxLevel; iLevel++)
                {
                    int nCollThumbsPerPageDim = 0x1 << (nCollPageSizeLog2 - iLevel);
                    int nThumbSize = 0x1 << iLevel;

                    int xPage = xMortonCur / nCollThumbsPerPageDim;
                    int yPage = yMortonCur / nCollThumbsPerPageDim;

                    int xPageThumb = xMortonCur % nCollThumbsPerPageDim;
                    int yPageThumb = yMortonCur % nCollThumbsPerPageDim;

                    string sThumbPagePath = sCollThumbsFolder + @"/"
                        + iLevel.ToString() + "/"
                        + xPage.ToString() + "_" + yPage.ToString() + ".jpg";

                    Bitmap bmpThumbPage = new Bitmap(nCollPageSize, nCollPageSize);
                    Graphics g = Graphics.FromImage(bmpThumbPage);
                    g.Clear(Color.Black);

                    if (File.Exists(sThumbPagePath))
                    {
                        Bitmap bmpThumbPageExisting = new Bitmap(sThumbPagePath);
                        g.DrawImage(bmpThumbPageExisting, new Point(0, 0));
                        bmpThumbPageExisting.Dispose();
                    }

                    string sSdiThumbnailPath = sdiCollImage.GetThumbnail_Beta2(iLevel);
                    if (!System.IO.File.Exists(sSdiThumbnailPath))
                        continue;

                    using (System.IO.Stream stream = new System.IO.FileStream(sSdiThumbnailPath, FileMode.Open))
                    {
                        using (Image img = Bitmap.FromStream(stream))
                        {
                            g.DrawImage(img, new Point(xPageThumb * nThumbSize, yPageThumb * nThumbSize));
                            sdiCollImage._ptfThumbSize = new PointF(img.Width / (float)nThumbSize, img.Height / (float)nThumbSize);
                            sdiCollImage._ptActualSize = new PointF((float)img.Width, (float)img.Height);
                        }
                    }

                    g.Dispose();

                    string sThumbPageDir = Path.GetDirectoryName(sThumbPagePath);
                    if (!Directory.Exists(sThumbPageDir))
                        Directory.CreateDirectory(sThumbPageDir);

                    if (File.Exists(sThumbPagePath))
                        File.Delete(sThumbPagePath);

                    bmpThumbPage.Save(sThumbPagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bmpThumbPage.Dispose();
                }
            }

            /// <summary>
            /// Ensures the collection XML.
            /// </summary>
            /// <param name="sCollMetadataXmlPath">The s coll metadata XML path.</param>
            internal static void EnsureCollectionXml(string sCollMetadataXmlPath)
            {
                if (File.Exists(sCollMetadataXmlPath))
                    return;

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

                XmlElement eltColl = doc.CreateElement("Collection");
                eltColl.SetAttribute("MaxLevel", nMaxLevel.ToString());
                eltColl.SetAttribute("TileSize", nCollPageSize.ToString());
                eltColl.SetAttribute("Format", "jpg");
                eltColl.SetAttribute("Quality", "90");
                eltColl.SetAttribute("NextItemId", "0");

                XmlElement eltItems = doc.CreateElement("Items");

                eltColl.AppendChild(eltItems);
                doc.AppendChild(eltColl);

                doc.Save(sCollMetadataXmlPath);
            }

            /// <summary>
            /// Adds the image XML record.
            /// </summary>
            /// <param name="sdiCollImage">The SDI coll image.</param>
            /// <param name="nItemMortonNumber">The n item morton number.</param>
            /// <param name="sImageXmlRelativeLocation">The s image XML relative location.</param>
            /// <param name="sCollMetadataXmlPath">The s coll metadata XML path.</param>
            internal static void AddImageXmlRecord(
                SdiCollectionImage sdiCollImage,
                int nItemMortonNumber,
                string sImageXmlRelativeLocation,
                string sCollMetadataXmlPath
                )
            {
                EnsureCollectionXml(sCollMetadataXmlPath);
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(sCollMetadataXmlPath);

                XmlElement eltColl = doc.SelectSingleNode("Collection") as XmlElement;
                int nNextItemIdCur = Int32.Parse(eltColl.GetAttribute("NextItemId"));
                eltColl.SetAttribute("NextItemId", Math.Max(nNextItemIdCur, nItemMortonNumber + 1).ToString());

                XmlElement eltItems = doc.SelectSingleNode("Collection/Items") as XmlElement;
                XmlNode ndIExisting = eltItems.SelectSingleNode("I[@N='" + nItemMortonNumber.ToString() + "']");
                if (null != ndIExisting)
                    eltItems.RemoveChild(ndIExisting);

                XmlElement eltI = doc.CreateElement("I");
                eltI.SetAttribute("Source", sImageXmlRelativeLocation);
                eltI.SetAttribute("Id", nItemMortonNumber.ToString());
                eltI.SetAttribute("N", nItemMortonNumber.ToString());
                eltI.SetAttribute("Type", "ImagePixelSource");

                XmlElement eltSize = doc.CreateElement("Size");
                eltSize.SetAttribute("Width",
                    //((int)(nCollPageSize * sdiCollImage._ptfThumbSize.X)).ToString());
                    sdiCollImage._ptActualSize.X.ToString());
                eltSize.SetAttribute("Height",
                    //((int)(nCollPageSize * sdiCollImage._ptfThumbSize.Y)).ToString());
                    sdiCollImage._ptActualSize.Y.ToString());

                float flViewportWidth = 1.0f / sdiCollImage._rcfBounds.Width;
                float flViewportOriginX = -sdiCollImage._rcfBounds.X * flViewportWidth;
                float flViewportOriginY = -sdiCollImage._rcfBounds.Y * flViewportWidth;

                XmlElement eltViewport = doc.CreateElement("Viewport");
                eltViewport.SetAttribute("X", flViewportOriginX.ToString());
                eltViewport.SetAttribute("Y", flViewportOriginY.ToString());
                eltViewport.SetAttribute("Width", flViewportWidth.ToString());

                eltI.AppendChild(eltSize);
                eltI.AppendChild(eltViewport);

                eltItems.AppendChild(eltI);

                doc.Save(sCollMetadataXmlPath);
            }
        }
    }
}
