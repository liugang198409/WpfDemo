using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Shapes;



using System.Windows;
using System.Windows.Controls;


namespace WpfRobot
{
    class WpfRobotBody
    {
        private RotateTransform3D rotateTransform;
        private TranslateTransform3D translateTransform;


        private Color color = Colors.Silver;


        private bool hasBelt = true;
        private bool hasBoots = true;
        private bool hasGauntlets = true;


        private Point3D hipJoint;
        private Point3D kneeJoint;
        private Point3D ankleJoint;
        private Point3D shoulderJointLeft;
        private Point3D shoulderJointRight;
        private Point3D elbowJoint;

        private GeometryModel3D thighModelLeft;
        private GeometryModel3D thighModelRight;
        private GeometryModel3D legModelLeft;
        private GeometryModel3D legModelRight;
        private GeometryModel3D footModelLeft;
        private GeometryModel3D footModelRight;
        private GeometryModel3D bootModelLeft;
        private GeometryModel3D bootModelRight;

        private GeometryModel3D armLeftModel;
        private GeometryModel3D armRightModel;
        private GeometryModel3D foreArmLeftModel;
        private GeometryModel3D foreArmRightModel;
        private GeometryModel3D handLeftModel;
        private GeometryModel3D handRightModel;
        private GeometryModel3D gauntletLeftModel;
        private GeometryModel3D gauntletRightModel;



        private Point3D origin;

        private double width = 0;
        private double height = 0;
        private double depth = 0;


        private Model3DGroup robotModelGroup;



        private Point3D eyePointLeft = new Point3D(0, 0, 0);
        private Point3D eyePointRight = new Point3D(0, 0, 0);




        public WpfRobotBody()
        {
            getDimensions();

            origin = getModelPlace();

            robotModelGroup = createModel(true);

            addMotionTransforms();

            workLegs();

            workArms();
        }


        public Model3DGroup getModelGroup()
        {
            return robotModelGroup;
        }


        public RotateTransform3D getRotateTransform()
        {
            return rotateTransform;
        }

        public TranslateTransform3D getTranslateTransform()
        {
           return translateTransform;
        }


        public Point3D getModelPlace()
        {
            Point3D p = new Point3D(-(getWidth() / 2), getHeight(), -depth / 2);

            return p;
        }

        public static double getHeight()
        {
            return WpfScene.sceneSize / 3;
        }

        public static double getHeadHeight()
        {
            return getHeight() / 8;
        }

        public static double getWidth()
        {
            return (getHeight() * 2) / 8;  // based on body width = 3 head widths
        }

        public void getDimensions()
        {
            height = getHeight();
            width = getWidth();

            double headHeight = (double)getHeight() / 8; // is 1/8 of person height
            depth = (headHeight * 8) / 7; // based upon rule of head depth being 7/8 of height
        }

        Point3D clonePoint(Point3D p)
        {
            return new Point3D(p.X, p.Y, p.Z);
        }

