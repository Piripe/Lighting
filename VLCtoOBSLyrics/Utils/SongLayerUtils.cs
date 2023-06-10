using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting.LightingEffects;
using VLCtoOBSLyrics.SongLighting.SongLayer;

namespace VLCtoOBSLyrics.Utils
{
    internal static class SongLayerUtils
    {
        internal static ISongLayer GetLightingEffectFromJObject(JObject obj)
        {
            SongLayerType effectType = (SongLayerType)obj.Value<int>("type");

            switch (effectType)
            {
                case SongLayerType.Color:
                    return obj.ToObject<ColorLayer>() ?? new ColorLayer();
                case SongLayerType.Bool:
                    return obj.ToObject<BoolLayer>() ?? new BoolLayer();
            }
            return new ColorLayer();
        }
    }
}
