namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia.Media;

    class Radar : SystemComponent
    {
        VirtualFunctionBus bus;
        AutomatedCar car;
        TriangleDetector triangle;
        int offsetLength;
        Vector2 dir;
        Vector2 anchor;

        public Vector2 Position
        {
            get => anchor - (dir * offsetLength);
        }
        public Vector2 Direction
        {
            get => dir;
        }
        public override void Process()
        {
            bus.RadarPackets = new List<IReadOnlyRadarPacket>();

            var collidableObjects = triangle
                .ScanVisibleObjects(
                    World.Instance.WorldObjects,
                    (car.X, car.Y),
                    ((int)Position.X, (int)Position.Y),
                    CorrectRotation(car.Rotation) + 180)
                .Where(
                    obj => obj.Collideable)
                .ToList();

            var sameLaneObjects = new List<WorldObject>();
            collidableObjects.ForEach(obj =>
            {
                var vecToObj = new Vector2(obj.X, obj.Y) - Position;
                var objRotInRadians = CorrectRotation(obj.Rotation) * (Math.PI / 180);
                var objDirVec = new Vector2((float)Math.Cos(objRotInRadians), (float)Math.Sin(objRotInRadians));
                bus.RadarPackets.Add(new RadarPacket()
                {
                    Angle = Math.Atan2(vecToObj.Y, vecToObj.X),
                    Distance = vecToObj.Length(),
                    RelativeVelocity = (dir * car.Speed) - (objDirVec * GetObjectSpeed(obj)),
                    Type = obj.WorldObjectType
                });

                if (IsInTheSameLane(obj))
                {
                    sameLaneObjects.Add(obj);
                }
            });
            var closestObj = sameLaneObjects?.MinBy(obj => (new Vector2(car.X, car.Y) - new Vector2(obj.X, obj.Y)).Length());
            if (closestObj != null)
                Debug.WriteLine($"Object at ({closestObj.X}, {closestObj.Y}) is closest.");
            //CarLaneDebug();
        }

        /// <summary>
        /// Determines if two objects (A and B) will collide based on their velocity.
        /// </summary>
        /// <param name="a1">Current position of object A</param>
        /// <param name="a2">Next position of object A (A + Velocity)</param>
        /// <param name="b1">Current position of object B</param>
        /// <param name="b2">Next position of object B (B + Velocity)</param>
        /// <param name="radAngle">An angle threshold in radians. E.g. if object B is not moving and the angle between (B-A) and <paramref name="a2"/>-<paramref name="a1"/>
        /// is smaller than <paramref name="radAngle"/>, the objects will collide.</param>
        /// <returns>The number of ticks in which the two objects will collide. Returns -1 if they won't.</returns>
        static int TrajectoriesCollide(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float radAngle = 0)
        {
            var p = GetLineIntersection(a1,a2,b1,b2);
            var dirA = a2 - a1;
            var dirB = b2 - b1;

            //Either the lines are parallel or one object is not moving.
            if (p == null)
            {
                //A is moving
                if (dirA != Vector2.Zero)
                {
                    //angle = asin(cross / (aLength * bLength))
                    var diffAB = b1 - a1;
                    var angleBw = Math.Abs(MathF.Asin((a1.X * diffAB.Y - a1.Y * diffAB.X) / (dirA.Length() * diffAB.Length())));
                    return angleBw <= radAngle ? (int)(diffAB.Length() * MathF.Cos(angleBw) / dirA.Length()) : -1;
                }
                //B is moving
                else if (dirB != Vector2.Zero)
                {
                    var diffAB = b1 - a1;
                    var angleBw = Math.Abs(MathF.Asin((b1.X * diffAB.Y - b1.Y * diffAB.X) / (dirB.Length() * diffAB.Length())));
                    return angleBw <= radAngle ? (int)(diffAB.Length() * MathF.Cos(angleBw) / dirB.Length()) : -1;
                }
                //Neither are moving, they cannot collide
                return -1;
            }

            var P = (Vector2)p;
            int distAP = (int)((a1 - P).Length() / dirA.Length());
            int distBP = (int)((b1 - P).Length() / dirB.Length());
            return distAP == distBP ? distAP : -1;
        }

        /// <summary>
        /// Calculates the intersection point of two lines that are defined by two vectors (two points for each vector).
        /// </summary>
        /// <param name="a1">Current position of a point 'A'.</param>
        /// <param name="a2">Next position of the point 'A' (A + aVec)</param>
        /// <param name="b1">Current position of a point 'B'</param>
        /// <param name="b2">Next position of the point 'B' (B + bVec)</param>
        /// <returns>The intersection point.</returns>
        static Vector2? GetLineIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            Vector2 dirA = a2 - a1;
            Vector2 dirB = b2 - b1;

            float cross = dirA.X * dirB.Y - dirA.Y * dirB.X;

            //Lines are parallel
            if (Math.Abs(cross) < 1e-8)
            {
                return null;
            }

            Vector2 delta = b1 - a1;
            float t = (delta.X * dirB.Y - delta.Y * dirB.X) / cross;

            return a1 + t * dirA;
        }

        /// <summary>
        /// Outputs the indices of lanes that the <see cref="car"/> touches.
        /// Uncomment method call in <see cref="Process"/> to run.
        /// </summary>
        void CarLaneDebug()
        {
            var roads = World.Instance.WorldObjects.Where(obj => obj.WorldObjectType == WorldObjectType.Road)
                .OrderBy(obj => (new Vector2(car.X, car.Y) - new Vector2(obj.X, obj.Y)).Length()).Take(3);
            var laneLvl = new List<int>();
            foreach (var road in roads)
            {
                var pieceLvl = GetLaneLevels(road, car);
                if (pieceLvl[0] != -1 || laneLvl.Count < pieceLvl.Count)
                    laneLvl = pieceLvl;
            }
            laneLvl.ForEach(v =>
            {
                Debug.Write($"{v},");
            });
            Debug.WriteLine("");
        }
        /// <summary>
        /// Compares a <see cref="WorldObject"/> with the <see cref="car"/> based on whether they are in the same lane or not.
        /// </summary>
        /// <param name="wo">The <see cref="WorldObject"/> to compare. Has to be an element of <see cref="World.WorldObjects"/> of <see cref="World.Instance"/>,
        /// and <see cref="WorldObject.Collideable"/> has to be true if called from <see cref="Process"/>.</param>
        /// <returns>True if they are in the same lane, false otherwise.</returns>
        bool IsInTheSameLane(WorldObject wo)
        {
            //The three road pieces that are closest to the world object
            var roadPieces = World.Instance.WorldObjects.Where(obj => obj.WorldObjectType == WorldObjectType.Road)
                .OrderBy(obj => (new Vector2(wo.X, wo.Y) - new Vector2(obj.X, obj.Y)).Length()).Take(3);

            //The three road pieces that are closest to the car
            var carRoadPieces = World.Instance.WorldObjects.Where(obj => obj.WorldObjectType == WorldObjectType.Road)
                .OrderBy(obj => (new Vector2(car.X, car.Y) - new Vector2(obj.X, obj.Y)).Length()).Take(3);

            List<int> woLaneLvl = new List<int>();
            List<int> carLaneLvl = new List<int>();

            foreach (var roadPiece in roadPieces)
            {
                var pieceLvl = GetLaneLevels(roadPiece, wo);
                if (pieceLvl[0] != -1 || woLaneLvl.Count < pieceLvl.Count)
                    woLaneLvl = pieceLvl;
            }
            foreach (var carRoadPiece in carRoadPieces)
            {
                var pieceLvl = GetLaneLevels(carRoadPiece, car);
                if (pieceLvl[0] != -1 || carLaneLvl.Count < pieceLvl.Count)
                    carLaneLvl = pieceLvl;
            }
            return carLaneLvl.Any(lvl => woLaneLvl.Any(woLvl => lvl == woLvl));
        }

        /// <summary>
        /// Constructs polygons from the lanes of a <paramref name="road"/> (stored in <see cref="WorldObject.GlobalPoints"/>) then checks if any of them
        /// intersect with the polygon of a <paramref name="wObject"/>.
        /// </summary>
        /// <param name="road">A <see cref="WorldObject"/>, preferrably one with multiple <see cref="WorldObject.Geometries"/>, such as a road with multiple lanes.</param>
        /// <param name="wObject">A <see cref="WorldObject"/> with a single geometry.</param>
        /// <returns>The indices of the polygons that intersect with <paramref name="wObject"/></returns>
        List<int> GetLaneLevels(WorldObject road, WorldObject wObject)
        {
            List<int> lanes = new List<int>();

            for (int i = 0; i < road.GlobalPoints.Count - 1; i++)
            {
                var lane = road.GlobalPoints[i];
                var nextLane = road.GlobalPoints[i + 1];
                var poly = CreatePolyFromLanes(lane, nextLane);
                if (DoPolygonsIntersect(poly, wObject.GlobalPoints[0]))
                    lanes.Add(i);
            }
            if (lanes.Count <= 0)
                lanes.Add(-1);

            return lanes;
        }
        /// <summary>
        /// Merges the points of lanes <paramref name="l1"/> and <paramref name="l2"/> to create a single polygon.
        /// Note: May not be suitable for all road layouts.
        /// </summary>
        /// <param name="l1">First lane as a set of points.</param>
        /// <param name="l2">Second lane as a set of points, whose order is reversed by the method.</param>
        /// <returns>The merged polygon</returns>
        static IList<Avalonia.Point> CreatePolyFromLanes(IList<Avalonia.Point> l1, IList<Avalonia.Point> l2)
        {
            var poly = new List<Avalonia.Point>(l1);
            foreach (var pt in l2.Reverse())
            {
                poly.Add(pt);
            }
            return poly;
        }
        static float GetObjectSpeed(WorldObject obj)
        {
            if (obj is Car car)
            {
                return car.Speed;
            }
            else if (obj is Npc npc)
            {
                return (float)Speed.FromKmPerHour(npc.Speed).InPixelsPerTick();
            }
            else
            {
                return 0;
            }
        }
        double CorrectRotation(double rotation)
        {
            if (rotation < 0)
                rotation += 360;
            rotation += 90;
            rotation %= 360;
            return rotation;
        }
        /// <summary>
        /// Checks if two polygons intersect.
        /// First checks for any edge intersections, then checks if either polygon is inside the other.
        /// </summary>
        /// <param name="poly1">First polygon as a set of points</param>
        /// <param name="poly2">Second polygons as a set of points</param>
        /// <returns>True if the polygons intersect, otherwise false.</returns>
        static bool DoPolygonsIntersect(IList<Avalonia.Point> poly1, IList<Avalonia.Point> poly2)
        {
            //Check edge intersection
            for (int i = 0; i < poly1.Count; i++)
            {
                var a1 = poly1[i];
                var a2 = poly1[(i + 1) % poly1.Count];

                for (int j = 0; j < poly2.Count; j++)
                {
                    var b1 = poly2[j];
                    var b2 = poly2[(j + 1) % poly2.Count];

                    if (LinesIntersect(a1, a2, b1, b2))
                        return true;
                }
            }

            //Check if one polygon is completely inside another
            if (PointInPolygon(poly1[0], poly2) || PointInPolygon(poly2[0], poly1))
                return true;

            return false;
        }
        #region DoPolygonsIntersectMethods
        static bool LinesIntersect(Avalonia.Point p1, Avalonia.Point p2, Avalonia.Point q1, Avalonia.Point q2)
        {
            float o1 = Orientation(p1, p2, q1);
            float o2 = Orientation(p1, p2, q2);
            float o3 = Orientation(q1, q2, p1);
            float o4 = Orientation(q1, q2, p2);

            if (o1 != o2 && o3 != o4)
                return true;

            //Special Cases: colinear and overlapping
            if (o1 == 0 && OnSegment(p1, q1, p2)) return true;
            if (o2 == 0 && OnSegment(p1, q2, p2)) return true;
            if (o3 == 0 && OnSegment(q1, p1, q2)) return true;
            if (o4 == 0 && OnSegment(q1, p2, q2)) return true;

            return false;
        }

        static float Orientation(Avalonia.Point a, Avalonia.Point b, Avalonia.Point c)
        {
            double val = (b.Y - a.Y) * (c.X - b.X) - (b.X - a.X) * (c.Y - b.Y);
            if (Math.Abs(val) < 1e-10) return 0; //colinear
            return (val > 0) ? 1 : 2; //clock or counterclock wise
        }

        static bool OnSegment(Avalonia.Point a, Avalonia.Point b, Avalonia.Point c)
        {
            return b.X <= Math.Max(a.X, c.X) && b.X >= Math.Min(a.X, c.X) &&
                   b.Y <= Math.Max(a.Y, c.Y) && b.Y >= Math.Min(a.Y, c.Y);
        }

        static bool PointInPolygon(Avalonia.Point point, IList<Avalonia.Point> polygon)
        {
            int count = polygon.Count;
            bool result = false;
            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                if ((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y) &&
                    (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) /
                     (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    result = !result;
                }
            }
            return result;
        }
        #endregion
        public Radar(VirtualFunctionBus virtualFunctionBus, AutomatedCar car, int offset) : base(virtualFunctionBus)
        {
            triangle = new TriangleDetector(500);
            this.car = car;
            this.bus = virtualFunctionBus;
            this.offsetLength = offset;
            this.anchor = new Vector2(car.X, car.Y);
            this.dir = new Vector2((float)Math.Cos(CorrectRotation(car.Rotation) * (Math.PI/180)), (float)Math.Sin(CorrectRotation(car.Rotation) * (Math.PI / 180)));
            this.car.PropertyChangedEvent += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case "X":
                        anchor.X = this.car.X;
                        break;
                    case "Y":
                        anchor.Y = this.car.Y;
                        break;
                    case "Rotation":
                        dir.X = (float)Math.Cos(CorrectRotation(car.Rotation) * (Math.PI / 180));
                        dir.Y = (float)Math.Sin(CorrectRotation(car.Rotation) * (Math.PI / 180));
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
