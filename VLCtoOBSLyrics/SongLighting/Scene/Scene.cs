using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCtoOBSLyrics.SongLighting.Scene
{
    public class Scene
    {
        [JsonProperty("elements")]
        public List<SceneElement> Elements { get; set; } = new List<SceneElement>();
    }
}
