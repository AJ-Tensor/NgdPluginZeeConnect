using Ngd.Dialog;
using System;
using System.ComponentModel;
using Tekla.Structures.Plugins;

namespace NgdPluginZeeConnect
{
    [DefaultValue(Value2)]
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ExampleEnum
    {
        [ImageName("Value1.bmp")]
        Value1,
        [ImageName("Value2.bmp")]
        [Description("Value 2 (default)")]
        Value2,
        [Obsolete]
        Value3,
        [ImageName("Value4.bmp")]
        Value4
    }

    public class NgdPluginZeeConnectStructuresData
    {
        [StructuresField(nameof(Thickness))]
        public double Thickness;
        [StructuresField(nameof(Weldsize))]
        public double Weldsize;
        [StructuresField(nameof(Boltactivation))]
        public int Boltactivation;
        [StructuresField(nameof(BoltStandard))]
        public string BoltStandard;
        [StructuresField(nameof(BoltDiameter))]
        public double BoltDiameter;

        [StructuresField(nameof(Extension))]
        public double Extension;
        [StructuresField(nameof(Margin))]
        public double Margin;
        [StructuresField(nameof(EBoltstandard))]
        public string EBoltstandard;
        [StructuresField(nameof(EBoltsize))]
        public double EBoltsize;

       
    }
}
