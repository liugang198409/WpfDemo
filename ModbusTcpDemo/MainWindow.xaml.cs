using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace ModbusTcpDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        TcpClient tcpClient = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectAndStartRetriveData(object sender, RoutedEventArgs e)
        {
            var port = TextPort.Text;
            var ipAddress = TextIpAddress.Text;
            var readTotalSize = TextTotalSize.Text;
            var slaveAddress = TextSlaveAddress.Text;
            var startReadAddress = TextStartAddress.Text;

            //1. 读取IP地址并验证是否是合法的
            if (!isValidIpAddress(ipAddress))
            {
                MessageBox.Show("不是合法的IP地址");
                return;
            }
            //2. 读取端口号并验证是否是合法的
            if (!isValidPort(port))
            {
                MessageBox.Show("不是合法的端口");
            }

            //3. 禁用“连接并启动”按钮
            BtnConnectAndStart.IsEnabled = false;

            //4. 建立TCP连接
            if (tcpClient == null)
            {
                tcpClient = new TcpClient();
            }
            tcpClient.Connect(ipAddress, int.Parse(port));

            //5. 建立Modbus连接
            Modbus.Device.ModbusIpMaster master = Modbus.Device.ModbusIpMaster.CreateIp(tcpClient);

            //6. 启动线程去循环获取数据
            Task.Run(()=> {
                while (true)
                {
                    ushort[] ushortArray = master.ReadHoldingRegisters((byte)int.Parse(slaveAddress), (byte)int.Parse(startReadAddress), (byte)int.Parse(readTotalSize));
                    //注意这里WPF线程更新UI需要用以下的方法 不然程序运行会出问题
                    Application.Current.Dispatcher.Invoke((Action)delegate ()
                    {
                        TextReceiveData.Text = ushortArray[0].ToString();
                    });
                }
            });
        }

        private bool isValidIpAddress(string ipAddress)
        {
            //TODO
            return true;
        }

        private bool isValidPort(string port)
        {
            //TODO
            return true;
        }

    }
}
