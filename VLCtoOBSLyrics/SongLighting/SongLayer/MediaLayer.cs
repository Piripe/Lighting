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
    public class MediaLayer : ISongLayer
    {
        [JsonProperty("type")]
        public SongLayerType Type { get => SongLayerType.Media; }
        [JsonProperty("action")]
        public SongLayerAction Action { get; set; } = SongLayerAction.Media;
        [JsonProperty("actionElement")]
        public string ActionElement { get; set; } = "";
        [JsonProperty("actionData")]
        public string[] ActionData { get; set; } = { };
        [JsonProperty("keyframes")]
        public Dictionary<int,MediaLayerKeyframe> Keyframes { get; set; } = new Dictionary<int, MediaLayerKeyframe>();


        public object GetValueAtFrame(int frame)
        {
            return GetActionsAtFrame(frame);
        }
        public MediaLayerKeyframe GetActionsAtFrame(int frame)
        {
            return Keyframes.ContainsKey(frame) ? Keyframes[frame] : new MediaLayerKeyframe();
        }
    }
}
