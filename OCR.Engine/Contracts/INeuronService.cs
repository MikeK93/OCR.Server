using OCR.Engine.Models;

namespace OCR.Engine.Contracts
{
    public interface INeuronService
    {
        void Save(Neuron neuron);
        Neuron Get(char symbol);
    }
}