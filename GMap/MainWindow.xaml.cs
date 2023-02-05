﻿using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
           
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
