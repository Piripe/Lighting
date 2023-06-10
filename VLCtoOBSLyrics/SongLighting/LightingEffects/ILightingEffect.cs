using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCtoOBSLyrics.SongLighting.LightingEffects
{
    public interface ILightingEffect
    {
        public LightingEffectType Type { get; }
        public int Layer { get; set; }
        public int Frame { get; set; }
        public int Length { get; set; }


        public Color GetColor(int frame);
    }
}
