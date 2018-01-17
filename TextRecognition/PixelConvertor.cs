using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TextRecognition
{
    public class PixelConvertor
    {
        public Pixel[,] ToPixels(BitmapSource source)
        {
            if (source.Format != PixelFormats.Bgra32)
                source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);

            int width = source.PixelWidth;
            int height = source.PixelHeight;
            Pixel[,] result = new Pixel[width, height];

            CopyPixels(source, result, width * 4, 0);
            return result;
        }

        private void CopyPixels(BitmapSource source, Pixel[,] pixels, int stride, int offset)
        {

            var height = source.PixelHeight;
            var width = source.PixelWidth;
            var pixelBytes = new byte[height * width * 4];
            source.CopyPixels(pixelBytes, stride, 0);
            int y0 = offset / width;
            int x0 = offset - width * y0;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    pixels[x + x0, y + y0] = new Pixel
                    {
                        Blue = pixelBytes[(y * width + x) * 4 + 0],
                        Green = pixelBytes[(y * width + x) * 4 + 1],
                        Red = pixelBytes[(y * width + x) * 4 + 2],
                        Alpha = pixelBytes[(y * width + x) * 4 + 3],
                    };
        }

        public BitmapSource ToBitMap(Pixel[,] pixels)
        {
            var width = pixels.GetLength(0);
            var height = pixels.GetLength(1);
            var buffer = new byte[width * height * 4];
            int k = 0;
            for (int i = 0; i < pixels.GetLength(1); i++)
            {
                for (int j = 0; j < pixels.GetLength(0); j++)
                {
                    buffer[k++] = pixels[j, i].Blue;
                    buffer[k++] = pixels[j, i].Green;
                    buffer[k++] = pixels[j, i].Red;
                    buffer[k++] = pixels[j, i].Alpha;
                }
            }
            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, buffer, width * 4);
        }
    }
}
