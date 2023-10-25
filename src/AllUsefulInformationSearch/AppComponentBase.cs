using ServiceStack.Blazor;

namespace AllUsefulInformationSearch;

/// <summary>
/// For Pages and Components that make use of ServiceStack functionality, e.g. Client
/// </summary>
public abstract class AppComponentBase : BlazorComponentBase, IHasJsonApiClient
{
}

/// <summary>
/// For Pages and Components requiring Authentication
/// </summary>
public abstract class AppAuthComponentBase : AuthBlazorComponentBase
{
}
