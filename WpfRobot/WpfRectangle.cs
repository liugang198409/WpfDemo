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
    public class WpfRectangle
    {
        private Point3D p0;
        private Point3D p1;
        private Point3D p2;
        private Point3D p3;

        public WpfRectangle(Point3D P0, Point3D P1, Point3D P2, Point3D P3)
        {
            p0 = P0;
            p1 = P1;
            p2 = P2;
            p3 = P3;
        }

        public WpfRectangle(Point3D P0, double w, double h, double d)
        {
            p0 = P0;

            if (w != 0.0 && h != 0.0) // front / back
            {
                p1 = new Point3D(p0.X + w, p0.Y, p0.Z);
                p2 = new Point3D(p0.X + w, p0.Y - h, p0.Z);
                p3 = new Point3D(p0.X,     p0.Y - h, p0.Z);
            }
            else if (w != 0.0 && d != 0.0) // top / bottom
            {
                p1 = new Point3D(p0.X,     p0.Y, p0.Z + d);
                p2 = new Point3D(p0.X + w, p0.Y, p0.Z + d);
                p3 = new Point3D(p0.X + w, p0.Y, p0.Z);
            }
            else if (h != 0.0 && d != 0.0) // side / side
            {
                p1 = new Point3D(p0.X, p0.Y, p0.Z + d);
                p2 = new Point3D(p0.X, p0.Y - h, p0.Z + d);
                p3 = new Point3D(p0.X, p0.Y - h, p0.Z);
            }
        }

        public void addToMesh(MeshGeometry3D mesh)
        {
            WpfTriangle.addTriangleToMesh(p0, p1, p2, mesh);
            WpfTriangle.addTriangleToMesh(p2, p3, p0, mesh);
        }

        public static void addRectangleToMesh(Point3D p0, Point3D p1, Point3D p2, Point3D p3, MeshGeometry3D mesh)
        {
            WpfTriangle.addTriangleToMesh(p0, p1, p2, mesh);
            WpfTriangle.addTriangleToMesh(p2, p3, p0, mesh);
        }

        public static GeometryModel3D CreateRectangleModel(Point3D p0, Point3D p1, Point3D p2, Point3D p3)
        {
            return CreateRectangleModel(p0, p1, p2, p3, false);
        }

        public static GeometryModel3D CreateRectangleModel(Point3D p0, Point3D p1, Point3D p2, Point3D p3, bool texture)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addRectangleToMesh(p0, p1, p2, p3, mesh);

            Material material = new DiffuseMaterial(
                new SolidColorBrush(Colors.White));

            GeometryModel3D model = new GeometryModel3D(
                mesh, material);

            if (texture)
            {
                // Create a collection of texture coordinates for the MeshGeometry3D.
                PointCollection textureCoordinatesCollection = new PointCollection();

                textureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
                textureCoordinatesCollection.Add(new System.Windows.Point(1, 0));
                textureCoordinatesCollection.Add(new System.Windows.Point(1, 1));

                textureCoordinatesCollection.Add(new System.Windows.Point(1, 1)); 
                textureCoordinatesCollection.Add(new System.Windows.Point(0, 1));
                textureCoordinatesCollection.Add(new System.Windows.Point(0, 0));

                mesh.TextureCoordinates = textureCoordinatesCollection;
            }

            return model;
        }

   }
}
