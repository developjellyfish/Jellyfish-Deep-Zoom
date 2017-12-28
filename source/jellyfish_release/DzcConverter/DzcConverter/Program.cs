using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace DzcConverter
{
    class Program
    {
        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        static int Main(string[] args)
        {
            string inputImagesDir = null;
            DirectoryInfo outputTilesDir = null;
            Int32 tileSize = 0;
            Int32 compression = 0;
            Int32 horizontalSpacing = 0;
            Int32 verticalSpacing = 0;
            string collectionXmlFile = "";
            string collectionImagesParentDirPath = "";
            string sourceImageListFile = "";
            string collectionImagesDirPath = "";

            try
            {
                int param = 0;
                // -------------------------------------------------
                // Get the value of each command parameter.
                // -------------------------------------------------
                inputImagesDir = args[param++].ToString();
                outputTilesDir = new DirectoryInfo(args[param++]);
                tileSize = Int32.Parse(args[param++]);
                compression = Int32.Parse(args[param++]);
                horizontalSpacing = Int32.Parse(args[param++]);
                verticalSpacing = Int32.Parse(args[param++]);
                collectionXmlFile = args[param++].ToString();
                collectionImagesParentDirPath = args[param++].ToString();
                sourceImageListFile = args[param++].ToString();
                collectionImagesDirPath = args[param++].ToString();
            }
            catch
            {
                Console.WriteLine("Usage: DzcConverter [inputImagesDir] [outputTilesDir] [tileSize] [compression] [horizontalSpacing] [verticalSpacing] [collectionXmlFile] [collectionImagesParentDirPath] [sourceImageListFile]");
                return -1;
            }

            try
            {

                if (!outputTilesDir.Exists)
                    outputTilesDir.Create();

                // *********************************************************************
                // Read the list of source image which should be converted to DZC.
                // *********************************************************************
                string line = "";
                Dictionary<string, string> sourceImageListDic = new Dictionary<string, string>();
                using (StreamReader sr = new StreamReader(sourceImageListFile, Encoding.GetEncoding("UTF-8")))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        sourceImageListDic.Add(line, line);
                    }
                }

                List<SeadragonImage> imagesToConvert = new List<SeadragonImage>(sourceImageListDic.Count);

                Point ptOrg = new Point(0, 0);
                int canvasWidth = 0;
                int canvasHeight = 0;
                int maxHeightCurrentRow = 0;
                int rowSize = 2;

                // ***************************************************
                // Decide how many tiles should be placed in 1 row.
                // ***************************************************
                if (sourceImageListDic.Count > 4)
                    rowSize = 3;

                if (sourceImageListDic.Count > 9)
                    rowSize = 4;

                if (sourceImageListDic.Count > 12) // if over 12 images, the number of tiles in 1 row is 5 tiles.
                    rowSize = 5;

                // ***************************************************
                // Repeat times by the number of the image file included in the input image folder.
                // ***************************************************
                int cntImages = 0;
                foreach (string value in sourceImageListDic.Values)
                {

                    FileInfo srcImage = new FileInfo(inputImagesDir + value);

                    Image originalImage = null;
                    try
                    {
                        using (FileStream fileStream = srcImage.OpenRead())
                        {
                            originalImage = Image.FromStream(fileStream, true, false);
                        }

                        // --------------------------------------
                        // Creating SeadragonImage data structure(object) which will be converted to SeadragonImage.
                        // --------------------------------------
                        SeadragonImage sdImage = new SeadragonImage(ptOrg, srcImage.FullName, originalImage.Width, originalImage.Height);
                        imagesToConvert.Add(sdImage);

                        // --------------------------------------
                        // Calculating the size of the canvas.
                        // --------------------------------------
                        canvasWidth = Math.Max(canvasWidth, ptOrg.X + originalImage.Width);
                        canvasHeight = Math.Max(canvasHeight, ptOrg.Y + originalImage.Height);
                        maxHeightCurrentRow = Math.Max(maxHeightCurrentRow, originalImage.Height);

                        // --------------------------------------
                        // Calculating the point of this tile.
                        // --------------------------------------
                        if (((cntImages + 1) % rowSize) == 0)
                        {
                            ptOrg = new Point(0, ptOrg.Y + maxHeightCurrentRow + verticalSpacing);
                            maxHeightCurrentRow = 0;
                        }
                        else
                        {
                            ptOrg = new Point(ptOrg.X + originalImage.Width + horizontalSpacing, ptOrg.Y);
                        }
                    }
                    finally
                    {
                        if (originalImage != null)
                            originalImage.Dispose();
                    }
                    cntImages++;
                }

                // Executing the method of converting image to SeadragonImage.
                SeadragonExporter.Export(outputTilesDir.FullName, imagesToConvert, canvasWidth, canvasHeight, tileSize, compression, collectionXmlFile, collectionImagesParentDirPath, collectionImagesDirPath);
                Console.WriteLine("DZC Converted");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failure: " + ex.Message + ex.StackTrace);
                return -1;
            }
        }
    }
}
