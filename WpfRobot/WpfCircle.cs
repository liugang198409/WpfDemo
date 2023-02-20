
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
    class WpfCircle
    {
        private int nSides = 6;

        private Point3D top;

        private double angle;

        Point3D center;

        public List<Point3D> points;

        private double radiusY;
        private double radiusX;

        public WpfCircle(int NSides, Point3D Center, double Radius)
        {
            nSides = NSides;

            angle = (double)360.0 / (double)nSides;

            center = new Point3D(Center.X, Center.Y, Center.Z);

            radiusY = Radius;
            radiusX = Radius;

            makeCircle();
        }

        public WpfCircle(int NSides, Point3D Center, double RadiusY, double RadiusX)
        {
            nSides = NSides;

            angle = (double)360.0 / (double)nSides;

            center = new Point3D(Center.X, Center.Y, Center.Z);

            radiusY = RadiusY;
            radiusX = RadiusX;

            makeCircle();
        }

        private void makeCircle()
        {
            points = new List<Point3D>();

            top = new Point3D(center.X, center.Y + radiusY, center.Z);
            points.Add(top);

            for (int i = 1; i < nSides; i++)
            {
                Point3D p = WpfUtils.RotatePointXY(top, center, WpfUtils.radians_from_degrees(angle * i));

                if (radiusX != radiusY)
                {
                    double diff = p.X - center.X;
                    diff *= radiusX;
                    diff /= radiusY;
                    p = new Point3D(center.X + diff, p.Y, p.Z);
                }

                points.Add(p);
            }
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

            center = WpfUtils.RotatePointZY(center, rotation_point, radians);

            points = newlist;
        }

        public void RotateXY(Point3D rotation_point, double radians)
        {
            List<Point3D> newlist = new List<Point3D>();

            foreach (Point3D p in points)
            {
                newlist.Add(WpfUtils.RotatePointXY(p, rotation_point, radians));
            }

            center = WpfUtils.RotatePointXY(center, rotation_point, radians);

            points = newlist;
        }

        public void RotateXZ(Point3D rotation_point, double radians)
        {
            List<Point3D> newlist = new List<Point3D>();

            foreach (Point3D p in points)
            {
                newlist.Add(WpfUtils.RotatePointXZ(p, rotation_point, radians));
            }

            center = WpfUtils.RotatePointXZ(center, rotation_point, radians);

            points = newlist;
        }


        public void addToMesh(MeshGeometry3D mesh, bool combineVertices)
        {
            if (points.Count > 2)
            {
                List<Point3D> temp = new List<Point3D>();

                foreach (Point3D p in points)
                {
                    temp.Add(p);
                }

                temp.Add(points[0]);

                for (int i = 1; i < temp.Count; i++)
                {
                    WpfTriangle.addTriangleToMesh(temp[i], center, temp[i - 1], mesh, combineVertices);
                }
                
            }
        }

        public GeometryModel3D createModel(Color color, bool combineVertices)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addToMesh(mesh, combineVertices);

            Material material = new DiffuseMaterial(
                new SolidColorBrush(color));

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            return model;
        }

        public static GeometryModel3D CreateCircleModel(int NSides, Point3D Center, double Diameter)
        {
            return CreateCircleModel(NSides, Center, Diameter, false);
        }

        public static GeometryModel3D CreateCircleModel(int NSides, Point3D Center, double Diameter, bool texture)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            WpfCircle circle = new WpfCircle(NSides, Center, Diameter);

            circle.addToMesh(mesh, false);

            Material material = new DiffuseMaterial(
                new SolidColorBrush(Colors.White));

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            if (texture)
            {
                PointCollection textureCoordinatesCollection = new PointCollection();

                for (int i = 0; i < circle.points.Count; i++)
                {
                    textureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
                    textureCoordinatesCollection.Add(new System.Windows.Point(1, 0));
                    textureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
                }

                textureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
                textureCoordinatesCollection.Add(new System.Windows.Point(0, 1));
                textureCoordinatesCollection.Add(new System.Windows.Point(0, 0));

                mesh.TextureCoordinates = textureCoordinatesCollection;
            }

            return model;
        }



    }
}
