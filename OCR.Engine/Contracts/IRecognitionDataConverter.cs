using System.Drawing;

namespace OCR.Engine.Contracts
{
    public interface IRecognitionDataConverter
    {
        int[,] ToArray(Bitmap image);
    }
}