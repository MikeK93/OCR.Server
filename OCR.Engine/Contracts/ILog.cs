namespace OCR.Engine.Contracts
{
    public interface ILog
    {
        void Info(string message);
        void Warn(string warning);
        void Error(string error);
    }
}