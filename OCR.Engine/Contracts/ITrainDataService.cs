using System.Collections.Generic;

namespace OCR.Engine.Contracts
{
    public interface ITrainDataService
    {
        IEnumerable<int[,]> Get(char symbol);
    }
}