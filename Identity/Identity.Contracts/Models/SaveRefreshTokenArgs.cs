using Common.Models;

namespace Identity.Contracts.Models;

public class SaveRefreshTokenArgs
{
    public string UserId { get; set; }
    
    public DeviceType DeviceType { get; set; }
    
    public string Token { get; set; }
}