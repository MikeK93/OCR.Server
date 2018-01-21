using OCR.Engine.Models;
using OCR.Engine.Contracts;
using System.IO;
using System.Text;
using System;
using OCR.Common.Contracts;
using OCR.Common.Code.Logging;
using OCR.DataAccess.Contracts;

namespace OCR.Engine.Code
{
    public class NeuronService : INeuronService
    {
        private static readonly ILog _logger = LogManager.GetLogger();
        private readonly string _weightsDirectory;
        private readonly INeuronFactory _factory;
        private readonly INeuronWriter _writer;

        public NeuronService(string weightsDirectory, INeuronFactory factory, INeuronWriter writer)
        {
            _weightsDirectory = weightsDirectory ?? throw new ArgumentNullException(nameof(weightsDirectory));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public Neuron Get(char symbol)
        {
            using (var file = new FileStream($"{_weightsDirectory}/{symbol}.txt", FileMode.OpenOrCreate))
            {
                return _factory.Create(symbol, file);
            }
        }

        public void Save(Neuron neuron)
        {
            _logger.Info($"Saving neuron [{neuron.Symbol}]");

            _writer.WriteTo($"{_weightsDirectory}/{neuron.Symbol}.txt", neuron.Weight);
        }
    }
}