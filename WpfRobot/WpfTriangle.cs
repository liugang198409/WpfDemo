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
    class WpfTriangle
    {
        private Point3D p1;
        private Point3D p2;
        private Point3D p3;

        public WpfTriangle(Point3D P1, Point3D P2, Point3D P3)
        {
            p1 = P1;
            p2 = P2;
            p3 = P3;
        }


        public static void addTriangleToMesh(Point3D p0, Point3D p1, Point3D p2, MeshGeometry3D mesh)
        {
            addTriangleToMesh(p0, p1, p2, mesh, false);
        }

        

        public static void addPointCombined(Point3D point, MeshGeometry3D mesh, Vector3D normal)
        {
            bool found = false;

            int i = 0;

            foreach (Point3D p in mesh.Positions)
            {
                if (p.Equals(point))
                {
                    found = true;
                    mesh.TriangleIndices.Add(i);
                    mesh.Positions.Add(point);
                    mesh.Normals.Add(normal);
                    break;
                }

                i++;
            }

            if (!found)
            {
                mesh.Positions.Add(point);
                mesh.TriangleIndices.Add(mesh.TriangleIndices.Count);
                mesh.Normals.Add(normal);
            }

        }



        
        public static void addTriangleToMesh(Point3D p0, Point3D p1, Point3D p2,
            MeshGeometry3D mesh, bool combine_vertices)
        {
            Vector3D normal = CalculateNormal(p0, p1, p2);

            if (combine_vertices)
            {
                addPointCombined(p0, mesh, normal);
                addPointCombined(p1, mesh, normal);
                addPointCombined(p2, mesh, normal);
            }
            else
            {
                mesh.Positions.Add(p0);
                mesh.Positions.Add(p1);
                mesh.Positions.Add(p2);
                mesh.TriangleIndices.Add(mesh.TriangleIndices.Count);
                mesh.TriangleIndices.Add(mesh.TriangleIndices.Count);
                mesh.TriangleIndices.Add(mesh.TriangleIndices.Count);
                mesh.Normals.Add(normal);
                mesh.Normals.Add(normal);
                mesh.Normals.Add(normal);
            }
        }


        public GeometryModel3D CreateTriangleModel(Color color)
        {
            return CreateTriangleModel(p1, p2, p3, color);
        }

        public static GeometryModel3D CreateTriangleModel(Point3D P0, Point3D P1, Point3D P2, Color color)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addTriangleToMesh(P0, P1, P2, mesh);

            Material material = new DiffuseMaterial(new SolidColorBrush(color));

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            return model;
        }

        public static Vector3D CalculateNormal(Point3D P0, Point3D P1, Point3D P2)
        {
            Vector3D v0 = new Vector3D(P1.X - P0.X, P1.Y - P0.Y, P1.Z - P0.Z);

            Vector3D v1 = new Vector3D(P2.X - P1.X, P2.Y - P1.Y, P2.Z - P1.Z);

            return Vector3D.CrossProduct(v0, v1);
        }
    }

}
