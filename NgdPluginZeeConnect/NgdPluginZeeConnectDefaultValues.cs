using Nci.Helper;

namespace NgdPluginZeeConnect
{
    public class NgdPluginZeeConnectDefaultValues
    {
        // Set default values in here

        public double Thickness { get; set; } = Distance.Inch2mm(0.25);
        public double Weldsize { get; set; } = Distance.Inch2mm(0.2);
        public int Boltactivation { get; set; } = 4;
        public string BoltStandard { get; set; } = "F1852";
        public double BoltDiameter { get; set; } = Distance.Inch2mm(0.5);
        public double Extension { get; set; } = Distance.Inch2mm(50);
        public double Margin { get; set; } = Distance.Inch2mm(10);
        public string EBoltstandard { get; set; } = "F1852";
        public double EBoltsize { get; set; } = Distance.Inch2mm(0.5);

    }
}
