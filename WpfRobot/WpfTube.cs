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
    class WpfTube
    {
        public bool closeTop = false;
        public bool closeBottom = false;

        public List<WpfCircle> circles;


        int nSides = 3;

        public WpfTube(int NSides)
        {
            nSides = NSides;
            circles = new List<WpfCircle>();
        }

        public void addCircle(WpfCircle c)
        {
            circles.Add(c);
        }

        public void addToMesh(MeshGeometry3D mesh, bool combineVertices)
        {
            for (int c = 1; c < circles.Count; c++)
            {
                WpfCircle front = circles[c - 1];
                WpfCircle back = circles[c];

                if (front.points.Count > 2)
                {
                    List<Point3D> frontPoints = new List<Point3D>();
                    foreach (Point3D p in front.points)
                    {
                        frontPoints.Add(p);
                    }
                    frontPoints.Add(front.points[0]);

                    List<Point3D> backPoints = new List<Point3D>();
                    foreach (Point3D p in back.points)
                    {
                        backPoints.Add(p);
                    }
                    backPoints.Add(back.points[0]);

                    for (int i = 1; i < frontPoints.Count; i++)
                    {
                        WpfTriangle.addTriangleToMesh(frontPoints[i - 1], backPoints[i - 1], frontPoints[i], mesh, combineVertices);
                        WpfTriangle.addTriangleToMesh(frontPoints[i], backPoints[i - 1], backPoints[i], mesh, combineVertices);
                    }
                }
            }

            if (closeTop)
            {
                circles[0].addToMesh(mesh, false);
            }

            if (closeBottom)
            {
                circles[circles.Count - 1].addToMesh(mesh, false);
            }

        }

        public GeometryModel3D CreateModel(Color color, bool combineVertices)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addToMesh(mesh, combineVertices);

            Material material = new DiffuseMaterial(new SolidColorBrush(color));

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            return model;
        }

        public void rotateCirclesZY(Point3D p, double degrees, WpfCircle start)
        {
            for (int i = 0; i < circles.Count; i++)
            {
                if (i >= circles.IndexOf(start))
                {
                    circles[i].RotateZY(p, WpfUtils.radians_from_degrees(degrees));
                }
            }
        }

        public void rotateCirclesZY(Point3D p, double degrees)
        {
            if (circles != null && circles.Count > 0)
            {
                rotateCirclesZY(p, degrees, circles[0]);
            }
        }

        public void rotateCirclesXY(Point3D p, double degrees)
        {
            for (int i = 0; i < circles.Count; i++)
            {
                circles[i].RotateXY(p, WpfUtils.radians_from_degrees(degrees));
            }
        }

        public void rotateCirclesXZ(Point3D p, double degrees)
        {
            for (int i = 0; i < circles.Count; i++)
            {
                circles[i].RotateXZ(p, WpfUtils.radians_from_degrees(degrees));
            }
        }


    }
}
