using SimpleEscPos.Net;

namespace ExampleEscPos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize printer instance with IP, Port and Print mode
            EscPrinter myprinter = new EscPrinter("192.168.1.30", 9100, EscPrinterMode.DirectMode);

            myprinter.PaperFeed(30);

            myprinter.PaperFeed(3);

            myprinter.PaperFeed(0);

            myprinter.Cut();

            // Dispose instance
            myprinter.Dispose();

            // Initialize instance in buffer mode
            myprinter = new EscPrinter("192.168.1.30", 9100, EscPrinterMode.BufferMode);

            myprinter.Cut();

            myprinter.Print();

            // Dispose instance
            myprinter.Dispose();

            Console.WriteLine("Press key to exit");
            Console.ReadKey();
        }
    }
}