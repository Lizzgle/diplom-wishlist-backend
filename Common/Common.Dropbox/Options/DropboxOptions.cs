namespace Common.Dropbox.Options;

public class DropboxOptions
{
    public const string SectionName = "Dropbox";
    
    public required string BasePath { get; set; }

    public required string ClientId { get; set; }

    public required string ClientSecret { get; set; }

    public required string RefreshToken { get; set; }

    public required string TokenUrl { get; set; }
}