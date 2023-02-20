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
    class Cylinder
    {
        private WpfCircle front;

        private WpfCircle back;

        private int nSides;

        private double frontRadius;

        private double backRadius;

        private double length;

        public Point3D center;

        private Point3D backcenter;

        public Cylinder(Point3D Center, int NSides, double FrontRadius, double BackRadius, 
            double Length)
        {
            center = Center;
            nSides = NSides;
            frontRadius = FrontRadius;
            backRadius = BackRadius;
            length = Length;

            front = new WpfCircle(nSides, center, frontRadius);

            backcenter = new Point3D(center.X, center.Y, center.Z - length);

            back = new WpfCircle(nSides, backcenter, backRadius);
        }

        public Cylinder(Point3D Center, int NSides, double FrontRadius, double BackRadius,
                            double Length, Point3D rotation_point, double radians)
        {
            center = Center;
            nSides = NSides;
            frontRadius = FrontRadius;
            backRadius = BackRadius;
            length = Length;

            front = new WpfCircle(nSides, center, frontRadius);
            backcenter = new Point3D(center.X, center.Y, center.Z - length);
            back = new WpfCircle(nSides, backcenter, backRadius);

            RotateZY(rotation_point, radians);
        }

        public void RotateZY(Point3D rotation_point, double radians)
        {
            front.RotateZY(rotation_point, radians);
            back.RotateZY(rotation_point, radians);
            backcenter = WpfUtils.RotatePointZY(backcenter, rotation_point, radians);
        }

        public void RotateXZ(Point3D rotation_point, double radians)
        {
            front.RotateXZ(rotation_point, radians);
            back.RotateXZ(rotation_point, radians);
            backcenter = WpfUtils.RotatePointXZ(backcenter, rotation_point, radians);
        }

        public void RotateXY(Point3D rotation_point, double radians)
        {
            front.RotateXY(rotation_point, radians);
            back.RotateXY(rotation_point, radians);
            backcenter = WpfUtils.RotatePointXY(backcenter, rotation_point, radians);
        }

        public void addToMesh(MeshGeometry3D mesh)
        {
            addToMesh(mesh, false, false);
        }

        public void addToMesh(MeshGeometry3D mesh, bool encloseTop, bool combineVertices)
        {
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
                backPoints.Reverse(); 

                for (int i = 1; i < frontPoints.Count; i++)
                {
                    WpfTriangle.addTriangleToMesh(frontPoints[i - 1], backPoints[i - 1], frontPoints[i], mesh);
                    WpfTriangle.addTriangleToMesh(frontPoints[i], backPoints[i - 1], backPoints[i], mesh);
                }

                if (encloseTop)
                {
                    front.addToMesh(mesh, false);
                    back.addToMesh(mesh, false);
                }
            }
        }

        public GeometryModel3D CreateModel(Color color)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addToMesh(mesh);

            Material material = new DiffuseMaterial(new SolidColorBrush(color));

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            return model;
        }

        public GeometryModel3D CreateModelEmissive(Color color)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addToMesh(mesh, true, false);


            Material materialBack = new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Colors.Black));
            Material material = new EmissiveMaterial(new SolidColorBrush(color));
            MaterialGroup materialGroup = new MaterialGroup();
            materialGroup.Children.Add(materialBack);
            materialGroup.Children.Add(material);

            GeometryModel3D model = new GeometryModel3D(mesh, materialGroup);

            return model;
        }

        public GeometryModel3D CreateModel(Color color, bool enclose)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addToMesh(mesh, enclose, false);

            Material material = new DiffuseMaterial(new SolidColorBrush(color));

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            return model;
        }



    }
}
