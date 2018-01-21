namespace OCR.DataAccess.Contracts
{
    public interface INeuronWriter
    {
        void WriteTo(string path, int[,] weight);
    }
}