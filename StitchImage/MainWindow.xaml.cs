using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace StitchImage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Width = SystemParameters.WorkArea.Width / 1.5;
            Height = SystemParameters.WorkArea.Height / 1.5;
            this.Loaded += Stitch_Image;
        }

        private void Stitch_Image(object sender, RoutedEventArgs e)
        {
            //1. 加载两张待拼接的图片
            string imageBasePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string imageFullPath_1 = imageBasePath + "\\Images\\image1.jpg";
            string imageFullPath_2 = imageBasePath + "\\Images\\image2.jpg";
            Mat image1 = new Mat(imageFullPath_1, ImreadModes.Color);
            Mat image2 = new Mat(imageFullPath_2, ImreadModes.Color);
            //2. 完成图片拼接
            Mat[] images = new Mat[] { image1, image2 };
            Stitcher stitcher = Stitcher.Create(Stitcher.Mode.Scans);
            Mat pano = new Mat();
            stitcher.Stitch(images, pano);
            //3. 拼接后的图片展示
            var converted = Convert(BitmapConverter.ToBitmap(pano));
            imgViewport.Source = converted;
        }

        BitmapImage Convert(Bitmap src)
        {
            Bitmap ImageOriginalBase = new Bitmap(src);
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                ImageOriginalBase.Save(ms, ImageFormat.Png);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }
    }
}
