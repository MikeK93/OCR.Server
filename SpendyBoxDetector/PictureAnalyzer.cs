using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognition
{
    public class PictureAnalyzer
    {
        public AnalyzeResult Analyze(Pixel[,] pixels)
        {
            var width = pixels.GetLength(0);
            var height = pixels.GetLength(1);
            var lineBlackRate = new double[height];
            var pixelWeight = 100.0 / width;
            var minX = width;
            var minY = height;
            var maxX = 0;
            var maxY = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {

                    if (pixels[i, j].Blue == 255 && pixels[i, j].Red == 255 && pixels[i, j].Green == 255)
                    {
                        lineBlackRate[j] += pixelWeight;
                        minX = Math.Min(i, minX);
                        minY = Math.Min(j, minY);
                        maxX = Math.Max(i, maxX);
                        maxY = Math.Max(j, maxY);
                    }
                }
            }
            return new AnalyzeResult { MinX = minX, MinY = minY, MaxX = maxX, MaxY = maxY, LineBlackRate = lineBlackRate };
        }

    }
}
