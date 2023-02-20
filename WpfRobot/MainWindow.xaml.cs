using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace WpfRobot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Viewport3D viewport;

        bool overheadCamera = false;

        WpfRobotBody robot;

        Point3D lookat = new Point3D(0, 0, 0);

        Storyboard storyBoard = new Storyboard();

        public DirectionalLight positionLight(Point3D position)
        {
            DirectionalLight directionalLight = new DirectionalLight();
            directionalLight.Color = Colors.Gray;
            directionalLight.Direction = new Point3D(0, 0, 0) - position;
            return directionalLight;
        }

        public DirectionalLight leftLight()
        {
            return positionLight(new Point3D(-WpfScene.sceneSize, WpfScene.sceneSize / 2, 0.0));
        }


        public PerspectiveCamera camera()
        {
            PerspectiveCamera perspectiveCamera = new PerspectiveCamera();

            if (overheadCamera)
            {
                perspectiveCamera.Position = new Point3D(0, WpfScene.sceneSize * 2, WpfScene.sceneSize / 50);
            }
            else
            {
                perspectiveCamera.Position = new Point3D(-WpfScene.sceneSize, WpfScene.sceneSize / 2, WpfScene.sceneSize);
            }

            perspectiveCamera.LookDirection = new Vector3D(lookat.X - perspectiveCamera.Position.X,
                                                           lookat.Y - perspectiveCamera.Position.Y,
                                                           lookat.Z - perspectiveCamera.Position.Z);

            perspectiveCamera.FieldOfView = 60;

            return perspectiveCamera;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Viewport3D_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Viewport3D)
            {
                viewport = (Viewport3D)sender;

                robot = new WpfRobotBody();

                double floorThickness = WpfScene.sceneSize / 100;
                GeometryModel3D floorModel = WpfCube.CreateCubeModel(
                    new Point3D(-WpfScene.sceneSize / 2,
                                -floorThickness,
                                -WpfScene.sceneSize / 2),
                    WpfScene.sceneSize, floorThickness, WpfScene.sceneSize, Colors.Tan);

                Model3DGroup groupScene = new Model3DGroup();

                groupScene.Children.Add(floorModel);

                groupScene.Children.Add(robot.getModelGroup());

                groupScene.Children.Add(leftLight());
                groupScene.Children.Add(new AmbientLight(Colors.Gray));

                viewport.Camera = camera();

                ModelVisual3D visual = new ModelVisual3D();

                visual.Content = groupScene;

                viewport.Children.Add(visual);

                storyboardRobot();
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

        private void storyboardRobot()
        {
            double turnDuration = 0.7;
            double totalDuration = 0.0;
            double walkDuration = 3.4;

            NameScope.SetNameScope(this, new NameScope());

            storyBoard = new Storyboard();


            Vector3D vector = new Vector3D(0, 1, 0);

            AxisAngleRotation3D rotation = new AxisAngleRotation3D(vector, 0.0);
            robot.getRotateTransform().Rotation = rotation;

            DoubleAnimation doubleAnimationTurn1 = new DoubleAnimation(0.0, 90.0, durationTS(turnDuration));
            DoubleAnimation doubleAnimationTurn2 = new DoubleAnimation(90.0, 180.0, durationTS(turnDuration));
            DoubleAnimation doubleAnimationTurn3 = new DoubleAnimation(180.0, 270.0, durationTS(turnDuration));
            DoubleAnimation doubleAnimationTurn4 = new DoubleAnimation(270.0, 360.0, durationTS(turnDuration));

            RegisterName("TurnRotation", rotation);

            RegisterName("MoveTransform", robot.getTranslateTransform());


            storyBoard.Children.Add(doubleAnimationTurn1);
            storyBoard.Children.Add(doubleAnimationTurn2);
            storyBoard.Children.Add(doubleAnimationTurn3);
            storyBoard.Children.Add(doubleAnimationTurn4);


            Storyboard.SetTargetName(doubleAnimationTurn1, "TurnRotation");
            Storyboard.SetTargetProperty(doubleAnimationTurn1, new PropertyPath(AxisAngleRotation3D.AngleProperty));
            Storyboard.SetTargetName(doubleAnimationTurn2, "TurnRotation");
            Storyboard.SetTargetProperty(doubleAnimationTurn2, new PropertyPath(AxisAngleRotation3D.AngleProperty));
            Storyboard.SetTargetName(doubleAnimationTurn3, "TurnRotation");
            Storyboard.SetTargetProperty(doubleAnimationTurn3, new PropertyPath(AxisAngleRotation3D.AngleProperty));
            Storyboard.SetTargetName(doubleAnimationTurn4, "TurnRotation");
            Storyboard.SetTargetProperty(doubleAnimationTurn4, new PropertyPath(AxisAngleRotation3D.AngleProperty));


            double offset = WpfScene.sceneSize * 0.45;



            DoubleAnimation doubleAnimationX1 = new DoubleAnimation(-offset, -offset, durationTS(walkDuration));
            DoubleAnimation doubleAnimationZ1 = new DoubleAnimation(-offset, offset, durationTS(walkDuration));
            Storyboard.SetTargetName(doubleAnimationX1, "MoveTransform");
            Storyboard.SetTargetProperty(doubleAnimationX1, new PropertyPath(TranslateTransform3D.OffsetXProperty));
            Storyboard.SetTargetName(doubleAnimationZ1, "MoveTransform");
            Storyboard.SetTargetProperty(doubleAnimationZ1, new PropertyPath(TranslateTransform3D.OffsetZProperty));
            storyBoard.Children.Add(doubleAnimationX1);
            storyBoard.Children.Add(doubleAnimationZ1);


            DoubleAnimation doubleAnimationX2 = new DoubleAnimation(-offset, offset, durationTS(walkDuration));
            DoubleAnimation doubleAnimationZ2 = new DoubleAnimation(offset, offset, durationTS(walkDuration));
            Storyboard.SetTargetName(doubleAnimationX2, "MoveTransform");
            Storyboard.SetTargetProperty(doubleAnimationX2, new PropertyPath(TranslateTransform3D.OffsetXProperty));
            Storyboard.SetTargetName(doubleAnimationZ2, "MoveTransform");
            Storyboard.SetTargetProperty(doubleAnimationZ2, new PropertyPath(TranslateTransform3D.OffsetZProperty));
            storyBoard.Children.Add(doubleAnimationX2);
            storyBoard.Children.Add(doubleAnimationZ2);


            DoubleAnimation doubleAnimationX3 = new DoubleAnimation(offset, offset, durationTS(walkDuration));
            DoubleAnimation doubleAnimationZ3 = new DoubleAnimation(offset, -offset, durationTS(walkDuration));
            Storyboard.SetTargetName(doubleAnimationX3, "MoveTransform");
            Storyboard.SetTargetProperty(doubleAnimationX3, new PropertyPath(TranslateTransform3D.OffsetXProperty));
            Storyboard.SetTargetName(doubleAnimationZ3, "MoveTransform");
            Storyboard.SetTargetProperty(doubleAnimationZ3, new PropertyPath(TranslateTransform3D.OffsetZProperty));
            storyBoard.Children.Add(doubleAnimationX3);
            storyBoard.Children.Add(doubleAnimationZ3);


            DoubleAnimation doubleAnimationX4 = new DoubleAnimation(offset, -offset, durationTS(walkDuration));
            DoubleAnimation doubleAnimationZ4 = new DoubleAnimation(-offset, -offset, durationTS(walkDuration));
            Storyboard.SetTargetName(doubleAnimationX4, "MoveTransform");
            Storyboard.SetTargetProperty(doubleAnimationX4, new PropertyPath(TranslateTransform3D.OffsetXProperty));
            Storyboard.SetTargetName(doubleAnimationZ4, "MoveTransform");
            Storyboard.SetTargetProperty(doubleAnimationZ4, new PropertyPath(TranslateTransform3D.OffsetZProperty));
            storyBoard.Children.Add(doubleAnimationX4);
            storyBoard.Children.Add(doubleAnimationZ4);


            doubleAnimationX1.BeginTime = durationTS(totalDuration);
            doubleAnimationZ1.BeginTime = durationTS(totalDuration);
            totalDuration += walkDuration;

            doubleAnimationTurn1.BeginTime = durationTS(totalDuration);
            totalDuration += turnDuration;

            doubleAnimationX2.BeginTime = durationTS(totalDuration);
            doubleAnimationZ2.BeginTime = durationTS(totalDuration);
            totalDuration += walkDuration;

            doubleAnimationTurn2.BeginTime = durationTS(totalDuration);
            totalDuration += turnDuration;

            doubleAnimationX3.BeginTime = durationTS(totalDuration);
            doubleAnimationZ3.BeginTime = durationTS(totalDuration);
            totalDuration += walkDuration;

            doubleAnimationTurn3.BeginTime = durationTS(totalDuration);
            totalDuration += turnDuration;

            doubleAnimationX4.BeginTime = durationTS(totalDuration);
            doubleAnimationZ4.BeginTime = durationTS(totalDuration);
            totalDuration += walkDuration;

            doubleAnimationTurn4.BeginTime = durationTS(totalDuration);
            totalDuration += turnDuration;



            storyBoard.RepeatBehavior = RepeatBehavior.Forever;

            storyBoard.Begin(this);
        }







        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is MainWindow)
            {
                Rect r = SystemParameters.WorkArea;

                MainWindow window = (MainWindow)sender;

                window.Width = r.Width;
                window.Height = r.Height;
                window.Left = 0;
                window.Top = 0;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            overheadCamera = !overheadCamera;
            viewport.Camera = camera();
        }






    }
}


