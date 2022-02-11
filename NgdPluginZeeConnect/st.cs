using Nci.Helper;

namespace NgdPluginZeeConnect
{
    public class NgdPluginZeeConnectDefaultValues
    {
        // Set default values in here

        public string Thickness { get; set; } = "0.25";
        public string Weldsize { get; set; } = "0.2";
        public string Boltactivation { get; set; } = "40";
        public string BoltStandard { get; set; } = "F1852";
        public string BoltDiameter { get; set; } = "0\"1/2";
        public string Extension { get; set; } = "50";
        public string Margin { get; set; } = "10";
        public string EBoltstandard { get; set; } = "F1852";
        public string EBoltsize { get; set; } = "0\"1/2";

        //public string DisplayText { get; set; } = "Hello World";

        //public string BoltStandard { get; set; } = "A325N";

        //public double BoltDiameter { get; set; } = Distance.Inch2mm(0.5);
    }
}
