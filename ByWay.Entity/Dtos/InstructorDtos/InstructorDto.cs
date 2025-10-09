namespace ByWay.Domain.DTOs;

public record InstructorDto(
    int Id,
    string FullName,
    string? Bio,
    double Rate,
    string JobTitle,
    string? ImagePath
);
