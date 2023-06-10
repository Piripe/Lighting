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
    public class GradientEffect : ILightingEffect
    {
        [JsonProperty("type")]
        public LightingEffectType Type { get; } = LightingEffectType.Gradient;
        [JsonProperty("layer")]
        public int Layer { get; set; }
        [JsonProperty("frame")]
        public int Frame { get; set; } = 0;
        [JsonProperty("length")]
        public int Length { get; set; } = 30;


        [JsonProperty("color")]
        public Color Color { get; set; } = Color.White;
        [JsonProperty("colorB")]
        public Color ColorB { get; set; } = Color.White;

        public Color GetColor(int frame)
        {
            return ColorUtils.BlendColors(Color,ColorB,frame/(float)Length);
        }
    }
}
