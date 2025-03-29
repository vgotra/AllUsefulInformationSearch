namespace Auis.StackOverflow.Models.MappingExtensions;

[Mapper]
public static partial class EntityToApiModelMappingExtensions
{
    [MapperIgnoreSource(nameof(WebDataFileEntity.Posts))]
    public static partial WebDataFileResponse ToResponse(this WebDataFileEntity entity);
}