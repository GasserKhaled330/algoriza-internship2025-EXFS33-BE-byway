namespace ByWay.Domain.Dtos.CartDtos;

public record CartItemDto(
    int CourseId,
    string Name,
    string InstructorName,
    double Rate,
    string Level,
    int TotalHours,
    int LecturesCount,
    decimal Cost,
    string ImagePath
);