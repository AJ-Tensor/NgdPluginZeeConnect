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
        [StructuresField(nameof(DisplayText))]
        public string DisplayText;

        [StructuresField(nameof(BoltStandard))]
        public string BoltStandard;

        [StructuresField(nameof(BoltDiameter))]
        public double BoltDiameter;

        [StructuresField(nameof(BoltOffset))]
        public double BoltOffset;

        [StructuresField(nameof(BoltRows))]
        public int BoltRows;

        [StructuresField(nameof(BoltSpacing))]
        public string BoltSpacing;

        [StructuresField(nameof(BoltGage))]
        public string BoltGage;

        [StructuresField(nameof(MyProfile))]
        public string MyProfile;

        [StructuresField(nameof(MyShape))]
        public string MyShape;

        [StructuresField(nameof(ComponentNumber))]
        public int ComponentNumber;

        [StructuresField(nameof(ComponentName))]
        public string ComponentName;

        [StructuresField(nameof(MyExampleEnum1))]
        public int MyExampleEnum1;

        [StructuresField(nameof(MyExampleEnum2))]
        public int MyExampleEnum2;
    }
}
