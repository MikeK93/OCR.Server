using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognition
{
    public class BlackAndWhiteFilter
    {
        public Pixel[,] ApplyFilter(Pixel[,] pixels, Pixel filterLevel)
        {

            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    if (pixels[i, j].Blue < filterLevel.Blue && pixels[i, j].Red < filterLevel.Red && pixels[i, j].Green < filterLevel.Green)
                    {
                        pixels[i, j].Green = pixels[i, j].Red = pixels[i, j].Blue = 0;
                    }
                    else
                    {
                        pixels[i, j].Green = pixels[i, j].Red = pixels[i, j].Blue = 255;
                    }
                }
            }
            return pixels;
        }
    }
}
