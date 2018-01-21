using OCR.Common.Contracts;
using System;

namespace OCR.Server
{
    internal class ConsoleLogger : ILog
    {
        public void Error(string error)
        {
            WriteToConsole(ConsoleColor.DarkRed, $"[Error] - {error}");
        }

        public void Info(string message)
        {
            WriteToConsole(ConsoleColor.DarkBlue, $"[Info] - {message}");
        }

        public void Warn(string warning)
        {
            WriteToConsole(ConsoleColor.DarkYellow, $"[Warn] - {warning}");
        }

        private static void WriteToConsole(ConsoleColor fontColor, string text)
        {
            Console.ForegroundColor = fontColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}