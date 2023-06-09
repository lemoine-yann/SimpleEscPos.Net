﻿using SimpleEscPos.Net;

namespace ExampleEscPos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize printer instance with IP, Port and Print mode
            EscPrinter myprinter = new EscPrinter("192.168.1.30", 9100, EscPrinterMode.DirectMode);

            //myprinter.Print(201);
            //myprinter.Print(205, 40);
            //myprinter.Print(new byte[] { 201, 205, 205, 205, 205, 205, 205, 205, 205, 205, 205, 29, 33, 119, 27, 45, 50 });

            //myprinter.SetCharacterSizeMagnification(4, 4);
            /*myprinter.SetUnderlineMode(UnderlineMode.OneDot);
            myprinter.Print("test");
            myprinter.SetUnderlineMode(UnderlineMode.TwoDot);
            myprinter.Print("retest");
            myprinter.SetUnderlineMode(UnderlineMode.Off);
            myprinter.SetCharacterSizeMagnification(4, 4);
            myprinter.Print("reretest");
            //myprinter.Print(new byte[] { 27, 69, 1 });
            myprinter.SetBold(true);
            myprinter.Print("reretest");
            myprinter.SetBold(false);*/
            myprinter.SetCharacterSizeMagnification(7,7);
            //myprinter.Print(new byte[] { 29, 66, 1 });
            myprinter.SetFont(FontMode.FontA);
            myprinter.Print("reretest");
            myprinter.SetFont(FontMode.FontB);
            myprinter.Print("reretest");

            /*myprinter.Print("╔══════════════════════╗");
            myprinter.Print("║ Bon de préparation   ║");
            myprinter.Print("╚══════════════════════╝");
            myprinter.Print("║ 0125 test 654 65 4   ║");*/

            //myprinter.Print(new byte[] { 201, 205, 205, 205, 205, 205, 205, 205, 205, 205, 205 });


            //myprinter.Print(new byte[] { 201 });

            //myprinter.Print("ceci est un test d'écrire de test @ç_");

            //myprinter.Print("ÖÖÜẞßöäÄ");

            //myprinter.Print(new byte[] { 84, 84, 84, 84, 10});

            //myprinter.Print(new byte[] { 84, 84, 84, 84 });

            //myprinter.PaperFeed(1);

            myprinter.PaperFeed(6);

            //myprinter.PaperFeed(0);

            //myprinter.Cut();

            // Dispose instance
            myprinter.Dispose();

            // Initialize instance in buffer mode
            myprinter = new EscPrinter("192.168.1.30", 9100, EscPrinterMode.BufferMode);

            myprinter.Print("ceci est un test");

            myprinter.PaperFeed(10);

            myprinter.Cut();

            myprinter.Print();

            // Dispose instance
            myprinter.Dispose();

            Console.WriteLine("Press key to exit");
            Console.ReadKey();
        }
    }
}