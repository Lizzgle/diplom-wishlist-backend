using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Api.Options;
using Core.Models;
using Identity.Application.Providers;
using Identity.Contracts.Models;
using Identity.Contracts.Services;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Presentation.Providers;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly UserManager<User> _userManager;
    private readonly IRedisService _redisService;

    public JwtProvider(IOptions<JwtOptions> jwtOptions, UserManager<User> userManager, IRedisService redisService)
    {
        _jwtOptions = jwtOptions.Value;
        _userManager = userManager;
        _redisService = redisService;
    }

    public async Task<string> GenerateJwtAsync(User user, CancellationToken cancellationToken)
    {
        var claims = await _userManager.GetClaimsAsync(user);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: _jwtOptions.ExpirationTime,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken(string userId, DeviceType deviceType, CancellationToken cancellationToken)
    {
        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        await _redisService.SaveRefreshTokenAsync(new SaveRefreshTokenArgs()
        {
            UserId = userId,
            DeviceType = deviceType,
            Token = refreshToken
        }, cancellationToken);

        return refreshToken;
    }

    public async Task<string> GetRefreshTokenForDeviceAsync(string userId, DeviceType deviceType, CancellationToken cancellationToken = default)
    {
        return await _redisService.GetRefreshTokenForDeviceAsync(userId, deviceType, cancellationToken);
    }

    private async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!)
        };

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count > 0)
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        return claims;
    }
}