        public Model3DGroup createModel(bool combineVertices)
        {
            int tuberes = 16;


            Model3DGroup modelGroup = new Model3DGroup();

            getDimensions();

            WpfCube cube = new WpfCube(origin, width, height, depth);

            double headHeightPercentage = cube.height / 8; // proportion rule of thumb
            double headWidthPercentage = (headHeightPercentage * 2) / 3; // proportion rule of thumb

            headHeightPercentage /= cube.height; // convert to a percent not absolute dimension
            headWidthPercentage /= cube.width; // convert to a percent not absolute dimension

            double footOffset = headHeightPercentage * 7.75;

            double torsoHeight = headHeightPercentage * 2.55;
            double torsoOffset = headHeightPercentage * 1.5;

            WpfWedge head = new WpfWedge(cube,
                           headWidthPercentage, //topWidth,
                           headWidthPercentage * 0.85, //bottomWidth,
                           0.75, //topDepth,
                           0.67, //bottomDepth,
                           headHeightPercentage, //height,
                           0, //offSet, // % offset from top
                           0, //xAlignmentTop, // % offset to left or right
                           0, //xAlignmentBottom,
                           0, //zAlignmentTop, // % offset to front or back
                           0 //zAlignmentBottom
                );

            WpfTube headTube = head.makeTube(tuberes, true, true);


            double eyeDiameter = getHeadHeight() / 10;
            double eyeLength = getHeadHeight() / 25;

            eyePointLeft = clonePoint(origin);
            eyePointLeft.X += width / 2;
            eyePointLeft.Z += depth * 0.9;
            eyePointLeft.Y -= getHeadHeight() / 2;
            eyePointLeft.X -= (getHeadHeight() / 6);

            eyePointRight = clonePoint(origin);
            eyePointRight.X += width / 2;
            eyePointRight.Z += depth * 0.9;
            eyePointRight.Y -= getHeadHeight() / 2;
            eyePointRight.X += (getHeadHeight() / 6);

            Cylinder leftEye = new Cylinder(eyePointLeft, 12, eyeDiameter, eyeDiameter, eyeLength);
            modelGroup.Children.Add(leftEye.CreateModelEmissive(Colors.Red));

            Cylinder rightEye = new Cylinder(eyePointRight, 12, eyeDiameter, eyeDiameter, eyeLength);
            modelGroup.Children.Add(rightEye.CreateModelEmissive(Colors.Red));


            WpfWedge neck = new WpfWedge(cube,
                           headWidthPercentage * 0.7, //topWidth,
                           headWidthPercentage * 0.7, //bottomWidth,
                           0.45, //topDepth,
                           0.45, //bottomDepth,
                           headHeightPercentage, //height,
                           headHeightPercentage, //offSet, // % offset from top
                           0, //xAlignmentTop, // % offset to left or right
                           0, //xAlignmentBottom,
                           -0.1, //zAlignmentTop, // % offset to front or back
                           -0.1 //zAlignmentBottom
                );
            WpfTube neckTube = neck.makeTube(tuberes, false, false);


            WpfWedge shoulder = new WpfWedge(cube,
                           headWidthPercentage * 0.7, //topWidth,
                           1, //bottomWidth,
                           0.46, //topDepth,
                           0.8, //bottomDepth,
                           headHeightPercentage * 0.23, //height,
                           torsoOffset - (headHeightPercentage * 0.22), //offSet, // % offset from top
                           0, //xAlignmentTop, // % offset to left or right
                           0, //xAlignmentBottom,
                           -0.1, //zAlignmentTop, // % offset to front or back
                           0 //zAlignmentBottom
                );
            WpfTube shoulderTube = shoulder.makeTube(tuberes, true, false);


            WpfWedge torso = new WpfWedge(cube,
                           headWidthPercentage * 2.2, //topWidth,
                           headWidthPercentage * 1.85, //bottomWidth,
                           0.8, //topDepth,
                           0.6, //bottomDepth,
                           torsoHeight, //height,
                           torsoOffset, //offSet, // % offset from top
                           0, //xAlignmentTop, // % offset to left or right
                           0, //xAlignmentBottom,
                           0, //zAlignmentTop, // % offset to front or back
                           0 //zAlignmentBottom
                );
            WpfTube torsoTube = torso.makeTube(tuberes, false, true);

            WpfWedge thighLeft = new WpfWedge(cube,
                           headWidthPercentage * 0.96, //topWidth,
                           headWidthPercentage * 0.57, //bottomWidth,
                           0.55, //topDepth,
                           0.5, //bottomDepth,
                           headHeightPercentage * 2.1, //height,
                           headHeightPercentage * 4, //offSet, // % offset from top
                           headWidthPercentage * 0.44, //xAlignmentTop, // % offset to left or right
                           headWidthPercentage * 0.45, //xAlignmentBottom,
                           0, //zAlignmentTop, // % offset to front or back
                           0 //zAlignmentBottom
                );
            WpfWedge thighRight = thighLeft.mirrorX();
            WpfTube thighRightTube = thighRight.makeTube(tuberes, false, false);
            WpfTube thighLeftTube = thighLeft.makeTube(tuberes, false, false);
            hipJoint = thighLeft.topCenter;

            WpfWedge legLeft = new WpfWedge(cube,
                           headWidthPercentage * 0.57, //topWidth,
                           headWidthPercentage * 0.47, //bottomWidth,
                           0.5, //topDepth,
                           0.45, //bottomDepth,
                           headHeightPercentage * 1.76, //height,
                           headHeightPercentage * 6, //offSet, // % offset from top
                           headWidthPercentage * 0.45, //xAlignmentTop, // % offset to left or right
                           headWidthPercentage * 0.4, //xAlignmentBottom,
                           0, //zAlignmentTop, // % offset to front or back
                           0.0 //zAlignmentBottom
                );
            WpfWedge legRight = legLeft.mirrorX();
            WpfTube legRightTube = legRight.makeTube(tuberes, false, true);
            WpfTube legLeftTube = legLeft.makeTube(tuberes, false, true);
            kneeJoint = legLeft.topFront;

            WpfWedge footLeft = new WpfWedge(cube,
                           headWidthPercentage * 0.4, //topWidth,
                           headWidthPercentage * 0.42, //bottomWidth,
                           0.35, //topDepth,
                           0.95, //bottomDepth,
                           headHeightPercentage / 3, //height,
                           footOffset, //offSet, // % offset from top
                           headWidthPercentage * 0.4, //xAlignmentTop, // % offset to left or right
                           headWidthPercentage * 0.4, //xAlignmentBottom,
                           0.0, //zAlignmentTop, // % offset to front or back
                           0.3 //zAlignmentBottom
                );
            WpfWedge footRight = footLeft.mirrorX();
            WpfTube footRightTube = footRight.makeTube(tuberes, false, false);
            WpfTube footLeftTube = footLeft.makeTube(tuberes, false, false);
            ankleJoint = footLeft.topCenter;


            WpfWedge armLeft = new WpfWedge(cube,
                           headWidthPercentage / 2, //topWidth,
                           headWidthPercentage / 2, //bottomWidth,
                           0.5, //topDepth,
                           0.46, //bottomDepth,
                           headHeightPercentage * 1.5, //height,
                           headHeightPercentage * 1.5, //offSet, // % offset from top
                           0.4, //xAlignmentTop, // % offset to left or right
                           0.44, //xAlignmentBottom,
                           0, //zAlignmentTop, // % offset to front or back
                           0 //zAlignmentBottom
                );
            WpfWedge armRight = armLeft.mirrorX();
            WpfTube armRightTube = armRight.makeTube(tuberes, false, true);
            WpfTube armLeftTube = armLeft.makeTube(tuberes, false, true);
            shoulderJointRight = armRight.topCenter;
            shoulderJointLeft = armLeft.topCenter;

            WpfWedge foreArmLeft = new WpfWedge(cube,
                           headWidthPercentage / 2, //topWidth,
                           headWidthPercentage / 4, //bottomWidth,
                           0.46, //topDepth,
                           0.26, //bottomDepth,
                           headHeightPercentage * 1.3, //height,
                           headHeightPercentage * 3, //offSet, // % offset from top
                           0.44, //xAlignmentTop, // % offset to left or right
                           0.43, //xAlignmentBottom,
                           0, //zAlignmentTop, // % offset to front or back
                           0 //zAlignmentBottom
                );
            WpfWedge foreArmRight = foreArmLeft.mirrorX();
            WpfTube foreArmRightTube = foreArmRight.makeTube(tuberes, true, true);
            WpfTube foreArmLeftTube = foreArmLeft.makeTube(tuberes, true, true);
            elbowJoint = foreArmRight.topCenter;

            WpfWedge handLeft = new WpfWedge(cube,
                           headWidthPercentage * 0.2, //topWidth,
                           headWidthPercentage * 0.16, //bottomWidth,
                           0.23, //topDepth,
                           0.21, //bottomDepth,
                           headHeightPercentage * 0.7, //height,
                           headHeightPercentage * 4.3, //offSet, // % offset from top
                           0.43, //xAlignmentTop, // % offset to left or right
                           0.38, //xAlignmentBottom,
                           0, //zAlignmentTop, // % offset to front or back
                           0 //zAlignmentBottom
                );
            handLeft.makeWedge();
            WpfWedge handRight = handLeft.mirrorX();
            handRight.makeWedge();



            modelGroup.Children.Add(headTube.CreateModel(color, combineVertices));
            modelGroup.Children.Add(neckTube.CreateModel(color, combineVertices));
            modelGroup.Children.Add(shoulderTube.CreateModel(color, combineVertices));



            modelGroup.Children.Add(torsoTube.CreateModel(color, combineVertices));



            legModelLeft = legLeftTube.CreateModel(color, combineVertices);
            legModelRight = legRightTube.CreateModel(color, combineVertices);
            modelGroup.Children.Add(legModelLeft);
            modelGroup.Children.Add(legModelRight);

            thighModelLeft = thighLeftTube.CreateModel(color, combineVertices);
            thighModelRight = thighRightTube.CreateModel(color, combineVertices);
            modelGroup.Children.Add(thighModelLeft);
            modelGroup.Children.Add(thighModelRight);

            footModelLeft = footLeftTube.CreateModel(color, combineVertices);
            footModelRight = footRightTube.CreateModel(color, combineVertices);
            modelGroup.Children.Add(footModelLeft);
            modelGroup.Children.Add(footModelRight);

            armLeftModel = armLeftTube.CreateModel(color, combineVertices);
            armRightModel = armRightTube.CreateModel(color, combineVertices);
            modelGroup.Children.Add(armLeftModel);
            modelGroup.Children.Add(armRightModel);

            foreArmLeftModel = foreArmLeftTube.CreateModel(color, combineVertices);
            foreArmRightModel = foreArmRightTube.CreateModel(color, combineVertices);
            modelGroup.Children.Add(foreArmRightModel);
            modelGroup.Children.Add(foreArmLeftModel);

            handLeftModel = handLeft.CreateModel(color, false);
            handRightModel = handRight.CreateModel(color, false);
            modelGroup.Children.Add(handLeftModel);
            modelGroup.Children.Add(handRightModel);

            if (hasBelt)
            {
                double beltPercent = 0.1;

                WpfWedge belt = new WpfWedge(cube,
                               headWidthPercentage * 1.96, //topWidth,
                               headWidthPercentage * 1.96, //bottomWidth,
                               0.68, //topDepth,
                               0.68, //bottomDepth,
                               beltPercent, //height,
                               (torsoOffset + torsoHeight) - beltPercent, //offSet, // % offset from top
                               0, //xAlignmentTop, // % offset to left or right
                               0, //xAlignmentBottom,
                               0, //zAlignmentTop, // % offset to front or back
                               0 //zAlignmentBottom
                    );
                WpfTube beltTube = belt.makeTube(tuberes, false, true);
                modelGroup.Children.Add(beltTube.CreateModel(color, combineVertices));
            }

            if (hasBoots)
            {
                double bootPercent = 0.15;

                WpfWedge bootLeft = new WpfWedge(cube,
                               headWidthPercentage * 0.7, //topWidth,
                               headWidthPercentage * 0.5, //bottomWidth,
                               0.57, //topDepth,
                               0.5, //bottomDepth,
                               bootPercent, //height,
                               footOffset - bootPercent, //offSet, // % offset from top
                               headWidthPercentage * 0.5, //xAlignmentTop, // % offset to left or right
                               headWidthPercentage * 0.4, //xAlignmentBottom,
                               0, //zAlignmentTop, // % offset to front or back
                               0 //zAlignmentBottom
                    );

                WpfWedge bootRight = bootLeft.mirrorX();
                WpfTube bootRightTube = bootRight.makeTube(tuberes, false, true);
                WpfTube bootLeftTube = bootLeft.makeTube(tuberes, false, true);
                bootModelLeft = bootLeftTube.CreateModel(color, combineVertices);
                bootModelRight = bootRightTube.CreateModel(color, combineVertices);
                modelGroup.Children.Add(bootModelRight);
                modelGroup.Children.Add(bootModelLeft);
            }


            if (hasGauntlets)
            {
                WpfWedge gauntletLeft = new WpfWedge(cube,
                               headWidthPercentage / 2, //topWidth,
                               headWidthPercentage / 2.5, //bottomWidth,
                               0.55, //topDepth,
                               0.32, //bottomDepth,
                               headHeightPercentage * 0.9, //height,
                               headHeightPercentage * 3.4, //offSet, // % offset from top
                               0.43, //xAlignmentTop, // % offset to left or right
                               0.43, //xAlignmentBottom,
                               0, //zAlignmentTop, // % offset to front or back
                               0 //zAlignmentBottom
                    );
                WpfWedge gauntletRight = gauntletLeft.mirrorX();
                WpfTube gauntletRightTube = gauntletRight.makeTube(tuberes, false, true);
                WpfTube gauntletLeftTube = gauntletLeft.makeTube(tuberes, false, true);

                gauntletLeftModel = gauntletLeftTube.CreateModel(color, combineVertices);
                gauntletRightModel = gauntletRightTube.CreateModel(color, combineVertices);

                modelGroup.Children.Add(gauntletRightModel);
                modelGroup.Children.Add(gauntletLeftModel);
            }


            return modelGroup;
        }


        

