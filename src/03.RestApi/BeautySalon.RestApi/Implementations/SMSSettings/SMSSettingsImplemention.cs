using BeautySalon.Common.Dtos;
using BeautySalon.Common.Interfaces;

namespace BeautySalon.RestApi.Implementations.SMSSettings;

public class SMSSettingsImplementation : ISMSSetting
{
    public SMSInformationDto SMSSettings { get; }

    public SMSSettingsImplementation(string environment, string contentRootPath)
    {

        var config = new ConfigurationBuilder()
            .SetBasePath(contentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();


        var SMSKey = config["SMSSettings:SMSReza_Key"]!;
        var ProviderNumber = config["SMSSettings:ProviderNumber"];
        var OtpBodyIdShared = config["SMSSettings:OtpBodyIdShared"]!;

        SMSSettings = new SMSInformationDto
        {
            SMSKey = config["SMSSettings:SMSReza_Key"]!,
            ProviderNumber = config["SMSSettings:ProviderNumber"]!,
            OtpBodyIdShared = int.Parse(config["SMSSettings:OtpBodyIdShared"]!),
            BaseUrl = config["SMSSettings:BaseUrl"]!,
            RegisterClient = int.Parse(config["SMSSettings:RegisterClient"]!),
            RemindingAppointment = int.Parse(config["SMSSettings:RemindingAppointment"]!),
        };
    }
}