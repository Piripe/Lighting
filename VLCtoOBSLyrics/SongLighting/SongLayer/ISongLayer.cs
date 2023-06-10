using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCtoOBSLyrics.SongLighting.SongLayer
{
    public interface ISongLayer
    {
        public SongLayerType Type { get; }
        public SongLayerAction Action { get; set; }
        public string ActionElement { get; set; }
        public string[] ActionData { get; set; }
        public object GetValueAtFrame(int frame);
    }
}
