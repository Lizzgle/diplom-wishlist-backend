namespace Core.Api.Options;

public class UrlOptions
{
    public const string SectionName = "Url";
    
    public string? ClientUrl { get; set; }
    
    public string? ServerUrl { get; set; }
    
    public string? IdentityUrl { get; set; }
}