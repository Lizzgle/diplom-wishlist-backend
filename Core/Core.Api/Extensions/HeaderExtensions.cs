using Core.Exceptions;
using Core.Models;
using Microsoft.AspNetCore.Http;

namespace Core.Api.Extensions;

public static class HeaderExtensions
{
    public static DeviceType GetDeviceType(this IHeaderDictionary headers)
    {
        var deviceType = headers["Device-Type"].ToString();
        
        if (deviceType is null)
            throw new InvalidAuthException("Device type is missing");
        
        return deviceType.ToLower() switch
        {
            "mobile" => DeviceType.Mobile,
            "web" => DeviceType.Web,
            _ => DeviceType.Web,
        };
    }
}