# SimpleEscPos.Net - a simple library that supports basic functionalities of the ESC/POS standard by Epson

[![Hugging Face](https://img.shields.io/badge/%F0%9F%A4%97%20Hugging%20Face-blue)](https://github.com/lemoine-yann)

Compiled with .Net 7 and tested on Star TSP800-II Emulation mode Esc/Pos

## Basic Usage

## Notes

Library use 1252 codepage, you can easily change it in the code (pageCode 32 by default). Check Epson documentation about [ESC,GS,t,xx]

If you use a printer on Samba protocol, you can easily replace the socket by copying buffer on network stream.

The library is minimalist, if you want more functionalities, you can easily add it by yourself or send ESC/POS bytecode using the Print(byte[] data) method.

## Licence

MIT

Contact : [lemoine.yann31@gmail.com](mailto:lemoine.yann31@gmail.com)
