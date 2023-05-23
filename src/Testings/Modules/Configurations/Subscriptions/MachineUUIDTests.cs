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
}
