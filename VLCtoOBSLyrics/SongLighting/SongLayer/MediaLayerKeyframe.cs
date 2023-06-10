using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCtoOBSLyrics.SongLighting.SongLayer
{
    public struct MediaLayerKeyframe
    {
        [JsonProperty("status")]
        public MediaLayerAction? Status;
        [JsonProperty("cursor")]
        public int? Cursor;

        public MediaLayerKeyframe(MediaLayerAction? status, int? cursor)
        {
            Status = status;
            Cursor = cursor;
        }
    }
}
