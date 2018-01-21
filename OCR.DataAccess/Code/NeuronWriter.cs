using OCR.DataAccess.Contracts;
using System.IO;
using System.Text;

namespace OCR.DataAccess.Code
{
    public class NeuronWriter : INeuronWriter
    {
        public void WriteTo(string path, int[,] weight)
        {
            var builder = new StringBuilder();
            using (var file = new FileStream(path, FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(file))
            {
                for (int i = 0; i < weight.GetLength(0); i++)
                {
                    builder.Clear();

                    for (int j = 0; j < weight.GetLength(1); j++)
                    {
                        builder.Append($"{weight[i, j]}");
                        if (j != weight.GetLength(1) - 1)
                        {
                            builder.Append(" ");
                        }
                    }

                    if (i != weight.GetLength(0) - 1)
                    {
                        builder.AppendLine();
                    }

                    writer.Write(builder.ToString());
                }
            }
        }
    }
}