using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace DzcConverterImg2Sdi
{

    /// <summary>
    /// HeaderBlock Class
    /// </summary>
    public class HeaderBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderBlock"/> class.
        /// </summary>
        public HeaderBlock()
            : this(1, 1)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderBlock"/> class.
        /// </summary>
        /// <param name="logicalWidth">Width of the logical.</param>
        /// <param name="logicalHeight">Height of the logical.</param>
        internal HeaderBlock(int logicalWidth, int logicalHeight)
        {
            width = (uint)logicalWidth;
            height = (uint)logicalHeight;

            numLevels = (ushort)(Math.Max(Math.Ceiling(Math.Log(width, 2)),
                Math.Ceiling(Math.Log(height, 2))) + 1);

            ulTilePresenceTableOffset = 0;
        }

        internal UInt32 signature = 845767795;
        internal UInt32 containerFormat = 1;
        internal UInt32 version = 0;

        internal UInt32 width;
        internal UInt32 height;

        internal UInt16 colorModel = 0; // 0: RGB24, 1:Gray8
        internal UInt16 numLevels;  // 0-31

        internal UInt32 tileAddressScheme = 0;  // L-XY
        internal float flBpp = 0;
        internal float flMeanSqrError = 0;

        internal UInt32 ulLevelTableOffset = 52; // sizeof(HeaderBlock) + 2 uint's
        internal UInt32 ulTilePresenceTableOffset = 0;
    }

    /// <summary>
    /// LevelBlock Class
    /// </summary>
    public class LevelBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelBlock"/> class.
        /// </summary>
        internal LevelBlock()
            : this(1, 1, 256)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelBlock"/> class.
        /// </summary>
        /// <param name="levelWidth">Width of the level.</param>
        /// <param name="levelHeight">Height of the level.</param>
        /// <param name="tileSize">Size of the tile.</param>
        internal LevelBlock(int levelWidth, int levelHeight, int tileSize)
        {
            width = (uint)levelWidth;
            height = (uint)levelHeight;

            sTileWidth = sTileHeight = (ushort)tileSize;
        }

