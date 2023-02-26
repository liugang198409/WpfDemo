using System.IO.Ports;

namespace SerialPortDemo
{
    public class SerialPortConfiguration
    {

        public string PortName { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;

        public int DataBits { get; set; } = 8;

        public StopBits StopBits { get; set; } = StopBits.One;

        public Parity Parity { get; set; } = Parity.None;

    }
}
