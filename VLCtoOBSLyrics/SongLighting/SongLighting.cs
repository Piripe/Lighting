using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VLCtoOBSLyrics.SongLighting.LightingEffects;
using VLCtoOBSLyrics.Utils;

namespace VLCtoOBSLyrics.SongLighting
{
    public class SongLighting
    {
        [JsonProperty("type")]
        SongLightingType Type { get; set; } = SongLightingType.Static;
        [JsonProperty("fogColor")]
        public Color FogColor { get; set; } = new Color();
        [JsonProperty("lightColor1")]
        public Color LightColor1 { get; set; } = new Color();
        [JsonProperty("lightColor2")]
        public Color LightColor2 { get; set; } = new Color();
        [JsonProperty("fogEffects")]
        public List<ILightingEffect> FogEffects { get; set; } = new List<ILightingEffect>();
        [JsonProperty("lightEffects1")]
        public List<ILightingEffect> LightEffects1 { get; set; } = new List<ILightingEffect>();
        [JsonProperty("lightEffects2")]
        public List<ILightingEffect> LightEffects2 { get; set; } = new List<ILightingEffect>();

        public SongLighting(SongLightingType type, Color fogColor, Color lightColor1, Color lightColor2, List<JObject>? fogEffects, List<JObject>? lightEffects1, List<JObject>? lightEffects2)
        {
            Type = type;
            FogColor = fogColor;
            LightColor1 = lightColor1;
            LightColor2 = lightColor2;
            if (fogEffects != null) FogEffects = fogEffects.Select((JObject effect)=>LightingEffectUtils.GetLightingEffectFromJObject(effect)).ToList();
            if (lightEffects1 != null) LightEffects1 = lightEffects1.Select((JObject effect)=> LightingEffectUtils.GetLightingEffectFromJObject(effect)).ToList();
            if (lightEffects2 != null) LightEffects2 = lightEffects2.Select((JObject effect) => LightingEffectUtils.GetLightingEffectFromJObject(effect)).ToList();
        }

        public LightsColor GetLightsColor(TimeSpan position)
        {
            int frame = (int)(position.TotalMilliseconds / (1000f / 30));

            return GetLightsColor(frame);
        }
        public LightsColor GetLightsColor(int frame)
        {
            LightsColor lights = new LightsColor();
            switch (Type)
            {
                case SongLightingType.Static:
                    lights.FogColor = FogColor;
                    lights.LightColor1 = LightColor1;
                    lights.LightColor2 = LightColor2;
                    break;
                case SongLightingType.Effects:
                    lights.FogColor = LightingEffectUtils.GetColorFromLightingEffects(FogEffects, frame);
                    lights.LightColor1 = LightingEffectUtils.GetColorFromLightingEffects(LightEffects1, frame);
                    lights.LightColor2 = LightingEffectUtils.GetColorFromLightingEffects(LightEffects2, frame);
                    break;
            }
            return lights;
        }
        public void SortLightingEffects()
        {
            List<ILightingEffect> effects = new List<ILightingEffect>();
            effects.AddRange(FogEffects);
            effects.AddRange(LightEffects1);
            effects.AddRange(LightEffects2);
            FogEffects = effects.FindAll((effect) => effect.Layer == 0);
            LightEffects1 = effects.FindAll((effect) => effect.Layer == 1);
            LightEffects2 = effects.FindAll((effect) => effect.Layer == 2);
        }
    }
    public struct LightsColor
    {
        public Color FogColor { get; set; }
        public Color LightColor1 { get; set; }
        public Color LightColor2 { get; set; }
        public
        static LightsColor BlendLightsColor(LightsColor a, LightsColor b, float ratio)
        {
            if (ratio <= 0) return a;
            if (ratio >= 1) return b;

            LightsColor lights = new LightsColor();

            lights.FogColor = ColorUtils.BlendColors(a.FogColor, b.FogColor, ratio);
            lights.LightColor1 = ColorUtils.BlendColors(a.LightColor1, b.LightColor1, ratio);
            lights.LightColor2 = ColorUtils.BlendColors(a.LightColor2, b.LightColor2, ratio);

            return lights;
        }

        public static bool operator !=(LightsColor left, LightsColor right) => !(left == right);
        public static bool operator ==(LightsColor left, LightsColor right) => (
            left.FogColor == right.FogColor &&
            left.LightColor1 == right.LightColor1 &&
            left.LightColor2 == right.LightColor2
            );
    }
}
