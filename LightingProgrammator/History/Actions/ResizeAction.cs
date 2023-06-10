using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator.History.Actions
{
    internal class ResizeAction : IHistoryAction
    {
        private ILightingEffect effect;
        private int fromFrame;
        private int toFrame;
        private int fromLength;
        private int toLength;

        public ResizeAction(ILightingEffect effect, int fromFrame, int toFrame, int fromLength, int toLength)
        {
            this.effect = effect;
            this.fromFrame = fromFrame;
            this.toFrame = toFrame;
            this.fromLength = fromLength;
            this.toLength = toLength;
        }

        public void Redo()
        {
            effect.Frame = toFrame;
            effect.Length = toLength;

            Program.form.timeline.UpdateDisplay();
        }

        public void Undo()
        {
            effect.Frame = fromFrame;
            effect.Length = fromLength;

            Program.form.timeline.UpdateDisplay();
        }
    }
}
