﻿namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public interface IWebDataFilesRepository
{
    Task<List<WebDataFileEntity>> GetWebDataFilesAsync(CancellationToken cancellationToken = default);
    
    Task SetProcessingStatusAsync(List<WebDataFileEntity> webDataFiles, ProcessingStatus status, CancellationToken cancellationToken = default);
}