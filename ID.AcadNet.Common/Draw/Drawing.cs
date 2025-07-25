﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ID.Infrastructure.Extensions;
using Intellidesk.AcadNet.Common.Extensions;
using Intellidesk.AcadNet.Common.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;

namespace Intellidesk.AcadNet.Common.Drawing
{
    /// <summary>
    /// The "Draw" module: directly draw entities (with AutoCAD-command-like functions)
    /// </summary>
    public static class Drawing
    {
        #region point

        /// <summary>
        /// Draws a point.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Point(Point3d position)
        {
            return DrawObject.Point(position).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws 'divide' entities.
        /// </summary>
        /// <param name="curve">The curve.</param>
        /// <param name="number">The number of curve segments.</param>
        /// <param name="entity">The entity to draw with. Either DBPoint or BlockReference.</param>
        /// <returns></returns>
        public static ObjectId[] Divide(Curve curve, int number, Entity entity)
        {
            return
                GetCurvePoints(curve, number: number)
                .PlaceEntity(entity)
                .AddToCurrentSpace();
        }

        /// <summary>
        /// Draws 'measure' entities.
        /// </summary>
        /// <param name="curve">The curve.</param>
        /// <param name="interval">The length of curve segments.</param>
        /// <param name="entity">The entity to draw with. Either DBPoint or BlockReference.</param>
        /// <returns></returns>
        public static ObjectId[] Measure(Curve curve, double interval, Entity entity)
        {
            return
                GetCurvePoints(curve, interval: interval)
                .PlaceEntity(entity)
                .AddToCurrentSpace();
        }

        private static IEnumerable<Point3d> GetCurvePoints(Curve curve, int number = -1, double interval = -1)
        {
            double start = curve.GetDistAtParam(curve.StartParam);
            double end = curve.GetDistAtParam(curve.EndParam);
            interval = interval == -1 ? (end - start) / number : interval;
            number = number == -1 ? (int)Math.Floor((end - start) / interval) : number;

            return Enumerable
                .Range(1, number - 1)
                .Select(n => curve.GetPointAtParam(start + n * interval));
        }

        private static IEnumerable<Entity> PlaceEntity(this IEnumerable<Point3d> positions, Entity entity)
        {
            foreach (var position in positions)
            {
                var newEntity = entity.Clone() as Entity;
                if (newEntity is DBPoint point)
                {
                    point.Position = position;
                }
                else if (newEntity is BlockReference insert)
                {
                    insert.Position = position;
                }

                yield return newEntity;
            }
        }

        #endregion

        #region line

        /// <summary>
        /// Draws a line.
        /// </summary>
        /// <param name="point1">The point 1.</param>
        /// <param name="point2">The point 2.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Line(Point3d point1, Point3d point2)
        {
            return DrawObject.Line(point1, point2).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws multiple lines by a sequence of points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The object IDs.</returns>
        public static ObjectId[] Line(params Point3d[] points)
        {
            return DrawObject.Line(points).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws an arc from 3 points.
        /// </summary>
        /// <param name="point1">The point 1.</param>
        /// <param name="point2">The point 2.</param>
        /// <param name="point3">The point 3.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Arc3P(Point3d point1, Point3d point2, Point3d point3)
        {
            return DrawObject.Arc3P(point1, point2, point3).AddToCurrentSpace();
        }

        //public static ObjectId ArcSCE(Point3d start, Point3d center, Point3d end)
        //{
        //}

        /// <summary>
        /// Draws an arc from start, center, and angle.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="center">The center point.</param>
        /// <param name="angle">The angle.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId ArcSCA(Point3d start, Point3d center, double angle)
        {
            return DrawObject.ArcSCA(start, center, angle).AddToCurrentSpace();
        }

        //public static ObjectId ArcSCL(Point3d start, Point3d center, double length)
        //{
        //}

        //public static ObjectId ArcSEA(Point3d start, Point3d end, double angle)
        //{
        //}

        //public static ObjectId ArcSED(Point3d start, Point3d end, Vector3d dir)
        //{
        //}

        //public static ObjectId ArcSER(Point3d start, Point3d end, double radius)
        //{
        //}

        /// <summary>
        /// Draws an arc from a geometry arc.
        /// </summary>
        /// <param name="arc">The geometry arc.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId ArcFromGeometry(CircularArc3d arc)
        {
            return DrawObject.ArcFromGeometry(arc).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws an arc from a geometry arc.
        /// </summary>
        /// <param name="arc">The geometry arc.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId ArcFromGeometry(CircularArc2d arc)
        {
            return DrawObject.ArcFromGeometry(arc).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws a polyline by a sequence of points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Pline(params Point3d[] points)
        {
            return DrawObject.Pline(points).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws a polyline by a sequence of points and a global width.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="globalWidth">The global width. Default is 0.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Pline(IEnumerable<Point3d> points, double globalWidth = 0)
        {
            return DrawObject.Pline(points, globalWidth).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws a polyline by a sequence of vertices (position + bulge).
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="globalWidth">The global width. Default is 0.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Pline(List<Tuple<Point3d, double>> vertices, double globalWidth = 0)
        {
            return DrawObject.Pline(vertices, globalWidth).AddToCurrentSpace();
        }

        public static ObjectId Pline(Point3dCollection points, Action<Polyline> action = null)
        {
            //Point3d[] arrPoints = new Point3d[] { };
            Point3d[] arrPoints = points.OfType<Point3d>().ToArray(); //.CopyTo(arrPoints, 0);

            var ent = DrawObject.Pline(arrPoints);
            if (action != null)
                action(ent);

            return ent.AddToCurrentSpace();
        }

        /// <summary>
        /// Draws a spline by fit points.
        /// </summary>
        /// <param name="points">The points to fit.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId SplineFit(Point3d[] points)
        {
            return DrawObject.SplineFit(points).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws a spline by control points.
        /// </summary>
        /// <param name="points">The control points.</param>
        /// <param name="closed">Whether to close the spline.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId SplineCV(Point3d[] points, bool closed = false)
        {
            return DrawObject.SplineCV(points, closed).AddToCurrentSpace();
        }

        #endregion

        #region shape

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="point1">The point 1.</param>
        /// <param name="point2">The point 2.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Rectang(Point3d point1, Point3d point2)
        {
            return DrawObject.Rectang(point1, point2).AddToCurrentSpace();
        }

        public static Entity Rectangle(double width, double height)
        {
            Point3d p1 = Point3d.Origin;
            Point3d p2 = Point3d.Origin + new Vector3d(0, 0, 0);

            var Db = HostApplicationServices.WorkingDatabase;
            var Doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;

            Doc.Editor.DrawVector(p1, p2, 1, true);

            double len = height; //p1.DistanceTo(p2);
            double wid = width;

            Doc.Editor.WriteMessage("\n\tLength:\t{0:f3}\tWidth:{1:f3}\n", len, wid);

            Plane plan = new Plane(Point3d.Origin, Vector3d.ZAxis);
            double ang = p1.GetVectorTo(p2).AngleOnPlane(plan);
            Point3dCollection pts = new Point3dCollection();

            Point3d c1 = p1.XGetPolarPoint(ang - Math.PI / 2, wid);
            Point3d c4 = p1.XGetPolarPoint(ang + Math.PI / 2, wid);
            Point3d c2 = c1.XGetPolarPoint(ang, len);
            Point3d c3 = c4.XGetPolarPoint(ang, len);

            pts.Add(c1);
            pts.Add(c2);
            pts.Add(c3);
            pts.Add(c4);

            Polyline poly = new Polyline();
            int idx = 0;
            foreach (Point3d p in pts)
            {
                poly.AddVertexAt(idx, p.XGetPoint2d(), 0, 0, 0);
                idx += 1;
            }
            poly.Closed = true;
            return poly;
        }

        public static Entity Rectangle(Point3dCollection corners)
        {
            Polyline ent = new Polyline();
            var Db = HostApplicationServices.WorkingDatabase;
            var Doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            ent.SetDatabaseDefaults();

            for (int i = 0; i < corners.Count; i++)
            {
                Point3d pt3d = corners[i];
                Point2d pt2d = new Point2d(pt3d.X, pt3d.Y);
                ent.AddVertexAt(i, pt2d, 0, Db.Plinewid, Db.Plinewid);
            };
            ent.Closed = true;
            ent.TransformBy(Doc.Editor.CurrentUserCoordinateSystem);
            return ent;
        }

        /// <summary>
        /// Draws a regular N-polygon.
        /// </summary>
        /// <param name="n">The N.</param>
        /// <param name="center">The center.</param>
        /// <param name="end">One vertex.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Polygon(int n, Point3d center, Point3d end)
        {
            if (n < 3)
            {
                return ObjectId.Null;
            }

            var direction = end - center;
            var points = Enumerable
                .Range(0, n)
                .Select(index => center.Add(direction.RotateBy(2 * Math.PI / n * index, Vector3d.ZAxis)))
                .ToArray();

            var result = Pline(points);
            result.XOpenForWrite<Polyline>(poly => poly.Closed = true);
            return result;
        }

        /// <summary>
        /// Draws a circle from center and radius.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Circle(Point3d center, double radius)
        {
            return DrawObject.Circle(center, radius).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws a circle from diameter ends.
        /// </summary>
        /// <param name="point1">The point 1.</param>
        /// <param name="point2">The point 2.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Circle(Point3d point1, Point3d point2)
        {
            return DrawObject.Circle(point1, point2).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws a circle from 3 points.
        /// </summary>
        /// <param name="point1">The point 1.</param>
        /// <param name="point2">The point 2.</param>
        /// <param name="point3">The point 3.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Circle(Point3d point1, Point3d point2, Point3d point3)
        {
            return DrawObject.Circle(point1, point2, point3).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws a circle from a geometry circle.
        /// </summary>
        /// <param name="circle">The geometry circle.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Circle(CircularArc3d circle)
        {
            return DrawObject.Circle(circle).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws a circle from a geometry circle.
        /// </summary>
        /// <param name="circle">The geometry circle.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Circle(CircularArc2d circle)
        {
            return DrawObject.Circle(circle).AddToCurrentSpace();
        }

        //public ObjectId void Circle(Line l1, Line l2, double radius)
        //{
        //}

        //public ObjectId void Circle(Line l1, Line l2, Line l3)
        //{
        //}

        /// <summary>
        /// Draws an ellipse by center, endX, and radiusY.
        /// </summary>
        /// <remarks>
        /// The ellipse will be drawn on the XY plane.
        /// </remarks>
        /// <param name="center">The center.</param>
        /// <param name="endX">The intersection point of the ellipse and its X axis.</param>
        /// <param name="radiusY">The Y radius.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Ellipse(Point3d center, Point3d endX, double radiusY)
        {
            return DrawObject.Ellipse(center, endX, radiusY).AddToCurrentSpace();
        }

        #endregion

        #region complex

        /// <summary>
        /// Draws hatch by seed.
        /// </summary>
        /// <param name="hatchName">The hatch name.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Hatch(string hatchName, Point3d seed)
        {
            var loop = Boundary(seed, BoundaryType.Polyline);
            var result = Hatch(new[] { loop }, hatchName);
            loop.Erase(); // newly 20140521
            return result;
        }

        /// <summary>
        /// Draws hatch by entities.
        /// </summary>
        /// <param name="hatchName">The hatch name.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Hatch(string hatchName, Entity[] entities)
        {
            // Step1 - find intersections
            var points = new Point3dCollection();
            for (int i = 0; i < entities.Length; i++)
            {
                for (int j = i + 1; j < entities.Length; j++)
                {
                    entities[i].IntersectWith3264(entities[j], Intersect.OnBothOperands, points);
                }
            }

            // Step2 - sort points
            var pointList = points.Cast<Point3d>().ToList();
            var centroid = new Point3d(
                pointList.Average(p => p.X),
                pointList.Average(p => p.Y),
                pointList.Average(p => p.Z));

            pointList = pointList
                .OrderBy(point =>
                {
                    var dir = point - centroid;
                    var angle = (point - centroid).GetAngleTo(Vector3d.XAxis);
                    if (dir.Y < 0)
                    {
                        angle = Math.PI * 2 - angle;
                    }
                    return angle;
                })
                .ToList();

            // Step2 - draw
            return Hatch(pointList, hatchName);
        }

        /// <summary>
        /// Draws hatch by closed area.
        /// </summary>
        /// <param name="loopIds">The loop IDs.</param>
        /// <param name="hatchName">The hatch name.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="associative">Whether it is associative.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Hatch(ObjectId[] loopIds, string hatchName = "SOLID", double scale = 1, double angle = 0, bool associative = false)
        {
            var db = DbHelper.GetDatabase(loopIds);
            using (var trans = db.TransactionManager.StartTransaction())
            {
                var hatch = new Hatch();
                var space = trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;
                var result = space.AppendEntity(hatch);
                trans.AddNewlyCreatedDBObject(hatch, true);

                hatch.SetDatabaseDefaults();
                hatch.Normal = new Vector3d(0, 0, 1);
                hatch.Elevation = 0.0;
                hatch.Associative = associative;
                hatch.PatternScale = scale;
                hatch.SetHatchPattern(HatchPatternType.PreDefined, hatchName);
                hatch.PatternAngle = angle; // PatternAngle has to be after SetHatchPattern(). This is AutoCAD .NET SDK violating Framework Design Guidelines, which requires properties to be set in arbitrary order.
                hatch.HatchStyle = HatchStyle.Outer;
                loopIds.ForEach(loop => hatch.AppendLoop(
                    HatchLoopTypes.External,
                    new ObjectIdCollection(new[] { loop })));
                //loopIds.ForEach(loop => hatch.AppendLoop(
                //    HatchLoopTypes.External,
                //    new ObjectIdCollection(new[] { loop })));

                hatch.EvaluateHatch(true);

                trans.Commit();
                return result;
            }
        }

        /// <summary>
        /// Draws hatch by a sequence of points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="hatchName">The hatch name.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="angle">The angle.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Hatch(IEnumerable<Point3d> points, string hatchName = "SOLID", double scale = 1, double angle = 0)
        {
            var pts = points.ToList();
            if (pts.First() != pts.Last())
            {
                pts.Add(pts.First());
            }
            var loop = Pline(pts);
            var result = Hatch(new[] { loop }, hatchName, scale, angle);
            loop.Erase();
            return result;
        }

        /// <summary>
        /// Draws boundary.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="type">The boundary type.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Boundary(Point3d seed, BoundaryType type)
        {
            var loop = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Editor.TraceBoundary(seed, false);
            if (loop.Count > 0)
            {
                if (type == BoundaryType.Polyline)
                {
                    var poly = loop[0] as Polyline;
                    if (poly.Closed)
                    {
                        poly.AddVertexAt(poly.NumberOfVertices, poly.StartPoint.XGetPoint2d(), 0, 0, 0);
                        poly.Closed = false;
                    }
                    return poly.AddToCurrentSpace();
                }
                else // type == BoundaryType.Region
                {
                    var region = Autodesk.AutoCAD.DatabaseServices.Region.CreateFromCurves(loop);
                    if (region.Count > 0)
                    {
                        return (region[0] as Region).AddToCurrentSpace();
                    }
                }
            }

            return ObjectId.Null;
        }

        /// <summary>
        /// Draws a region.
        /// </summary>
        /// <param name="curveId">The boundary curve.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Region(ObjectId curveId)
        {
            var curve = curveId.XOpenForRead<Curve>();
            if (curve != null)
            {
                var region = Autodesk.AutoCAD.DatabaseServices.Region.CreateFromCurves(new DBObjectCollection { curve });
                if (region.Count > 0)
                {
                    return (region[0] as Region).AddToCurrentSpace();
                }
            }

            return ObjectId.Null;
        }

        /// <summary>
        /// Draws a DT.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="height">The height.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="centerAligned">Whether to center align.</param>
        /// <param name="textStyle">The text style name.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Text(string text, double height, Point3d position, double rotation = 0, bool centerAligned = false, string textStyle = Consts.TextStyleName)
        {
            return DrawObject.Text(text, height, position, rotation, centerAligned, textStyle).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws an MT.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="height">The height.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="centerAligned">Whether to center align.</param>
        /// <param name="width">The width.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId MText(string text, double height, Point3d position, double rotation = 0, bool centerAligned = false, double width = 0)
        {
            return DrawObject.MText(text, height, position, rotation, centerAligned, width).AddToCurrentSpace();
        }

        /// <summary>
        /// Draws wipeout from a sequence of points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Wipeout(params Point3d[] points)
        {
            var wipe = new Wipeout();
            wipe.SetFrom(
                points: new Point2dCollection(points.Select(point => point.XtoPoint2D()).ToArray()),
                normal: Vector3d.ZAxis);

            var result = wipe.AddToCurrentSpace();
            result.Draworder(DraworderOperation.MoveToTop);
            return result;
        }

        /// <summary>
        /// Draws wipeout from an entity.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Wipeout(ObjectId entityId)
        {
            var extent = entityId.XOpenForRead<Entity>().GeometricExtents;
            return Wipeout(extent);
        }

        /// <summary>
        /// Draws wipeout from extents.
        /// </summary>
        /// <param name="extents">The extents.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Wipeout(Extents3d extents)
        {
            var a = new Point3d(extents.MinPoint.X, extents.MaxPoint.Y, 0);
            var b = new Point3d(extents.MaxPoint.X, extents.MinPoint.Y, 0);
            return Wipeout(extents.MinPoint, a, extents.MaxPoint, b, extents.MinPoint);
        }

        /// <summary>
        /// Inserts block reference.
        /// </summary>
        /// <param name="blockName">The block name.</param>
        /// <param name="position">The insert position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="scale">The scale.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Insert(string blockName, Point3d position, double rotation = 0, double scale = 1)
        {
            return DrawObject
                .Insert(
                    DbHelper.GetBlockId(blockName),
                    position,
                    rotation,
                    scale)
                .AddToCurrentSpace();
        }

        /// <summary>
        /// Defines a block given entities.
        /// </summary>
        /// <param name="entityIds">The entity IDs.</param>
        /// <param name="blockName">The block name.</param>
        /// <returns>The block table record ID.</returns>
        public static ObjectId Block(IEnumerable<ObjectId> entityIds, string blockName)
        {
            return Block(
                entityIds,
                blockName,
                basePoint: entityIds.GetCenter());
        }

        /// <summary>
        /// Defines a block given entities.
        /// </summary>
        /// <param name="entityIds">The entity IDs.</param>
        /// <param name="blockName">The block name.</param>
        /// <param name="basePoint">The base point.</param>
        /// <param name="overwrite">Whether to overwrite.</param>
        /// <returns>The block table record ID.</returns>
        public static ObjectId Block(IEnumerable<ObjectId> entityIds, string blockName, Point3d basePoint, bool overwrite = true)
        {
            return Block(
                entityIds.XOpenForRead<Entity>(),
                blockName,
                basePoint,
                overwrite);
        }

        /// <summary>
        /// Defines a block given entities. 
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="blockName">The block name.</param>
        /// <param name="basePoint">The base point.</param>
        /// <param name="overwrite">Whether to overwrite.</param>
        /// <returns>The block table record ID.</returns>
        public static ObjectId Block(IEnumerable<Entity> entities, string blockName, Point3d basePoint, bool overwrite = true)
        {
            var db = HostApplicationServices.WorkingDatabase;
            using (var trans = db.TransactionManager.StartTransaction())
            {
                var blockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                if (blockTable.Has(blockName))
                {
                    var oldBlock = trans.GetObject(blockTable[blockName], OpenMode.ForRead) as BlockTableRecord;
                    if (!overwrite)
                    {
                        Interaction.Write($"Block '{blockName}' already exists and was not overwritten.");
                        return oldBlock.Id;
                    }

                    blockTable.UpgradeOpen();
                    oldBlock.UpgradeOpen();
                    oldBlock.Erase();
                }

                var block = new BlockTableRecord
                {
                    Name = blockName
                };

                foreach (var entity in entities)
                {
                    var copy = entity.Clone() as Entity;
                    copy.TransformBy(Matrix3d.Displacement(-basePoint.GetAsVector()));
                    block.AppendEntity(copy);
                }

                var result = blockTable.Add(block);
                trans.AddNewlyCreatedDBObject(block, true);
                trans.Commit();
                return result;
            }
        }

        /// <summary>
        /// Creates a block and then inserts a reference to the model space.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="blockName">The block name.</param>
        /// <param name="blockBasePoint">The block base point.</param>
        /// <param name="insertPosition">The insert position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="overwrite">Whether to overwrite.</param>
        /// <returns>The insert object ID.</returns>
        public static ObjectId CreateBlockAndInsertReference(IEnumerable<Entity> entities, string blockName, Point3d blockBasePoint, Point3d insertPosition, double rotation = 0, double scale = 1, bool overwrite = true)
        {
            Block(entities, blockName, blockBasePoint, overwrite);
            return Insert(blockName, insertPosition, rotation, scale);
        }

        /// <summary>
        /// Defines a block by copying from another DWG.
        /// </summary>
        /// <param name="blockName">The block name.</param>
        /// <param name="sourceDwg">The source DWG.</param>
        /// <returns>The block table record ID.</returns>
        public static ObjectId BlockInDwg(string blockName, string sourceDwg)
        {
            var db = HostApplicationServices.WorkingDatabase;
            using (var trans = db.TransactionManager.StartTransaction())
            {
                var blockTable = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                if (blockTable.Has(blockName))
                {
                    return blockTable[blockName];
                }

                var sourceDb = new Database(false, false);
                sourceDb.ReadDwgFile(sourceDwg, FileOpenMode.OpenForReadAndAllShare, true, string.Empty);
                var blockId = DbHelper.GetBlockId(blockName, sourceDb);
                if (blockId == ObjectId.Null)
                {
                    return ObjectId.Null;
                }

                var tempDb = sourceDb.Wblock(blockId);
                var result = db.Insert(blockName, tempDb, false);
                trans.Commit();
                return result;
            }
        }

        /// <summary>
        /// Defines a block by taking another DWG as a whole.
        /// </summary>
        /// <param name="blockName">The block name.</param>
        /// <param name="sourceDwg">The source DWG.</param>
        /// <returns>The block table record ID.</returns>
        public static ObjectId BlockOfDwg(string blockName, string sourceDwg)
        {
            var sourceDb = new Database(false, false);
            sourceDb.ReadDwgFile(sourceDwg, FileOpenMode.OpenForReadAndAllShare, true, string.Empty);
            return HostApplicationServices.WorkingDatabase.Insert(blockName, sourceDb, false);
        }

        /// <summary>
        /// Draws a table.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="title">The title.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="rowHeight">The row height.</param>
        /// <param name="columnWidth">The column width.</param>
        /// <param name="textHeight">The text height.</param>
        /// <param name="textStyle">The text style.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Table(Point3d position, string title, List<List<string>> contents, double rowHeight, double columnWidth, double textHeight, string textStyle = Consts.TextStyleName)
        {
            var tb = new Table
            {
                TableStyle = HostApplicationServices.WorkingDatabase.Tablestyle,
                Position = position
            };

            var numRow = contents.Count + 1;
            tb.InsertRows(0, rowHeight, numRow);
            var numCol = contents.Max(row => row.Count);
            tb.InsertColumns(0, columnWidth, numCol);
            tb.DeleteRows(numRow, 1);
            tb.DeleteColumns(numCol, 1);
            tb.SetRowHeight(rowHeight);
            tb.SetColumnWidth(columnWidth);
            tb.Cells.TextHeight = textHeight;
            tb.Cells.TextStyleId = DbHelper.GetTextStyleId(textStyle);

            tb.MergeCells(CellRange.Create(tb, 0, 0, 0, numCol - 1));
            tb.Cells[0, 0].TextString = title;
            for (var i = 0; i < tb.Rows.Count - 1; i++)
            {
                for (var j = 0; j < tb.Columns.Count; j++)
                {
                    if (j < contents[i].Count)
                    {
                        tb.Cells[i + 1, j].TextString = contents[i][j];
                        tb.Cells[i + 1, j].Alignment = CellAlignment.MiddleCenter;
                    }
                }
            }

            return tb.AddToCurrentSpace();
        }

        /// <summary>
        /// Draws a polygon mesh.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="m">The m value.</param>
        /// <param name="n">The n value.</param>
        /// <param name="mClosed">Whether to close mesh in m dimension.</param>
        /// <param name="nClosed">Whether to close mesh in n dimension.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId PolygonMesh(List<Point3d> points, int m, int n, bool mClosed = false, bool nClosed = false)
        {
            var mesh = new PolygonMesh(PolyMeshType.SimpleMesh, m, n, new Point3dCollection(points.ToArray()), mClosed, nClosed);
            return mesh.AddToCurrentSpace();
        }

        #endregion

        #region dimensions

        /// <summary>
        /// Draws a linear dimension.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="dim">The dim.</param>
        /// <returns>The object ID.</returns>
        public static ObjectId Dimlin(Point3d start, Point3d end, Point3d dim)
        {
            double dist = start.DistanceTo(end);
            var ad = new AlignedDimension(start, end, dim, dist.ToString(), HostApplicationServices.WorkingDatabase.Dimstyle);
            return ad.AddToCurrentSpace();
        }

        #endregion

        #region helpers

        /// <summary>
        /// Adds an entity to the model space.
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        [Obsolete("Most of the time you should call AddToCurrentSpace().")]
        public static ObjectId AddToModelSpace(this Entity ent)
        {
            ObjectId id;
            Database db = HostApplicationServices.WorkingDatabase;
            using (var trans = db.TransactionManager.StartTransaction())
            {
                id = ((BlockTableRecord)trans.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite, false)).AppendEntity(ent);
                trans.AddNewlyCreatedDBObject(ent, true);
                trans.Commit();
            }
            return id;
        }

        /// <summary>
        /// Adds an entity to current space.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="db">The database.</param>
        /// <returns>The objected IDs.</returns>
        public static ObjectId AddToCurrentSpace(this Entity entity)
        {
            var db = entity.Database ?? HostApplicationServices.WorkingDatabase;
            Document doc = acadApp.DocumentManager.GetDocument(db);

            using (doc.LockDocument())
            using (var tr = db.TransactionManager.StartTransaction())
            {
                var currentSpace = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite, false);
                var id = currentSpace.AppendEntity(entity);
                tr.AddNewlyCreatedDBObject(entity, true);
                tr.Commit();
                return id;
            }
        }

        /// <summary>
        /// Adds entities to current space.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="db">The database.</param>
        /// <returns>The objected IDs.</returns>
        public static ObjectId[] AddToCurrentSpace(this IEnumerable<Entity> entities, Database db = null)
        {
            db = db ?? HostApplicationServices.WorkingDatabase;
            using (var trans = db.TransactionManager.StartTransaction())
            {
                var currentSpace = (BlockTableRecord)trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite, false);
                var ids = entities
                    .ToArray() // Truly get entities before moving on.
                    .Select(entity =>
                    {
                        var id = currentSpace.AppendEntity(entity);
                        trans.AddNewlyCreatedDBObject(entity, true);
                        return id;
                    })
                    .ToArray();

                trans.Commit();
                return ids;
            }
        }

        #endregion
    }
}
