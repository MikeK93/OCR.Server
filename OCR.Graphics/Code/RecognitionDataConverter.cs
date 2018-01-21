using OCR.Graphics.Contracts;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace OCR.Graphics.Code
{
    public class RecognitionDataConverter : IRecognitionDataConverter
    {
        public Bitmap Scale(Bitmap image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = System.Drawing.Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.Default;//HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;//HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.Default;//HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.Default;//HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public int[,] ToArray(Bitmap image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            var toRecognize = new int[image.Width, image.Height];

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    toRecognize[x, y] = image.GetPixel(y, x).R <= 15 ? 1 : 0;
                }
            }

            return toRecognize;
        }

        public char ToSymbol(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}