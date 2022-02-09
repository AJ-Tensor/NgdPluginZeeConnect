using Nci.Helper;
using Nci.Tekla.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Tekla.Structures;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Plugins;
using Tekla.Structures.Model.UI;

namespace NgdPluginZeeConnect
{
    [Plugin("PluginZeeConnect")]
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
            //List<InputDefinition> input = null;
            List<InputDefinition> inputDefinitions = new List<InputDefinition>();
            try
            {
                


                Picker picker = new Picker();
                ModelObject _column = picker.PickObject(Picker.PickObjectEnum.PICK_ONE_OBJECT, "Pick Column/Rafter");
                ModelObjectEnumerator setOfpurlin = picker.PickObjects(Picker.PickObjectsEnum.PICK_N_PARTS, "Pick one or two Girt/Purlin, Press middle mouse button to confirm");

                List<Beam> _purlins = new List<Beam>();
                foreach (var item in setOfpurlin)
                {
                    _purlins.Add(item as Beam);
                }

                InputDefinition Input1 = new InputDefinition(_column.Identifier);
                inputDefinitions.Add(Input1);

                foreach (Beam item in _purlins)
                {
                    InputDefinition Input = new InputDefinition(item.Identifier);
                    inputDefinitions.Add(Input);
                }

               
                //var picker = new Tekla.Structures.Model.UI.Picker();
                //var part1 = picker.PickObject(Tekla.Structures.Model.UI.Picker.PickObjectEnum.PICK_ONE_PART, "Pick a part.") as Part;
                //var insertPoint = picker.PickPoint("Pick an insert point.");

                //input = new List<InputDefinition>();
                //input.Add(new InputDefinition(part1.Identifier));
                //input.Add(new InputDefinition(insertPoint));
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

            return inputDefinitions;
        }

        public override bool Run(List<InputDefinition> input)
        {
            bool result = false;
            DateTime startTime = DateTime.Now;
            try
            {
                ModelObject column = (ModelObject)Model.SelectModelObject((Identifier)input[0].GetInput());
                List<Beam> purlins = new List<Beam>();

                for (int i = 1; i < input.Count; i++)
                {
                    purlins.Add((Beam)Model.SelectModelObject((Identifier)input[i].GetInput()));
                }

                //// This is an appropriate time to allow the message file to be renamed if required.
                //NciMessage.RenameReportFileWhenRequired();

                //if (input.Count < 3)
                //    throw new NciTeklaException($"There must be three inputs but only {input.Count} were provided.");

                //var primaryInput = input[0];
                //if (primaryInput.GetInputType() != InputDefinition.InputTypeEnum.INPUT_ONE_OBJECT)
                //    throw new NciTeklaException($"The first input must be a ModelObject but {primaryInput.GetInputType()} was provided.");

                //var pointInput = input[1];
                //if (pointInput.GetInputType() != InputDefinition.InputTypeEnum.INPUT_ONE_POINT)
                //    throw new NciTeklaException($"The third input must be a point but {pointInput.GetInputType()} was provided.");

                //var primaryPart = Model.SelectModelObject(primaryInput.GetInput() as Identifier) as Part;
                //var insertPoint = pointInput.GetInput() as Point;

                var engine = new NgdPluginZeeConnectEngine(this.Data);

                result = engine.Insert(column, purlins).Count > 0;

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
