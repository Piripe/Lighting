using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator.History.Actions
{
    internal class DropAction : IHistoryAction
    {
        private ILightingEffect effect;
        private int frame;
        private int layer;

        public DropAction(ILightingEffect effect, int frame, int layer)
        {
            this.effect = effect;
            this.frame = frame;
            this.layer = layer;
        }

        public void Redo()
        {
            if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
            {
                SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

                effect.Frame = frame;
                effect.Layer = layer;

                if (layer == 0) lighting.FogEffects.Add(effect);
                else if (layer == 1) lighting.LightEffects1.Add(effect);
                else if (layer == 2) lighting.LightEffects2.Add(effect);

                Program.form.timeline.UpdateDisplay();
            }
        }

        public void Undo()
        {
            Program.form.timeline.DeleteEffect(effect);
        }
    }
}
