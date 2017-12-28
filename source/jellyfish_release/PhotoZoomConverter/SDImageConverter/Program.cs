using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace SDImageConverter
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
            DirectoryInfo inputImagesDir = null;
            DirectoryInfo outputTilesDir = null;
            Int32 tileSize = 0;
            Int32 compression = 0;
            Int32 horizontalSpacing = 0;
            Int32 verticalSpacing = 0;
            Int32 DzcOutputFlag = 0;
            string Owner = "";

            try
            {
                int param = 0;
                inputImagesDir = new DirectoryInfo(args[param++]);
                outputTilesDir = new DirectoryInfo(args[param++]);
                tileSize = Int32.Parse(args[param++]);
                compression = Int32.Parse(args[param++]);
                horizontalSpacing = Int32.Parse(args[param++]);
                verticalSpacing = Int32.Parse(args[param++]);
                DzcOutputFlag = Int32.Parse(args[param++]);
                Owner = args[param++].ToString();
            }
            catch
            {
                Console.WriteLine("Usage: SDImageConverter [inputImagesDir] [outputTilesDir] [tileSize] [compression] [horizontalSpacing] [verticalSpacing] [DzcOutputFlag] [Owner]");
                return -1;
            }

            try
            {
                //第２引数で指定されたフォルダ名で出力フォルダを作成
                if (!outputTilesDir.Exists)
                    outputTilesDir.Create();

                //第１引数（入力フォルダ）内のすべてのJPGファイル情報を取得
                FileInfo[] srcImages = inputImagesDir.GetFiles("*.jpg");
                //取得したJPGファイルの個数分のイメージコンバータを生成
                List<SeadragonImage> imagesToConvert = new List<SeadragonImage>(srcImages.Length);
                Point ptOrg = new Point(0, 0);
                int canvasWidth = 0;
                int canvasHeight = 0;
                int maxHeightCurrentRow = 0;
                int rowSize = 2;

                if (srcImages.Length > 4)
                    rowSize = 3;

                if (srcImages.Length > 9)
                    rowSize = 4;

                if (srcImages.Length > 12)
                    rowSize = 5;

                //入力フォルダに格納されているJPGファイルの個数分、処理を繰り返す
                for (int i = 0; i < srcImages.Length; i++)
                {
                    FileInfo srcImage = srcImages[i];
                    Image originalImage = null;
                    try
                    {
                        using (FileStream fileStream = srcImage.OpenRead())
                        {
                            originalImage = Image.FromStream(fileStream, true, false);
                            //originalImage = Image.FromFile(srcImage.FullName);
                        }

                        SeadragonImage sdImage = new SeadragonImage(ptOrg, srcImage.FullName, originalImage.Width, originalImage.Height);
                        imagesToConvert.Add(sdImage);

                        canvasWidth = Math.Max(canvasWidth, ptOrg.X + originalImage.Width);
                        canvasHeight = Math.Max(canvasHeight, ptOrg.Y + originalImage.Height);
                        maxHeightCurrentRow = Math.Max(maxHeightCurrentRow, originalImage.Height);

                        if (((i + 1) % rowSize) == 0)
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
                }

                SeadragonExporter.Export(outputTilesDir.FullName, imagesToConvert, canvasWidth, canvasHeight, tileSize, compression, DzcOutputFlag, Owner);
                Console.WriteLine("Images Converted");
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
