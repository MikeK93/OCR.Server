using OCR.Engine.Contracts;

namespace OCR.Engine.Code.Logging
{
    public class EmptyLogger : ILog
    {
        public void Error(string error) { }
        public void Info(string message) { }
        public void Warn(string warning) { }
    }
}