using OCR.Engine.Constants;
using OCR.Engine.Contracts;
using OCR.Engine.Models;

namespace OCR.Engine.Code
{
    public class NeuronService : INeuronService
    {
        public bool IsRecognized(int[,] input, Neuron neuron)
        {
            return neuron.GetSum(input) >= RecognitionSettings.NeuronSumThreshold;
        }

        public void Learn(int[,] input, Neuron neuron)
        {
            for (int i = 0; i < neuron.N; i++)
            {
                for (int j = 0; j < neuron.M; j++)
                {
                    neuron.Weight[i, j] += input[i, j];
                }
            }
        }

        public void Forget(int[,] input, Neuron neuron)
        {
            for (int i = 0; i < neuron.N; i++)
            {
                for (int j = 0; j < neuron.M; j++)
                {
                    neuron.Weight[i, j] -= input[i, j];
                }
            }
        }
    }
}