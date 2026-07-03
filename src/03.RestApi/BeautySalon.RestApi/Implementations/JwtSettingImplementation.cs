using BeautySalon.Common.Dtos;
using BeautySalon.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BeautySalon.RestApi.Implementations;

public class JwtSettingImplementation : IJwtSettingService
{
    public JwtSettingDto JwtSetting { get; }

    public JwtSettingImplementation(string contentRootPath)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(contentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        JwtSetting = new JwtSettingDto
        {
            Key = config["JwtConfig:Key"]!,
            Issuer = config["JwtConfig:Issuer"]!,
            Audience = config["JwtConfig:Audience"]!,
            AccessTokenExpirationMinutes =
                int.Parse(config["JwtConfig:AccessTokenExpirationMinutes"]!),
            RefreshTokenExpirationDays =
                int.Parse(config["JwtConfig:RefreshTokenExpirationDays"]!)
        };
    }
}