using Wishlist.Domain;

namespace Wishlist.Contracts.Providers;

public interface IFileProvider
{
    Task<FileData> UploadFileAsync(string userId, string fileName, Stream fileStream);

    Task DeleteFileAsync(string userId, string fileName);
}