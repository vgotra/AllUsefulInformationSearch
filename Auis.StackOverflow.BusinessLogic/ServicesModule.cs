namespace Auis.StackOverflow.BusinessLogic;

public static class ServicesModule
{
    public static IServiceCollection AddStackOverflowServices(this IServiceCollection services)
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

        services.AddTransient<IStackOverflowProcessingSubWorkflow, StackOverflowProcessingSubWorkflow>();
        services.AddTransient<IStackOverflowProcessingWorkflow, StackOverflowProcessingWorkflow>();

        services.AddTransient<IFileUtilityService, FileUtilityService>();

        return services;
    }
}