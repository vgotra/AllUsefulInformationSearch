namespace AllDataSearch.ServiceModel;

[Route("/hello/{Name}")]
public class Hello : IReturn<HelloResponse>
{
    public string Name { get; set; } = null!;
}

[Route("/hellosecure/{Name}")]
[ValidateIsAuthenticated]
public class HelloSecure : IReturn<HelloResponse>
{
    public string Name { get; set; } = null!;
}

public class HelloResponse
{
    public string? Result { get; set; }
    public ResponseStatus? ResponseStatus { get; set; }
}