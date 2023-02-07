using HelixToolkit.Wpf;
using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace _3DDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //模型加载
            string modelPath1 = AppDomain.CurrentDomain.BaseDirectory + "A.stl"; //模型地址

            ModelImporter import = new ModelImporter();
            var initGroup1 = import.Load(modelPath1);

            string modelPath2 = AppDomain.CurrentDomain.BaseDirectory + "B.stl";

            var initGroup2 = import.Load(modelPath2);
            //材质
            //GeometryModel3D geometryModel3D = initGroup.Children[0] as GeometryModel3D;
            //DiffuseMaterial diffMat = new DiffuseMaterial(new SolidColorBrush(Colors.White));
            //geometryModel3D.Material = diffMat;
            //ViewPort3D进行显示
            ModelVisual3D modelVisual3d1 = new ModelVisual3D();
            modelVisual3d1.Content = initGroup1;
            viewPort3D.Children.Add(modelVisual3d1);
            ModelVisual3D modelVisual3d2 = new ModelVisual3D();
            modelVisual3d2.Content = initGroup2;
            viewPort3D.Children.Add(modelVisual3d2);

        }
    }
}
