using HelixToolkit.Wpf;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace _3DTransformDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Model3D model;
        public MainWindow()
        {
            InitializeComponent();
            model = LoadModel("IRB6700-MH3_245-300_IRC5_rev02_LINK01_CAD.stl");
            Display();
        }

        public Model3D LoadModel(string modelName)
        {
            ModelImporter import = new ModelImporter();
            string basePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\3D_Models\\";
            Model3D model = import.Load(basePath + modelName);
            return model;
        }

        public void TransformModel(double angle)
        {
            RotateTransform3D rotateTransform = new RotateTransform3D();
            AxisAngleRotation3D rotateAxis = new AxisAngleRotation3D(new Vector3D(0, 0, 1), angle);
            Rotation3DAnimation rotateAnimation = new Rotation3DAnimation(rotateAxis, TimeSpan.FromSeconds(5));
            rotateAnimation.DecelerationRatio = 0.8;

            Transform3DGroup transform3DGroup = new Transform3DGroup();
            transform3DGroup.Children.Add(rotateTransform);
            transform3DGroup.Children.Add(model.Transform);
            model.Transform = transform3DGroup;
            rotateTransform.BeginAnimation(RotateTransform3D.RotationProperty, rotateAnimation);
        }

        public void Display()
        {
            ModelVisual3D modelVisual3d = new ModelVisual3D();
            modelVisual3d.Content = model;
            viewPort3D.Children.Add(modelVisual3d);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TransformModel(180);
            Display();
        }
    }
}
