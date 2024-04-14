namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public interface ISettingsRepository
{
    Task<List<SettingEntity>> GetAllSettingsAsync(CancellationToken cancellationToken = default);
}