using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognition
{
    public class Cropper
    {
        public Pixel[,] Crop(Pixel[,] pixels, int fromX, int fromY, int width, int height)
        {
            var newPixels = new Pixel[width, height];
            var k = 0;
            var m = 0;
            for (int i = fromX; i < fromX + width; i++)
            {
                for (int j = fromY; j < fromY + height; j++)
                {
                    newPixels[k, m++] = pixels[i, j];
                }
                k++;
                m = 0;
            }
            return newPixels;
        }
    }
}