//        internal UInt16 siLevel;

        internal UInt16 sLevelTileFormat = 1; // 0:tif, 1:jpg, 2:wdp, 3:png

        internal UInt32 width;
        internal UInt32 height;

        internal UInt16 sTileWidth = 256;
        internal UInt16 sTileHeight = 256;

        internal UInt16 sTileOverlapLeft = 0;
        internal UInt16 sTileOverlapTop = 0;
        internal UInt16 sTileOverlapRight = 0;
        internal UInt16 sTileOverlapBottom = 0;

        internal float flLevelOffsetWidth = 0;
        internal float flLevelOffsetHeight = 0;
    }


    /// <summary>
    /// DZC Converter Img2Sdi Class
    /// </summary>
    public class DzcConverterImg2Sdi
    {
        //->del kazumichi 20080704
        //public static void ReadInfoBin(string sInfoBinPath, out HeaderBlock header, out LevelBlock[] rgLevels)
        //{
        //  using (FileStream fs = new FileStream(sInfoBinPath, FileMode.Open))
        //  {
        //    HeaderBlock hb = new HeaderBlock();
        //    hb.signature = ItemsBinReadWrite.ReadUInt32(fs);
        //    hb.containerFormat = ItemsBinReadWrite.ReadUInt32(fs);
        //    hb.version = ItemsBinReadWrite.ReadUInt32(fs);

        //    hb.width = ItemsBinReadWrite.ReadUInt32(fs);
        //    hb.height = ItemsBinReadWrite.ReadUInt32(fs);

        //    hb.colorModel = ItemsBinReadWrite.ReadUInt16(fs); // 0: RGB24, 1:Gray8
        //    hb.numLevels = ItemsBinReadWrite.ReadUInt16(fs);  // 0-31

        //    hb.tileAddressScheme = ItemsBinReadWrite.ReadUInt32(fs);  // L-XY
        //    hb.flBpp = ItemsBinReadWrite.ReadFloat(fs);
        //    hb.flMeanSqrError = ItemsBinReadWrite.ReadFloat(fs);

        //    hb.ulLevelTableOffset = ItemsBinReadWrite.ReadUInt32(fs);
        //    hb.ulTilePresenceTableOffset = ItemsBinReadWrite.ReadUInt32(fs);

        //    header = hb;
        //    rgLevels = new LevelBlock[hb.numLevels];

        //    // ??
        //    uint cLevels = ItemsBinReadWrite.ReadUInt32(fs);
        //    uint cbLevelBlock = ItemsBinReadWrite.ReadUInt32(fs);

        //    for (int iLevel = 0; iLevel < hb.numLevels; iLevel++)
        //    {
        //      LevelBlock lb = new LevelBlock();
        //      lb.siLevel = ItemsBinReadWrite.ReadUInt16(fs);

        //      lb.sLevelTileFormat = ItemsBinReadWrite.ReadUInt16(fs); // 0:tif, 1:jpg, 2:wdp, 3:png

        //      lb.width = ItemsBinReadWrite.ReadUInt32(fs);
        //      lb.height = ItemsBinReadWrite.ReadUInt32(fs);

        //      lb.sTileWidth = ItemsBinReadWrite.ReadUInt16(fs);
        //      lb.sTileHeight = ItemsBinReadWrite.ReadUInt16(fs);

        //      lb.sTileOverlapLeft = ItemsBinReadWrite.ReadUInt16(fs);
        //      lb.sTileOverlapTop = ItemsBinReadWrite.ReadUInt16(fs);
        //      lb.sTileOverlapRight = ItemsBinReadWrite.ReadUInt16(fs);
        //      lb.sTileOverlapBottom = ItemsBinReadWrite.ReadUInt16(fs);

        //      lb.flLevelOffsetWidth = ItemsBinReadWrite.ReadFloat(fs);
        //      lb.flLevelOffsetHeight = ItemsBinReadWrite.ReadFloat(fs);

        //      rgLevels[iLevel] = lb;
        //    }
        //  }
        //}
        //<-del kazumichi 20080704
        //->del kazumichi 20080704
        //internal const uint cbHeaderBlock = 44;
        //internal const uint cbLevelBlock = 32;

        //public static void WriteInfoBin(string sInfoBinPath, HeaderBlock hb, LevelBlock[] rgLevels)
        //{
        //  using (FileStream fs = new FileStream(sInfoBinPath, FileMode.Create))
        //  {
        //    ItemsBinReadWrite.WriteUInt32(hb.signature, fs, 0, 0);
        //    ItemsBinReadWrite.WriteUInt32(hb.containerFormat, fs, 0, 0);
        //    ItemsBinReadWrite.WriteUInt32(hb.version, fs, 0, 0);

        //    ItemsBinReadWrite.WriteUInt32(hb.width, fs, 0, 0);
        //    ItemsBinReadWrite.WriteUInt32(hb.height, fs, 0, 0);

        //    ItemsBinReadWrite.WriteUInt16(hb.colorModel, fs, 0, 0); // 0: RGB24, 1:Gray8
        //    ItemsBinReadWrite.WriteUInt16(hb.numLevels, fs, 0, 0);

        //    ItemsBinReadWrite.WriteUInt32(hb.tileAddressScheme, fs, 0, 0);
        //    ItemsBinReadWrite.WriteFloat(hb.flBpp, fs, 0, 0);
        //    ItemsBinReadWrite.WriteFloat(hb.flMeanSqrError, fs, 0, 0);

        //    ItemsBinReadWrite.WriteUInt32(hb.ulLevelTableOffset, fs, 0, 0);
        //    ItemsBinReadWrite.WriteUInt32(hb.ulTilePresenceTableOffset, fs, 0, 0);

        //    ItemsBinReadWrite.WriteUInt32(hb.numLevels, fs, 0, 0);
        //    ItemsBinReadWrite.WriteUInt32(cbLevelBlock, fs, 0, 0);

        //    for (int iLevel = 0; iLevel < hb.numLevels; iLevel++)
        //    {
        //      LevelBlock lb = rgLevels[iLevel];
        //      ItemsBinReadWrite.WriteUInt16(lb.siLevel, fs, 0, 0);

        //      ItemsBinReadWrite.WriteUInt16(lb.sLevelTileFormat, fs, 0, 0);

        //      ItemsBinReadWrite.WriteUInt32((ushort)lb.width, fs, 0, 0);
        //      ItemsBinReadWrite.WriteUInt32((ushort)lb.height, fs, 0, 0);

        //      ItemsBinReadWrite.WriteUInt16(lb.sTileWidth, fs, 0, 0);
        //      ItemsBinReadWrite.WriteUInt16(lb.sTileHeight, fs, 0, 0);

        //      ItemsBinReadWrite.WriteUInt16(lb.sTileOverlapLeft, fs, 0, 0);
        //      ItemsBinReadWrite.WriteUInt16(lb.sTileOverlapTop, fs, 0, 0);

        //      ItemsBinReadWrite.WriteUInt16(lb.sTileOverlapRight, fs, 0, 0);
        //      ItemsBinReadWrite.WriteUInt16(lb.sTileOverlapBottom, fs, 0, 0);

        //      ItemsBinReadWrite.WriteFloat(lb.flLevelOffsetWidth, fs, 0, 0);
        //      ItemsBinReadWrite.WriteFloat(lb.flLevelOffsetHeight, fs, 0, 0);

        //      rgLevels[iLevel] = lb;
        //    }

        //  }
        //}
        //<-del kazumichi 20080704

        //->del kazumichi 20080704
        //    public static void Image2Sdi_Beta1(string sInputImage, string sSdiOutputPath)
        //    {
        //      string sSdiOutputFolder = Path.GetDirectoryName(sSdiOutputPath)
        //        + @"\" + Path.GetFileNameWithoutExtension(sSdiOutputPath);

        //      Directory.CreateDirectory(sSdiOutputFolder);
        //      using (Bitmap inputImage = new Bitmap(sInputImage))
        //      {
        //        HeaderBlock header = new HeaderBlock(inputImage.Width, inputImage.Height);
        //        LevelBlock[] rgLevels = new LevelBlock[header.numLevels];

        //        int cnTileSize = 256;
        //        rgLevels = new LevelBlock[header.numLevels];
        //        for (int iLevel = 0; iLevel < header.numLevels; iLevel++)
        //        {
        //          double dfLevelSampling = Math.Pow(2, header.numLevels - iLevel - 1);
        //          int nLevelWidth = (int)Math.Ceiling(header.width / dfLevelSampling);
        //          int nLevelHeight = (int)Math.Ceiling(header.height / dfLevelSampling);

        //          LevelBlock lb = new LevelBlock(nLevelWidth, nLevelHeight, cnTileSize);
        //          lb.siLevel = (ushort)iLevel;
        //          rgLevels[iLevel] = lb;
        //        }
        ////->del kazumichi 20080626
        ////        WriteInfoBin(sSdiOutputFolder + @"\info.bin", header, rgLevels);
        ////<-del kazumichi 20080626
        //        for (int iLevel = 0; iLevel < header.numLevels; iLevel++)
        //        {
        //          double dfLevelSampling = Math.Pow(2, header.numLevels - iLevel - 1);
        //          int nLevelWidth = (int)Math.Ceiling(header.width / dfLevelSampling);
        //          int nLevelHeight = (int)Math.Ceiling(header.height / dfLevelSampling);

        //          Bitmap imgScaled = ScaleImage(inputImage, nLevelWidth, nLevelHeight);

        //          try
        //          {
        //            WriteLevel(imgScaled, cnTileSize, sSdiOutputFolder + @"\" + iLevel.ToString());
        //          }
        //          finally
        //          {
        //            if (imgScaled != inputImage)
        //              imgScaled.Dispose();
        //          }
        //        }
        //      }
        //    }
        //<-del kazumichi 20080704


        /// <summary>
        /// Writes the image XML.
        /// </summary>
        /// <param name="sInputImage">The s input image.</param>
        /// <param name="sXmlOutputPath">The s XML output path.</param>
        /// <param name="nImageWidth">Width of the n image.</param>
        /// <param name="nImageHeight">Height of the n image.</param>
        /// <param name="nTileSize">Size of the n tile.</param>
        /// <param name="nTileOverlap">The n tile overlap.</param>
        public static void WriteImageXml(string sInputImage, string sXmlOutputPath,
          int nImageWidth, int nImageHeight, int nTileSize, int nTileOverlap)
        {
            XmlDocument docOut = new XmlDocument();
            XmlElement eltImg = docOut.CreateElement("Image");
            eltImg.SetAttribute("TileSize", nTileSize.ToString());
            eltImg.SetAttribute("Overlap", nTileOverlap.ToString());
            eltImg.SetAttribute("Format", "jpg");

            docOut.AppendChild(eltImg);

            XmlElement eltImageSize = docOut.CreateElement("Size");
            eltImageSize.SetAttribute("Width", nImageWidth.ToString());
            eltImageSize.SetAttribute("Height", nImageHeight.ToString());

            eltImg.AppendChild(eltImageSize);
            docOut.Save(sXmlOutputPath);
        }

        /// <summary>
        /// Create a DZI from an input image file.
        /// </summary>
        /// <remarks>
        /// if the name of a input image file is 'dog.jpg',
        /// this method creates 'collection_images/dog.xml' and
        /// 'collection_images/dog_files/'.
        /// </remarks>
        /// <param name="sInputImage">The input image.</param>
        /// <param name="sXmlOutputPath">The DZI output path.</param>
        public static void Image2Sdi_Beta2(string sInputImage, string sXmlOutputPath)
        {
            string sTilesRootDir = Path.GetDirectoryName(sXmlOutputPath)
                + @"\" + Path.GetFileNameWithoutExtension(sXmlOutputPath)
                + "_files";

            Directory.CreateDirectory(Path.GetDirectoryName(sTilesRootDir));
            using (Bitmap inputImage = new Bitmap(sInputImage))
            {
                const int cnTileSize = 256;
                string sInputImageName = Path.GetFileNameWithoutExtension(sInputImage);

                int nImageWidth = inputImage.Width;
                int nImageHeight = inputImage.Height;

                // -------------------------------------------
                // Create DZI xml file.
                // -------------------------------------------
                WriteImageXml(sInputImageName, sXmlOutputPath,
                    nImageWidth, nImageHeight, cnTileSize, 0);

                int cLevels = (int)Math.Max(
                    Math.Ceiling(Math.Log(nImageWidth, 2)),
                    Math.Ceiling(Math.Log(nImageHeight, 2))
                    ) + 1;

                // -------------------------------------------
                // Create DZI by each zoom level.
                // -------------------------------------------
                for (int iLevel = 0; iLevel < cLevels; iLevel++)
                {
                    double dfLevelSampling = Math.Pow(2, cLevels - iLevel - 1);
                    int nLevelWidth = (int)Math.Ceiling(nImageWidth / dfLevelSampling);
                    int nLevelHeight = (int)Math.Ceiling(nImageHeight / dfLevelSampling);

                    Bitmap imgScaled = ScaleImage(inputImage, nLevelWidth, nLevelHeight);

                    try
                    {
                        WriteLevel(imgScaled, cnTileSize, sTilesRootDir + @"\" + iLevel.ToString());
                    }
                    finally
                    {
                        if (imgScaled != inputImage)
                            imgScaled.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Scales the image.
        /// </summary>
        /// <param name="imageSource">The image source.</param>
        /// <param name="nScaledWidth">Width of the n scaled.</param>
        /// <param name="nScaledHeight">Height of the n scaled.</param>
        /// <returns></returns>
        private static Bitmap ScaleImage(Bitmap imageSource, int nScaledWidth, int nScaledHeight)
        {
            if (imageSource.Width == nScaledWidth && imageSource.Height == nScaledHeight)
                return imageSource;

            Bitmap bmpScaled = new Bitmap(nScaledWidth, nScaledHeight);

            using (Graphics g = Graphics.FromImage(bmpScaled))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imageSource, new Rectangle(0, 0, nScaledWidth, nScaledHeight));
            }

            return bmpScaled;
        }

        /// <summary>
        /// Writes the level.
        /// </summary>
        /// <param name="imgLod">The img lod.</param>
        /// <param name="nTileSize">Size of the n tile.</param>
        /// <param name="sLevelTileFolder">The s level tile folder.</param>
        private static void WriteLevel(Bitmap imgLod, int nTileSize, string sLevelTileFolder)
        {
            Directory.CreateDirectory(sLevelTileFolder);

            using (Graphics gImgLod = Graphics.FromImage(imgLod))
            {

                int cTilesX = (imgLod.Width + nTileSize - 1) / nTileSize;
                int cTilesY = (imgLod.Height + nTileSize - 1) / nTileSize;

                for (int tileX = 0; tileX < cTilesX; tileX++)
                    for (int tileY = 0; tileY < cTilesY; tileY++)
                    {
                        int tileLeft = tileX * nTileSize;
                        int tileTop = tileY * nTileSize;
                        int tileRight = Math.Min(tileLeft + nTileSize, imgLod.Width);
                        int tileBottom = Math.Min(tileTop + nTileSize, imgLod.Height);

                        System.Drawing.Imaging.BitmapData bmd = imgLod.LockBits(
                            new Rectangle(tileLeft, tileTop, tileRight - tileLeft, tileBottom - tileTop),
                            System.Drawing.Imaging.ImageLockMode.ReadOnly,
                            imgLod.PixelFormat);

                        using (Bitmap bmpTile = new Bitmap(tileRight - tileLeft, tileBottom - tileTop,
                            bmd.Stride, bmd.PixelFormat, bmd.Scan0))
                        {
                            bmpTile.Save(sLevelTileFolder + @"\" + tileX.ToString() + "_" + tileY.ToString() + ".jpg",
                                System.Drawing.Imaging.ImageFormat.Jpeg);

                        }

                        imgLod.UnlockBits(bmd);
                    }
            }
        }
    }
}
