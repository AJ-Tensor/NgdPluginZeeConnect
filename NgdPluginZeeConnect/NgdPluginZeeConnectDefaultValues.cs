using Nci.Helper;

namespace NgdPluginZeeConnect
{
    public class NgdPluginZeeConnectDefaultValues
    {
        // Set default values in here

        public string DisplayText { get; set; } = "Hello World";

        public string BoltStandard { get; set; } = "A325N";

        public double BoltDiameter { get; set; } = Distance.Inch2mm(0.5);
    }
}
