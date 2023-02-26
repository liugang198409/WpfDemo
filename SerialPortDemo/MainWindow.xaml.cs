using System;
using System.Linq;
using System.Windows;

namespace SerialPortDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MySerialPort mySerialPort = new MySerialPort();
        private SerialPortConfiguration serialPortConfiguration;
        public MainWindow()
        {
            InitializeComponent();
            InitializeSerialPort();
        }

        private void InitializeSerialPort()
        {
            serialPortConfiguration = new SerialPortConfiguration();
            mySerialPort.ReceiveClientDataEvent += ReceDataClick;
            mySerialPort.SendClientDataEvent += IsHEXClick;
        }

        private bool ReceSerialPort(string strIn)
        {
            var strCompare = strIn.Split('-');
            if (strCompare.Count() > 1)
            {
                if (strCompare[0] == "#Sp000")
                {
                    T_Data.Text = serialPortConfiguration.PortName + "已连接";
                    return false;
                }
                if (strCompare[0] == "#Sp404")
                {
                    T_Data.Text = serialPortConfiguration.PortName + "已断开";
                    return false;
                }
            }
            return true;
        }

        private bool IsHEXClick(string strIn)
        {
            if (strIn == "#HEXSend")
                return (bool)CB_SendType.IsChecked;
            return true;
        }

        private bool ReceDataClick(string strIn)
        {
            if (strIn == "#HEXRece")
                return (bool)CB_ReceType.IsChecked;

            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                if (!ReceSerialPort(strIn))
                    return;
                if (false)
                    TxRece.Text += strIn;
                else
                    TxRece.Text += strIn + "\r\n";
            });
            return true;
        }

        private void BtClearTxRece_Click(object sender, RoutedEventArgs e) => TxRece.Text = string.Empty;

        private void BtTxSend_Click(object sender, RoutedEventArgs e) => mySerialPort.SendSerialPortData(TxSend.Text);

        private void BtConnect_Click(object sender, RoutedEventArgs e) => mySerialPort.OpenSerialPort(serialPortConfiguration);

        private void BtBreak_Click(object sender, RoutedEventArgs e) => mySerialPort.CloseSerialPort();

    }
}
