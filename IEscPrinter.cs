
// EscPrinter by Lemoine Yann
// Visit https://github.com/lemoine-yann/SimpleEscPos.Net

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEscPos.Net
{
    public interface IEscPrinter : IDisposable
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
        /// Reset printer to default values
        /// </summary>
        public void ReinitializePrinter();

        /// <summary>
        /// Clear buffer
        /// </summary>
        public void ClearBuffer();

        /// <summary>
        /// Cut paper
        /// </summary>
        public void Cut();

        /// <summary>
        /// Print data in buffer
        /// </summary>
        public void Print();

        /// <summary>
        /// Print custom bytecodes
        /// </summary>
        /// <param name="data"></param>
        public void Print(byte[] data);

        /// <summary>
        /// Print byte, number times
        /// </summary>
        /// <param name="data"></param>
        /// <param name="number"></param>
        public void Print(byte data, int number = 1);

        /// <summary>
        /// Print string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="addCarrierReturn"></param>
        /// <param name="pageCode">printer page code</param>
        /// <param name="encoding">data page code</param>
        public void Print(string data, bool addCarrierReturn = true, byte pageCode = 32, int encoding = 1252);

        /// <summary>
        /// Feed paper for n lines
        /// </summary>
        /// <param name="lines"></param>
        public void PaperFeed(byte lines);

        /// <summary>
        /// Set character size (0 to +7 vertical/horizontal)
        /// </summary>
        /// <param name="horizontalSize"></param>
        /// <param name="verticalSize"></param>
        public void SetCharacterSizeMagnification(byte horizontalSize, byte verticalSize);

        /// <summary>
        /// Set underline mode (none/onedot/twodots)
        /// </summary>
        /// <param name="underlineMode"></param>
        public void SetUnderlineMode(UnderlineMode underlineMode);

        /// <summary>
        /// Set bold mode
        /// </summary>
        /// <param name="bold"></param>
        public void SetBold(bool bold);

        /// <summary>
        /// Set inverted mode black/white
        /// </summary>
        /// <param name="inverted"></param>
        public void SetInverted(bool inverted);

        /// <summary>
        /// Select font A/B
        /// </summary>
        /// <param name="fontMode"></param>
        public void SetFont(FontMode fontMode);

        /// <summary>
        /// Set clockwise rotation mode
        /// </summary>
        /// <param name="rotate"></param>
        public void SetClockwiseRotation(bool rotate);

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
        public void PrintBarcode(BarcodeType barcodeType, string data, byte height = 162, byte width = 3, BarcodeTextPosition position = BarcodeTextPosition.Below, BarcodeCode code = BarcodeCode.CodeB, FontMode fontMode = FontMode.FontA);

        /// <summary>
        /// Print 2D Barcode / QR Code
        /// </summary>
        /// <param name="barcodeType"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="correctionLevel"></param>
        public void PrintBarcode2D(Barcode2DType barcodeType, string data, Barcode2DSize size = Barcode2DSize.Normal, Barcode2DCorrectionLevel correctionLevel = Barcode2DCorrectionLevel.Percent7);

        /// <summary>
        /// Print Image (Note: printer is reset to default values after printing)
        /// </summary>
        /// <param name="image">Source image</param>
        /// <param name="threshold">threshold for black and white balance</param>
        /// <param name="multiplier">size multiplier</param>
        /// <param name="inverted">invert black and white</param>
        /// <returns></returns>
        public void PrintImage(Bitmap image, int threshold = 127, double multiplier = 300, bool inverted = false);
    }
}
