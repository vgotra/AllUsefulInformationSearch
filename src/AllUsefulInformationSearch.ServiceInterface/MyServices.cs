namespace AllUsefulInformationSearch.ServiceInterface;

public class MyServices : Service
{
    public static string AssertName(string name) => name.IsNullOrEmpty() ? throw new ArgumentNullException(nameof(name)) : name;

    public object Any(Hello request) => new HelloResponse { Result = $"Hello, {AssertName(request.Name)}!" };

    public object Any(HelloSecure request) => new HelloResponse { Result = $"Hello, {AssertName(request.Name)}!" };
}