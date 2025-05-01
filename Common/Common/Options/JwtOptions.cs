namespace Common.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    
    public string? SecretKey { get; set; }

    public string? Audience { get; set; }

    public string? Issuer { get; set; }

    public DateTime? ExpirationTime { get; set; }

}