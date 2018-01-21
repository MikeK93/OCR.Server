using OCR.Common.Contracts;

namespace OCR.Common.Code.Logging
{
    public class EmptyLogger : ILog
    {
        public void Error(string error) { }
        public void Info(string message) { }
        public void Warn(string warning) { }
    }
}