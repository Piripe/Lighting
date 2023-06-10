using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace VLCtoOBSLyrics.Utils
{
    internal static class LightingEffectUtils
    {
        internal static ILightingEffect GetLightingEffectFromJObject(JObject obj)
        {
            LightingEffectType effectType = (LightingEffectType)obj.Value<int>("type");

            switch (effectType)
            {
                case LightingEffectType.Static:
                    return obj.ToObject<StaticEffect>() ?? new StaticEffect();
                case LightingEffectType.Gradient:
                    return obj.ToObject<GradientEffect>() ?? new GradientEffect();
                case LightingEffectType.Blink:
                    return obj.ToObject<BlinkEffect>() ?? new BlinkEffect();
                case LightingEffectType.Breathe:
                    return obj.ToObject<BreatheEffect>() ?? new BreatheEffect();
            }
            return new StaticEffect();
        }

        internal static Color GetColorFromLightingEffects(this List<ILightingEffect> effects, TimeSpan position)
        {
            int frame = (int)(position.TotalMilliseconds / (1000f / 30));

            return GetColorFromLightingEffects(effects,frame);
        }
        internal static Color GetColorFromLightingEffects(this List<ILightingEffect> effects, int frame)
        {
            List<ILightingEffect> workingEffects = effects.FindAll((effect) => effect.Frame <= frame && effect.Frame + effect.Length > frame);
            workingEffects.Sort((a, b) => a.Frame.CompareTo(b.Frame));

            Color blendedColor = workingEffects.Count == 2 ? 
                ColorUtils.BlendColors(workingEffects[0].GetColor(frame - workingEffects[0].Frame), workingEffects[1].GetColor(frame - workingEffects[1].Frame), (float)(frame - workingEffects[1].Frame+1) / ((workingEffects[0].Frame + workingEffects[0].Length)- workingEffects[1].Frame+1))
                : ColorUtils.BlendColors(workingEffects.Select((effect) => effect.GetColor(frame - effect.Frame)));

            return blendedColor;
        }
    }
}
