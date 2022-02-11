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

        private TSD.String thickness;
        private TSD.String weld_size;        
        private TSD.String no_of_bolts;
        private TSD.String bolt_standard;
        private TSD.String bolt_size;
        private TSD.String extension;
        private TSD.String end_margin;
        private TSD.String ebolt_standard;
        private TSD.String ebolt_size;
        #endregion

        [StructuresDialog(nameof(Thickness), typeof(TSD.String))]
        public string Thickness
        {
            get { return thickness; }
            set { thickness = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(Weldsize), typeof(TSD.String))]
        public string Weldsize
        {
            get { return weld_size; }
            set { weld_size = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(Boltactivation), typeof(TSD.String))]
        public string Boltactivation
        {
            get { return no_of_bolts; }
            set { no_of_bolts = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(BoltStandard), typeof(TSD.String))]
        public string BoltStandard
        {
            get { return bolt_standard; }
            set { bolt_standard = value; OnPropertyChanged(); }
        }
        [StructuresDialog(nameof(BoltDiameter), typeof(TSD.String))]
        public string BoltDiameter
        {
            get { return bolt_size; }
            set { bolt_size = value; OnPropertyChanged(); }
        }
        [StructuresDialog(nameof(Extension), typeof(TSD.String))]
        public string Extension
        {
            get { return extension; }
            set { extension = value; OnPropertyChanged(); }
        }
        [StructuresDialog(nameof(Margin), typeof(TSD.String))]
        public string Margin
        {
            get { return end_margin; }
            set { end_margin = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(EBoltstandard), typeof(TSD.String))]
        public string EBoltstandard
        {
            get { return ebolt_standard; }
            set { ebolt_standard = value; OnPropertyChanged(); }
        }

        [StructuresDialog(nameof(EBoltsize), typeof(TSD.String))]
        public string EBoltsize
        {
            get { return ebolt_size; }
            set { ebolt_size = value; OnPropertyChanged(); }
        }
        #region Methods
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
