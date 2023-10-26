namespace AllUsefulInformationSearch.DataAccess;

public interface IInformationSourceDataFileRepository : IRepository<InformationSourceDataFile>
{
    Task<List<InformationSourceDataFile>> GetAllAsync(CancellationToken token = default);
    Task<List<InformationSourceDataFile>> GetAllByProviderAsync(InformationProvider provider, CancellationToken token = default);
}