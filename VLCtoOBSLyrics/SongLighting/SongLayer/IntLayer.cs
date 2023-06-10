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
    public class IntLayer : ISongLayer
    {
        [JsonProperty("type")]
        public SongLayerType Type { get => SongLayerType.Int; }
        [JsonProperty("action")]
        public SongLayerAction Action { get; set; } = SongLayerAction.FilterSetting;
        [JsonProperty("actionElement")]
        public string ActionElement { get; set; } = "";
        [JsonProperty("actionData")]
        public string[] ActionData { get; set; } = { };
        [JsonProperty("keyframes")]
        public Dictionary<int,KeyframeData<int>> Keyframes { get; set; } = new Dictionary<int, KeyframeData<int>>();


        public object GetValueAtFrame(int frame)
        {
            return GetIntAtFrame(frame);
        }
        public int GetIntAtFrame(int frame)
        {
            List<int> originValidKeyframes = Keyframes.Keys.ToList().FindAll((int keyframe) => keyframe <= frame);
            List<int> nextValidKeyframes = Keyframes.Keys.ToList().FindAll((int keyframe) => keyframe <= frame);

            int originKeyframe = 0;
            KeyframeData<int> originKeyframeData = new KeyframeData<int>(Ease.None, 0);
            int nextKeyframe = 0;
            KeyframeData<int> nextKeyframeData = new KeyframeData<int>(Ease.None, 0);

            if (originValidKeyframes.Count == 0)
            {
                if (Keyframes.Count != 0)
                {
                    originKeyframe = Keyframes.Min((KeyValuePair<int, KeyframeData<int>> x) => x.Key);
                    originKeyframeData = Keyframes[originKeyframe];
                }
            }
            else
            {
                originKeyframe = originValidKeyframes.Max();
                originKeyframeData = Keyframes[originKeyframe];
            }

            if (nextValidKeyframes.Count == 0)
            {
                if (Keyframes.Count != 0)
                {
                    nextKeyframe = Keyframes.Min((KeyValuePair<int, KeyframeData<int>> x) => x.Key);
                    nextKeyframeData = Keyframes[nextKeyframe];
                }
            }
            else
            {
                nextKeyframe = nextValidKeyframes.Max();
                nextKeyframeData = Keyframes[nextKeyframe];
            }

            return (int)EaseHelper.GetEaseValue(originKeyframe,originKeyframeData,nextKeyframe,nextKeyframeData,frame);
        }
    }
}
