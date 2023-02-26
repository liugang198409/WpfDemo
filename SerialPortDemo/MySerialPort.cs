using System;
using System.IO.Ports;
using System.Text;

namespace SerialPortDemo
{
    public class MySerialPort
    {
        private SerialPort SeialPort = null;
        public delegate bool ReceiveClientData(string data);
        public event ReceiveClientData ReceiveClientDataEvent;
        public event ReceiveClientData SendClientDataEvent;

        public bool OpenSerialPort(SerialPortConfiguration serialPortConfiguration)
        {
            try
            {
                if (SeialPort != null)
                {
                    return false;
                }

                SeialPort = new SerialPort
                {
                    PortName = serialPortConfiguration.PortName,
                    BaudRate = serialPortConfiguration.BaudRate,
                    DataBits = serialPortConfiguration.DataBits,
                    StopBits = serialPortConfiguration.StopBits,
                    Parity = serialPortConfiguration.Parity,
                };

                SeialPort.DataReceived += new SerialDataReceivedEventHandler(ReceivedSerialPortData);
                SeialPort.Open();
                ReceiveClientDataEvent(string.Format("#Sp000-{0}", serialPortConfiguration.PortName));
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool CloseSerialPort()
        {
            try
            {
                if (SeialPort != null && SeialPort.IsOpen)
                {
                    SeialPort.Close();
                    SeialPort = null;
                    ReceiveClientDataEvent("#Sp404-");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool SendSerialPortData(string data)
        {
            try
            {
                if (SeialPort != null && SeialPort.IsOpen)
                {
                    if (SendClientDataEvent("#HEXSend"))
                    {
                        byte[] decBytes = StringToConver(data);
                        SeialPort.Write(decBytes, 0, decBytes.Length);
                    }
                    else
                    {
                        SeialPort.Write(data.ToCharArray(), 0, data.Length);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private void ReceivedSerialPortData(object sender, SerialDataReceivedEventArgs args)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate ()
                {
                    if (ReceiveClientDataEvent("#HEXRece"))
                    {
                        int length = SeialPort.BytesToRead;
                        byte[] Readbuffer = new byte[length];
                        SeialPort.Read(Readbuffer, 0, length);
                        ReceiveClientDataEvent(ConverToString(Readbuffer));
                    }
                    else
                    {
                        string ReceivedData = SeialPort.ReadExisting();
                        ReceiveClientDataEvent(ReceivedData);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ReceiveClientDataEvent(string.Format("[Error]-{0}", ex.Message));
            }
        }

        private byte[] StringToConver(string stringData)
        {
            String[] SendArr = stringData.Split(' ');
            byte[] decBytes = new byte[SendArr.Length];
            for (int i = 0; i < SendArr.Length; i++)
                decBytes[i] = Convert.ToByte(SendArr[i], 16);
            return decBytes;
        }

        private string ConverToString(byte[] bytesData)
        {
            StringBuilder stb = new StringBuilder();
            for (int i = 0; i < bytesData.Length; i++)
            {
                if ((int)bytesData[i] > 15)
                {
                    stb.Append(Convert.ToString(bytesData[i], 16).ToUpper());
                }
                else
                {
                    stb.Append("0" + Convert.ToString(bytesData[i], 16).ToUpper());
                }
                if (i != bytesData.Length - 1)
                    stb.Append(" ");
            }
            return stb.ToString();
        }

    }
}
