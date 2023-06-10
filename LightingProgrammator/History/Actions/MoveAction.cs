using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator.History.Actions
{
    internal class MoveAction : IHistoryAction
    {
        private List<ILightingEffect> effects;
        private List<int> fromFrame;
        private List<int> toFrame;
        private List<int> fromLayer;
        private List<int> toLayer;

        public MoveAction(IEnumerable<ILightingEffect> effects, IEnumerable<int> fromFrame, IEnumerable<int> toFrame, IEnumerable<int> fromLayer, IEnumerable<int> toLayer)
        {
            this.effects = effects.ToList();
            this.fromFrame = fromFrame.ToList();
            this.toFrame = toFrame.ToList();
            this.fromLayer = fromLayer.ToList();
            this.toLayer = toLayer.ToList();
        }

        public void Redo()
        {
            for (int i = 0; i<effects.Count(); i++)
            {
                effects[i].Frame = toFrame[i];
                effects[i].Layer = toLayer[i];
            }

            if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
            {
                SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];
                lighting.SortLightingEffects();
            }

            Program.form.timeline.UpdateDisplay();
        }

        public void Undo()
        {
            for (int i = 0; i < effects.Count(); i++)
            {
                effects[i].Frame = fromFrame[i];
                effects[i].Layer = fromLayer[i];
            }

            if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
            {
                SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];
                lighting.SortLightingEffects();
            }

            Program.form.timeline.UpdateDisplay();
        }
    }
}
