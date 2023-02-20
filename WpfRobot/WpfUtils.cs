using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;



using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Shapes;



namespace WpfRobot
{
    class WpfUtils
    {
        static double one_rad_in_degrees = (double)57.0 + ((double)17.0 / (double)60.0) + ((double)44.6 / ((double)3600.0));

        public static double GetMax(List<double> list)
        {
            if (list != null && list.Count > 0)
            {
                double most = double.MinValue;

                foreach (double d in list)
                {
                    if (d > most)
                    {
                        most = d;
                    }
                }

                return most;
            }

            return 0.0;
        }

        public static double GetMin(List<double> list)
        {
            if (list != null && list.Count > 0)
            {
                double least = double.MaxValue;

                foreach (double d in list)
                {
                    if (d < least)
                    {
                        least = d;
                    }
                }

                return least;
            }

            return 0.0;
        }

        public static Rect3D GetBoundsXY(List<Point3D> points)
        {
            double z = 0.0;

            List<double> xvalues = new List<double>();
            List<double> yvalues = new List<double>();

            foreach (Point3D p in points)
            {
                z = p.Z;
                xvalues.Add(p.X);
                yvalues.Add(p.Y);
            }

            double leastx = GetMin(xvalues);
            double mostx = GetMax(xvalues);
            double leasty = GetMin(yvalues);
            double mosty = GetMax(yvalues);

            Rect3D result = new Rect3D(leastx, leasty, z, mostx - leastx, mosty - leasty, 0.0);

            return result;
        }


        public static Point3D RotatePointXY(Point3D p, Point3D rotation_point, double radians)
        {
            Point3D new_point = new Point3D(rotation_point.X, rotation_point.Y, rotation_point.Z);

            try
            {
                if (radians != 0)
                {
                    double ydiff = p.Y - rotation_point.Y;
                    double xdiff = p.X - rotation_point.X;

                    double xd = (xdiff * Math.Cos(radians)) - (ydiff * Math.Sin(radians));

                    double yd = (xdiff * Math.Sin(radians)) + (ydiff * Math.Cos(radians));

                    new_point.X += xd;
                    new_point.Y += yd;
                    new_point.Z = p.Z;
                }
                else
                {
                    new_point.X = p.X;
                    new_point.Y = p.Y;
                    new_point.Z = p.Z;
                }
            }
            catch
            {
                
            }

            return new_point;
        }

        public static Point3D RotatePointXZ(Point3D p, Point3D rotation_point, double radians)
        {
            Point3D new_point = new Point3D(rotation_point.X, rotation_point.Y, rotation_point.Z);

            try
            {
                if (radians != 0)
                {
                    double ydiff = p.Z - rotation_point.Z;
                    double xdiff = p.X - rotation_point.X;

                    double xd = (xdiff * Math.Cos(radians)) - (ydiff * Math.Sin(radians));

                    double yd = (xdiff * Math.Sin(radians)) + (ydiff * Math.Cos(radians));

                    new_point.X += xd;
                    new_point.Z += yd;
                    new_point.Y = p.Y;
                }
                else
                {
                    new_point.X = p.X;
                    new_point.Y = p.Y;
                    new_point.Z = p.Z;
                }
            }
            catch
            {
                
            }

            return new_point;
        }

        public static Point3D RotatePointZY(Point3D p, Point3D rotation_point, double radians)
        {
            Point3D new_point = new Point3D(rotation_point.X, rotation_point.Y, rotation_point.Z);

            try
            {
                if (radians != 0)
                {
                    double ydiff = p.Y - rotation_point.Y;
                    double xdiff = p.Z - rotation_point.Z;

                    double xd = (xdiff * Math.Cos(radians)) - (ydiff * Math.Sin(radians));
                    double yd = (xdiff * Math.Sin(radians)) + (ydiff * Math.Cos(radians));

                    new_point.Z += xd;
                    new_point.Y += yd;
                    new_point.X = p.X;
                }
                else
                {
                    new_point.X = p.X;
                    new_point.Y = p.Y;
                    new_point.Z = p.Z;
                }
            }
            catch
            {
                
            }

            return new_point;
        }


        public static double radians_from_degrees(double degrees)
        {
            return degrees / one_rad_in_degrees;
        }

        public static double degrees_from_radians(double radians)
        {
            return radians * one_rad_in_degrees;
        }




    }
}
