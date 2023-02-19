using System.Windows;
using System.Windows.Media.Media3D;

namespace _3DCubeTransformDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // WindowLoad();
        }

        private void rotateX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Transform3DGroup transform3DGroup = new Transform3DGroup();
            RotateTransform3D rotateTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), rotateX.Value));
            transform3DGroup.Children.Add(rotateTransform);
            viewPort3D.Transform = transform3DGroup;
        }

        private void rotateY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Transform3DGroup transform3DGroup = new Transform3DGroup();
            RotateTransform3D rotateTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), rotateY.Value));
            transform3DGroup.Children.Add(rotateTransform);
            viewPort3D.Transform = transform3DGroup;
        }

        private void rotateZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Transform3DGroup transform3DGroup = new Transform3DGroup();
            RotateTransform3D rotateTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), rotateZ.Value));
            transform3DGroup.Children.Add(rotateTransform);
            viewPort3D.Transform = transform3DGroup;
        }
    }
}
