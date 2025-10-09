namespace ByWay.Domain.DTOs;

public record PagingResponseDto<T>
{
  public int PageSize { get; set; }
  public int PageIndex { get; set; }
  public int TotalCount { get; set; }
  public IEnumerable<T>? Data { get; set; }
}