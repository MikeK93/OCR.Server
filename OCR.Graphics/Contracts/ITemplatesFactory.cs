namespace OCR.Graphics.Contracts
{
    public interface ITemplatesFactory
    {
        /// <summary>
        /// Creates template data for recognition
        /// </summary>
        /// <param name="sourceDirectory">Directory of all symbols images</param>
        /// <param name="targetDirectory">Directory of all symbols weights files</param>
        void Create(string sourceDirectory, string targetDirectory);
    }
}