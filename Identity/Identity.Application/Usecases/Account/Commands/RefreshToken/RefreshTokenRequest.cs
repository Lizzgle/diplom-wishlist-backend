namespace Identity.Application.Usecases.Account.Commands.RefreshToken;

public class RefreshTokenRequest
{
    public required string RefreshToken { get; set; }
    
    public required string Id { get; set; }
}