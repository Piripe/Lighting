using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VLCtoOBSLyrics.SongLighting.LightingEffects;
using VLCtoOBSLyrics.SongLighting.SongLayer;
using VLCtoOBSLyrics.Utils;

namespace VLCtoOBSLyrics.SongLighting
{
    public class SongLightingV3
    {
        [JsonProperty("scene")]
        public string Scene { get; set; } = "";
        [JsonProperty("layers")]
        public List<ISongLayer> Layers { get; set; } = new List<ISongLayer>();

        public SongLightingV3(string scene, List<JObject>? layers)
        {
            Scene = scene;
            if (layers != null) Layers = layers.Select((JObject layer)=>SongLayerUtils.GetLightingEffectFromJObject(layer)).ToList();
        }
    }
}
