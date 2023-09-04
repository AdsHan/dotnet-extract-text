namespace ExtractText.Extract;

interface IExtractText<T>
{
    Task<T> GetResults();
}
