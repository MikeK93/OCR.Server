using System.Drawing;

namespace OCR.Graphics.Contracts
{
    public interface IRecognitionDataConverter
    {
        Bitmap Scale(Bitmap image, int width, int height);

        int[,] ToArray(Bitmap image);

        char ToSymbol(string filePath);
    }
}