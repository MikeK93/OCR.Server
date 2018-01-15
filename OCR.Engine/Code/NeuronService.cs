using OCR.Engine.Models;
using OCR.Engine.Contracts;
using System.IO;
using System.Text;
using System;

namespace OCR.Engine.Code
{
    public class NeuronService : INeuronService
    {
        private readonly string _weightsDirectory;
        private readonly INeuronFactory _factory;

        public NeuronService(string weightsDirectory, INeuronFactory factory)
        {
            _weightsDirectory = weightsDirectory ?? throw new ArgumentNullException(nameof(weightsDirectory));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
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
            var builder = new StringBuilder();
            using (var file = new FileStream($"{_weightsDirectory}/{neuron.Symbol}.txt", FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(file))
            {
                for (int i = 0; i < neuron.N; i++)
                {
                    builder.Clear();

                    for (int j = 0; j < neuron.M; j++)
                    {
                        builder.Append(neuron.Weight[i, j]);
                        if (j != neuron.M - 1)
                        {
                            builder.Append(" ");
                        }
                    }

                    builder.AppendLine();

                    writer.Write(builder.ToString());
                }
            }
        }
    }
}