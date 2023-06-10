using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VLCtoOBSLyrics.Utils
{
    public static class ColorUtils
    {
        internal static Color BlendColors(Color a, Color b, float ratio)
        {
            ratio = Math.Min(1,Math.Max(0,ratio));

            byte A = (byte)Math.Floor(a.A * Math.Abs(ratio - 1) + b.A * ratio);
            byte R = (byte)Math.Floor(a.R * Math.Abs(ratio - 1) + b.R * ratio);
            byte G = (byte)Math.Floor(a.G * Math.Abs(ratio - 1) + b.G * ratio);
            byte B = (byte)Math.Floor(a.B * Math.Abs(ratio - 1) + b.B * ratio);

            return Color.FromArgb(A, R, G, B);
        }
        internal static Color BlendColors(IEnumerable<Color> colors)
        {
            if (colors == null) return Color.Black;
            if (colors.Count() == 0) return Color.Black;
            byte A = (byte)Math.Floor(colors.Select((c)=>(decimal)c.A).Sum() / colors.Count());
            byte R = (byte)Math.Floor(colors.Select((c)=>(decimal)c.R).Sum() / colors.Count());
            byte G = (byte)Math.Floor(colors.Select((c)=>(decimal)c.G).Sum() / colors.Count());
            byte B = (byte)Math.Floor(colors.Select((c) => (decimal)c.B).Sum() / colors.Count());

            return Color.FromArgb(A, R, G, B);
        }
            public static int ToInt(this Color color)
        {
            return color.R + (color.G << 8) + (color.B << 16) + (color.A << 24);
        }
    
    }
}
