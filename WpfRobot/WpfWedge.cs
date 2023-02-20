using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WpfRobot
{
    class WpfWedge
    {
        WpfPolygon front;
        WpfPolygon back;
        WpfCube cube;

        public Point3D topCenter;
        public Point3D topFront;

        double topWidthPercent;
        double bottomWidthPercent;
        double topDepthPercent;
        double bottomDepthPercent;
        double heightPercent;
        double topOffSetPercent;
        double xAlignmentTopPercent;
        double xAlignmentBottomPercent;
        double zAlignmentTopPercent;
        double zAlignmentBottomPercent;


        double topWidth;
        double bottomWidth;
        double topDepth;
        double bottomDepth;
        double height;
        double topOffSet;
        double xAlignmentTop;
        double xAlignmentBottom;
        double zAlignmentTop;
        double zAlignmentBottom;


        private Point3D cubeOffset(double x, double y, double z)
        {
            return new Point3D(
                cube.origin.X + x,
                cube.origin.Y - y,
                cube.origin.Z + z
                    );
        }

        public WpfWedge mirrorX()
        {
            WpfWedge wedge2 = new WpfWedge(cube, 
                topWidthPercent,
                bottomWidthPercent,
                topDepthPercent,
                bottomDepthPercent,
                heightPercent,
                topOffSetPercent,
                -xAlignmentTopPercent,
                -xAlignmentBottomPercent,
                zAlignmentTopPercent,
                zAlignmentBottomPercent);

            return wedge2;
        }

        
        public WpfTube makeTube(int NSides, bool addBottom, bool addTop)
        {
            Point3D p = cube.centerTop();

            p.Y -= topOffSet;
            p.X += xAlignmentTop;
            p.Z += zAlignmentTop;

            WpfCircle top = new WpfCircle(NSides, p, topDepth / 2, topWidth / 2);
            top.RotateZY(p, WpfUtils.radians_from_degrees(90));

            Point3D p2 = cube.centerTop();
            p2.Y -= topOffSet + height;
            p2.X += xAlignmentBottom;
            p2.Z += zAlignmentBottom;

            WpfCircle bottom = new WpfCircle(NSides, p2, bottomDepth / 2, bottomWidth / 2);
            bottom.RotateZY(p2, WpfUtils.radians_from_degrees(90));

            WpfTube tube = new WpfTube(NSides);

            if (addTop)
            {
                tube.closeTop = true;
            }

            tube.addCircle(top);
            tube.addCircle(bottom);

            if (addBottom)
            {
                tube.closeBottom = true;
            }

            return tube;
        }


        public WpfWedge(WpfCube containingCube,
            // the values below are all expressed as percent of the cube dimensions
            double TopWidth,
            double BottomWidth,
            double TopDepth,
            double BottomDepth,
            double Height,
            double TopOffSet, // % offset from top
            double XAlignmentTop, // % offset to left or right
            double XAlignmentBottom,
            double ZAlignmentTop, // % offset to front or back
            double ZAlignmentBottom
            )
        {
            cube = new WpfCube(containingCube);

            front = new WpfPolygon();
            back = new WpfPolygon();


            // save percentages
            topWidthPercent = TopWidth;
            bottomWidthPercent = BottomWidth;
            topDepthPercent = TopDepth;
            bottomDepthPercent = BottomDepth;
            heightPercent = Height;
            topOffSetPercent = TopOffSet;
            xAlignmentTopPercent = XAlignmentTop;
            xAlignmentBottomPercent = XAlignmentBottom;
            zAlignmentTopPercent = ZAlignmentTop;
            zAlignmentBottomPercent = ZAlignmentBottom;

            // convert to actual dimensions based on cube
            TopWidth *= cube.width;
            BottomWidth *= cube.width;
            TopDepth *= cube.depth;
            BottomDepth *= cube.depth;
            Height *= cube.height;
            TopOffSet *= cube.height;
            XAlignmentTop *= cube.width;
            XAlignmentBottom *= cube.width;
            ZAlignmentTop *= cube.depth;
            ZAlignmentBottom *= cube.depth;


            topWidth = TopWidth;
            bottomWidth = BottomWidth;
            topDepth = TopDepth;
            bottomDepth = BottomDepth;
            height = Height;
            topOffSet = TopOffSet;
            xAlignmentTop = XAlignmentTop;
            xAlignmentBottom = XAlignmentBottom;
            zAlignmentTop = ZAlignmentTop;
            zAlignmentBottom = ZAlignmentBottom;




            topCenter = cube.centerTop();
            topCenter.Y -= TopOffSet;
            topCenter.Z += ZAlignmentTop;

            topFront = cube.centerTop();
            topFront.Y -= TopOffSet;
            topFront.Z += ZAlignmentTop;
            topFront.Z += topDepth / 2;
        }

        public void makeWedge()
        {
            double xpad = (cube.width - topWidth) / 2;
            double zpad = (cube.depth - topDepth) / 2;

            Point3D p1 = cubeOffset(
                xpad + xAlignmentTop,
                topOffSet,
                (cube.depth - zpad) + zAlignmentTop
                );
            front.addPoint(p1);

            Point3D p2 = cubeOffset(
                (cube.width - xpad) + xAlignmentTop,
                topOffSet,
                (cube.depth - zpad) + zAlignmentTop
                );
            front.addPoint(p2);

            xpad = (cube.width - bottomWidth) / 2;
            zpad = (cube.depth - bottomDepth) / 2;

            Point3D p3 = cubeOffset(
                (cube.width - xpad) + xAlignmentBottom,
                topOffSet + height,
                (cube.depth - zpad) + zAlignmentBottom
                );
            front.addPoint(p3);

            Point3D p4 = cubeOffset(
                xpad + xAlignmentBottom,
                topOffSet + height,
                (cube.depth - zpad) + zAlignmentBottom
                );
            front.addPoint(p4);

            xpad = (cube.width - topWidth) / 2;
            zpad = (cube.depth - topDepth) / 2;

            Point3D p5 = cubeOffset(
                xpad + xAlignmentTop,
                topOffSet,
                zpad + zAlignmentTop
                );
            back.addPoint(p5);

            Point3D p6 = cubeOffset(
                (cube.width - xpad) + xAlignmentTop,
                topOffSet,
                zpad + zAlignmentTop
                );
            back.addPoint(p6);

            xpad = (cube.width - bottomWidth) / 2;
            zpad = (cube.depth - bottomDepth) / 2;

            Point3D p7 = cubeOffset(
                (cube.width - xpad) + xAlignmentBottom,
                topOffSet + height,
                zpad + zAlignmentBottom
                );
            back.addPoint(p7);

            Point3D p8 = cubeOffset(
                xpad + xAlignmentBottom,
                topOffSet + height,
                zpad + zAlignmentBottom
                );
            back.addPoint(p8);
        }

        public void RotateZY(Point3D rotation_point, double radians)
        {
            front.RotateZY(rotation_point, radians);
            back.RotateZY(rotation_point, radians);
        }

        public void addToMesh(MeshGeometry3D mesh, bool combineVertices)
        {
            if (front.points.Count > 2)
            {
                front.reversePoints();
                back.reversePoints();

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

            back.reversePoints();

            front.addToMesh(mesh);
            back.addToMesh(mesh);
        }

        static Point3D clonePoint(Point3D p)
        {
            Point3D p2 = new Point3D(p.X, p.Y, p.Z);
            return p2;
        }


        public void move(Point3D from, Point3D to)
        {
            front.move(from, to);
            back.move(from, to);
        }

        public GeometryModel3D CreateModel(Color color, bool combineVertices)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            addToMesh(mesh, combineVertices);

            Material material = new DiffuseMaterial(new SolidColorBrush(color));

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            return model;
        }



    }
}
