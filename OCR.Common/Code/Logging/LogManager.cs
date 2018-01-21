using OCR.Common.Contracts;
using System.Threading;

namespace OCR.Common.Code.Logging
{
    public static class LogManager
    {
        private static ILog _logger;
        
        public static void RegisterLogger(ILog logger)
        {
            if (logger == null)
            {
                return;
            }

            Interlocked.CompareExchange(ref _logger, logger, _logger);
        }

        public static ILog GetLogger()
        {
            if (_logger == null)
            {
                Interlocked.CompareExchange(ref _logger, new EmptyLogger(), null);
            }

            return _logger;
        }
    }
}