namespace ExtractText.Extract;

internal class ExtractText<T>
{
    private readonly IExtractText<T> strategy;

    public ExtractText(IExtractText<T> strategy)
    {
        this.strategy = strategy;
    }

    public Task<T> GetResults()
    {
        return strategy.GetResults();
    }
}
