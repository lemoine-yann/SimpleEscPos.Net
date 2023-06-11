# SimpleEscPos.Net - a simple library that supports basic functionalities of the ESC/POS standard by Epson

[![Hugging Face](https://img.shields.io/badge/%F0%9F%A4%97%20Hugging%20Face-blue)](https://github.com/lemoine-yann)

Compiled with .Net 7 and tested on Star TSP800-II Emulation mode Esc/Pos

## Preview

![Output Preview](https://s3.eu-west-1.amazonaws.com/lemoine.yann/github/escpos/escpos_preview.jpg)

## Basic Usage

### Instantiate the library in direct mode

```
EscPrinter myprinter = new EscPrinter("192.168.1.30", 9100, EscPrinterMode.DirectMode);
myprinter.Print("Hello world !"); // Data is sent directly to the printer and instantly printed
myprinter.Print("Other line");
myprinter.Dispose(); // Dipose lib
```

### Alternatively, you can use the library in buffer mode

```
EscPrinter myprinter = new EscPrinter("192.168.1.30", 9100, EscPrinterMode.BufferMode);
myprinter.Print("Hello world !"); // Data is stored in a buffer, nothing is sent to the printer
myprinter.Print("Other line"); // Data is stored in a buffer, nothing is sent to the printer
myprinter.Print(); // Data is sent to the printer and printed
myprinter.Dispose(); // Dipose lib
```

### Print a barcode

```
myprinter.PrintBarcode(BarcodeType.Code128, "123456789");
```

### Print image

```
myprinter.PrintImage(bitmap source);
```

## List of commands

|    Command                     |          Action                   |
|:------------------------------:|:---------------------------------:|
| ReinitializePrinter            | Reset printer                     |
| ClearBuffer                    | Clear buffer                      |
| Cut                            | Cut the paper                     |
| Print()                        | Print the buffer (in buffer mode) |
| Print(byte[])                  | Print bytes or direct commands    |
| Print(byte)                    | Print byte                        |
| Print(string, parameters       | Print text                        |
| PaperFeed                      | Feed paper                        |
| SetCharacterSizeMagnification  | Set characters size               |
| SetUnderlineMode               | Set/Unset underline text          |
| SetBold                        | Set/Unset bold text               |
| SetInverted                    | Set/Unset inverted text (B&W)     |
| SetClockwiseRotation           | Set text/objects rotation         |
| PrintBarcode                   | Print 1D barcode                  |
| PrintBarcode2D                 | Print 2D/QR code                  |
| PrintImage                     | Print images                      |

## Notes

Library use 1252 codepage, if you need another pagecode please check Espon documentation about [ESC,GS,t,xx] , in my case parameter 32 mean 1252

If you use a printer on Samba protocol, you can easily replace the socket by copying buffer on network stream.

The library is minimalist, if you want more functionalities, you can easily add it by yourself or send ESC/POS bytecode using the Print(byte[] data) method.

## Licence

MIT

Contact : [lemoine.yann31@gmail.com](mailto:lemoine.yann31@gmail.com)
