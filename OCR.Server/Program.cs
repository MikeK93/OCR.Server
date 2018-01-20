using OCR.Engine.Code;
using OCR.Engine.Code.Logging;
using OCR.Engine.Models;
using System;

namespace OCR.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var trainDataDirectory = @"D:\Files\Apps\OCR.Server\Resources\TrainData\Samples";
            var weightDirectory = @"D:\Files\Apps\OCR.Server\Resources\TrainData\Weights";

            LogManager.RegisterLogger(new ConsoleLogger());

            var factory = new NeuronFactory();
            var service = new NeuronService(weightDirectory, factory);

            var recongizeService = new NeuronRecognizeService();

            var converter = new RecognitionDataConverter();
            var trainDataService = new TrainDataService(trainDataDirectory, converter);

            var neuronA = service.Get('A');
            
            foreach (var trainData in trainDataService.Get('A'))
            {
                Recognize(trainData, neuronA);
            }

            service.Save(neuronA);

            Console.WriteLine("Done");
            Console.ReadLine();

            void Recognize(int[,] trainData, Neuron neuron)
            {
                if (recongizeService.IsRecognized(trainData, neuron))
                {
                    Console.WriteLine($"Easy! It's [{neuron.Symbol}]. Isn't it? [y/n]: ");
                    if (Console.ReadLine() != "y")
                    {
                        recongizeService.Forget(trainData, neuron);
                        Recognize(trainData, neuron);
                    }

                    return;
                }

                Console.WriteLine($"Hm... cannot figure out [{neuron.Symbol}]. Is it {neuron.Symbol}? [y/n]");
                if (Console.ReadLine() == "y")
                {
                    recongizeService.Learn(trainData, neuron);
                    Recognize(trainData, neuron);
                }
            }
        }
    }
}