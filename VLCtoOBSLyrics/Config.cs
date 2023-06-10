using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting.Scene;

namespace VLCtoOBSLyrics
{
    public class Config
    {
        [JsonProperty("version")]
        public int Version { get; set; } = 1;
        [JsonProperty("v1")]
        public V1 V1 { get; set; } = new V1();

        [JsonProperty("v2")]
        public V2 V2 { get; set; } = new V2();

        [JsonProperty("v3")]
        public V3 V3 { get; set; } = new V3();

    }
    public class V1
    {
        [JsonProperty("sceneName")]
        public string SceneName { get; set; } = "Radio tah les fous mais en mode karaoké";
        [JsonProperty("sceneToSetName")]
        public string SceneToSetName { get; set; } = "Radio tah les fous mais en mode karaoké";
        [JsonProperty("sceneWoLyricsName")]
        public string SceneWOLyricsName { get; set; } = "Radio tah les fous";
        [JsonProperty("sceneWoLyricsToSetName")]
        public string SceneWOLyricsToSetName { get; set; } = "Radio tah les fous";
        [JsonProperty("trackNameSource")]
        public string TrackNameSource { get; set; } = "Music Artist - Title";
    }
    public class V2
    {
        [JsonProperty("sceneName")]
        public string SceneName { get; set; } = "Radio V2";
        [JsonProperty("sourceFog")]
        public string SourceFog { get; set; } = "[COLOR] Radio Fog";
        [JsonProperty("sourceL1")]
        public string SourceL1 { get; set; } = "[VID] Radio Light 1";
        [JsonProperty("sourceL2")]
        public string SourceL2 { get; set; } = "[VID] Radio Light 2";
        [JsonProperty("songsLighting")]
        public Dictionary<string, SongLighting.SongLighting> SongsLighting { get; set; } = new Dictionary<string, SongLighting.SongLighting>();
    }
    public class V3
    {
        [JsonProperty("sceneName")]
        public string SceneName { get; set; } = "Radio V3";
        [JsonProperty("scenes")]
        public Dictionary<string, Scene> Scenes { get; set; } = new Dictionary<string, Scene>();
        [JsonProperty("songsLighting")]
        public Dictionary<string, SongLighting.SongLightingV3> SongsLighting { get; set; } = new Dictionary<string, SongLighting.SongLightingV3>();
    }
}
