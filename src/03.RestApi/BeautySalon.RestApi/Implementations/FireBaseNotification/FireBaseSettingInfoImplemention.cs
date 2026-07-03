using BeautySalon.Common.Dtos;
using BeautySalon.Common.Interfaces;

namespace BeautySalon.RestApi.Implementations.FireBaseNotification;

public class FireBaseSettingInfoImplemention : IFireBaseSettingInfo
{
    public FireBaseSettingInfoDto FireBaseSettingInfo { get; private set; }

    public FireBaseSettingInfoImplemention(string environment, string contentRootPath)
    {

        var config = new ConfigurationBuilder()
              .SetBasePath(contentRootPath)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();
        FireBaseSettingInfo = new FireBaseSettingInfoDto
        {
            ServerKey = config["FireBaseSetting:FireBase_ServerKey"]!,
            SenderId = config["FireBaseSetting:FireBase_SenderId"]!
        };
    }
}
