namespace AllUsefulInformationSearch.StackOverflow.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WebDataFilesController(StackOverflowDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<WebDataFileResponse>> Get([FromQuery]WebDataFileRequest request)
    {
        //TODO Apply default sorting for paging
        var query = dbContext.WebDataFiles.AsNoTracking().ApplyPaging(request).ApplySorting(request);
        
        if (!string.IsNullOrWhiteSpace(request.Name)) // Create filtering extensions later 
            query = query.Where(x => x.Name.Contains(request.Name));
        
        var result = await query.ToListAsync();
        return result.Select(x => x.ToResponse());
    }
}