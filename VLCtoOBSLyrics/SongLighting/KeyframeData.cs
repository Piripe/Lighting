using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCtoOBSLyrics.SongLighting
{
    public struct KeyframeData<T>
    {
        [JsonProperty("ease")]
        public Ease Ease;
        [JsonProperty("value")]
        public T Value;

        public KeyframeData(Ease ease, T value) {
            Ease = ease;
            Value = value;
         }
    }
}
