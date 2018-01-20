using OCR.Engine.Contracts;
using System;
using System.Drawing;

namespace OCR.Engine.Code
{
    public class RecognitionDataConverter : IRecognitionDataConverter
    {
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
    }
}