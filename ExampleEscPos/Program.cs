using SimpleEscPos.Net;
using System.Drawing;
using System.Reflection;

namespace ExampleEscPos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize printer instance with IP, Port and Print mode
            EscPrinter myprinter = new EscPrinter("192.168.1.30", 9100, EscPrinterMode.DirectMode);
            // Center align
            myprinter.SetAlign(AlignMode.Center);
            // Print logo, 127 is a good start for threshold but this image need 10 for a good contrast
            myprinter.PrintImage(new Bitmap(Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "cognitys_logo.png"))), 10);
            // Print text
            myprinter.Print("https://cognitys.com");
            // Print a tab with random data
            myprinter.Print("╔════════════════════════════════════════════════╗");
            for(int i = 0; i < 10; i++)
            {
                myprinter.Print("║ " + Guid.NewGuid().ToString() + " ║" + " 15.55 €" + " ║");
            }
            myprinter.Print("╚════════════════════════════════════════════════╝");
            // Center align
            myprinter.SetAlign(AlignMode.Center);
            // Print inverted text with size 5
            myprinter.SetInverted(true);
            myprinter.SetCharacterSizeMagnification(3, 3);
            myprinter.Print("Inverted text");
            // feed paper
            myprinter.PaperFeed(2);
            // Print barcode
            myprinter.PrintBarcode(BarcodeType.Code128, "123456789");
            // feed paper
            myprinter.PaperFeed(2);
            // Special chars
            myprinter.SetInverted(false);
            myprinter.SetCharacterSizeMagnification(1, 1);
            myprinter.Print("áéíóúçãõàèìòùâêîôû");

            // feed paper
            myprinter.PaperFeed(10);
            // cut paper
            myprinter.Cut();

            // Dispose instance
            myprinter.Dispose();

            Console.WriteLine("Press key to exit");
            Console.ReadKey();
        }
    }

}