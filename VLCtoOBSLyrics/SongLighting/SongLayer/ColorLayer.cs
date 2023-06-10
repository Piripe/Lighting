using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting.LightingEffects;
using VLCtoOBSLyrics.Utils;

namespace VLCtoOBSLyrics.SongLighting.SongLayer
{
    public class ColorLayer : ISongLayer
    {
        [JsonProperty("type")]
        public SongLayerType Type { get => SongLayerType.Color; }
        [JsonProperty("action")]
        public SongLayerAction Action { get; set; } = SongLayerAction.FilterSetting;
        [JsonProperty("actionElement")]
        public string ActionElement { get; set; } = "";
        [JsonProperty("actionData")]
        public string[] ActionData { get; set; } = { };
        [JsonProperty("colorEffects")]
        public List<ILightingEffect> ColorEffects { get; set; } = new List<ILightingEffect>();

        [JsonConstructor]
        public ColorLayer(SongLayerAction action, string actionElement, string[] actionData, List<JObject> colorEffects)
        {
            Action = action;
            ActionElement = actionElement;
            ActionData = actionData;
            if (colorEffects != null) ColorEffects = colorEffects.Select((JObject effect) => LightingEffectUtils.GetLightingEffectFromJObject(effect)).ToList();
        }
        public ColorLayer() {}

        public object GetValueAtFrame(int frame)
        {
            return GetColorAtFrame(frame).ToInt();
        }
        public Color GetColorAtFrame(int frame)
        {
            return LightingEffectUtils.GetColorFromLightingEffects(ColorEffects, frame);
        }
    }
}
