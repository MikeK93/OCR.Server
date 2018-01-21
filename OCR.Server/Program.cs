using OCR.Common.Code.Logging;
using OCR.DataAccess.Code;
using OCR.Engine.Code;
using OCR.Engine.Models;
using OCR.Graphics.Code;
using OCR.Graphics.Contracts;
using System;

namespace OCR.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.RegisterLogger(new ConsoleLogger());

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("// [q] - to exit the program.\n" +
                                  "// [s] - to study\n" +
                                  "// [p] - to populate templates data\n");
                Console.ResetColor();

                Console.Write("> ");
                switch (Console.ReadLine())
                {
                    case "s": Study(); break;
                    case "p": Populate(); break;
                    case "q": return;
                }
            } while (Console.ReadLine() != "q");
        }

        private static void Populate()
        {
            ITemplatesFactory factory = new TemplatesFactory(new RecognitionDataConverter(), new NeuronWriter());

            factory.Create(
                @"D:\Files\Apps\OCR.Server\Resources\TrainData\Weights\TemplatesSource",
                @"D:\Files\Apps\OCR.Server\Resources\TrainData\Weights");
        }

        private static void Study()
        {
            var trainDataDirectory = @"D:\Files\Apps\OCR.Server\Resources\TrainData\Samples";
            var weightDirectory = @"D:\Files\Apps\OCR.Server\Resources\TrainData\Weights";

            var service = new NeuronService(weightDirectory, new NeuronFactory(), new NeuronWriter());

            var recongizeService = new NeuronRecognizeService();

            var trainDataService = new TrainDataService(trainDataDirectory, new RecognitionDataConverter());

            var neuronA = service.Get('A');

            foreach (var trainData in trainDataService.Get('A'))
            {
                Recognize(trainData, neuronA);
            }

            service.Save(neuronA);
            
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