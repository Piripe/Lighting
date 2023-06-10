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
    public class BlinkEffect : ILightingEffect
    {
        [JsonProperty("type")]
        public LightingEffectType Type { get; } = LightingEffectType.Blink;
        [JsonProperty("layer")]
        public int Layer { get; set; }
        [JsonProperty("frame")]
        public int Frame { get; set; } = 0;
        [JsonProperty("length")]
        public int Length { get; set; } = 30;


        [JsonProperty("colorA")]
        public Color ColorA { get; set; } = Color.White;
        [JsonProperty("colorB")]
        public Color ColorB { get; set; } = Color.White;
        [JsonProperty("lengthA")]
        public int LengthA { get; set; } = 1;
        [JsonProperty("lengthB")]
        public int LengthB { get; set; } = 1;
        [JsonProperty("offset")]
        public int Offset { get; set; } = 0;

        public Color GetColor(int frame)
        {
            if ((frame + Offset) % (LengthA + LengthB) < LengthA) return ColorA;
            return ColorB;
        }
    }
}
