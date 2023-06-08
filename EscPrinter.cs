
// EscPrinter by Lemoine Yann
// Visit https://github.com/lemoine-yann/SimpleEscPos.Net

using System.Formats.Asn1;
using System.Net;
using System.Net.Sockets;

namespace SimpleEscPos.Net
{
    /// <summary>
    /// DirectMode : The printer prints the data in standard mode. All instructions are processed and printed immediately.
    /// BufferMode : The printer prints the data in buffer mode. All instructions are stored in the buffer and printed when the buffer is flushed with Print() method
    /// </summary>
    public enum EscPrinterMode
    {
        DirectMode,
        BufferMode
    }

    public class EscPrinter : IEscPrinter
    {
        /// <summary>
        /// Printer mode, see enum for details
        /// </summary>
        public EscPrinterMode PrinterMode { get; }

        /// <summary>
        /// Printer IP
        /// </summary>
        public string Ip { get; }

        /// <summary>
        /// Printer Port
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Printer buffer
        /// </summary>
        private MemoryStream _buffer = new MemoryStream();

        /// <summary>
        /// Initialize a new instance
        /// </summary>
        /// <param name="ip">Printer IP</param>
        /// <param name="port">Printer Port</param>
        /// <param name="printerMode">Printer Mode</param>
        public EscPrinter(string ip, int port, EscPrinterMode printerMode)
        {
            Ip = ip;
            Port = port;
            PrinterMode = printerMode;

            ReinitializeBuffer(true);
        }

        private void ReinitializeBuffer(bool firstInit)
        {
            _buffer.Close();
            _buffer.Dispose();
            _buffer = new MemoryStream();
            if (firstInit)
                _buffer.Write(new byte[] { 27, 64, 27, 83 }); // ESC Reinit [ESC,@] ESC Standard mode [ESC,S]
        }

        /// <summary>
        /// Write buffer data to printer
        /// </summary>
        private void FlushBuffer()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(5);

            using Socket socket = new Socket(SocketType.Stream, ProtocolType.IP)
            {
                SendTimeout = timeout.Milliseconds,
                ReceiveTimeout = timeout.Milliseconds
            };

            socket.Connect(Ip, Port);
            socket.Send(_buffer.ToArray());
        }

        /// <summary>
        /// Print data in buffer
        /// </summary>
        public void Print()
        {
            if (PrinterMode != EscPrinterMode.BufferMode)
                throw new System.Exception("Printer mode is not BufferMode");

            FlushBuffer();
        }

        /// <summary>
        /// Feed paper for n lines
        /// </summary>
        /// <param name="lines"></param>
        public void PaperFeed(byte lines)
        {
            _buffer.Write(new byte[] { 27, 100, lines }); // ESC Feed lines [ESC,d,lines]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        public void Cut()
        {
            _buffer.Write(new byte[] { 29, 86, 48 }); // ESC Paper Cut [GS,V,0]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Dispose instance
        /// </summary>
        public void Dispose()
        {
            _buffer.Close();
            _buffer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}