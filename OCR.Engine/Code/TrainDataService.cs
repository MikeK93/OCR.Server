using OCR.Engine.Code.Logging;
using OCR.Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace OCR.Engine.Code
{
    public class TrainDataService : ITrainDataService
    {
        private static readonly ILog _logger = LogManager.GetLogger();
        private readonly string _trainDataDirectory;
        private readonly IRecognitionDataConverter _recognitionDataConverter;

        public TrainDataService(string trainDataDirectory, IRecognitionDataConverter recognitionDataConverter)
        {
            _trainDataDirectory = trainDataDirectory ?? throw new ArgumentNullException(nameof(trainDataDirectory));
            _recognitionDataConverter = recognitionDataConverter ?? throw new ArgumentNullException(nameof(recognitionDataConverter));
        }

        public IEnumerable<int[,]> Get(char symbol)
        {
            var directory = GetTrainDataDirectory(symbol);

            foreach (var trainSamplePath in Directory.EnumerateFiles(directory))
            {
                _logger.Info($"Preparing image sample [{trainSamplePath}].");

                var image = new Bitmap(trainSamplePath);

                yield return _recognitionDataConverter.ToArray(image);
            }
        }

        private string GetTrainDataDirectory(char symbol)
        {
            if (!Directory.Exists(_trainDataDirectory))
            {
                throw new InvalidOperationException($"Could not find directory: {_trainDataDirectory}");
            }

            if (!Directory.Exists($@"{_trainDataDirectory}\{symbol}"))
            {
                throw new InvalidOperationException($@"Could not find directory: {_trainDataDirectory}\{symbol}");
            }

            return $@"{_trainDataDirectory}\{symbol}";
        }
    }
}