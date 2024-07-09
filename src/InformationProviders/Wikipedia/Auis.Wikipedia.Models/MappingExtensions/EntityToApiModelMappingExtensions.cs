namespace Auis.Wikipedia.Models.MappingExtensions;

[Mapper]
public static partial class EntityToApiModelMappingExtensions
{
    public static partial WebDataFileResponse ToResponse(this WebDataFileEntity entity);
}