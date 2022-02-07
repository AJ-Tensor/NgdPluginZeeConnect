using Ngd.Dialog;
using System;
using Tekla.Structures.Dialog;

namespace NgdPluginZeeConnect
{
    /// <summary>
    /// Interaction logic for NgdPluginZeeConnectWPF.xaml
    /// </summary>
    public partial class NgdPluginZeeConnect : PluginWindowBase
    {
        NgdPluginZeeConnectViewModel DataModel;
        public NgdPluginZeeConnect(NgdPluginZeeConnectViewModel dataModel)
        {
            InitializeComponent();
            DataModel = dataModel;
            // ContentRendered is called once before window is shown
            ContentRendered += (s, e) => UncheckFiltersIfSet();
            // AttributesLoadedFromModel is called each time data is loaded from an instance of this plugin
            AttributesLoadedFromModel += (s, e) => UncheckFiltersIfSet();
        }
        /// <summary>
        /// If AttributeFiltersUncheckedByDefault flag is set then uncheck all filter checkboxes.
        /// </summary>
        private void UncheckFiltersIfSet()
        {
            if (Nci.Tekla.Model.TeklaOption.AttributeFiltersUncheckedByDefault())
                this.ForceToggleSelection(false);
        }
        /// <summary>
        /// Toggle all filter checkboxes to off by toggling them then checking one and if checked
        /// then toggle again.
        /// </summary>
        private void ForceToggleSelection(bool check)
        {
            this.ToggleSelection();
            var checkbox = this.GetChildOfType<Tekla.Structures.Dialog.UIControls.WpfFilterCheckBox>();
            if ((checkbox?.IsChecked ?? false) != check)
                this.ToggleSelection();
        }

        private void WpfOkApplyModifyGetOnOffCancel_ApplyClicked(object sender, EventArgs e)
        {
            Apply();
        }

        private void WpfOkApplyModifyGetOnOffCancel_OkClicked(object sender, EventArgs e)
        {
            Apply();
            Close();
        }

        private void WpfOkApplyModifyGetOnOffCancel_ModifyClicked(object sender, EventArgs e)
        {
            Modify();
        }

        private void WpfOkApplyModifyGetOnOffCancel_GetClicked(object sender, EventArgs e)
        {
            Get();
        }

        private void WpfOkApplyModifyGetOnOffCancel_OnOffClicked(object sender, EventArgs e)
        {
            ToggleSelection();
        }

        private void WpfOkApplyModifyGetOnOffCancel_CancelClicked(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Link F1 help to the same help topic as the SaveLoad control.
        /// </summary>
        private void HelpExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            HelpViewer.DisplayHelpTopic(SaveLoad.HelpKeyword);
        }

        private void MyProfileCatalog_SelectClicked(object sender, EventArgs e)
        {
            MyProfileCatalog.SelectedProfile = DataModel.MyProfile;
        }

        private void MyProfileCatalog_SelectionDone(object sender, EventArgs e)
        {
            DataModel.MyProfile = MyProfileCatalog.SelectedProfile;
        }

        private void MyShapeCatalog_SelectClicked(object sender, EventArgs e)
        {
            MyShapeCatalog.SelectedShape = DataModel.MyShape;
        }

        private void MyShapeCatalog_SelectionDone(object sender, EventArgs e)
        {
            DataModel.MyShape = MyShapeCatalog.SelectedShape;
        }

        private void ComponentCatalog_SelectClicked(object sender, EventArgs e)
        {
            ComponentCatalog.SelectedNumber = DataModel.ComponentNumber;
            ComponentCatalog.SelectedName = DataModel.ComponentName;
        }

        private void ComponentCatalog_SelectionDone(object sender, EventArgs e)
        {
            DataModel.ComponentNumber = ComponentCatalog.SelectedNumber;
            DataModel.ComponentName = ComponentCatalog.SelectedName;
        }
    }
}
