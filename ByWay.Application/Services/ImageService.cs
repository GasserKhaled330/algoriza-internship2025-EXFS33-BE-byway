using ByWay.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ByWay.Application.Services;

public class ImageService : IImageService
{
  private readonly IWebHostEnvironment _env;

  public ImageService(IWebHostEnvironment env)
  {
    _env = env;
  }

  public async Task<string> UploadImageAsync(
      IFormFile image,
      string relativeFolder,
      int maxSizeInMb = 2,
      string[]? allowedExtensions = null)
  {
    allowedExtensions ??= [".jpg", ".jpeg", ".png", ".webp"];
    if (image is null || image.Length == 0)
      throw new ArgumentException("No file uploaded.", nameof(image));

    if (image.Length > maxSizeInMb * 1024 * 1024) // 2MB defalut limit
      throw new ArgumentException($"File size exceeds the {maxSizeInMb}MB limit.");

    var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

    if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
      throw new ArgumentException("Invalid file type. Only .jpg, .jpeg, and .png are allowed.");

    var fileName = $"{Path.GetRandomFileName()}{fileExtension}";
    var folderPath = Path.Combine(_env.WebRootPath, relativeFolder);

    Directory.CreateDirectory(folderPath); // Ensure the directory exists

    var filePath = Path.Combine(folderPath, fileName);

    await using var stream = new FileStream(filePath, FileMode.Create);
    await image.CopyToAsync(stream);


    return Path.Combine(relativeFolder, fileName).Replace("\\", "/", StringComparison.Ordinal);
  }

  public void DeleteImage(string relativeImagePath)
  {
    if (string.IsNullOrEmpty(relativeImagePath))
      return;

    var filePath = Path.Combine(_env.WebRootPath, relativeImagePath);

    if (File.Exists(filePath))
    {
      File.Delete(filePath);
    }
  }
}