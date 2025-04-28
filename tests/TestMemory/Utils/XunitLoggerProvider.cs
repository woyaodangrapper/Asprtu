namespace Asprtu.Repository.MemoryCache.Tests;

public class XunitLoggerProvider(ITestOutputHelper testOutputHelper, string? fileName = null) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) =>
        new XunitLogger(testOutputHelper, fileName ?? categoryName, fileName);

    public void Dispose() { }
}