
// EscPrinter by Lemoine Yann
// Visit https://github.com/lemoine-yann/SimpleEscPos.Net

using System.Collections;
using System.Data;
using System.Drawing;
using System.Formats.Asn1;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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

    public enum FontMode
    {
        FontA,
        FontB
    }

    /// <summary>
    /// Barcode type
    /// </summary>
    public enum BarcodeType
    {
        UpcA = 65,
        UpcE = 66,
        Jan13Ean13 = 67,
        Jan8Ean8 = 68,
        Code39 = 69,
        Itf = 70,
        CodebarNw7 = 71,
        Code93 = 72,
        Code128 = 73,
        Gs1128 = 74,
        Gs1DatabarOmnidirectional = 75,
        Gs1DatabarTruncated = 76,
        Gs1DatabarLimited = 77,
        Gs1DatabarEexpanded = 78,
    }

    /// <summary>
    /// Barcode 2D type
    /// </summary>
    public enum Barcode2DType
    {
        Pdf417 = 0,
        QrcodeModel1 = 49,
        QrcodeModel2 = 50,
        QrcodeMicro = 51,
    }

    /// <summary>
    /// Barcode 2D size
    /// </summary>
    public enum Barcode2DSize
    {
        Tiny = 2,
        Small = 3,
        Normal = 4,
        Large = 5,
        Extra = 6,
    }

    /// <summary>
    /// Barcode 2D correction level
    /// </summary>
    public enum Barcode2DCorrectionLevel
    {
        Percent7 = 48,
        Percent15 = 49,
        Percent25 = 50,
        Percent30 = 51,
    }

    /// <summary>
    /// Barcode code
    /// </summary>
    public enum BarcodeCode
    {
        CodeA = 0x41,
        CodeB = 0x42,
        CodeC = 0x43,
    }

    /// <summary>
    /// Barcode characters position
    /// </summary>
    public enum BarcodeTextPosition
    {
        None = 0,
        Above = 1,
        Below = 2,
        Both = 3
    }

    /// <summary>
    /// Text/objects align mode
    /// </summary>
    public enum AlignMode
    {
        Left,
        Center,
        Right
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
        /// Reset printer to default values
        /// </summary>
        public void ReinitializePrinter()
        {
            _buffer.Write(new byte[] { 27, 64, 27, 83 }); // ESC Reinit [ESC,@] ESC Standard mode [ESC,S]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Clear buffer
        /// </summary>
        public void ClearBuffer()
        {
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
        /// Set bold mode
        /// </summary>
        /// <param name="bold"></param>
        public void SetBold(bool bold)
        {
            _buffer.Write(new byte[] {27, 69, bold ? (byte) 1 : (byte) 0}); // Select bold mode [ESC,E,boldmode]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Set inverted mode black/white
        /// </summary>
        /// <param name="inverted"></param>
        public void SetInverted(bool inverted)
        {
            _buffer.Write(new byte[]
                {29, 66, inverted ? (byte) 1 : (byte) 0}); // Select inverted mode [GS,B,invertedmode]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Select font A/B
        /// </summary>
        /// <param name="fontMode"></param>
        public void SetFont(FontMode fontMode)
        {
            _buffer.Write(new byte[] {27, 77, (byte) fontMode}); // Select font [ESC,M,fontmode]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Set clockwise rotation mode
        /// </summary>
        /// <param name="rotate"></param>
        public void SetClockwiseRotation(bool rotate)
        {
            _buffer.Write(new byte[]
                {27, 86, rotate ? (byte) 1 : (byte) 0}); // Select clockwise rotation mode [ESC,V,rotate]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Align text
        /// </summary>
        /// <param name="align"></param>
        public void SetAlign(AlignMode align)
        {
            _buffer.Write(new byte[] {27, 97, (byte) align}); // Select align [ESC,a,align]

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Print barcode
        /// </summary>
        /// <param name="barcodeType">barcode type</param>
        /// <param name="data">barcode data</param>
        /// <param name="height">height</param>
        /// <param name="width">width</param>
        /// <param name="position">position for characters</param>
        /// <param name="code">code for code128 A/B/C</param>
        /// <param name="fontMode">font mode</param>
        /// <exception cref="System.Exception"></exception>
        public void PrintBarcode(BarcodeType barcodeType, string data, byte height = 162, byte width = 3,
            BarcodeTextPosition position = BarcodeTextPosition.Below, BarcodeCode code = BarcodeCode.CodeB,
            FontMode fontMode = FontMode.FontA)
        {
            if (width < 1 || width > 6)
                throw new System.Exception("Width must be between 1 and 6");

            if (barcodeType == BarcodeType.Code128) // Code128 must have a start code
            {
                if (code == BarcodeCode.CodeC) // Code C must have an even number of digits
                {
                    byte[] bytesData = Encoding.ASCII.GetBytes(data);
                    byte[] fixedData = new byte[bytesData.Length / 2];
                    for (int i = 0, fi = 0; i < bytesData.Length; i += 2)
                    {
                        fixedData[fi++] = (byte) (((bytesData[i] - '0') * 10) + (bytesData[i + 1] - '0'));
                    }

                    data = Encoding.ASCII.GetString(fixedData);
                }

                data = data.Replace("{", "{{"); // Escape { character
                data = $"{(char) 0x7B}{(char) code}" + data; // Add start code
            }

            // Set position
            _buffer.Write(new byte[]
                {29, 72, (byte) position}); // Select print position of HRI characters [GS,H,position]
            // Set font mode
            _buffer.Write(new byte[] {29, 102, (byte) fontMode}); // Select font for HRI characters [GS,f,fontmode]
            // Set height
            _buffer.Write(new byte[] {29, 104, height}); // Set barcode height [GS,h,height]
            // Set width
            _buffer.Write(new byte[] {29, 119, width}); // Set barcode width [GS,w,width]

            _buffer.Write(new byte[] {29, 107, (byte) barcodeType}); // Select barcode type [GS,k,barcodetype]
            _buffer.Write(new byte[] {(byte) data.Length}); // barcode length
            _buffer.Write(Encoding.ASCII.GetBytes(data)); // barcode data

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Print 2D Barcode / QR Code
        /// </summary>
        /// <param name="barcodeType"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="correctionLevel"></param>
        public void PrintBarcode2D(Barcode2DType barcodeType, string data, Barcode2DSize size = Barcode2DSize.Normal, Barcode2DCorrectionLevel correctionLevel = Barcode2DCorrectionLevel.Percent7)
        {
            // Set data
            int num = data.Length + 3;

            if (barcodeType == Barcode2DType.Pdf417)
            {
                // Set size
                _buffer.Write(new byte[] { 29, 40, 107, 3, 0, 48, 67, (byte)size });
                // Set correction level
                _buffer.Write(new byte[] { 29, 40, 107, 4, 0, 48, 69, 48, (byte)correctionLevel });
                // Store data
                _buffer.Write(new byte[] {29, 40, 107, (byte) num, 0, 48, 80, 48});

                // Barcode data
                _buffer.Write(Encoding.ASCII.GetBytes(data));

                // print barcode
                _buffer.Write(new byte[] { 29, 40, 107, 3, 0, 48, 81, 48 });
            }
            else
            {
                // Set model
                _buffer.Write(new byte[] { 29, 40, 107, 4, 0, 49, 65, (byte)barcodeType, 0 });
                // Set size
                _buffer.Write(new byte[] { 29, 40, 107, 3, 0, 49, 67, (byte)size });
                // Set correction level
                _buffer.Write(new byte[] { 29, 40, 107, 3, 0, 49, 69, (byte)correctionLevel });

                int pL = num % 256;
                int pH = num / 256;
                // Store data
                _buffer.Write(new byte[] {29, 40, 107, (byte) pL, (byte) pH, 49, 80, 48});

                // Barcode data
                _buffer.Write(Encoding.ASCII.GetBytes(data));

                // print barcode
                _buffer.Write(new byte[] { 29, 40, 107, 3, 0, 49, 81, 48 });
            }

            if (PrinterMode == EscPrinterMode.DirectMode)
                FlushBuffer();
        }

        /// <summary>
        /// Print Image (Note: printer is reset to default values after printing)
        /// </summary>
        /// <param name="image">Source image</param>
        /// <param name="threshold">threshold for black and white balance</param>
        /// <param name="multiplier">size multiplier</param>
        /// <param name="inverted">invert black and white</param>
        /// <returns></returns>
        public void PrintImage(Bitmap image, int threshold = 127, double multiplier = 300, bool inverted = false)
        {
            BitmapExtended data = ConvertToMonochrome(image, threshold, multiplier, inverted);
            BitArray dots = data.Data;

            byte[] width = BitConverter.GetBytes(data.Width);

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(stream))
                {
                    // Set the line spacing at 24 (we'll print 24 dots high)
                    binaryWriter.Write((char) 0x1B);
                    binaryWriter.Write('3');
                    binaryWriter.Write((byte) 24);

                    int offset = 0;
                    while (offset < data.Height)
                    {
                        binaryWriter.Write((char) 0x1B);
                        binaryWriter.Write('*');
                        binaryWriter.Write((byte) 33); // 24-dot double-density
                        binaryWriter.Write(width[0]); // width low byte
                        binaryWriter.Write(width[1]); // width high byte

                        for (int x = 0; x < data.Width; ++x)
                        {
                            for (int k = 0; k < 3; ++k)
                            {
                                byte slice = 0;
                                for (int b = 0; b < 8; ++b)
                                {
                                    int y = (offset / 8 + k) * 8 + b;
                                    int i = (y * data.Width) + x;
                                    bool v = false;
                                    if (i < dots.Length)
                                    {
                                        v = dots[i];
                                    }

                                    slice |= (byte) ((v ? 1 : 0) << (7 - b));
                                }

                                binaryWriter.Write(slice);
                            }
                        }

                        offset += 24;
                        binaryWriter.Write((char) 0x0A);
                    }

                }

                _buffer.Write(stream.ToArray());
                _buffer.Write(new byte[] { 27, 64, 27, 83 }); // ESC Reinit [ESC,@] ESC Standard mode [ESC,S]
            }

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

        /// <summary>
        /// Convert bitmap to monochrome 1bit per pixel
        /// </summary>
        /// <param name="original"></param>
        /// <param name="threshold"></param>
        /// <param name="multiplier"></param>
        /// <param name="inverted"></param>
        /// <returns></returns>
        private BitmapExtended ConvertToMonochrome(Bitmap original, int threshold = 127, double multiplier = 300, bool inverted = false)
        {
            // Calculate the scaling factor for the image
            double scale = multiplier / original.Width;
            // Calculate the new height and width
            int height = (int)(original.Height * scale);
            int width = (int)(original.Width * scale);
            int dimensions = width * height;

            BitArray dots = new BitArray(dimensions);

            int index = 0;
            // Loop through the pixels to generate the monochrome bit array
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int scaledX = (int)(j / scale);
                    int scaledY = (int)(i / scale);
                    // Get the pixel from the original image
                    Color pixelColor = original.GetPixel(scaledX, scaledY);
                    // Get the luminance from the pixel
                    int luminance = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.16 + pixelColor.B * 0.114);
                    // Set the dot value to true if the luminance is below threshold
                    if (!inverted)
                        dots[index] = luminance > threshold;
                    else
                        dots[index] = luminance < threshold;
                    index++;
                }
            }

            return new BitmapExtended(width, height, dots);
        }
    }
}