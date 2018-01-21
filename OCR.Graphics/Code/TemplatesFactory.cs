using OCR.Common.Constants;
using OCR.DataAccess.Contracts;
using OCR.Graphics.Contracts;
using System.Drawing;
using System.IO;

namespace OCR.Graphics.Code
{
    public class TemplatesFactory : ITemplatesFactory
    {
        private readonly IRecognitionDataConverter _recognitionDataConverter;
        private readonly INeuronWriter _writer;

        public TemplatesFactory(IRecognitionDataConverter recognitionDataConverter, INeuronWriter writer)
        {
            _recognitionDataConverter = recognitionDataConverter;
            _writer = writer;
        }

        public void Create(string sourceDirectory, string targetDirectory)
        {
            foreach(var templateImagePath in Directory.EnumerateFiles(sourceDirectory))
            {
                var templateImage = new Bitmap(templateImagePath);

                var scaledTemplateImage = _recognitionDataConverter.Scale(templateImage, RecognitionSettings.DefaultWeightWidth, RecognitionSettings.DefaultWeightHeight);

                var weights = _recognitionDataConverter.ToArray(scaledTemplateImage);

                var startNeuronNameIndex = templateImagePath.LastIndexOf('\\');
                var endNeuronNameIndex = templateImagePath.LastIndexOf('.');
                var neuronName = templateImagePath.Substring(startNeuronNameIndex + 1, endNeuronNameIndex - startNeuronNameIndex - 1);

                _writer.WriteTo($"{targetDirectory}/{neuronName}.txt", weights);
            }
        }
    }
}