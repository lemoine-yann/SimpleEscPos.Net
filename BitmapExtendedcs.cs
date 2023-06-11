using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEscPos.Net
{
    public class BitmapExtended
    {
        public BitArray Data { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public BitmapExtended(int width, int height, BitArray data)
        {
            this.Width = width;
            this.Height = height;
            this.Data = data;
        }
    }
}
