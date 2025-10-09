using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Service;
using ByWay.Domain.Interfaces.UnitOfWork;
using ByWay.Infrastructure.Specifications;

namespace ByWay.Application.Services;

public class CategoryService : ICategoryService
{
  private readonly IUnitOfWork _unitOfWork;


  public CategoryService(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public async Task<IReadOnlyList<Category>> GetCategoriesAsync()
  {
    var spec = new CategorySpecification();
    var categories = await _unitOfWork.Categories.GetAllBySpecAsync(spec);
    return categories;
  }
}