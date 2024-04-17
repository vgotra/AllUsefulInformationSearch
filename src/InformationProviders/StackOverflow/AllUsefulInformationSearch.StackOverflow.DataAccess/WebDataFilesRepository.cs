namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public class WebDataFilesRepository : IWebDataFilesRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public WebDataFilesRepository(IDbConnectionFactory dbConnectionFactory) => _dbConnectionFactory = dbConnectionFactory;

    public async Task<List<WebDataFileEntity>> GetWebDataFileListAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await _dbConnectionFactory.GetAndOpenDefaultDbConnection(cancellationToken);
        var result = await connection.QueryAsync<WebDataFileEntity>($"SELECT * FROM StackOverflow.WebDataFiles");
        return result.ToList();
    }

    public async Task AddWebDataFileListAsync(List<WebDataFileEntity> dataFiles, CancellationToken cancellationToken = default)
    {
        //TODO Source generators can be useful there
        await using var connection = await _dbConnectionFactory.GetAndOpenDefaultDbConnection(cancellationToken);
        //TODO Check this later how to correctly use it with transactions in docs
        await using var writer = await connection.BeginBinaryImportAsync(@"
            COPY StackOverflow.WebDataFiles
            (Id, Name, Link, Size, LastModified, ProcessingStatus, LastUpdated)
            FROM STDIN (FORMAT BINARY)", cancellationToken);

        foreach (var dataFile in dataFiles)
        {
            writer.StartRow();
            writer.Write(dataFile.Id, NpgsqlTypes.NpgsqlDbType.Uuid);
            writer.Write(dataFile.Name, NpgsqlTypes.NpgsqlDbType.Text);
            writer.Write(dataFile.Link, NpgsqlTypes.NpgsqlDbType.Text);
            writer.Write(dataFile.Size, NpgsqlTypes.NpgsqlDbType.Bigint);
            writer.Write(dataFile.LastModified.UtcDateTime, NpgsqlTypes.NpgsqlDbType.TimestampTz);
            writer.Write((int)dataFile.ProcessingStatus, NpgsqlTypes.NpgsqlDbType.Integer);
            writer.Write(dataFile.LastUpdated.UtcDateTime, NpgsqlTypes.NpgsqlDbType.TimestampTz);
        }

        await writer.CompleteAsync(cancellationToken);
    }

    public async Task UpdateWebDataFileListAsync(List<WebDataFileEntity> dataFiles, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dbConnectionFactory.GetAndOpenDefaultDbConnection(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            foreach (var dataFile in dataFiles)
            {
                await using var cmd = new NpgsqlCommand(@"
                UPDATE StackOverflow.WebDataFiles
                SET Name = @Name, Link = @Link, Size = @Size, LastModified = @LastModified, ProcessingStatus = @ProcessingStatus, LastUpdated = @LastUpdated
                WHERE Id = @Id", connection);

                cmd.Parameters.AddWithValue("Id", dataFile.Id);
                cmd.Parameters.AddWithValue("Name", dataFile.Name);
                cmd.Parameters.AddWithValue("Link", dataFile.Link);
                cmd.Parameters.AddWithValue("Size", dataFile.Size);
                cmd.Parameters.AddWithValue("LastModified", dataFile.LastModified.UtcDateTime);
                cmd.Parameters.AddWithValue("ProcessingStatus", (int)dataFile.ProcessingStatus);
                cmd.Parameters.AddWithValue("LastUpdated", dataFile.LastUpdated.UtcDateTime);

                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}