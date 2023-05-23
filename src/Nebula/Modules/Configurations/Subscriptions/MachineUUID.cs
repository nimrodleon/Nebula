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

        return processorId.Trim();
    }

    public string GetHardDriveModel()
    {
        string model = string.Empty;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Model FROM Win32_DiskDrive");

        foreach (ManagementObject obj in searcher.Get())
        {
            model = obj["Model"].ToString();
            break; // Solo obtenemos el primer modelo encontrado
        }

        return model.Trim();
    }

    public string GetHardDriveSerialNumber()
    {
        string serialNumber = string.Empty;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive");

        foreach (ManagementObject obj in searcher.Get())
        {
            serialNumber = obj["SerialNumber"].ToString();
            break; // Solo obtenemos el primer número de serie encontrado
        }

        return serialNumber.Trim();
    }

    public string GetMotherboardProductInfo()
    {
        string productInfo = string.Empty;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Product FROM Win32_BaseBoard");

        foreach (ManagementObject obj in searcher.Get())
        {
            productInfo = obj["Product"].ToString();
            break; // Solo obtenemos la información del primer producto encontrado
        }

        return productInfo.Trim();
    }

}
