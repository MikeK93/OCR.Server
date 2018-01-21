﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognition
{

    public class BoxDetectorArgs
    {
       public  Action<Pixel[,], Box> BoxDetectCallback { get; set; }
       public  int MinHeight { get; set; }
       public int MinWidth { get; set; }
       public int MaxWidth { get; set; }
       public int MaxHeight { get; set; }
       public bool AndFilterStrategy { get; set; }
    }
    public class BoxDetector
    {
        public BoxDetector()
        {
            
        }

        
        public IEnumerable<Box> GetBoxes(Pixel[,] pixels,BoxDetectorArgs args)
        {
            if (args == null)
                throw new ArgumentNullException();
            var sw = new Stopwatch();
            ConcurrentQueue<Box> Boxes = new ConcurrentQueue<Box>();
            List<Box> boxesForChecking = new List<Box>();

            var width = pixels.GetLength(0);
            var height = pixels.GetLength(1);
            var startRemovingFrom = 0;
            for (int j = 0; j < pixels.GetLength(1);j++)
            {
   
                if (startRemovingFrom <= j)
                {
                    if(boxesForChecking.Any())
                    {
                        startRemovingFrom = height;
                        foreach (var box in boxesForChecking.ToList())
                        {
                            //delete all  boxes from top position. 
                            if (box.Y2 < j)
                                boxesForChecking.Remove(box);
                            else
                                //detect coords of first box for removing 
                                startRemovingFrom = Math.Min(startRemovingFrom, box.Y2);
                        }
                    }
                }
                // Console.WriteLine("i= "+i);
                for (int i = 0; i < pixels.GetLength(0); )
                {


                    var minX = width;
                    var minY = height;
                    var maxX = 0;
                    var maxY = 0;

                    if (pixels[i, j].IsBlack())
                    {
                        var intersectedBox = boxesForChecking.FirstOrDefault(b => b.CheckIfInBox(i, j));
                        if (intersectedBox != null)
                        {
                            //skip  box pixels
                            i = intersectedBox.X2 + 1;
                            continue;
                        }
                        var m = i;
                        var k = j;
                        var prevm = 0;
                        var prevk = 0;
                        var prevDirection = Direction.SouthWest;

                        do
                        {
                            prevm = m;
                            prevk = k;

                            
                            var coords = GetNextPoint(pixels, m, k, prevDirection);


                            m = coords.x;
                            k = coords.y;
                            prevDirection = coords.lastDirection;
                            minX = Math.Min(m, minX);
                            minY = Math.Min(k, minY);
                            maxX = Math.Max(m, maxX);
                            maxY = Math.Max(k, maxY);
                            if (prevk == k && prevm == m)
                                break;
                        }
                        //move to next pixel until we back to start position 
                        while ((i != m || j != k));
                        //skip pixel in box
                        i = maxX + 1;

                        //filter for too big or too small boxes
                        var discardBox = false;
                        if (args.AndFilterStrategy)
                            discardBox = maxX - minX < args.MinWidth && maxY - minY < args.MinHeight ||
                                         maxX - minX > args.MaxWidth && maxY - minY > args.MaxHeight;
                        else
                            discardBox = maxX - minX < args.MinWidth || maxY - minY < args.MinHeight ||
                                     maxX - minX > args.MaxWidth || maxY - minY > args.MaxHeight;
                        if (discardBox)
                            continue;
                        var boxX1 = minX > 0 ? minX  : 0;
                        var boxY1 = minY > 0 ? minY  : 0;
                        var boxX2 = maxX >= width ? width - 1 : maxX;
                        var boxY2 = maxY >= height ? height - 1 : maxY;
                        var box = new Box { X1 = boxX1, X2 = boxX2, Y1 = boxY1, Y2 = boxY2 };
                        Boxes.Enqueue(box);
                        args.BoxDetectCallback?.Invoke(pixels,box);
                        boxesForChecking.Add(box);

                        //only for visualization
                        //Task.Factory.StartNew(() =>
                        //{
                        //    //Console.WriteLine("Count= " + Boxes.Count);
                        //    for (int boundX = boxX1; boundX <= boxX2; boundX++)
                        //    {
                        //        pixels[boundX, boxY1].PaintGreen();
                        //        pixels[boundX, boxY2].PaintGreen();
                        //    }
                        //    for (int boundY = boxY1; boundY <= boxY2; boundY++)
                        //    {
                        //        pixels[boxX1, boundY].PaintGreen();
                        //        pixels[boxX2, boundY].PaintGreen();
                        //    }
                        //});
                    }
                    else
                        i++;

                }
            }
            Console.WriteLine("Box detection Ellapsed=" + sw.ElapsedMilliseconds);
            return Boxes;

        }

        private (int x, int y, Direction lastDirection) GetNextPoint(Pixel[,] pixel, int x, int y, Direction lastDirection)
        {
            var width = pixel.GetLength(0);
            var height = pixel.GetLength(1);
            //check shape borders
            foreach (var coords in GetShift(lastDirection))
            {
                var c = (x: x + coords.x, y: y + coords.y, d: coords.d);
                if (c.x < 0 || c.y < 0 || c.x >= width || c.y >= height)
                    continue;
                //if point is white then draw border
                if (pixel[c.x, c.y].IsWhite())
                    pixel[c.x, c.y].PaintRed();

                else if (pixel[c.x, c.y].IsBlack())
                    //go to next black point 
                    return c;
            }
            return (x, y, lastDirection);
        }

        private IEnumerable<(int x, int y, Direction d)> GetShift(Direction direction)
        {
            //some indian code 
            switch (direction)
            {
                case Direction.NoDirection:
                    break;
                case Direction.North:
                    yield return (-1, 1, Direction.SouthWest);
                    yield return (-1, 0, Direction.West);
                    yield return (-1, -1, Direction.NorthWest);
                    yield return (0, -1, Direction.North);
                    yield return (1, -1, Direction.NorthEast);
                    yield return (1, 0, Direction.East);
                    yield return (1, 1, Direction.SouthEast);
                    yield return (0, 1, Direction.South);
                    yield break;
                case Direction.NorthEast:
                    yield return (-1, 0, Direction.West);
                    yield return (-1, -1, Direction.NorthWest);
                    yield return (0, -1, Direction.North);
                    yield return (1, -1, Direction.NorthEast);
                    yield return (1, 0, Direction.East);
                    yield return (1, 1, Direction.SouthEast);
                    yield return (0, 1, Direction.South);
                    yield return (-1, 1, Direction.SouthWest);
                    yield break;
                case Direction.East:
                    yield return (-1, -1, Direction.NorthWest);
                    yield return (0, -1, Direction.North);
                    yield return (1, -1, Direction.NorthEast);
                    yield return (1, 0, Direction.East);
                    yield return (1, 1, Direction.SouthEast);
                    yield return (0, 1, Direction.South);
                    yield return (-1, 1, Direction.SouthWest);
                    yield return (-1, 0, Direction.West);
                    break;
                case Direction.SouthEast:
                    yield return (0, -1, Direction.North);
                    yield return (1, -1, Direction.NorthEast);
                    yield return (1, 0, Direction.East);
                    yield return (1, 1, Direction.SouthEast);
                    yield return (0, 1, Direction.South);
                    yield return (-1, 1, Direction.SouthWest);
                    yield return (-1, 0, Direction.West);
                    yield return (-1, -1, Direction.NorthWest);
                    yield break;
                case Direction.South:
                    yield return (1, -1, Direction.NorthEast);
                    yield return (1, 0, Direction.East);
                    yield return (1, 1, Direction.SouthEast);
                    yield return (0, 1, Direction.South);
                    yield return (-1, 1, Direction.SouthWest);
                    yield return (-1, 0, Direction.West);
                    yield return (-1, -1, Direction.NorthWest);
                    yield return (0, -1, Direction.North);
                    yield break;
                case Direction.SouthWest:
                    yield return (1, 0, Direction.East);
                    yield return (1, 1, Direction.SouthEast);
                    yield return (0, 1, Direction.South);
                    yield return (-1, 1, Direction.SouthWest);
                    yield return (-1, 0, Direction.West);
                    yield return (-1, -1, Direction.NorthWest);
                    yield return (0, -1, Direction.North);
                    yield return (1, -1, Direction.NorthEast);
                    yield break;
                case Direction.West:

                    yield return (1, 1, Direction.SouthEast);
                    yield return (0, 1, Direction.South);
                    yield return (-1, 1, Direction.SouthWest);
                    yield return (-1, 0, Direction.West);
                    yield return (-1, -1, Direction.NorthWest);
                    yield return (0, -1, Direction.North);
                    yield return (1, -1, Direction.NorthEast);
                    yield return (1, 0, Direction.East);
                    yield break;
                case Direction.NorthWest:
                    yield return (0, 1, Direction.South);
                    yield return (-1, 1, Direction.SouthWest);
                    yield return (-1, 0, Direction.West);
                    yield return (-1, -1, Direction.NorthWest);
                    yield return (0, -1, Direction.North);
                    yield return (1, -1, Direction.NorthEast);
                    yield return (1, 0, Direction.East);
                    yield return (1, 1, Direction.SouthEast);
                    yield break;
            }

        }

    }
}