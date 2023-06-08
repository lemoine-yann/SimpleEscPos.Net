
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
        /// Feed paper for n lines
        /// </summary>
        /// <param name="lines"></param>
        public void PaperFeed(byte lines);

        /// <summary>
        /// Cut paper
        /// </summary>
        public void Cut();
    }
}
