using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator.History.Actions
{
    internal class PasteAction : IHistoryAction
    {
        private List<ILightingEffect> effects;

        public PasteAction(List<ILightingEffect> effects)
        {
            this.effects = effects;
        }

        public void Redo()
        {
            if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
            {
                if (effects.Count > 0)
                {
                    SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

                    lighting.FogEffects.AddRange(effects);
                    lighting.SortLightingEffects();

                    Program.form.timeline.UpdateDisplay();
                }
            }
        }

        public void Undo()
        {
            Program.form.timeline.DeleteEffects(effects);
        }
    }
}
