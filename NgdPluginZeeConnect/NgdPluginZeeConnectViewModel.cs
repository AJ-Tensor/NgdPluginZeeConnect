using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tekla.Structures.Dialog;
using TSD = Tekla.Structures.Datatype;

namespace NgdPluginZeeConnect
{
    public class NgdPluginZeeConnectViewModel : INotifyPropertyChanged
    {
        #region Constructors and Initializers
        public NgdPluginZeeConnectViewModel()
        {
            // This is only needed if there are Enum filled combo boxes
            EnumIntegerConverterProvider = TypeDescriptor.AddAttributes(typeof(TSD.Integer), new TypeConverterAttribute(typeof(Ngd.Dialog.EnumIntegerConverter)));
        }

        ~NgdPluginZeeConnectViewModel()
        {
            // This is only needed if there are Enum filled combo boxes
            if (EnumIntegerConverterProvider != null)
                TypeDescriptor.RemoveProvider(EnumIntegerConverterProvider, typeof(TSD.Integer));
        }
        #endregion

        #region Fields
        // This is only needed if there are Enum filled combo boxes
        private TypeDescriptionProvider EnumIntegerConverterProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        private TSD.String _displayText;
        private TSD.String _boltStandard;
        private TSD.Distance _boltDiameter;
        private TSD.Distance _boltOffset;
        private TSD.Integer _boltRows;
        private TSD.DistanceList _boltSpacing;
        private TSD.DistanceList _boltGage;
        private string _boltGageWatermark;
        private TSD.String _myProfile;
        private TSD.String _myShape;
        private TSD.Integer _componentNumber;
        private TSD.String _componentName;
        private ExampleEnum _myExampleEnum1;
        private ExampleEnum _myExampleEnum2;
        #endregion

        #region Properties
        [StructuresDialog(nameof(DisplayText), typeof(TSD.String))]
        public string DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; OnPropertyChanged(); }
        }
        public string DisplayTextWatermark
        {
            get { return NgdPluginZeeConnectEngine.DefaultValues.DisplayText; }
        }

        [StructuresDialog(nameof(BoltStandard), typeof(TSD.String))]
        public string BoltStandard
        {
            get { return _boltStandard; }
            set { _boltStandard = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(BoltDiameter), typeof(TSD.Distance))]
        public TSD.Distance BoltDiameter
        {
            get { return _boltDiameter; }
            set
            {
                _boltDiameter = value;
                OnPropertyChanged();
                SetBoltGageWatermark();
            }
        }

        [StructuresDialog(nameof(BoltOffset), typeof(TSD.Distance))]
        public TSD.Distance BoltOffset
        {
            get { return _boltOffset; }
            set { _boltOffset = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(BoltRows), typeof(TSD.Integer))]
        public TSD.Integer BoltRows
        {
            get { return _boltRows; }
            set { _boltRows = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(BoltSpacing), typeof(TSD.DistanceList))]
        public TSD.DistanceList BoltSpacing
        {
            get { return _boltSpacing; }
            set { _boltSpacing = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(BoltGage), typeof(TSD.DistanceList))]
        public TSD.DistanceList BoltGage
        {
            get { return _boltGage; }
            set { _boltGage = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// An example of having a watermark that is dependent on UI changes and displays in the current units.
        /// </summary>
        public string BoltGageWatermark
        {
            get { return _boltGageWatermark; }
            set { _boltGageWatermark = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Set the bolt gage watermark based on the default bolt gage for the current bolt size.
        /// </summary>
        private void SetBoltGageWatermark()
        {
            var boltDiameter = this.BoltDiameter;
            if (boltDiameter.Millimeters == 0)
                boltDiameter = new TSD.Distance(NgdPluginZeeConnectEngine.DefaultValues.BoltDiameter);
            // Calcluate the default bolt gage based on the Current Bolt size
            var boltGageInch = Nci.Helper.NCIConnectionStandards.StandardBoltGage(boltDiameter.ConvertTo(TSD.Distance.UnitType.Inch), false);
            var distance = new TSD.Distance(boltGageInch, TSD.Distance.UnitType.Inch);
            this.BoltGageWatermark = distance.ToString();
        }

        [StructuresDialog(nameof(MyProfile), typeof(TSD.String))]
        public string MyProfile
        {
            get { return _myProfile; }
            set { _myProfile = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(MyShape), typeof(TSD.String))]
        public string MyShape
        {
            get { return _myShape; }
            set { _myShape = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(ComponentNumber), typeof(TSD.Integer))]
        public TSD.Integer ComponentNumber
        {
            get { return _componentNumber; }
            set { _componentNumber = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(ComponentName), typeof(TSD.String))]
        public string ComponentName
        {
            get { return _componentName; }
            set { _componentName = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(MyExampleEnum1), typeof(TSD.Integer))]
        public ExampleEnum MyExampleEnum1
        {
            get { return _myExampleEnum1; }
            set { _myExampleEnum1 = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(MyExampleEnum2), typeof(TSD.Integer))]
        public ExampleEnum MyExampleEnum2
        {
            get { return _myExampleEnum2; }
            set { _myExampleEnum2 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
