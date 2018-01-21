using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TextRecognition;

namespace SpendyBoxDetector
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

        public Pixel[,] ApplyFilter(Pixel[,] pixels)
        {

            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    var colorByte =(byte) ((pixels[i, j].Red * 0.3 + pixels[i, j].Green * 0.59 + pixels[i, j].Blue * 0.11) / 3);
                    pixels[i, j].Green = pixels[i, j].Red = pixels[i, j].Blue = colorByte;
                }
            }
            return pixels;
        }


    }

    public class NoizeFilter
    {
        public Pixel[,] ApplyFilter(Pixel[,] pixels,double percent)
        {


            var width = pixels.GetLength(0);
            var height = pixels.GetLength(1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!pixels[i, j].IsBlack())
                        continue;
                    var position = PixelPosition.Inside;
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            position = PixelPosition.LTCorner;
                        }
                        else if (j == height - 1)
                        {
                            position=PixelPosition.LBCorner;
                        }
                        else
                        {
                            position = PixelPosition.LeftEdge;
                        }
                    }
                    else if (i == width - 1)
                    {
                        if (j == 0)
                        {
                            position = PixelPosition.RTCorner;
                        }
                        else if (j == height - 1)
                        {
                            position = PixelPosition.RBCorner;
                        }
                        else
                        {
                            position = PixelPosition.RightEdge;
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            position = PixelPosition.TopEdge;
                        }
                        else if (j == height - 1)
                        {
                            position = PixelPosition.BottomEdge;
                        }
                        else
                        {
                            position = PixelPosition.Inside;
                        }
                    }
                    
                    CheckPixel(pixels,i,j,position,percent);
                }
            }
            return pixels;
        }

        private void CheckPixel(Pixel[,] pixels,int x,int y,PixelPosition position,double pc)
        {
            const int normalNeighborsCount = 8;
            const int edgeNeighborsCount = 5;
            const int cornerNeighborsCount = 3;

            var neigborsCount = 0;
            var startPosition = 0;
            switch (position)
            {
                case PixelPosition.Inside:
                    neigborsCount = normalNeighborsCount;
                    break;
                case PixelPosition.LeftEdge:
                    neigborsCount = edgeNeighborsCount;
                    startPosition = 1;
                    break;
                case PixelPosition.RightEdge:
                    neigborsCount = edgeNeighborsCount;
                    startPosition = 5;
                    break;
                case PixelPosition.TopEdge:
                    neigborsCount = edgeNeighborsCount;
                    startPosition = 3;
                    break;
                case PixelPosition.BottomEdge:
                    neigborsCount = edgeNeighborsCount;
                    startPosition = 7;
                    break;
                case PixelPosition.LTCorner:
                    neigborsCount = cornerNeighborsCount;
                    startPosition = 3;
                    break;
                case PixelPosition.RTCorner:
                    neigborsCount = cornerNeighborsCount;
                    startPosition = 5;
                    break;
                case PixelPosition.RBCorner:
                    neigborsCount = cornerNeighborsCount;
                    startPosition = 7;
                    break;
                case PixelPosition.LBCorner:
                    neigborsCount = cornerNeighborsCount;
                    startPosition = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }
            var skip = startPosition ;
            
            var blackCount= GetNeigborPixelShift().Skip(skip).Take(neigborsCount)
                .Select(sh=>(x:x+sh.shx,y:y+sh.shy)).Count(p=>pixels[p.x,p.y].IsBlack());
            var percent =  (double)blackCount/neigborsCount*100.0;
            if (percent < pc)
            {
                pixels[x,y].PaintWhite();
            }
        }

        private IEnumerable<(int shx, int shy)> GetNeigborPixelShift()
        {
            while (true)
            {
                yield return (-1, -1); //LT
                yield return (0, -1);//T
                yield return (1, -1);//RT
                yield return (1, 0);//R
                yield return (1, 1);//RB
                yield return (0, 1);//B
                yield return (-1, 1);//LB
                yield return (-1, 0);//L
            }
        }

        private enum PixelPosition
        {
            Inside,
            LeftEdge,
            RightEdge,
            TopEdge,
            BottomEdge,
            LTCorner,
            RTCorner,
            RBCorner,
            LBCorner,
        }
    }



}
