using Common.Dropbox.Managers;
using Common.Dropbox.Options;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.Extensions.Options;
using Wishlist.Contracts.Providers;
using Wishlist.Domain;

namespace Wishlist.Infrastructure.Providers;

public class FileProvider : IFileProvider
{
    private readonly DropboxClient _client;
    private readonly DropboxOptions _dropboxOptions;
    private readonly DropboxTokenManager _dropboxTokenService;

    public FileProvider(IOptions<DropboxOptions> dropboxOptions, DropboxTokenManager dropboxTokenService)
    {
        _dropboxOptions = dropboxOptions.Value;
        _dropboxTokenService = dropboxTokenService;

        var accessToken = _dropboxTokenService.GetAccessTokenAsync().Result;
        _client = new DropboxClient(accessToken);
    }
    
    public async Task<FileData> UploadFileAsync(string userId, string fileName, Stream fileStream)
    {
        var filePath = GetFilePath(userId, fileName);
        
        var fileMetadata = await _client.Files.UploadAsync(
            path: filePath, 
            mode: WriteMode.Overwrite.Instance,
            body: fileStream);

        var sharedLink = await _client.Sharing.CreateSharedLinkWithSettingsAsync(filePath);

        return new FileData
        {
            FileName = fileMetadata.Name,
            FileUrl = sharedLink.Url
        };
    }

    public async Task DeleteFileAsync(string userId, string fileName)
    {
        var filePath = GetFilePath(userId, fileName);
        
        await _client.Files.DeleteV2Async(filePath);
    }

    private string GetFilePath(string userId, string fileName)
    {
        return $"{_dropboxOptions.BasePath}/{userId}/{fileName}";
    }
}