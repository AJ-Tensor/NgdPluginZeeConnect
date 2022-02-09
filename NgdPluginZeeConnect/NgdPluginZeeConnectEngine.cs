using Nci.Helper;
using Nci.Tekla.Model;
using Ngd.Tekla.Geometry3d.Extension;
using Ngd.Tekla.Model.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSG = Tekla.Structures.Geometry3d;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Solid;
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
        Model Model = new Model();
        public string Thickness { get; set; } = DefaultValues.Thickness;
        public string Weldsize { get; set; } = DefaultValues.Weldsize;
        public string Boltactivation { get; set; } = DefaultValues.Boltactivation;
        public string BoltStandard { get; set; } = DefaultValues.BoltStandard;
        public string BoltDiameter { get; set; } = DefaultValues.BoltDiameter;
        public string Extension { get; set; } = DefaultValues.Extension;
        public string Margin { get; set; } = DefaultValues.Margin;
        public string EBoltsize { get; set; } = DefaultValues.EBoltsize;
        public string EBoltstandard { get; set; } = DefaultValues.EBoltstandard;


        //public string DisplayText { get; set; } = DefaultValues.DisplayText;
        //public string BoltStandard { get; set; } = DefaultValues.BoltStandard;
        //public double BoltDiameter { get; set; } = DefaultValues.BoltDiameter;
        //public double BoltOffset { get; set; }
        //public TSD.DistanceList BoltSpacing { get; set; }
        //public TSD.DistanceList BoltGage { get; set; }
        //public string MyProfile { get; set; }
        //public string MyShape { get; set; }
        //public int ComponentNumber { get; set; }
        //public string ComponentName { get; set; }
        //public ExampleEnum MyExampleEnum1 { get; set; }
        //public ExampleEnum MyExampleEnum2 { get; set; }

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
            if (!TeklaHelper.IsDefaultValue(data.Thickness))
                Thickness = data.Thickness;
            if (!TeklaHelper.IsDefaultValue(data.Weldsize))
                Weldsize = data.Weldsize;
            if (!TeklaHelper.IsDefaultValue(data.Boltactivation))
                Boltactivation = data.Boltactivation;
            if (!TeklaHelper.IsDefaultValue(data.BoltStandard))
                BoltStandard = data.BoltStandard;
            if (!TeklaHelper.IsDefaultValue(data.BoltDiameter))
                BoltDiameter = data.BoltDiameter;
            if (!TeklaHelper.IsDefaultValue(data.Extension))
                Extension = data.Extension;
            if (!TeklaHelper.IsDefaultValue(data.Margin))
                Margin = data.Margin;
            if (!TeklaHelper.IsDefaultValue(data.EBoltstandard))
                EBoltstandard = data.EBoltstandard;
            if (!TeklaHelper.IsDefaultValue(data.EBoltsize))
                EBoltsize = data.EBoltsize;

            //if (!TeklaHelper.IsDefaultValue(data.DisplayText))
            //    DisplayText = data.DisplayText;

            //if (!TeklaHelper.IsDefaultValue(data.BoltStandard))
            //    BoltStandard = data.BoltStandard;

            //if (!TeklaHelper.IsDefaultValue(data.BoltDiameter))
            //    BoltDiameter = data.BoltDiameter;

            //// TODO: If bolt offset is zero or less then this should be changed to use the default offset based on bolt size 
            //if (!TeklaHelper.IsDefaultValue(data.BoltOffset))
            //    BoltOffset = data.BoltOffset;

            //if (!TeklaHelper.IsDefaultValue(data.BoltSpacing))
            //    BoltSpacing = TSD.DistanceList.Parse(data.BoltSpacing);
            //if (BoltSpacing.Count < data.BoltRows)
            //{
            //    // TODO: If bolt quantity is specified but no spacing, or more bolts specified than spacing then additional
            //    // bolts should be added to match the quantity using the default spacing per bolt size
            //    ;
            //}

            //if (!TeklaHelper.IsDefaultValue(data.BoltGage))
            //    BoltGage = TSD.DistanceList.Parse(data.BoltGage);

            //MyProfile = data.MyProfile;

            //MyShape = data.MyShape;

            //ComponentNumber = data.ComponentNumber;
            //ComponentName = data.ComponentName;

            //if (Enum.IsDefined(typeof(ExampleEnum), data.MyExampleEnum1))
            //    MyExampleEnum1 = (ExampleEnum)data.MyExampleEnum1;
            //else
            //    MyExampleEnum1 = Ngd.Dialog.TypeExtension.GetDefaultValue<ExampleEnum>();

            //if (Enum.IsDefined(typeof(ExampleEnum), data.MyExampleEnum2))
            //    MyExampleEnum2 = (ExampleEnum)data.MyExampleEnum2;
            //else
            //    MyExampleEnum2 = Ngd.Dialog.TypeExtension.GetDefaultValue<ExampleEnum>();
        }

        #endregion

        #region Insert Methods

        public List<ModelObject> Insert(ModelObject column, List<Beam> purlins)
        {
            
            var addedObjects = new List<ModelObject>();

            /* Add logic here for producing the proper connection */
            bool HR = false, BU = false;

            if (ChildrenBeam(column).Count == 0)
            {
                HR = true;
            }
            else
            {
                BU = true;
            }
            if (BU)
            {
                if (ChildrenBeam(column)[0].GetAssembly().Name == "COLUMN" || ChildrenBeam(column)[0].GetAssembly().Name == "RAFTER")
                {

                    if (purlins.Count <= 2)
                    {
                        BUPrimary(column, purlins);


                    }
                    else
                    {
                        throw new Exception("Select One or two Purlin/Girt");
                    }
                }
                else
                {
                    throw new Exception("Select Primary part as COLUMN/RAFTER");
                }
                
               
            }
            if (HR)
            {
                Beam HRColumn = column as Beam;
                //SW.MessageBox.Show(HRColumn.GetAssembly().Name);
                if (HRColumn.GetAssembly().Name == "COLUMN" || HRColumn.GetAssembly().Name == "RAFTER")
                {

                    if (purlins.Count <= 2)
                    {
                        HRPrimary(column, purlins);

                    }
                    else
                    {
                        throw new Exception("Select One or two Purlin/Girt");
                    }
                }
                else
                {
                    throw new Exception("Select Primary part as COLUMN/RAFTER");
                }
                
            }


            return addedObjects;
        }
        #endregion

        #region Support Methods
        public void BUPrimary(ModelObject column, List<Beam> purlins)
        {
            #region Face Detection           
            Line centerLinePurlin = CenterLine(purlins[0]);
            Beam Finalbeam = ClosestChildrenBeam(column, centerLinePurlin);

            Line centreLineFinalBeam = CenterLine(Finalbeam);
            Face Originface = BUClosestFacetoLine(Finalbeam, centerLinePurlin);
            Face PurlinGirtface = HRClosestFacetoLine(purlins[0], centreLineFinalBeam);
            #endregion

            #region Coordinatesystem

            GraphicsDrawer graphicsDrawer = new GraphicsDrawer();
            Tekla.Structures.Model.UI.Color color = new Tekla.Structures.Model.UI.Color(0, 0, 1);

            Point OriginLinept1 = Intersection.LineToPlane(MaxLinesinFace(Originface)[0], PlanefromFace(PurlinGirtface));
            Point OriginLinept2 = Intersection.LineToPlane(MaxLinesinFace(Originface)[1], PlanefromFace(PurlinGirtface));

            double centredistance = 0.5 * (TSG.Distance.PointToPoint(OriginLinept1, OriginLinept2));

            graphicsDrawer.DrawLineSegment(OriginLinept1, OriginLinept2, color);

            CoordinateSystem localCoordinateSystem = new CoordinateSystem();
            localCoordinateSystem.Origin = CentrePoint(OriginLinept1, OriginLinept2);
            localCoordinateSystem.AxisX = Originface.Normal;
            localCoordinateSystem.AxisY = PurlinGirtface.Normal;

            #endregion


            WorkPlaneHandler myWorkPlaneHandler = Model.GetWorkPlaneHandler();
            TransformationPlane currentPlane = myWorkPlaneHandler.GetCurrentTransformationPlane();
            TransformationPlane localplane = new TransformationPlane(localCoordinateSystem); ;
            myWorkPlaneHandler.SetCurrentTransformationPlane(localplane);



            #region Offset Calculcation
            List<double> distanceFromOrigin = new List<double>();
            for (int j = 0; j < MaxLinesinFace(PurlinGirtface).Count; j++)
            {
                distanceFromOrigin.Add(TSG.Distance.PointToLine(localCoordinateSystem.Origin, MaxLinesinFace(PurlinGirtface)[j]));
            }
            double MaxDistanceofPurlin = distanceFromOrigin.Max();
            double MinDistanceofPurlin = distanceFromOrigin.Min();
            #endregion

            #region Extension and End bolting
            bool endBoltsactivaiton = false;
            if (purlins.Count == 2)
            {
                endBoltsactivaiton = true;
            }

            for (int i = 0; i < purlins.Count; i++)
            {
                #region Side Detection Fitting
                FittingExtension(purlins[i], MinDistanceofPurlin, MaxDistanceofPurlin, endBoltsactivaiton, Convert.ToDouble(Extension), Convert.ToDouble(Margin));
                #endregion
            }
            #endregion


            #region Plate
            double plate_thickness = Convert.ToDouble(Thickness);
            ContourPoint point = new ContourPoint(new Point(0, 0, centredistance), null);
            ContourPoint point1 = new ContourPoint(new Point(MinDistanceofPurlin + inch2mm(6.75), 0, centredistance), null);
            ContourPoint point2 = new ContourPoint(new Point(MinDistanceofPurlin + inch2mm(6.75), 0, -centredistance), null);
            ContourPoint point3 = new ContourPoint(new Point(0, 0, -centredistance), null);
            ContourPlate CP = new ContourPlate();
            CP.AddContourPoint(point);
            CP.AddContourPoint(point1);
            CP.AddContourPoint(point2);
            CP.AddContourPoint(point3);
            CP.Profile.ProfileString = "PL" + inch2mm(plate_thickness);
            CP.Material.MaterialString = "A36";
            CP.Position.Depth = Position.DepthEnum.FRONT;
            CP.Class = "2";
            CP.Insert();
            #endregion

            Welds(Finalbeam, CP, Convert.ToDouble(Weldsize));


            #region BoltMain
            BoltArray boltArray1 = new BoltArray();
            for (int i = 0; i < purlins.Count; i++)
            {
                boltArray1.PartToBeBolted = purlins[i];
            }
            boltArray1.PartToBoltTo = CP;
            boltArray1.FirstPosition = new Point(MinDistanceofPurlin + inch2mm(6.75), 0, centredistance);
            boltArray1.SecondPosition = new Point(MinDistanceofPurlin, 0, -centredistance);
            //boltArray1.BoltSize = inch2mm(Convert.ToDouble(_Bolt_size));
            boltArray1.BoltSize = inch2mm(Convertstring(BoltDiameter));
            boltArray1.Tolerance = inch2mm(0.25);
            boltArray1.BoltStandard = BoltStandard;
            boltArray1.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
            boltArray1.CutLength = inch2mm(10);
            boltArray1.ExtraLength = inch2mm(1);
            boltArray1.ThreadInMaterial = BoltGroup.BoltThreadInMaterialEnum.THREAD_IN_MATERIAL_NO;
            boltArray1.Position.Depth = Position.DepthEnum.MIDDLE;
            boltArray1.Position.Plane = Position.PlaneEnum.MIDDLE;
            boltArray1.Position.Rotation = Position.RotationEnum.TOP;
            if (Convert.ToDouble(Boltactivation) == 0)
            {
                boltArray1.Bolt = false;
            }
            else
            {
                boltArray1.Bolt = true;
            }
            boltArray1.Washer1 = true;
            boltArray1.Washer2 = true;
            boltArray1.Washer3 = true;

            boltArray1.Nut1 = true;
            boltArray1.Nut2 = true;
            boltArray1.Hole1 = true;
            boltArray1.Hole2 = true;
            boltArray1.Hole3 = true;
            boltArray1.Hole4 = true;
            boltArray1.Hole5 = true;
            boltArray1.AddBoltDistX(inch2mm(5.66));
            double offsetx = 0.5 * (Math.Sqrt(Math.Pow(6.75, 2) + Math.Pow(2 * mm2inch(centredistance), 2)) - Math.Sqrt(32));

            boltArray1.StartPointOffset.Dx = inch2mm(offsetx);
            boltArray1.AddBoltDistY(inch2mm(0));
            boltArray1.Insert();

            BoltArray boltArray2 = new BoltArray();
            for (int i = 0; i < purlins.Count; i++)
            {
                boltArray2.PartToBeBolted = purlins[i];
            }
            boltArray2.PartToBoltTo = CP;
            boltArray2.FirstPosition = new Point(MinDistanceofPurlin + inch2mm(6.75), 0, -centredistance);
            boltArray2.SecondPosition = new Point(MinDistanceofPurlin, 0, centredistance);
            //boltArray2.BoltSize = inch2mm(Convert.ToDouble(_Bolt_size));
            boltArray2.BoltSize = inch2mm(Convertstring(BoltDiameter));

            boltArray2.Tolerance = inch2mm(0.25);
            boltArray2.BoltStandard = BoltStandard;
            boltArray2.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
            boltArray2.CutLength = inch2mm(10);
            boltArray2.ExtraLength = inch2mm(1);
            boltArray2.ThreadInMaterial = BoltGroup.BoltThreadInMaterialEnum.THREAD_IN_MATERIAL_NO;
            boltArray2.Position.Depth = Position.DepthEnum.MIDDLE;
            boltArray2.Position.Plane = Position.PlaneEnum.MIDDLE;
            boltArray2.Position.Rotation = Position.RotationEnum.TOP;
            if (Convert.ToDouble(Boltactivation) == 0 || Convert.ToDouble(Boltactivation) == 2)
            {
                boltArray2.Bolt = false;
            }
            else
            {
                boltArray2.Bolt = true;
            }
            boltArray2.Washer1 = true;
            boltArray2.Washer2 = true;
            boltArray2.Washer3 = true;
            boltArray2.Nut1 = true;
            boltArray2.Nut2 = true;
            boltArray2.Hole1 = true;
            boltArray2.Hole2 = true;
            boltArray2.Hole3 = true;
            boltArray2.Hole4 = true;
            boltArray2.Hole5 = true;
            boltArray2.AddBoltDistX(inch2mm(5.66));
            boltArray2.StartPointOffset.Dx = inch2mm(offsetx);
            boltArray2.AddBoltDistY(inch2mm(0));
            boltArray2.Insert();
            #endregion

            myWorkPlaneHandler.SetCurrentTransformationPlane(currentPlane);
        }
        public void HRPrimary(ModelObject column, List<Beam> purlins)
        {
            #region Face Detection           
            //Line centerLinePurlin = CenterLine(purlins[0]);
            //Beam Finalbeam = ClosestChildrenBeam(column, centerLinePurlin);

            //Line centreLineFinalBeam = CenterLine(Finalbeam);
            //Face Originface = BUClosestFacetoLine(Finalbeam, centerLinePurlin);
            //Face PurlinGirtface = HRClosestFacetoLine(purlins[0], centreLineFinalBeam);
            Line centerLinePurlin = CenterLine(purlins[0]);
            Beam Finalbeam = column as Beam;

            Line centreLineFinalBeam = CenterLine(Finalbeam);

            Face Originface = Finalface(DominatingFaceHR(Finalbeam), centerLinePurlin);
            Face PurlinGirtface = HRClosestFacetoLine(purlins[0], centreLineFinalBeam);

            #endregion

            #region Coordinatesystem

            GraphicsDrawer graphicsDrawer = new GraphicsDrawer();
            Tekla.Structures.Model.UI.Color color = new Tekla.Structures.Model.UI.Color(0, 0, 1);

            Point OriginLinept1 = Intersection.LineToPlane(MaxLinesinFace(Originface)[0], PlanefromFace(PurlinGirtface));
            Point OriginLinept2 = Intersection.LineToPlane(MaxLinesinFace(Originface)[1], PlanefromFace(PurlinGirtface));

            double centredistance = 0.5 * (TSG.Distance.PointToPoint(OriginLinept1, OriginLinept2));

            graphicsDrawer.DrawLineSegment(OriginLinept1, OriginLinept2, color);

            CoordinateSystem localCoordinateSystem = new CoordinateSystem();
            localCoordinateSystem.Origin = CentrePoint(OriginLinept1, OriginLinept2);
            localCoordinateSystem.AxisX = Originface.Normal;
            localCoordinateSystem.AxisY = PurlinGirtface.Normal;

            #endregion


            WorkPlaneHandler myWorkPlaneHandler = Model.GetWorkPlaneHandler();
            TransformationPlane currentPlane = myWorkPlaneHandler.GetCurrentTransformationPlane();
            TransformationPlane localplane = new TransformationPlane(localCoordinateSystem); ;
            myWorkPlaneHandler.SetCurrentTransformationPlane(localplane);



            #region Offset Calculcation
            List<double> distanceFromOrigin = new List<double>();
            for (int j = 0; j < MaxLinesinFace(PurlinGirtface).Count; j++)
            {
                distanceFromOrigin.Add(TSG.Distance.PointToLine(localCoordinateSystem.Origin, MaxLinesinFace(PurlinGirtface)[j]));
            }
            double MaxDistanceofPurlin = distanceFromOrigin.Max();
            double MinDistanceofPurlin = distanceFromOrigin.Min();
            #endregion

            #region Extension and End bolting
            bool endBoltsactivaiton = false;
            if (purlins.Count == 2)
            {
                endBoltsactivaiton = true;
            }

            for (int i = 0; i < purlins.Count; i++)
            {
                #region Side Detection Fitting
                FittingExtension(purlins[i], MinDistanceofPurlin, MaxDistanceofPurlin, endBoltsactivaiton, Convert.ToDouble(Extension), Convert.ToDouble(Margin));
                #endregion
            }
            #endregion


            #region Plate
            double plate_thickness = Convert.ToDouble(Thickness);
            ContourPoint point = new ContourPoint(new Point(0, 0, centredistance), null);
            ContourPoint point1 = new ContourPoint(new Point(MinDistanceofPurlin + inch2mm(6.75), 0, centredistance), null);
            ContourPoint point2 = new ContourPoint(new Point(MinDistanceofPurlin + inch2mm(6.75), 0, -centredistance), null);
            ContourPoint point3 = new ContourPoint(new Point(0, 0, -centredistance), null);
            ContourPlate CP = new ContourPlate();
            CP.AddContourPoint(point);
            CP.AddContourPoint(point1);
            CP.AddContourPoint(point2);
            CP.AddContourPoint(point3);
            CP.Profile.ProfileString = "PL" + inch2mm(plate_thickness);
            CP.Material.MaterialString = "A36";
            CP.Position.Depth = Position.DepthEnum.FRONT;
            CP.Class = "2";
            CP.Insert();
            #endregion

            Welds(Finalbeam, CP, Convert.ToDouble(Weldsize));


            #region BoltMain
            BoltArray boltArray1 = new BoltArray();
            for (int i = 0; i < purlins.Count; i++)
            {
                boltArray1.PartToBeBolted = purlins[i];
            }
            boltArray1.PartToBoltTo = CP;
            boltArray1.FirstPosition = new Point(MinDistanceofPurlin + inch2mm(6.75), 0, centredistance);
            boltArray1.SecondPosition = new Point(MinDistanceofPurlin, 0, -centredistance);
            //boltArray1.BoltSize = inch2mm(Convert.ToDouble(_Bolt_size));
            boltArray1.BoltSize = inch2mm(Convertstring(BoltDiameter));
            boltArray1.Tolerance = inch2mm(0.25);
            boltArray1.BoltStandard = BoltStandard;
            boltArray1.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
            boltArray1.CutLength = inch2mm(10);
            boltArray1.ExtraLength = inch2mm(1);
            boltArray1.ThreadInMaterial = BoltGroup.BoltThreadInMaterialEnum.THREAD_IN_MATERIAL_NO;
            boltArray1.Position.Depth = Position.DepthEnum.MIDDLE;
            boltArray1.Position.Plane = Position.PlaneEnum.MIDDLE;
            boltArray1.Position.Rotation = Position.RotationEnum.TOP;
            if (Convert.ToDouble(Boltactivation) == 0)
            {
                boltArray1.Bolt = false;
            }
            else
            {
                boltArray1.Bolt = true;
            }
            boltArray1.Washer1 = true;
            boltArray1.Washer2 = true;
            boltArray1.Washer3 = true;

            boltArray1.Nut1 = true;
            boltArray1.Nut2 = true;
            boltArray1.Hole1 = true;
            boltArray1.Hole2 = true;
            boltArray1.Hole3 = true;
            boltArray1.Hole4 = true;
            boltArray1.Hole5 = true;
            boltArray1.AddBoltDistX(inch2mm(5.66));
            double offsetx = 0.5 * (Math.Sqrt(Math.Pow(6.75, 2) + Math.Pow(2 * mm2inch(centredistance), 2)) - Math.Sqrt(32));

            boltArray1.StartPointOffset.Dx = inch2mm(offsetx);
            boltArray1.AddBoltDistY(inch2mm(0));
            boltArray1.Insert();

            BoltArray boltArray2 = new BoltArray();
            for (int i = 0; i < purlins.Count; i++)
            {
                boltArray2.PartToBeBolted = purlins[i];
            }
            boltArray2.PartToBoltTo = CP;
            boltArray2.FirstPosition = new Point(MinDistanceofPurlin + inch2mm(6.75), 0, -centredistance);
            boltArray2.SecondPosition = new Point(MinDistanceofPurlin, 0, centredistance);
            //boltArray2.BoltSize = inch2mm(Convert.ToDouble(_Bolt_size));
            boltArray2.BoltSize = inch2mm(Convertstring(BoltDiameter));

            boltArray2.Tolerance = inch2mm(0.25);
            boltArray2.BoltStandard = BoltStandard;
            boltArray2.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
            boltArray2.CutLength = inch2mm(10);
            boltArray2.ExtraLength = inch2mm(1);
            boltArray2.ThreadInMaterial = BoltGroup.BoltThreadInMaterialEnum.THREAD_IN_MATERIAL_NO;
            boltArray2.Position.Depth = Position.DepthEnum.MIDDLE;
            boltArray2.Position.Plane = Position.PlaneEnum.MIDDLE;
            boltArray2.Position.Rotation = Position.RotationEnum.TOP;
            if (Convert.ToDouble(Boltactivation) == 0 || Convert.ToDouble(Boltactivation) == 2)
            {
                boltArray2.Bolt = false;
            }
            else
            {
                boltArray2.Bolt = true;
            }
            boltArray2.Washer1 = true;
            boltArray2.Washer2 = true;
            boltArray2.Washer3 = true;
            boltArray2.Nut1 = true;
            boltArray2.Nut2 = true;
            boltArray2.Hole1 = true;
            boltArray2.Hole2 = true;
            boltArray2.Hole3 = true;
            boltArray2.Hole4 = true;
            boltArray2.Hole5 = true;
            boltArray2.AddBoltDistX(inch2mm(5.66));
            boltArray2.StartPointOffset.Dx = inch2mm(offsetx);
            boltArray2.AddBoltDistY(inch2mm(0));
            boltArray2.Insert();
            #endregion

            myWorkPlaneHandler.SetCurrentTransformationPlane(currentPlane);
        }
        private OBB Createobb(Beam beam)
        {
            OBB obb = null;

            if (beam != null)
            {
                WorkPlaneHandler workPlaneHandler = Model.GetWorkPlaneHandler();
                TransformationPlane originalTransformationPlane = workPlaneHandler.GetCurrentTransformationPlane();
                CoordinateSystem originalcoordinate = new CoordinateSystem();
                originalcoordinate.Origin = new Point(0, 0, 0);
                originalcoordinate.AxisX = new Vector(1, 0, 0);
                originalcoordinate.AxisY = new Vector(0, 1, 0);



                Solid solid = beam.GetSolid();
                Point minPointInCurrentPlane = solid.MinimumPoint;
                Point maxPointInCurrentPlane = solid.MaximumPoint;


                Point centerPoint = CalculateCenterPoint(minPointInCurrentPlane, maxPointInCurrentPlane);

                CoordinateSystem coordSys = beam.GetCoordinateSystem();
                TransformationPlane localTransformationPlane = new TransformationPlane(coordSys);
                workPlaneHandler.SetCurrentTransformationPlane(localTransformationPlane);

                solid = beam.GetSolid();
                Point minPoint = solid.MinimumPoint;
                Point maxPoint = solid.MaximumPoint;
                double extent0 = (maxPoint.X - minPoint.X) / 2;
                double extent1 = (maxPoint.Y - minPoint.Y) / 2;
                double extent2 = (maxPoint.Z - minPoint.Z) / 2;

                workPlaneHandler.SetCurrentTransformationPlane(originalTransformationPlane);


                obb = new OBB(centerPoint, coordSys.AxisX, coordSys.AxisY,
                                coordSys.AxisX.Cross(coordSys.AxisY), extent0, extent1, extent2);

                Solid solid1 = beam.GetSolid();

            }

            return obb;
        }
        private Point CalculateCenterPoint(Point min, Point max)
        {
            double x = min.X + ((max.X - min.X) / 2);
            double y = min.Y + ((max.Y - min.Y) / 2);
            double z = min.Z + ((max.Z - min.Z) / 2);

            return new Point(x, y, z);
        }
        public Line CenterLine(Beam beam)
        {
            bool blean = true;
            ArrayList centrePoints = beam.GetCenterLine(blean);
            Line Centerline = new Line(centrePoints[0] as Point, centrePoints[1] as Point);
            return Centerline;
        }
        public List<Beam> ChildrenBeam(ModelObject modelObject)
        {
            ModelObjectEnumerator modelObjectEnumerator = modelObject.GetChildren();
            Beam beam = new Beam();

            List<Beam> BeamChildren = new List<Beam>();
            foreach (var item in modelObjectEnumerator)
            {
                if (item.GetType() == beam.GetType())
                {
                    BeamChildren.Add(item as Beam);
                }
            }
            return BeamChildren;

        }
        public Beam ClosestChildrenBeam(ModelObject modelobject, Line otherCenterLine)
        {
            List<double> ObbtoDistance = new List<double>();
            foreach (Beam item in ChildrenBeam(modelobject))
            {
                ObbtoDistance.Add(Createobb(item).DistanceTo(otherCenterLine));
            }
            Beam Finalbeam = new Beam();
            for (int i = 0; i < ObbtoDistance.Count; i++)
            {
                if (ObbtoDistance[i] == ObbtoDistance.Min())
                {
                    Finalbeam = ChildrenBeam(modelobject)[i];
                }
            }
            return Finalbeam;
        }
        public Point FaceCenter(Face face)
        {
            List<Loop> loops = new List<Loop>();
            List<Point> listvert = new List<Point>();

            LoopEnumerator loopEnumerator = face.GetLoopEnumerator();
            while (loopEnumerator.MoveNext())
            {
                loops.Add(loopEnumerator.Current);

            }

            VertexEnumerator vertexenum = loops[0].GetVertexEnumerator();
            while (vertexenum.MoveNext())
            {
                listvert.Add(vertexenum.Current);

            }
            Point centerPoint = new Point();
            centerPoint.X = 0.25 * (listvert[0].X + listvert[1].X + listvert[2].X + listvert[3].X);
            centerPoint.Y = 0.25 * (listvert[0].Y + listvert[1].Y + listvert[2].Y + listvert[3].Y);
            centerPoint.Z = 0.25 * (listvert[0].Z + listvert[1].Z + listvert[2].Z + listvert[3].Z);
            return centerPoint;
        }
        public double AreaOfFace(Face face)
        {
            List<Loop> loops = new List<Loop>();
            List<Point> listvert = new List<Point>();

            LoopEnumerator loopEnumerator = face.GetLoopEnumerator();
            while (loopEnumerator.MoveNext())
            {
                loops.Add(loopEnumerator.Current);
            }

            VertexEnumerator vertexenum = loops[0].GetVertexEnumerator();
            while (vertexenum.MoveNext())
            {
                listvert.Add(vertexenum.Current);

            }
            double line1 = TSG.Distance.PointToPoint(listvert[0], listvert[1]);
            double line2 = TSG.Distance.PointToPoint(listvert[1], listvert[2]);
            double facearea = line1 * line2;
            return facearea;
        }
        public List<Face> BUMaximumAreaFace(Beam beam)
        {
            List<Face> allfaces = new List<Face>();
            List<Face> faces = new List<Face>();
            Solid solid = beam.GetSolid();
            FaceEnumerator faceEnumerator = solid.GetFaceEnumerator();
            while (faceEnumerator.MoveNext())
            {
                allfaces.Add(faceEnumerator.Current);
            }


            List<double> area = new List<double>();

            for (int i = 0; i < allfaces.Count; i++)
            {
                area.Add(AreaOfFace(allfaces[i]));
            }
            for (int i = 0; i < area.Count; i++)
            {
                if (area[i] > 0.9 * area.Max())
                {
                    faces.Add(allfaces[i]);
                }
            }
            return faces;
        }
        public List<Face> HRMaximumAreaFace(Beam beam)
        {
            List<Face> allfaces = new List<Face>();
            List<Face> faces = new List<Face>();
            Solid solid = beam.GetSolid();
            FaceEnumerator faceEnumerator = solid.GetFaceEnumerator();
            while (faceEnumerator.MoveNext())
            {
                allfaces.Add(faceEnumerator.Current);
            }


            List<double> area = new List<double>();

            for (int i = 0; i < allfaces.Count; i++)
            {
                area.Add(AreaOfFace(allfaces[i]));
            }
            for (int i = 0; i < area.Count; i++)
            {
                if (area[i] > 0.9 * area.Max())
                {
                    faces.Add(allfaces[i]);
                }
            }
            return faces;
        }
        public Face BUClosestFacetoLine(Beam beam, Line line)
        {
            List<double> distanceFace = new List<double>();
            Face face = null;
            for (int i = 0; i < BUMaximumAreaFace(beam).Count; i++)
            {
                distanceFace.Add(TSG.Distance.PointToLine(FaceCenter(BUMaximumAreaFace(beam)[i]), line));
            }
            for (int i = 0; i < distanceFace.Count; i++)
            {
                if (distanceFace[i] == distanceFace.Min())
                {
                    face = BUMaximumAreaFace(beam)[i];
                }
            }
            GraphicsDrawer graphicsDrawer = new GraphicsDrawer();
            Tekla.Structures.Model.UI.Color color = new Tekla.Structures.Model.UI.Color(1, 0, 0);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[0], FaceVertex(face)[1], color);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[1], FaceVertex(face)[2], color);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[2], FaceVertex(face)[3], color);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[3], FaceVertex(face)[0], color);
            return face;
        }
        public Face HRClosestFacetoLine(Beam beam, Line line)
        {
            List<double> distanceFace = new List<double>();
            Face face = null;
            for (int i = 0; i < HRMaximumAreaFace(beam).Count; i++)
            {
                distanceFace.Add(TSG.Distance.PointToLine(FaceCenter(HRMaximumAreaFace(beam)[i]), line));
            }
            for (int i = 0; i < distanceFace.Count; i++)
            {
                if (distanceFace[i] == distanceFace.Min())
                {
                    face = HRMaximumAreaFace(beam)[i];
                }
            }
            GraphicsDrawer graphicsDrawer = new GraphicsDrawer();
            Tekla.Structures.Model.UI.Color color = new Tekla.Structures.Model.UI.Color(1, 0, 0);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[0], FaceVertex(face)[1], color);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[1], FaceVertex(face)[2], color);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[2], FaceVertex(face)[3], color);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[3], FaceVertex(face)[0], color);
            return face;
        }
        public List<Point> FaceVertex(Face face)
        {
            List<Loop> loops = new List<Loop>();
            List<Point> listvert = new List<Point>();

            LoopEnumerator loopEnumerator = face.GetLoopEnumerator();
            while (loopEnumerator.MoveNext())
            {
                loops.Add(loopEnumerator.Current);

            }

            VertexEnumerator vertexenum = loops[0].GetVertexEnumerator();
            while (vertexenum.MoveNext())
            {
                listvert.Add(vertexenum.Current);
            }
            return listvert;
        }
        public GeometricPlane PlanefromFace(Face face)
        {
            GeometricPlane geometricPlane = new GeometricPlane(FaceVertex(face)[0], face.Normal);
            return geometricPlane;
        }
        public List<Line> MaxLinesinFace(Face Orignface)
        {
            List<double> edgeDistance = new List<double>();
            List<Line> Lines = new List<Line>();

            edgeDistance.Add(TSG.Distance.PointToPoint(FaceVertex(Orignface)[0], FaceVertex(Orignface)[1]));
            edgeDistance.Add(TSG.Distance.PointToPoint(FaceVertex(Orignface)[1], FaceVertex(Orignface)[2]));
            edgeDistance.Add(TSG.Distance.PointToPoint(FaceVertex(Orignface)[2], FaceVertex(Orignface)[3]));
            edgeDistance.Add(TSG.Distance.PointToPoint(FaceVertex(Orignface)[3], FaceVertex(Orignface)[0]));

            for (int i = 0; i < edgeDistance.Count; i++)
            {
                if (edgeDistance[i] > 0.9 * edgeDistance.Max())
                {
                    if (i == 0)
                    {
                        Lines.Add(new Line(FaceVertex(Orignface)[0], FaceVertex(Orignface)[1]));
                    }
                    if (i == 1)
                    {
                        Lines.Add(new Line(FaceVertex(Orignface)[1], FaceVertex(Orignface)[2]));
                    }
                    if (i == 2)
                    {
                        Lines.Add(new Line(FaceVertex(Orignface)[2], FaceVertex(Orignface)[3]));
                    }
                    if (i == 3)
                    {
                        Lines.Add(new Line(FaceVertex(Orignface)[3], FaceVertex(Orignface)[0]));
                    }
                }
            }

            return Lines;
        }
        public Point CentrePoint(Point point1, Point point2)
        {
            Point point = new Point();
            point.X = 0.5 * (point1.X + point2.X);
            point.Y = 0.5 * (point1.Y + point2.Y);
            point.Z = 0.5 * (point1.Z + point2.Z);
            return point;
        }
        public double inch2mm(double inch)
        {
            double mm = inch * 25.4;
            return mm;
        }
        public double mm2inch(double mm)
        {
            double Inch = (mm / 25.4);
            return Inch;
        }
        public Fitting FittingExtension(Beam purlin, double MinDistanceofPurlin, double MaxDistanceofPurlin, bool endBoltactivation, double extension, double margin)
        {

            bool a = true;
            ArrayList purlinPoints = purlin.GetCenterLine(a);
            double purlinPt1 = TSG.Distance.PointToPoint(purlinPoints[0] as Point, new Point(0, 0, 0));
            double purlinPt2 = TSG.Distance.PointToPoint(purlinPoints[1] as Point, new Point(0, 0, 0));

            Point closePoint;
            Point farPoint;
            if (purlinPt1 < purlinPt2)
            {
                closePoint = purlinPoints[0] as Point;
                farPoint = purlinPoints[1] as Point;
            }
            else
            {
                closePoint = purlinPoints[1] as Point;
                farPoint = purlinPoints[0] as Point;
            }
            Vector vectorClose = new Vector(closePoint);
            Vector vectorFar = new Vector(farPoint);


            double extensionBolt = inch2mm(extension) - inch2mm(margin);

            if ((Convert.ToInt16(vectorFar.Z) < 0 && Convert.ToInt16(vectorClose.Z) == 0) || Convert.ToInt16(vectorFar.Z) < 0)
            {
                Fitting myFitting = new Fitting();
                myFitting.Plane = new Plane();
                myFitting.Plane.Origin = new Point(0, 0, inch2mm(extension));
                myFitting.Plane.AxisX = new Vector(1, 0, 0);
                myFitting.Plane.AxisY = new Vector(0, 1, 0);
                myFitting.Father = purlin;
                myFitting.Insert();
                if (endBoltactivation)
                {
                    BoltArray boltArray = new BoltArray();
                    boltArray.PartToBeBolted = purlin;
                    boltArray.FirstPosition = new Point(MinDistanceofPurlin, 0, extensionBolt);
                    boltArray.SecondPosition = new Point(MaxDistanceofPurlin, 0, extensionBolt);
                    //boltArray.BoltSize = inch2mm(Convert.ToDouble(_EBolt_size));
                    boltArray.BoltSize = inch2mm(Convertstring(EBoltsize));

                    boltArray.Tolerance = inch2mm(0.25);
                    boltArray.BoltStandard = EBoltstandard;
                    boltArray.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
                    boltArray.CutLength = inch2mm(1);
                    boltArray.ExtraLength = inch2mm(1);
                    boltArray.ThreadInMaterial = BoltGroup.BoltThreadInMaterialEnum.THREAD_IN_MATERIAL_NO;
                    boltArray.Position.Depth = Position.DepthEnum.MIDDLE;
                    boltArray.Position.Plane = Position.PlaneEnum.MIDDLE;
                    boltArray.Position.Rotation = Position.RotationEnum.BELOW;
                    boltArray.Bolt = true;
                    boltArray.Washer1 = true;
                    boltArray.Washer2 = true;
                    boltArray.Washer3 = true;
                    boltArray.Nut1 = true;
                    boltArray.Nut2 = true;
                    boltArray.Hole1 = true;
                    boltArray.Hole2 = true;
                    boltArray.Hole3 = true;
                    boltArray.Hole4 = true;
                    boltArray.Hole5 = true;
                    boltArray.AddBoltDistX(inch2mm(4));
                    boltArray.StartPointOffset.Dx = (MaxDistanceofPurlin - MinDistanceofPurlin - inch2mm(4)) * 0.5;
                    boltArray.AddBoltDistY(inch2mm(0));
                    boltArray.Insert();

                }
                return myFitting;


            }
            if ((Convert.ToInt16(vectorFar.Z) > 0 && Convert.ToInt16(vectorClose.Z) == 0) || Convert.ToInt16(vectorFar.Z) > 0)
            {
                Fitting myFitting = new Fitting();
                myFitting.Plane = new Plane();
                myFitting.Plane.Origin = new Point(0, 0, -inch2mm(extension));
                myFitting.Plane.AxisX = new Vector(1, 0, 0);
                myFitting.Plane.AxisY = new Vector(0, 1, 0);
                myFitting.Father = purlin;
                myFitting.Insert();
                if (endBoltactivation)
                {
                    BoltArray boltArray = new BoltArray();
                    boltArray.PartToBeBolted = purlin;
                    boltArray.FirstPosition = new Point(MinDistanceofPurlin, 0, -extensionBolt);
                    boltArray.SecondPosition = new Point(MaxDistanceofPurlin, 0, -extensionBolt);
                    //boltArray.BoltSize = inch2mm(Convert.ToDouble(_EBolt_size));
                    boltArray.BoltSize = inch2mm(Convertstring(EBoltsize));
                    boltArray.Tolerance = inch2mm(0.25);
                    boltArray.BoltStandard = EBoltstandard;
                    boltArray.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
                    boltArray.CutLength = inch2mm(1);
                    boltArray.ExtraLength = inch2mm(1);
                    boltArray.ThreadInMaterial = BoltGroup.BoltThreadInMaterialEnum.THREAD_IN_MATERIAL_NO;
                    boltArray.Position.Depth = Position.DepthEnum.MIDDLE;
                    boltArray.Position.Plane = Position.PlaneEnum.MIDDLE;
                    boltArray.Position.Rotation = Position.RotationEnum.BELOW;
                    boltArray.Bolt = true;
                    boltArray.Washer1 = true;
                    boltArray.Washer2 = true;
                    boltArray.Washer3 = true;
                    boltArray.Nut1 = true;
                    boltArray.Nut2 = true;
                    boltArray.Hole1 = true;
                    boltArray.Hole2 = true;
                    boltArray.Hole3 = true;
                    boltArray.Hole4 = true;
                    boltArray.Hole5 = true;
                    boltArray.AddBoltDistX(inch2mm(4));
                    boltArray.StartPointOffset.Dx = (MaxDistanceofPurlin - MinDistanceofPurlin - inch2mm(4)) * 0.5;
                    boltArray.AddBoltDistY(inch2mm(0));
                    boltArray.Insert();
                }


                return myFitting;
            }
            else
            {
                Fitting myFitting = new Fitting();
                //MessageBox.Show("Fitting Failed");
                return myFitting;
            }

        }
        public void Welds(Beam beam, ContourPlate Plate, double Weldsize)
        {
            Weld weld1 = new Weld();
            weld1.MainObject = beam;
            weld1.SecondaryObject = Plate;
            weld1.AroundWeld = false;
            weld1.ShopWeld = true;
            weld1.Position = Weld.WeldPositionEnum.WELD_POSITION_PLUS_Y;
            weld1.ConnectAssemblies = false;
            weld1.IntermittentType = BaseWeld.WeldIntermittentTypeEnum.CONTINUOUS;
            weld1.Placement = BaseWeld.WeldPlacementTypeEnum.PLACEMENT_AUTO;
            weld1.Preparation = BaseWeld.WeldPreparationTypeEnum.PREPARATION_NONE;
            weld1.TypeAbove = BaseWeld.WeldTypeEnum.WELD_TYPE_FILLET;
            weld1.SizeAbove = inch2mm(Weldsize);
            weld1.Insert();

            Weld weld2 = new Weld();
            weld2.MainObject = beam;
            weld2.SecondaryObject = Plate;
            weld2.AroundWeld = false;
            weld2.ShopWeld = true;
            weld2.Position = Weld.WeldPositionEnum.WELD_POSITION_MINUS_Y;
            weld2.ConnectAssemblies = false;
            weld2.IntermittentType = BaseWeld.WeldIntermittentTypeEnum.CONTINUOUS;
            weld2.Placement = BaseWeld.WeldPlacementTypeEnum.PLACEMENT_AUTO;
            weld2.Preparation = BaseWeld.WeldPreparationTypeEnum.PREPARATION_NONE;
            weld2.TypeAbove = BaseWeld.WeldTypeEnum.WELD_TYPE_FILLET;
            weld2.SizeAbove = inch2mm(Weldsize);
            weld2.Insert();
        }
        public List<Face> DominatingFaceHR(Beam beam)
        {
            List<Face> allfaces = new List<Face>();
            List<Face> faces = new List<Face>();
            Solid solid = beam.GetSolid();
            FaceEnumerator faceEnumerator = solid.GetFaceEnumerator();
            while (faceEnumerator.MoveNext())
            {
                allfaces.Add(faceEnumerator.Current);
            }


            List<double> area = new List<double>();

            for (int i = 0; i < allfaces.Count; i++)
            {
                area.Add(AreaOfFace(allfaces[i]));
            }
            for (int i = 0; i < area.Count; i++)
            {
                if (area[i] > 0.4 * area.Max())
                {
                    faces.Add(allfaces[i]);
                }
            }
            return faces;
        }
        public Face Finalface(List<Face> usefulFaces, Line line)
        {
            List<double> distanceFace = new List<double>();
            Face face = null;
            for (int i = 0; i < usefulFaces.Count; i++)
            {
                distanceFace.Add(TSG.Distance.PointToLine(FaceCenter(usefulFaces[i]), line));
            }
            for (int i = 0; i < distanceFace.Count; i++)
            {
                if (distanceFace[i] == distanceFace.Min())
                {
                    face = usefulFaces[i];
                }
            }

            GraphicsDrawer graphicsDrawer = new GraphicsDrawer();
            Tekla.Structures.Model.UI.Color color = new Tekla.Structures.Model.UI.Color(1, 0, 0);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[0], FaceVertex(face)[1], color);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[1], FaceVertex(face)[2], color);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[2], FaceVertex(face)[3], color);
            graphicsDrawer.DrawLineSegment(FaceVertex(face)[3], FaceVertex(face)[0], color);

            return face;
        }
        public double Convertstring(string s)
        {
            int a = s.IndexOf("\"");
            string s1 = s.Substring(0, a);
            string s2 = s.Substring(a + 1);
            int b = s2.IndexOf("/");
            string s21 = s2.Substring(0, b);
            string s22 = s2.Substring(b + 1);
            double Final = Convert.ToDouble(s1) + (Convert.ToDouble(s21) / Convert.ToDouble(s22));
            return Final;

        }
        #endregion
    }
}
