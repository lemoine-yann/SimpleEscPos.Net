# SimpleEscPos.Net - a simple library that supports basic functionalities of the ESC/POS standard by Epson

[![Hugging Face](https://img.shields.io/badge/%F0%9F%A4%97%20Hugging%20Face-blue)](https://github.com/lemoine-yann)

Compiled with .Net 7 and tested on Star TSP800-II Emulation mode Esc/Pos

## Preview

![Output Preview](https://s3.eu-west-1.amazonaws.com/lemoine.yann/github/escpos/escpos_preview.jpg)

## Basic Usage

## Notes

Library use 1252 codepage, if you need another pagecode please check Espon documentation about [ESC,GS,t,xx] , in my case parameter 32 mean 1252

If you use a printer on Samba protocol, you can easily replace the socket by copying buffer on network stream.

The library is minimalist, if you want more functionalities, you can easily add it by yourself or send ESC/POS bytecode using the Print(byte[] data) method.

## Licence

MIT

Contact : [lemoine.yann31@gmail.com](mailto:lemoine.yann31@gmail.com)
