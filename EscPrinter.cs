
// EscPrinter by Lemoine Yann
// Visit https://github.com/lemoine-yann/SimpleEscPos.Net

using System.Data;
using System.Formats.Asn1;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

    /// <summary>
    /// Underline mode
    /// </summary>
    public enum UnderlineMode
    {
        Off,
        OneDot,
        TwoDot
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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

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
                _buffer.Write(new byte[] {27, 64, 27, 83}); // ESC Reinit [ESC,@] ESC Standard mode [ESC,S]
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

            ReinitializeBuffer(false);
        }

        /// <summary>
        /// Print data in buffer
        /// </summary>
        public void Print()
        {
            if (PrinterMode != EscPrinterMode.BufferMode)
                throw new System.Exception("Printer is not BufferMode");

            FlushBuffer();
        }

        /// <summary>
        /// Print custom bytecodes
        /// </summary>
        /// <param name="data"></param>
        public void Print(byte[] data)
        {
            _buffer.Write(data);

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Print byte, number times
        /// </summary>
        /// <param name="data"></param>
        /// <param name="number"></param>
        public void Print(byte data, int number = 1)
        {
            if (number < 1)
                throw new System.Exception("Number must be greater than 0");

            for (int i = 0; i < number; i++)
                _buffer.WriteByte(data);

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Print string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="addCarrierReturn"></param>
        /// <param name="pageCode">printer page code</param>
        /// <param name="encoding">data page code</param>
        public void Print(string data, bool addCarrierReturn = true, byte pageCode = 32, int encoding = 1252)
        {
            data = data.Replace("\r", string.Empty).Replace("\n", string.Empty);

            if (addCarrierReturn)
                data = string.Concat(data, "\n");

            _buffer.Write(new byte[]
            {
                27, 29, 116, pageCode
            }); // Set Code printer codepage, by default 32 mean 1252 [ESC,GS,t,32] , see ESC/POS documentation for details
            _buffer.Write(Encoding.GetEncoding(encoding).GetBytes(data));

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Feed paper for n lines
        /// </summary>
        /// <param name="lines"></param>
        public void PaperFeed(byte lines)
        {
            _buffer.Write(new byte[] {27, 100, lines}); // Feed lines [ESC,d,lines]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Cut paper
        /// </summary>
        public void Cut()
        {
            _buffer.Write(new byte[] {29, 86, 0}); // Paper Cut [GS,V,NULL]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Set character size (0 to +7 vertical/horizontal)
        /// </summary>
        /// <param name="horizontalSize"></param>
        /// <param name="verticalSize"></param>
        public void SetCharacterSizeMagnification(byte horizontalSize, byte verticalSize)
        {
            if (horizontalSize > 7 || verticalSize > 7)
                throw new System.Exception("Horizontal and vertical size must be between 0 and 7");

            _buffer.Write(new byte[]
            {
                29, 33, (byte) ((horizontalSize << 4) | verticalSize)
            }); // Select character size [GS,!,horizontalSize/verticalSize]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Set underline mode (none/onedot/twodots)
        /// </summary>
        /// <param name="underlineMode"></param>
        public void SetUnderlineMode(UnderlineMode underlineMode)
        {
            _buffer.Write(new byte[] {27, 45, (byte) underlineMode}); // Select underline mode [ESC,-,underlineMode]

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