
// EscPrinter by Lemoine Yann
// Visit https://github.com/lemoine-yann/SimpleEscPos.Net

using System;
using System.Collections.Generic;
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
        /// Cut paper
        /// </summary>
        public void Cut();
    }
}
