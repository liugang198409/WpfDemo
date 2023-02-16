using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace TCPClientDemo
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket _socket;
        public MainWindow()
        {
            InitializeComponent();
            btnSendMsg.Click += BtnSendMsg_Click;//注册事件
            btnStartServer.Click += BtnConnect_Click;
            Closing += ClientWindows_Closing;
        }

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        private void ClientWindows_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServerExit(null, _socket);//向服务端说我下线了。
        }

        /// <summary
        /// 连接按钮事件
        /// </summary>
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            //1、创建Socket对象
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket = socket;
            //2、连接服务器,绑定IP 与 端口
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(txtIp.Text), int.Parse(txtPort.Text));
            try
            {
                socket.Connect(iPEndPoint);
                MessageBox.Show("连接成功！", "提示");
            }
            catch (Exception)
            {
                MessageBox.Show("连接失败，请重新连接!", "提示");
                return;
            }
            //3、接收消息
            ThreadPool.QueueUserWorkItem(new WaitCallback(ReceiveServerMsg), socket);
        }

        /// <summary>
        /// 不断接收客户端信息子线程方法
        /// </summary>
        /// <param name="obj">参数Socke对象</param>
        private void ReceiveServerMsg(object obj)
        {
            var proxSocket = obj as Socket;
            //创建缓存内存，存储接收的信息   ,不能放到while中，这块内存可以循环利用
            byte[] data = new byte[1020 * 1024];
            while (true)
            {
                int len;
                try
                {
                    //接收消息,返回字节长度
                    len = proxSocket.Receive(data, 0, data.Length, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    //7、关闭Socket
                    //异常退出
                    try
                    {
                        ServerExit(string.Format("服务端：{0}非正常退出", proxSocket.RemoteEndPoint.ToString()), proxSocket);
                    }
                    catch (Exception)
                    {
                    }

                    return;//让方法结束，终结当前客户端数据的异步线程，方法退出，即线程结束

                }

                if (len <= 0)//判断接收的字节数
                {
                    //7、关闭Socket
                    //小于0表示正常退出
                    try
                    {
                        ServerExit(string.Format("服务端：{0}正常退出", proxSocket.RemoteEndPoint.ToString()), proxSocket);
                    }
                    catch (Exception)
                    {

                    }

                    return;//让方法结束，终结当前客户端数据的异步线程，方法退出，即线程结束

                }

                //将消息显示到TxtLog
                string msgStr = Encoding.Default.GetString(data, 0, len);

                //拼接字符串
                AppendTxtLogText(string.Format("接收到服务端：{0}的消息：{1}", proxSocket.RemoteEndPoint.ToString(), msgStr));
            }

        }

        /// <summary>
        /// 客户端退出调用
        /// </summary>
        /// <param name="msg"></param>
        private void ServerExit(string msg, Socket proxSocket)
        {
            AppendTxtLogText(msg);
            try
            {
                if (proxSocket.Connected)//如果是连接状态
                {
                    proxSocket.Shutdown(SocketShutdown.Both);//关闭连接
                    proxSocket.Close(100);//100秒超时间
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 发送信息按钮事件
        /// </summary>
        private void BtnSendMsg_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = Encoding.Default.GetBytes(this.txtMsg.Text);
            //6、发送消息
            _socket.Send(data, 0, data.Length, SocketFlags.None); //指定套接字的发送行为
            this.txtMsg.Text = null;
        }

        /// <summary>
        /// 向文本框中追加信息
        /// </summary>
        /// <param name="str"></param>
        private void AppendTxtLogText(string str)
        {
            if (!(txtLog.Dispatcher.CheckAccess()))//判断跨线程访问
            {
                //异步方法
                this.Dispatcher.BeginInvoke(new Action<string>(s =>
                {
                    this.txtLog.Text = string.Format("{0}\r\n{1}", s, txtLog.Text);
                }), str);
            }
            else
            {
                this.txtLog.Text = string.Format("{0}\r\n{1}", str, txtLog.Text);
            }
        }
    }

}