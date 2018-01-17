using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace TextRecognition
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Pixel
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;

        public bool IsBlack()
        {
            return Blue == 0 && Green ==0 && Red == 0;
        }
        public bool IsWhite()
        {
            return Blue == 255 && Green == 255 && Red == 255;
        }

        public  void PaintRed()
        {
            Blue = 0;
            Green = 0;
            Red = 255;
        }

        public void PaintGreen()
        {
            Blue = 0;
            Green = 255;
            Red = 0;
        }

        public bool IsRed()
        {
            return Blue == 0 && Green == 0 && Red == 255;
        }
    }

}
