using Nci.Helper;
using Nci.Tekla.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Tekla.Structures;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Plugins;

namespace NgdPluginZeeConnect
{
    [Plugin("NgdPluginZeeConnect")]
    [PluginUserInterface("NgdPluginZeeConnect.NgdPluginZeeConnect")]
    public class NgdPluginZeeConnectPlugin : PluginBase, INciMessageInterface
    {
        #region Properties
        public Model Model { get; set; } = new Model();

        private NgdPluginZeeConnectStructuresData Data { get; set; }

        /// <summary>
        /// An option for what to report
        /// 0 - Errors only; 1 - Errors/Warnings/FinalSuccess; 2 - All Messages
        /// </summary>
        public int ReportVerbosity { get; set; } = 1;

        NciMessageBase _nciMessage;
        NciMessageBase NciMessage
        {
            get
            {
                if (_nciMessage == null)
                    _nciMessage = new NciMessageBase(this);
                return _nciMessage;
            }
        }
        #endregion

        #region Constructor
        public NgdPluginZeeConnectPlugin(NgdPluginZeeConnectStructuresData data)
        {
            this.Data = data;
        }
        #endregion

        #region Overrides
        public override List<InputDefinition> DefineInput()
        {
            List<InputDefinition> input = null;
            try
            {
                var picker = new Tekla.Structures.Model.UI.Picker();
                var part1 = picker.PickObject(Tekla.Structures.Model.UI.Picker.PickObjectEnum.PICK_ONE_PART, "Pick a part.") as Part;
                var insertPoint = picker.PickPoint("Pick an insert point.");

                input = new List<InputDefinition>();
                input.Add(new InputDefinition(part1.Identifier));
                input.Add(new InputDefinition(insertPoint));
            }
            catch (NciTeklaException e)
            {
                if (!NciMessage.WriteErrorNotification(e.Message, e, e.TeklaObjectsAndDescriptions))
                    System.Windows.MessageBox.Show(e.ToString());
            }
            catch (Exception e)
            {
                if (!NciMessage.WriteErrorNotification(e.Message, e, this))
                    System.Windows.MessageBox.Show(e.Message);
            }

            return input;
        }

        public override bool Run(List<InputDefinition> input)
        {
            bool result = false;
            DateTime startTime = DateTime.Now;
            try
            {
                // This is an appropriate time to allow the message file to be renamed if required.
                NciMessage.RenameReportFileWhenRequired();

                if (input.Count < 3)
                    throw new NciTeklaException($"There must be three inputs but only {input.Count} were provided.");

                var primaryInput = input[0];
                if (primaryInput.GetInputType() != InputDefinition.InputTypeEnum.INPUT_ONE_OBJECT)
                    throw new NciTeklaException($"The first input must be a ModelObject but {primaryInput.GetInputType()} was provided.");

                var pointInput = input[1];
                if (pointInput.GetInputType() != InputDefinition.InputTypeEnum.INPUT_ONE_POINT)
                    throw new NciTeklaException($"The third input must be a point but {pointInput.GetInputType()} was provided.");

                var primaryPart = Model.SelectModelObject(primaryInput.GetInput() as Identifier) as Part;
                var insertPoint = pointInput.GetInput() as Point;

                var engine = new NgdPluginZeeConnectEngine(this.Data);

                result = engine.Insert(primaryPart, insertPoint).Count > 0;

                NciMessage.WriteGeneralNotification("Component Complete", this);
            }
            catch (NciTeklaException e)
            {
                if (!NciMessage.WriteErrorNotification(e.Message, e, e.TeklaObjectsAndDescriptions))
                    System.Windows.MessageBox.Show(e.ToString());
            }
            catch (Exception e)
            {
                if (!NciMessage.WriteErrorNotification(e.Message, e, this))
                    System.Windows.MessageBox.Show(e.Message);
            }
            finally
            {
                if (NciMessage.ShouldShowReport)
                    NciMessage.ShowProcessReport(new NotificationType[] { NotificationType.Error, NotificationType.Warning }, startTime);
            }

            return result;
        }
        #endregion

        #region Private methods

        #endregion
    }
}
