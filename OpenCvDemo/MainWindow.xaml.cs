using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Management;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace OpenCvDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private VideoCapture capCamera;
        private Thread cameraThread;
        private Mat matImage = new Mat();
        public List<string> CameraArray
        {
            get { return (List<string>)GetValue(CameraArrayProperty); }
            set { SetValue(CameraArrayProperty, value); }
        }

        public static readonly DependencyProperty CameraArrayProperty =
            DependencyProperty.Register("CameraArray", typeof(List<string>), typeof(MainWindow), new PropertyMetadata(null));
        public int CameraIndex
        {
            get { return (int)GetValue(CameraIndexProperty); }
            set { SetValue(CameraIndexProperty, value); }
        }

        public static readonly DependencyProperty CameraIndexProperty =
            DependencyProperty.Register("CameraIndex", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        public MainWindow()
        {
            InitializeComponent();
            Width = SystemParameters.WorkArea.Width / 1.5;
            Height = SystemParameters.WorkArea.Height / 1.5;
            this.Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCamera();
        }
        private void InitializeCamera()
        {
            CameraArray = GetAllConnectedCameras();
        }
        List<string> GetAllConnectedCameras()
        {
            var cameraNames = new List<string>();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
            {
                foreach (var device in searcher.Get())
                {
                    cameraNames.Add(device["Caption"].ToString());
                }
            }

            return cameraNames;
        }
        private void ComboBoxCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CameraArray.Count - 1 < CameraIndex)
                return;

            if (capCamera != null && cameraThread != null)
            {
                cameraThread.Abort();
                StopDispose();
            }

            capCamera = new VideoCapture(CameraIndex);
            capCamera.Fps = 30;
            CreateCamera();
        }
        void CreateCamera()
        {
            cameraThread = new Thread(PlayCamera);
            cameraThread.Start();
        }
        private void PlayCamera()
        {
            while (capCamera != null && !capCamera.IsDisposed)
            {
                capCamera.Read(matImage);
                if (matImage.Empty()) break;
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    //灰度
                    Mat gray = new Mat();
                    Cv2.CvtColor(matImage, gray, ColorConversionCodes.RGB2GRAY);
                    ////边缘化检测
                    //Mat canny = new Mat();
                    //Cv2.Canny(gray, canny, 128, 200);
                    //二值化
                    //Mat threshold = new Mat();
                    //Cv2.Threshold(gray, threshold, 50, 255, ThresholdTypes.Binary);
                    //图像旋转
                    Mat flip = new Mat();
                    Cv2.Flip(matImage, flip, FlipMode.X);

                    var converted = Convert(BitmapConverter.ToBitmap(flip));
                    imgViewport.Source = converted;
                }));
            }
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
        void StopDispose()
        {
            if (capCamera != null && capCamera.IsOpened())
            {
                capCamera.Dispose();
                capCamera = null;
            }
        }
    }
}
