using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.Utils;

namespace VLCtoOBSLyrics.SongLighting.LightingEffects
{
    [Serializable]
    public class BreatheEffect : ILightingEffect
    {
        [JsonProperty("type")]
        public LightingEffectType Type { get; } = LightingEffectType.Breathe;
        [JsonProperty("layer")]
        public int Layer { get; set; }
        [JsonProperty("frame")]
        public int Frame { get; set; } = 0;
        [JsonProperty("length")]
        public int Length { get; set; } = 30;


        [JsonProperty("colorA")]
        public Color ColorA { get; set; } = Color.White;
        [JsonProperty("colorB")]
        public Color ColorB { get; set; } = Color.Black;
        [JsonProperty("bpm")]
        public int BPM { get; set; } = 60;
        [JsonProperty("gradientLength")]
        public int GradientLength { get; set; } = 1;
        [JsonProperty("offset")]
        public int Offset { get; set; } = 0;

        public Color GetColor(int frame)
        {
            return ColorUtils.BlendColors(ColorA, ColorB, ((frame + Offset) / 30f / 60 * BPM)%1/(GradientLength/30f/60*BPM));
        }
    }
}
