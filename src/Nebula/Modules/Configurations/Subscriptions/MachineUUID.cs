using DeviceId;

namespace Nebula.Modules.Configurations.Subscriptions;

public class MachineUUID
{
    public string GetUUID()
    {
        string deviceId = new DeviceIdBuilder()
            .AddMachineName().AddUserName()
            .AddMacAddress(excludeWireless: true)
            .AddOsVersion().ToString();
        return deviceId;
    }
}