        private void workArms()
        {
            double seconds = 0.4;

            if (armLeftModel != null && armRightModel != null)
            {
                double elbowDegree = -16;
                double shoulderDegreeH = 14;

                Transform3DGroup leftHandGroup = new Transform3DGroup();
                Transform3DGroup rightHandGroup = new Transform3DGroup();

                Transform3DGroup leftShoulderGroup = new Transform3DGroup();
                Transform3DGroup rightShoulderGroup = new Transform3DGroup();


                armRightModel.Transform = rightShoulderGroup;
                armLeftModel.Transform = leftShoulderGroup;

                foreArmRightModel.Transform = rightHandGroup;
                foreArmLeftModel.Transform = leftHandGroup;

                handRightModel.Transform = rightHandGroup;
                handLeftModel.Transform = leftHandGroup;

                if (gauntletLeftModel != null && gauntletRightModel != null)
                {
                    gauntletLeftModel.Transform = leftHandGroup;
                    gauntletRightModel.Transform = rightHandGroup;
                }


                AxisAngleRotation3D axisElbowRotationLeft = new AxisAngleRotation3D(new Vector3D(1, 0, 0), elbowDegree);
                RotateTransform3D rotateElbowTransformLeft = new RotateTransform3D(axisElbowRotationLeft, elbowJoint);
                AxisAngleRotation3D axisShoulderRotationLeft = new AxisAngleRotation3D(new Vector3D(1, 0, 0), shoulderDegreeH);
                RotateTransform3D rotateShoulderTransformLeft = new RotateTransform3D(axisShoulderRotationLeft, shoulderJointLeft);

                AxisAngleRotation3D axisElbowRotationRight = new AxisAngleRotation3D(new Vector3D(1, 0, 0), elbowDegree);
                RotateTransform3D rotateElbowTransformRight = new RotateTransform3D(axisElbowRotationRight, elbowJoint);
                AxisAngleRotation3D axisShoulderRotationRight = new AxisAngleRotation3D(new Vector3D(1, 0, 0), shoulderDegreeH);
                RotateTransform3D rotateShoulderTransformRight = new RotateTransform3D(axisShoulderRotationRight, shoulderJointLeft);




                leftHandGroup.Children.Add(rotateElbowTransformLeft);
                rightHandGroup.Children.Add(rotateElbowTransformRight);
                leftHandGroup.Children.Add(rotateShoulderTransformLeft);
                rightHandGroup.Children.Add(rotateShoulderTransformRight);

                leftShoulderGroup.Children.Add(rotateShoulderTransformLeft);
                rightShoulderGroup.Children.Add(rotateShoulderTransformRight);





                DoubleAnimation shoulderAnimationRight = new DoubleAnimation(shoulderDegreeH, -shoulderDegreeH, durationTS(seconds));
                DoubleAnimation shoulderAnimationLeft = new DoubleAnimation(-shoulderDegreeH, shoulderDegreeH, durationTS(seconds));
                shoulderAnimationLeft.RepeatBehavior = RepeatBehavior.Forever;
                shoulderAnimationRight.RepeatBehavior = RepeatBehavior.Forever;

                DoubleAnimation elbowAnimationRight = new DoubleAnimation(elbowDegree, 0, durationTS(seconds));
                DoubleAnimation elbowAnimationLeft = new DoubleAnimation(0, elbowDegree, durationTS(seconds));
                elbowAnimationLeft.RepeatBehavior = RepeatBehavior.Forever;
                elbowAnimationRight.RepeatBehavior = RepeatBehavior.Forever;

                elbowAnimationLeft.AutoReverse = true;
                elbowAnimationRight.AutoReverse = true;
                shoulderAnimationLeft.AutoReverse = true;
                shoulderAnimationRight.AutoReverse = true;

                elbowAnimationLeft.BeginTime = durationTS(0.0);
                elbowAnimationRight.BeginTime = durationTS(0.0);
                shoulderAnimationLeft.BeginTime = durationTS(0.0);
                shoulderAnimationRight.BeginTime = durationTS(0.0);

                axisShoulderRotationLeft.BeginAnimation(AxisAngleRotation3D.AngleProperty, shoulderAnimationLeft);
                axisElbowRotationLeft.BeginAnimation(AxisAngleRotation3D.AngleProperty, elbowAnimationLeft);

                axisShoulderRotationRight.BeginAnimation(AxisAngleRotation3D.AngleProperty, shoulderAnimationRight);
                axisElbowRotationRight.BeginAnimation(AxisAngleRotation3D.AngleProperty, elbowAnimationRight);

            }
        }


