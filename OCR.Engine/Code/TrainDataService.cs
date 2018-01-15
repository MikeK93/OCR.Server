using OCR.Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace OCR.Engine.Code
{
    public class TrainDataService : ITrainDataService
    {
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

            foreach (var trainSample in Directory.EnumerateFiles(directory))
            { 
                var image = new Bitmap(trainSample);

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