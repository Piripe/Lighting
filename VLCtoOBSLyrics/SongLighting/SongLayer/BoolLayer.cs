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
    public class BoolLayer : ISongLayer
    {
        [JsonProperty("type")]
        public SongLayerType Type { get => SongLayerType.Bool; }
        [JsonProperty("action")]
        public SongLayerAction Action { get; set; } = SongLayerAction.FilterSetting;
        [JsonProperty("actionElement")]
        public string ActionElement { get; set; } = "";
        [JsonProperty("actionData")]
        public string[] ActionData { get; set; } = { };
        [JsonProperty("keyframes")]
        public Dictionary<int,bool> Keyframes { get; set; } = new Dictionary<int, bool>();


        public object GetValueAtFrame(int frame)
        {
            return GetBoolAtFrame(frame);
        }
        public bool GetBoolAtFrame(int frame)
        {
            List<int> validKeyframes = Keyframes.Keys.ToList().FindAll((int keyframe) => keyframe <= frame);
            return validKeyframes.Count == 0 ? (Keyframes.Count == 0 ? false : Keyframes[Keyframes.Min((KeyValuePair<int, bool> x) => x.Key)]) : Keyframes[validKeyframes.Max()];
        }
    }
}
