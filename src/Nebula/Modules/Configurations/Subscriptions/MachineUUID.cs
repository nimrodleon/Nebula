using System.Management;

namespace Nebula.Modules.Configurations.Subscriptions;

public class MachineUUID
{
    public string GetUUID()
    {
        //string deviceId = new DeviceIdBuilder()
        //    .AddMachineName().AddUserName()
        //    .AddMacAddress(excludeWireless: true)
        //    .AddOsVersion().ToString();
        //return deviceId;
        return $"{GetProcessorId()}:-";
    }

    public string GetProcessorId()
    {
        string processorId = string.Empty;
        try
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
            ManagementObjectCollection collection = searcher.Get();

            foreach (ManagementObject obj in collection)
            {
                processorId = obj["ProcessorId"].ToString();
                break; // Obtener solo el primer identificador de procesador
            }
        }
        catch (Exception ex)
        {
            // Manejo de excepciones
        }

        return processorId ;
    }
}
