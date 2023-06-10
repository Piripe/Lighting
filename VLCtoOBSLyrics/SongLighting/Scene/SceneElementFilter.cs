using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCtoOBSLyrics.SongLighting.Scene
{
    public class SceneElementFilter
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "NewFilter";
        [JsonProperty("kind")]
        public string Kind { get; set; } = "color_filter";
        [JsonProperty("settings")]
        public JObject? Settings { get; set; }
    }
}
