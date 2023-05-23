using Nebula.Modules.Configurations.Subscriptions;

namespace Testings.Modules.Configurations.Subscriptions;

[TestClass]
public class MachineUUIDTests
{
    [TestMethod]
    public void GetProcessorId_ReturnsValidProcessorId()
    {
        MachineUUID machineUUID = new MachineUUID();
        string processorId = machineUUID.GetProcessorId();
        Assert.IsNotNull(processorId);
        Assert.AreNotEqual(string.Empty, processorId);
    }

    [TestMethod]
    public void GetHardDriveModel_ReturnsValidModel()
    {
        MachineUUID hardDriveModel = new MachineUUID();
        string model = hardDriveModel.GetHardDriveModel();
        Assert.IsNotNull(model);
        Assert.AreNotEqual(string.Empty, model);
    }

    [TestMethod]
    public void GetHardDriveSerialNumber_ReturnsValidSerialNumber()
    {
        MachineUUID hardwareDriveSerialNumber = new MachineUUID();
        string serialNumber = hardwareDriveSerialNumber.GetHardDriveSerialNumber();
        Assert.IsNotNull(serialNumber);
        Assert.AreNotEqual(string.Empty, serialNumber);
    }
}
