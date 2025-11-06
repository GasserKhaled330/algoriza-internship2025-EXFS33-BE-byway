using ByWay.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
      int maxDimension = 600,
      string[]? allowedExtensions = null)
  {
    allowedExtensions ??= [".jpg", ".jpeg", ".png", ".webp"];
    if (image is null || image.Length == 0)
      throw new ArgumentException("No file uploaded.", nameof(image));

    //if (image.Length > maxSizeInMb * 1024 * 1024) // 2MB defalut limit
    //  throw new ArgumentException($"File size exceeds the {maxSizeInMb}MB limit.");

    var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

    if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
    {
      throw new ArgumentException("Invalid file type. Only .jpg, .jpeg, .webp, and .png are allowed.");
    }

    //var fileName = $"{Path.GetRandomFileName()}{fileExtension}";
    var outputFileName = $"{Path.GetRandomFileName()}.webp";
    var folderPath = Path.Combine(_env.WebRootPath, relativeFolder);

    Directory.CreateDirectory(folderPath); // Ensure the directory exists

    var filePath = Path.Combine(folderPath, outputFileName);

    //await using var stream = new FileStream(filePath, FileMode.Create);
    //await image.CopyToAsync(stream);
    await using var imageStream = image.OpenReadStream();
    using var img = await Image.LoadAsync<Rgba32>(imageStream);
    int newWidth = img.Width;
    int newHeight = img.Height;

    if (img.Width > maxDimension || img.Height > maxDimension)
    {
      // Resize based on the largest dimension (width or height)
      if (img.Width > img.Height)
      {
        newWidth = maxDimension;
        newHeight = (int)((float)img.Height * maxDimension / img.Width);
      }
      else
      {
        newHeight = maxDimension;
        newWidth = (int)((float)img.Width * maxDimension / img.Height);
      }
    }
    img.Mutate(x => x.Resize(newWidth, newHeight));

    var encoder = new WebpEncoder
    {
      Quality = 90,
      FileFormat = WebpFileFormatType.Lossy
    };
    await img.SaveAsync(filePath, encoder);
    return Path.Combine(relativeFolder, outputFileName).Replace("\\", "/", StringComparison.Ordinal);
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