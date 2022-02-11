using Nci.Tekla.Model;
using Ngd.Dialog;
using Ngd.Tekla.Geometry3d;
using Ngd.Tekla.Model.Extension;
using NH = Nci.Helper;
using NT = Nci.Tekla;
using ND = Ngd.Dialog;
using NTG = Ngd.Tekla.Geometry3d;


using Tekla.Structures.Model.UI;
using Tekla.Structures.Model;
using TSS = Tekla.Structures.Solid;
using Tekla.Structures.Geometry3d;

//using Nci.Helper;

using Ngd.Tekla.Geometry3d.Extension;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSG = Tekla.Structures.Geometry3d;

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

        public List<ModelObject> Insert(ModelObject _column, List<Beam> purlins)
        {
            
            var addedObjects = new List<ModelObject>();

            /* Add logic here for producing the proper connection */
            Part columnWeb;
            Part columnInnerFlange;
            Part columnOuterFlange;
            Part primaryPart = null;

            if (_column is CustomPart)
            {
                var parts = (_column as CustomPart).GetComponentObjects<Part>();
                columnWeb = parts.Where(x => x.IsWeb()).OrderByDescending(y => y.GetPlateThickness()).FirstOrDefault();
                columnInnerFlange = parts.Where(x => x.IsInsideFlange()).OrderByDescending(y => y.GetFlangeThickness()).FirstOrDefault();
                columnOuterFlange = parts.Where(x => x.IsOutsideFlange()).OrderByDescending(y => y.GetFlangeThickness()).FirstOrDefault();
                primaryPart = columnOuterFlange;
            }
            else
            {
                primaryPart = _column as Part;
            }

            if (primaryPart.GetAssembly().Name == "COLUMN" || primaryPart.GetAssembly().Name == "RAFTER")
            {


                ZeeConnection(primaryPart, purlins);
            }
            else
            {
                throw new NciTeklaException("Select primary part as Column or Rafter");
            }

            return addedObjects;
        }
        #endregion
        #region Support Methods
        public void ZeeConnection(ModelObject column, List<Beam> purlins)
        {
            if (purlins.Count > 2)
            {
                throw new NciTeklaException("Select Maximum 2 Secondary Part");
            }
            if (purlins.Count == 2)
            {
                if (!ExtendedCreateobb(purlins[0], 2, 1, 1).Intersects(ExtendedCreateobb(purlins[1], 2, 1, 1)))
                {
                    throw new NciTeklaException("Select Appropiate Pair of secondary part");
                }
            }
            #region Face Detection           
            Line centerLinePurlin = CenterLine(purlins[0]);
            Beam Finalbeam = column as Beam;
            Line centreLineFinalBeam = CenterLine(Finalbeam);
            TSS.Face Originface = PrimaryFace(column as Part, centerLinePurlin);
            TSS.Face PurlinGirtface = SecondaryFace(purlins[0] as Beam, centreLineFinalBeam);
            #endregion

            #region Coordinatesystem
            GraphicsDrawer graphicsDrawer = new GraphicsDrawer();
            Tekla.Structures.Model.UI.Color color = new Tekla.Structures.Model.UI.Color(0, 0, 1);
            Point OriginLinept1 = Intersection.LineToPlane(MaxLinesinFace(Originface)[0], NT.Model.TeklaSurface.GetPlaneFromFace(PurlinGirtface));
            Point OriginLinept2 = Intersection.LineToPlane(MaxLinesinFace(Originface)[1], NT.Model.TeklaSurface.GetPlaneFromFace(PurlinGirtface));
            double centredistance = 0.5 * (Distance.PointToPoint(OriginLinept1, OriginLinept2));
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
                distanceFromOrigin.Add(Distance.PointToLine(localCoordinateSystem.Origin, MaxLinesinFace(PurlinGirtface)[j]));
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
                if (!ExtendedCreateobb(purlins[i], 1, 5, 1).Intersects(ExtendedCreateobb(column as Beam, 1, 5, 1)))
                {
                    throw new NciTeklaException("Solution is not physible, Reselect Appropiate Primary and Secondary Part");
                }
                FittingExtension(purlins[i], MinDistanceofPurlin, MaxDistanceofPurlin, endBoltsactivaiton, Convert.ToDouble(Extension), Convert.ToDouble(Margin));
            }
            #endregion

            #region Plate
            double plate_thickness = Convert.ToDouble(Thickness);
            ContourPoint point = new ContourPoint(new Point(0, 0, centredistance), null);
            ContourPoint point1 = new ContourPoint(new Point(MinDistanceofPurlin + NH.Distance.Inch2mm(6.75), 0, centredistance), null);
            ContourPoint point2 = new ContourPoint(new Point(MinDistanceofPurlin + NH.Distance.Inch2mm(6.75), 0, -centredistance), null);
            ContourPoint point3 = new ContourPoint(new Point(0, 0, -centredistance), null);
            ContourPlate CP = new ContourPlate();
            CP.AddContourPoint(point);
            CP.AddContourPoint(point1);
            CP.AddContourPoint(point2);
            CP.AddContourPoint(point3);
            CP.Profile.ProfileString = "PL" + NH.Distance.Inch2mm(plate_thickness);
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
            boltArray1.FirstPosition = new Point(MinDistanceofPurlin + NH.Distance.Inch2mm(6.75), 0, centredistance);
            boltArray1.SecondPosition = new Point(MinDistanceofPurlin, 0, -centredistance);
            //boltArray1.BoltSize = inch2mm(Convert.ToDouble(_Bolt_size));
            boltArray1.BoltSize = NH.Distance.Inch2mm(Convertstring(BoltDiameter));
            boltArray1.Tolerance = NH.Distance.Inch2mm(0.25);
            boltArray1.BoltStandard = BoltStandard;
            boltArray1.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
            boltArray1.CutLength = NH.Distance.Inch2mm(10);
            boltArray1.ExtraLength = NH.Distance.Inch2mm(1);
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
            boltArray1.AddBoltDistX(NH.Distance.Inch2mm(5.66));
            double offsetx = 0.5 * (Math.Sqrt(Math.Pow(6.75, 2) + Math.Pow(2 * NH.Distance.mm2Inch(centredistance), 2)) - Math.Sqrt(32));

            boltArray1.StartPointOffset.Dx = NH.Distance.Inch2mm(offsetx);
            boltArray1.AddBoltDistY(NH.Distance.Inch2mm(0));
            boltArray1.Insert();

            BoltArray boltArray2 = new BoltArray();
            for (int i = 0; i < purlins.Count; i++)
            {
                boltArray2.PartToBeBolted = purlins[i];
            }
            boltArray2.PartToBoltTo = CP;
            boltArray2.FirstPosition = new Point(MinDistanceofPurlin + NH.Distance.Inch2mm(6.75), 0, -centredistance);
            boltArray2.SecondPosition = new Point(MinDistanceofPurlin, 0, centredistance);
            //boltArray2.BoltSize = inch2mm(Convert.ToDouble(_Bolt_size));
            boltArray2.BoltSize = NH.Distance.Inch2mm(Convertstring(BoltDiameter));

            boltArray2.Tolerance = NH.Distance.Inch2mm(0.25);
            boltArray2.BoltStandard = BoltStandard;
            boltArray2.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
            boltArray2.CutLength = NH.Distance.Inch2mm(10);
            boltArray2.ExtraLength = NH.Distance.Inch2mm(1);
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
            boltArray2.AddBoltDistX(NH.Distance.Inch2mm(5.66));
            boltArray2.StartPointOffset.Dx = NH.Distance.Inch2mm(offsetx);
            boltArray2.AddBoltDistY(NH.Distance.Inch2mm(0));
            boltArray2.Insert();
            #endregion

            myWorkPlaneHandler.SetCurrentTransformationPlane(currentPlane);
        }
        public TSS.Face PrimaryFace(Part column, Line line)
        {
            TSS.Face face = null;

            List<TeklaSurface> teklaSurfaces = TeklaSurface.GetOuterFlangeSurfaces(column);
            List<TSS.Face> faces = new List<TSS.Face>();
            List<double> distance = new List<double>();
            foreach (TeklaSurface item in teklaSurfaces)
            {
                distance.Add(Distance.PointToLine(TeklaSurface.GetPointFarthestFromLine(item.Face, line), line));
            }
            for (int i = 0; i < teklaSurfaces.Count; i++)
            {
                if (distance[i] == distance.Min())
                {
                    face = teklaSurfaces[i].Face;
                }
            }
            Tekla.Structures.Model.UI.Color color = new Tekla.Structures.Model.UI.Color(1, 0, 0);
            TeklaSurface.OutlineFace(face, color);
            return face;
        }
        public TSS.Face SecondaryFace(Beam purlin, Line line)
        {
            Solid solid = purlin.GetSolid();
            List<TeklaSurface> surfaces = TeklaSurface.GetSurfaces(solid);
            List<double> area = new List<double>();
            List<TSS.Face> faces = new List<TSS.Face>();

            foreach (TeklaSurface item in surfaces)
            {
                area.Add((int)TeklaSurface.CalculateArea(item.Face));
            }
            for (int i = 0; i < surfaces.Count; i++)
            {
                if (area[i] == area.Max())
                {
                    faces.Add(surfaces[i].Face);
                }
            }
            List<double> distanceFace = new List<double>();
            TSS.Face face = null;
            for (int i = 0; i < faces.Count; i++)
            {
                distanceFace.Add(Distance.PointToLine(FaceCenter(faces[i]), line));
            }
            for (int i = 0; i < distanceFace.Count; i++)
            {
                if (distanceFace[i] == distanceFace.Min())
                {
                    face = faces[i];
                }
            }
            Tekla.Structures.Model.UI.Color color = new Tekla.Structures.Model.UI.Color(1, 0, 0);
            TeklaSurface.OutlineFace(face, color);
            return face;
        }
        public Line CenterLine(Beam beam)
        {
            bool blean = true;
            ArrayList centrePoints = beam.GetCenterLine(blean);
            Line Centerline = new Line(centrePoints[0] as Point, centrePoints[1] as Point);
            return Centerline;
        }
        public List<Line> MaxLinesinFace(TSS.Face Orignface)
        {
            GraphicsDrawer graphicsDrawer = new GraphicsDrawer();
            Tekla.Structures.Model.UI.Color color = new Tekla.Structures.Model.UI.Color(0, 0, 1);

            List<double> edgeDistance = new List<double>();
            List<Line> Lines = new List<Line>();

            edgeDistance.Add(Distance.PointToPoint(FaceVertices(Orignface)[0], FaceVertices(Orignface)[1]));
            edgeDistance.Add(Distance.PointToPoint(FaceVertices(Orignface)[1], FaceVertices(Orignface)[2]));
            edgeDistance.Add(Distance.PointToPoint(FaceVertices(Orignface)[2], FaceVertices(Orignface)[3]));
            edgeDistance.Add(Distance.PointToPoint(FaceVertices(Orignface)[3], FaceVertices(Orignface)[0]));

            for (int i = 0; i < edgeDistance.Count; i++)
            {
                if (edgeDistance[i] > 0.9 * edgeDistance.Max())
                {
                    if (i == 0)
                    {
                        Lines.Add(new Line(FaceVertices(Orignface)[0], FaceVertices(Orignface)[1]));
                    }
                    if (i == 1)
                    {
                        Lines.Add(new Line(FaceVertices(Orignface)[1], FaceVertices(Orignface)[2]));
                    }
                    if (i == 2)
                    {
                        Lines.Add(new Line(FaceVertices(Orignface)[2], FaceVertices(Orignface)[3]));
                    }
                    if (i == 3)
                    {
                        Lines.Add(new Line(FaceVertices(Orignface)[3], FaceVertices(Orignface)[0]));
                    }
                }
            }

            return Lines;
        }
        public List<Point> FaceVertices(TSS.Face face)
        {
            List<Point> points = TeklaSurface.GetOutsideVertices(face);
            return points;
        }
        public Point FaceCenter(TSS.Face face)
        {
            List<TSS.Loop> loops = new List<TSS.Loop>();
            List<Point> listvert = new List<Point>();

            TSS.LoopEnumerator loopEnumerator = face.GetLoopEnumerator();
            while (loopEnumerator.MoveNext())
            {
                loops.Add(loopEnumerator.Current);
            }

            TSS.VertexEnumerator vertexenum = loops[0].GetVertexEnumerator();
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
        public double Convertstring(string s)
        {
            int a = s.IndexOf("\"");
            double Final;
            try
            {
                string s1 = s.Substring(0, a);
                string s2 = s.Substring(a + 1);
                int b = s2.IndexOf("/");
                string s21 = s2.Substring(0, b);
                string s22 = s2.Substring(b + 1);
                Final = Convert.ToDouble(s1) + (Convert.ToDouble(s21) / Convert.ToDouble(s22));
            }
            catch
            {
                string s1 = s.Substring(0, a);
                Final = Convert.ToDouble(s1);
            }
            
            return Final;

        }
        public Fitting FittingExtension(Beam purlin, double MinDistanceofPurlin, double MaxDistanceofPurlin, bool endBoltactivation, double extension, double margin)
        {

            bool a = true;
            ArrayList purlinPoints = purlin.GetCenterLine(a);
            double purlinPt1 = Distance.PointToPoint(purlinPoints[0] as Point, new Point(0, 0, 0));
            double purlinPt2 = Distance.PointToPoint(purlinPoints[1] as Point, new Point(0, 0, 0));

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


            double extensionBolt = NH.Distance.Inch2mm(extension) - NH.Distance.Inch2mm(margin);

            if ((Convert.ToInt16(vectorFar.Z) < 0 && Convert.ToInt16(vectorClose.Z) == 0) || Convert.ToInt16(vectorFar.Z) < 0)
            {
                Fitting myFitting = new Fitting();
                myFitting.Plane = new Plane();
                myFitting.Plane.Origin = new Point(0, 0, NH.Distance.Inch2mm(extension));
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
                    boltArray.BoltSize = NH.Distance.Inch2mm(Convertstring(EBoltsize));

                    boltArray.Tolerance = NH.Distance.Inch2mm(0.25);
                    boltArray.BoltStandard = EBoltstandard;
                    boltArray.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
                    boltArray.CutLength = NH.Distance.Inch2mm(1);
                    boltArray.ExtraLength = NH.Distance.Inch2mm(1);
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
                    boltArray.AddBoltDistX(NH.Distance.Inch2mm(4));
                    boltArray.StartPointOffset.Dx = (MaxDistanceofPurlin - MinDistanceofPurlin - NH.Distance.Inch2mm(4)) * 0.5;
                    boltArray.AddBoltDistY(NH.Distance.Inch2mm(0));
                    boltArray.Insert();

                }
                return myFitting;


            }
            if ((Convert.ToInt16(vectorFar.Z) > 0 && Convert.ToInt16(vectorClose.Z) == 0) || Convert.ToInt16(vectorFar.Z) > 0)
            {
                Fitting myFitting = new Fitting();
                myFitting.Plane = new Plane();
                myFitting.Plane.Origin = new Point(0, 0, -NH.Distance.Inch2mm(extension));
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
                    boltArray.BoltSize = NH.Distance.Inch2mm(Convertstring(EBoltsize));
                    boltArray.Tolerance = NH.Distance.Inch2mm(0.25);
                    boltArray.BoltStandard = EBoltstandard;
                    boltArray.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
                    boltArray.CutLength = NH.Distance.Inch2mm(1);
                    boltArray.ExtraLength = NH.Distance.Inch2mm(1);
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
                    boltArray.AddBoltDistX(NH.Distance.Inch2mm(4));
                    boltArray.StartPointOffset.Dx = (MaxDistanceofPurlin - MinDistanceofPurlin - NH.Distance.Inch2mm(4)) * 0.5;
                    boltArray.AddBoltDistY(NH.Distance.Inch2mm(0));
                    boltArray.Insert();
                }


                return myFitting;
            }
            else
            {
                Fitting myFitting = new Fitting();
                throw new NciTeklaException("Fitting failed");
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
            weld1.SizeAbove = NH.Distance.Inch2mm(Weldsize);
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
            weld2.SizeAbove = NH.Distance.Inch2mm(Weldsize);
            weld2.Insert();
        }
        public Point CentrePoint(Point point1, Point point2)
        {
            Point point = new Point();
            point.X = 0.5 * (point1.X + point2.X);
            point.Y = 0.5 * (point1.Y + point2.Y);
            point.Z = 0.5 * (point1.Z + point2.Z);
            return point;
        }
        private OBB ExtendedCreateobb(Beam beam, int extensionx, int extensiony, int extensionz)
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

                double extent0 = extensionx * (maxPoint.X - minPoint.X) / 2;
                double extent1 = extensiony * (maxPoint.Y - minPoint.Y) / 2;
                double extent2 = extensionz * (maxPoint.Z - minPoint.Z) / 2;

                //GraphicsDrawer graphicsDrawer = new GraphicsDrawer();
                //Tekla.Structures.Model.UI.Color color = new Tekla.Structures.Model.UI.Color(1, 0, 0);

                //graphicsDrawer.DrawLineSegment(coordSys.Origin,minPoint, color);
                //graphicsDrawer.DrawLineSegment(coordSys.Origin,maxPoint, color);

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
        #endregion
    }
}
