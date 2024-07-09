namespace Auis.Wikipedia.BusinessLogic;

public static class ServicesModule
{
    public static IServiceCollection AddWikipediaServices(this IServiceCollection services)
    {
        services.AddSingleton<IPostTextCleanupService, PostTextCleanupService>();

        //TODO Check for possibility to reuse singleton for all services
        services.AddTransient<IPostsArchiveFileProcessingService, PostsArchiveFileProcessingService>();
        services.AddTransient<IPostsDeserializationService, PostsDeserializationService>();
        services.AddTransient<IPostsArchiveFileParserService, PostsArchiveFileParserService>();
        services.AddTransient<IPostsArchiveFileProcessingService, PostsArchiveFileProcessingService>();
        services.AddTransient<IPostsSynchronizationService, PostsSynchronizationService>();

        services.AddTransient<IWebArchivesSynchronizationService, WebArchivesSynchronizationService>();
        services.AddTransient<IWebArchiveParserService, WebArchiveParserService>();

        services.AddTransient<IWikipediaProcessingSubWorkflow, WikipediaProcessingSubWorkflow>();
        services.AddTransient<IWikipediaProcessingWorkflow, WikipediaProcessingWorkflow>();

        services.AddTransient<IFileUtilityService>(_ => Environment.OSVersion switch
        {
            { Platform: PlatformID.Win32NT } => new WindowsFileUtilityService(),
            { Platform: PlatformID.Unix } => new LinuxFileUtilityService(),
            _ => throw new InvalidOperationException("Unsupported OS.")
        });

        return services;
    }
}