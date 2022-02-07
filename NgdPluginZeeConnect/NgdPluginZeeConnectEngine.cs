using Nci.Helper;
using Nci.Tekla.Model;
using Ngd.Tekla.Geometry3d.Extension;
using Ngd.Tekla.Model.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Plugins;
using TSD = Tekla.Structures.Datatype;

namespace NgdPluginZeeConnect
{
    public class NgdPluginZeeConnectEngine
    {
        #region Fields
        private static NgdPluginZeeConnectDefaultValues _defaultValues;
        #endregion

        #region Properties
        public string DisplayText { get; set; } = DefaultValues.DisplayText;
        public string BoltStandard { get; set; } = DefaultValues.BoltStandard;
        public double BoltDiameter { get; set; } = DefaultValues.BoltDiameter;
        public double BoltOffset { get; set; }
        public TSD.DistanceList BoltSpacing { get; set; }
        public TSD.DistanceList BoltGage { get; set; }
        public string MyProfile { get; set; }
        public string MyShape { get; set; }
        public int ComponentNumber { get; set; }
        public string ComponentName { get; set; }
        public ExampleEnum MyExampleEnum1 { get; set; }
        public ExampleEnum MyExampleEnum2 { get; set; }

        public static NgdPluginZeeConnectDefaultValues DefaultValues
        {
            get
            {
                if (_defaultValues == null)
                {
                    // Read default values in

                    // Create default values if reading failed
                    _defaultValues = new NgdPluginZeeConnectDefaultValues();
                }
                return _defaultValues;
            }
        }

        #endregion

        #region Constructors and Initializers

        public NgdPluginZeeConnectEngine(NgdPluginZeeConnectStructuresData data)
        {
            SetData(data);
        }

        public void SetData(NgdPluginZeeConnectStructuresData data)
        {
            if (!TeklaHelper.IsDefaultValue(data.DisplayText))
                DisplayText = data.DisplayText;

            if (!TeklaHelper.IsDefaultValue(data.BoltStandard))
                BoltStandard = data.BoltStandard;

            if (!TeklaHelper.IsDefaultValue(data.BoltDiameter))
                BoltDiameter = data.BoltDiameter;

            // TODO: If bolt offset is zero or less then this should be changed to use the default offset based on bolt size 
            if (!TeklaHelper.IsDefaultValue(data.BoltOffset))
                BoltOffset = data.BoltOffset;

            if (!TeklaHelper.IsDefaultValue(data.BoltSpacing))
                BoltSpacing = TSD.DistanceList.Parse(data.BoltSpacing);
            if (BoltSpacing.Count < data.BoltRows)
            {
                // TODO: If bolt quantity is specified but no spacing, or more bolts specified than spacing then additional
                // bolts should be added to match the quantity using the default spacing per bolt size
                ;
            }

            if (!TeklaHelper.IsDefaultValue(data.BoltGage))
                BoltGage = TSD.DistanceList.Parse(data.BoltGage);

            MyProfile = data.MyProfile;

            MyShape = data.MyShape;

            ComponentNumber = data.ComponentNumber;
            ComponentName = data.ComponentName;

            if (Enum.IsDefined(typeof(ExampleEnum), data.MyExampleEnum1))
                MyExampleEnum1 = (ExampleEnum)data.MyExampleEnum1;
            else
                MyExampleEnum1 = Ngd.Dialog.TypeExtension.GetDefaultValue<ExampleEnum>();

            if (Enum.IsDefined(typeof(ExampleEnum), data.MyExampleEnum2))
                MyExampleEnum2 = (ExampleEnum)data.MyExampleEnum2;
            else
                MyExampleEnum2 = Ngd.Dialog.TypeExtension.GetDefaultValue<ExampleEnum>();
        }

        #endregion

        #region Insert Methods

        public List<ModelObject> Insert(Part primaryPart, Point insertPoint)
        {
            var addedObjects = new List<ModelObject>();

            /* Add logic here for producing the proper connection */

            return addedObjects;
        }
        #endregion

        #region Support Methods

        #endregion
    }
}
