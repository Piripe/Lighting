using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator.Settings
{
    public interface ISettingsPanel
    {
        public ILightingEffect Effect { get; }
        public void UpdateDatas();
    }
}
