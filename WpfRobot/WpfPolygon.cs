using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WpfRobot
{
    class WpfPolygon
    {
        public List<Point3D> points;

        public void offset(Point3D offset)
        {
            List<Point3D> newpoints = new List<Point3D>();

            foreach (Point3D p in points)
            {
                Point3D p2 = clonePoint(p);
                p2.X += offset.X;
                p2.Y += offset.Y;
                p2.Z += offset.Z;
                newpoints.Add(p2);
            }

            points = newpoints;
        }

        public void move(Point3D from, Point3D to)
        {
            double xDiff = to.X - from.X;
            double yDiff = to.Y - from.Y;
            double zDiff = to.Z - from.Z;

            List<Point3D> newpoints = new List<Point3D>();

            foreach (Point3D p in points)
            {
                Point3D p1 = new Point3D(p.X + xDiff,
                                         p.Y + yDiff,
                                         p.Z + zDiff);
                newpoints.Add(p1);
            }

            points = newpoints;
        }

        public WpfPolygon()
        {
            points = new List<Point3D>();
        }

        public WpfPolygon(List<Point3D> Points)
        {
            points = new List<Point3D>();

            foreach (Point3D p in Points)
            {
                Point3D p1 = clonePoint(p);
                points.Add(p1);
            }
        }

        public void addPoint(Point3D p)
        {
            points.Add(p);
        }

        public void reversePoints()
        {
            points.Reverse();
        }

        public void RotateZY(Point3D rotation_point, double radians)
        {
            List<Point3D> newlist = new List<Point3D>();

            foreach (Point3D p in points)
            {
                newlist.Add(WpfUtils.RotatePointZY(p, rotation_point, radians));
            }

            points = newlist;
        }

        public void RotateXY(Point3D rotation_point, double radians)
        {
            List<Point3D> newlist = new List<Point3D>();

            foreach (Point3D p in points)
            {
                newlist.Add(WpfUtils.RotatePointXY(p, rotation_point, radians));
            }

            points = newlist;
        }


        public Point3D GetCenter()
        {
            Rect3D r = WpfUtils.GetBoundsXY(points);

            Point3D p = new Point3D(r.X, r.Y, r.Z);

            p.X += r.SizeX / 2;
            p.Y += r.SizeY / 2;

            return p;
        }

        public void addToMesh(MeshGeometry3D mesh)
        {
            addToMesh(mesh, points);
        }

        public void addToMesh(MeshGeometry3D mesh, bool reverse)
        {
            if (reverse == false)
            {
                addToMesh(mesh, points);
            }
            else
            {
                List<Point3D> pointList = new List<Point3D>();
                foreach (Point3D p in points)
                {
                    pointList.Add(p);
                }
                pointList.Reverse();
                addToMesh(mesh, pointList);
            }
        }

        public void addToMesh(MeshGeometry3D mesh, List<Point3D> pointList)
        {
            if (pointList.Count == 3)
            {
                WpfTriangle.addTriangleToMesh(pointList[0], pointList[1], pointList[2], mesh);
            }
            else if (pointList.Count == 4)
            {
                WpfTriangle.addTriangleToMesh(pointList[0], pointList[1], pointList[2], mesh);
                WpfTriangle.addTriangleToMesh(pointList[0], pointList[2], pointList[3], mesh);

            }
            else if (pointList.Count > 4)
            {
                Point3D center = GetCenter();

                List<Point3D> temp = new List<Point3D>();

                foreach (Point3D p in pointList)
                {
                    temp.Add(p);
                }

                temp.Add(pointList[0]);

                for (int i = 1; i < temp.Count; i++)
                {
                    WpfTriangle.addTriangleToMesh(temp[i], center, temp[i - 1], mesh);
                }

            }
        }

        static Point3D clonePoint(Point3D p)
        {
            Point3D p2 = new Point3D(p.X, p.Y, p.Z);
            return p2;
        }

        public GeometryModel3D CreatePolygonModel(Color color)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addToMesh(mesh, points);

            Material material = new DiffuseMaterial(
                new SolidColorBrush(color));

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            return model;
        }



    }
}
