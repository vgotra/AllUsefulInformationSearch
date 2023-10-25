namespace AllUsefulInformationSearch.Tests;

public class UnitTest
{
    private readonly ServiceStackHost _appHost;

    public UnitTest()
    {
        _appHost = new BasicAppHost().Init();
        _appHost.Container.AddTransient<MyServices>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => _appHost.Dispose();

    [Test]
    public void Can_call_MyServices()
    {
        var service = _appHost.Container.Resolve<MyServices>();

        var response = (HelloResponse)service.Any(new Hello { Name = "World" });

        Assert.That(response.Result, Is.EqualTo("Hello, World!"));
    }
}
