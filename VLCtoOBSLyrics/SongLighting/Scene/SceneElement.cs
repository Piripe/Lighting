using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCtoOBSLyrics.SongLighting.Scene
{
    public class SceneElement
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "NewElement";
        [JsonProperty("kind")]
        public string Kind { get; set; } = "ffmpeg_source";
        [JsonProperty("blendMode")]
        public string BlendMode { get; set; } = "normal";
        [JsonProperty("settings")]
        public JObject? Settings { get; set; }
        [JsonProperty("transform")]
        public JObject? Transform { get; set; }
        [JsonProperty("filters")]
        public List<SceneElementFilter> Filters { get; set; } = new List<SceneElementFilter>();

        
    }
}
