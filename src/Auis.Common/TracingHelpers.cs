using System.Diagnostics;

namespace Auis.Common;

public static class TracingHelpers
{
    public static Task ExecuteWithTracingAsync(this ActivitySource activitySource, string name, Func<Activity?, CancellationToken, Task> action, CancellationToken cancellationToken = default)
    {
        using (var activity = activitySource.StartActivity())
        {
            activity?.SetTag("Name", name);
            return action(activity, cancellationToken);
        }
    }

    public static Task ExecuteWithTracingAsync(this ActivitySource activitySource, string name, Func<Activity?, CancellationToken, Task> action, Action<Activity?> tagsAction, CancellationToken cancellationToken = default)
    {
        using (var activity = activitySource.StartActivity(name))
        {
            activity?.SetTag("Name", name);
            var result = action(activity, cancellationToken);
            tagsAction(activity);
            return result;
        }
    }
}