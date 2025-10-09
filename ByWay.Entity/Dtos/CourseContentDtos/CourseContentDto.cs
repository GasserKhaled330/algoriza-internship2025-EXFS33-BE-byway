using Swashbuckle.AspNetCore.Annotations;

namespace ByWay.Domain.DTOs.CourseContent;

public record CourseContentDto([SwaggerSchema(ReadOnly = true)] int Id, string Name, int LecturesCount, int DurationInHours);