        public void workLegs()
        {
            double stepSeconds = 0.4;

            if (legModelLeft != null && legModelRight != null)
            {
                double hipDegree = 10;
                double kneeDegree = -9;

                Transform3DGroup leftKneeTransformGroup = new Transform3DGroup();
                Transform3DGroup rightKneeTransformGroup = new Transform3DGroup();

                AxisAngleRotation3D axisHipRotationLeft = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0.0);
                RotateTransform3D rotateHipTransformLeft = new RotateTransform3D(axisHipRotationLeft, hipJoint);

                AxisAngleRotation3D axisHipRotationRight = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0.0);
                RotateTransform3D rotateHipTransformRight = new RotateTransform3D(axisHipRotationRight, hipJoint);

                AxisAngleRotation3D axisKneeRotationLeft = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0.0);
                RotateTransform3D rotateKneeTransformLeft = new RotateTransform3D(axisKneeRotationLeft, kneeJoint);

                AxisAngleRotation3D axisKneeRotationRight = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0.0);
                RotateTransform3D rotateKneeTransformRight = new RotateTransform3D(axisKneeRotationRight, kneeJoint);



                leftKneeTransformGroup.Children.Add(rotateKneeTransformLeft);
                rightKneeTransformGroup.Children.Add(rotateKneeTransformRight);

                leftKneeTransformGroup.Children.Add(rotateHipTransformLeft);
                rightKneeTransformGroup.Children.Add(rotateHipTransformRight);

                thighModelLeft.Transform = rotateHipTransformLeft;
                thighModelRight.Transform = rotateHipTransformRight;

                footModelRight.Transform = rightKneeTransformGroup;
                footModelLeft.Transform = leftKneeTransformGroup;
                legModelRight.Transform = rightKneeTransformGroup;
                legModelLeft.Transform = leftKneeTransformGroup;

                if (bootModelLeft != null && bootModelRight != null)
                {
                    bootModelRight.Transform = rightKneeTransformGroup;
                    bootModelLeft.Transform = leftKneeTransformGroup;
                }


                DoubleAnimation legAnimationRight = new DoubleAnimation(-hipDegree, hipDegree, durationTS(stepSeconds));
                DoubleAnimation legAnimationLeft = new DoubleAnimation(hipDegree, -hipDegree, durationTS(stepSeconds));
                legAnimationLeft.RepeatBehavior = RepeatBehavior.Forever;
                legAnimationRight.RepeatBehavior = RepeatBehavior.Forever;


                DoubleAnimation kneeAnimationRight = new DoubleAnimation(-kneeDegree, 0, durationTS(stepSeconds));
                DoubleAnimation kneeAnimationLeft = new DoubleAnimation(0, -kneeDegree, durationTS(stepSeconds));


                kneeAnimationLeft.RepeatBehavior = RepeatBehavior.Forever;
                kneeAnimationRight.RepeatBehavior = RepeatBehavior.Forever;

                kneeAnimationLeft.AutoReverse = true;
                kneeAnimationRight.AutoReverse = true;
                legAnimationLeft.AutoReverse = true;
                legAnimationRight.AutoReverse = true;

                kneeAnimationLeft.BeginTime = durationTS(0.0);
                kneeAnimationRight.BeginTime = durationTS(0.0);
                legAnimationLeft.BeginTime = durationTS(0.0);
                legAnimationRight.BeginTime = durationTS(0.0);

                axisHipRotationLeft.BeginAnimation(AxisAngleRotation3D.AngleProperty, legAnimationLeft);
                axisHipRotationRight.BeginAnimation(AxisAngleRotation3D.AngleProperty, legAnimationRight);

                axisKneeRotationLeft.BeginAnimation(AxisAngleRotation3D.AngleProperty, kneeAnimationLeft);
                axisKneeRotationRight.BeginAnimation(AxisAngleRotation3D.AngleProperty, kneeAnimationRight);
            }
        }


        private int durationM(double seconds)
        {
            int milliseconds = (int)(seconds * 1000);
            return milliseconds;
        }

        public TimeSpan durationTS(double seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, durationM(seconds));
            return ts;
        }


        public Point3D getCenter()
        {
            Point3D center = new Point3D(origin.X + (width / 2), origin.Y, origin.Z + (depth / 2));
            return center;
        }


        public void addMotionTransforms()
        {
            Vector3D vector = new Vector3D(0, 1, 0);

            AxisAngleRotation3D rotation = new AxisAngleRotation3D(vector, 0.0);

            rotateTransform = new RotateTransform3D(rotation, getCenter());

            addTransform(robotModelGroup, rotateTransform);

            translateTransform = new TranslateTransform3D(0, 0, 0);

            addTransform(robotModelGroup, translateTransform);
        }


        public void addTransform(Model3DGroup model, Transform3D transform)
        {
            Transform3DGroup group = new Transform3DGroup();

            if (model.Transform != null && model.Transform != Transform3D.Identity)
            {
                if (model.Transform is Transform3D)
                {
                    group.Children.Add(model.Transform);
                }
                else if (model.Transform is Transform3DGroup)
                {
                    Transform3DGroup g = (Transform3DGroup)(model.Transform);
                    foreach (Transform3D t in g.Children)
                    {
                        group.Children.Add(t);
                    }
                }
            }
            group.Children.Add(transform);
            model.Transform = group;
        }


    }
}
