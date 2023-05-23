using Nebula.Modules.Configurations.Subscriptions;
using System.Diagnostics;

namespace Testings.Modules.Configurations.Subscriptions;

[TestClass]
public class MachineUUIDTests
{
    private static readonly TraceSource logger = new TraceSource("MachineUUIDTests");

    [TestMethod]
    public void GetProcessorId_ReturnsValidProcessorId()
    {
        MachineUUID machineUUID = new MachineUUID();
        string processorId = machineUUID.GetProcessorId();
        logger.TraceInformation("Processor ID: " + processorId);
        Assert.IsNotNull(processorId);
        Assert.AreNotEqual(string.Empty, processorId);
    }
}
