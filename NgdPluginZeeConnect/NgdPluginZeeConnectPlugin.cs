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
            List<InputDefinition> inputDefinitions = new List<InputDefinition>();
            try
            {
                Picker picker = new Picker();
                // Selection of Primary part as Column or Rafter
                ModelObject _ColumnRafter = picker.PickObject(Picker.PickObjectEnum.PICK_ONE_OBJECT, "Pick Column/Rafter");
                if (_ColumnRafter == null)
                    throw new NciTeklaException("Invalid Selection of Primary part. Expecting a Part or Custom Part object.");

                // Selection of Single or Multiple secondary Part as Purlin or Girt
                ModelObjectEnumerator setofPurlinGirt = picker.PickObjects(Picker.PickObjectsEnum.PICK_N_PARTS, "Pick one or two Girt/Purlin, Press middle mouse button to confirm");
                if (setofPurlinGirt == null)
                    throw new NciTeklaException("Invalid Selection of Secondary part. Expecting a Part or Custom Part object.");
                
                List<Beam> _PurlinGirt = new List<Beam>();
                foreach (var item in setofPurlinGirt)
                {
                    _PurlinGirt.Add(item as Beam);
                }
                InputDefinition Input1 = new InputDefinition(_ColumnRafter.Identifier);
                inputDefinitions.Add(Input1);
                foreach (Beam item in _PurlinGirt)
                {
                    InputDefinition Input = new InputDefinition(item.Identifier);
                    inputDefinitions.Add(Input);
                }
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
           
            DateTime startTime = DateTime.Now;
            try
            {
                ModelObject ColumnRafter = (ModelObject)Model.SelectModelObject((Identifier)input[0].GetInput());
                List<Beam> PurlinGirt = new List<Beam>();

                for (int i = 1; i < input.Count; i++)
                {
                    PurlinGirt.Add((Beam)Model.SelectModelObject((Identifier)input[i].GetInput()));
                }
                // Number of Secondary Part can not More than two.
                if (PurlinGirt.Count > 2)
                {
                    throw new NciTeklaException("Select Maximum 2 Secondary Part");
                }
                // Secondary Part name shoud be purlin or girt
                for (int i = 0; i < PurlinGirt.Count; i++)
                {
                    if (!(PurlinGirt[i].Name == "PURLIN" || PurlinGirt[i].Name == "GIRT"))
                    {
                        throw new NciTeklaException("Secondary part should be purlin or Girt");
                    }                    
                }


                if (ColumnRafter == null)
                    throw new NciTeklaException("Invalid primary object.");
                if (PurlinGirt == null)
                    throw new NciTeklaException("Invalid Secondary object.");

                var engine = new NgdPluginZeeConnectEngine(this.Data);
                engine.Insert(ColumnRafter, PurlinGirt);

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

            return true;
        }
        #endregion
    }
}
