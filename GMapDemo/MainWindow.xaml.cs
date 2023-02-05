using GMap.NET;
using System;
using System.Windows;
using System.Windows.Input;

namespace GMapDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Rect rc = SystemParameters.WorkArea; //获取工作区大小
            this.Left = 0; //设置位置
            this.Top = 0;
            this.Width = rc.Width;
            this.Height = rc.Height;
            this.Map_Loaded();//加载地图
        }

        private void Map_Loaded()
        {
            try
            {
                System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("ditu.google.cn");
            }
            catch
            {
                MainMap.Manager.Mode = AccessMode.CacheOnly;
                System.Windows.MessageBox.Show("没有可用的internet连接，正在进入缓存模式!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            MainMap.CacheLocation = Environment.CurrentDirectory + "\\GMapCache\\"; //缓存位置
            MainMap.MapProvider = AMapProvider.Instance; //加载高德地图
            MainMap.MinZoom = 2;  //最小缩放
            MainMap.MaxZoom = 17; //最大缩放
            MainMap.Zoom = 8;     //当前缩放
            MainMap.ShowCenter = false; //不显示中心十字点
            MainMap.DragButton = MouseButton.Left; //右键拖拽地图
            MainMap.Position = new PointLatLng(39.909149, 116.397486); //地图中心位置：北京
            //MainMap.MouseLeftButtonDown += new MouseButtonEventHandler(mapControl_MouseLeftButtonDown);
        }

    }
}
