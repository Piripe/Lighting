using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator
{
    internal static class Static
    {
        static internal readonly List<string> LightingEffectIDs = new string[] { "Static", "Gradient", "Blink", "Breathe" }.ToList();
        static internal readonly Type[] LightingEffectTypes = { typeof(StaticEffect), typeof(GradientEffect), typeof(BlinkEffect), typeof(BreatheEffect) };
        static internal Config config;
        static internal string currentSongID = "";


    }
}
