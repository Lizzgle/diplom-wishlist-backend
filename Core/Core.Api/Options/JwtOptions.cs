namespace Core.Api.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    
    public string? Key { get; set; }

    public string? Audience { get; set; }

    public string? Issuer { get; set; }

    public DateTime? ExpirationTime { get; set; }

}