namespace Auis.Tests.Common;

public class TestContextLogger<T>(TestContext testContext) : ILogger<T>
{
    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => testContext.WriteLine($"{logLevel}: {formatter(state, exception)}");
}