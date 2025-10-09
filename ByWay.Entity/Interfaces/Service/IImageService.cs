using Microsoft.AspNetCore.Http;

namespace ByWay.Domain.Interfaces.Service;

public interface IImageService
{
  Task<string> UploadImageAsync(
      IFormFile image,
      string relativeFolder,
      int maxSizeInMb = 2,
      string[]? allowedExtensions = null);

  void DeleteImage(string relativeImagePath);
}