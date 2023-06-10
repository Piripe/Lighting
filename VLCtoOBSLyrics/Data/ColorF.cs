using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCtoOBSLyrics.Data
{
    internal struct ColorF
    {
        internal float A;
        internal float R;
        internal float G;
        internal float B;


        internal static ColorF FromARGB(float a,float r, float g, float b)
        {
            ColorF c = new ColorF();
            c.A = a;
            c.R = r;
            c.G = g;
            c.B = b;

            return c;
        }

        internal Color ToColor()
        {
            unchecked
            {
                return Color.FromArgb(
                    (byte)Math.Floor(Math.Max(0, Math.Min(255, A))), 
                    (byte)Math.Floor(Math.Max(0, Math.Min(255, R))), 
                    (byte)Math.Floor(Math.Max(0, Math.Min(255, G))), 
                    (byte)Math.Floor(Math.Max(0, Math.Min(255, B)))
                    );
            }
        }
    }
}
