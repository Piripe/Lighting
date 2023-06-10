using LightingProgrammator.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator.Settings
{
    public partial class StaticEffectSettings : UserControl, ISettingsPanel
    {
        public ILightingEffect Effect { get => effect; }
        private StaticEffect effect;
        private Timeline timeline;

        public StaticEffectSettings(StaticEffect effect, Timeline timeline)
        {
            this.effect = effect;
            this.timeline = timeline;
            InitializeComponent();
            colorInput1.Color = effect.Color;
            colorInput1.ColorChanged += ColorInput1_ColorChanged;
        }

        public void UpdateDatas()
        {
            colorInput1.Color = effect.Color;
        }

        private void ColorInput1_ColorChanged(object? sender, EventArgs e)
        {
            //effect.Color = colorInput1.Color;
            if (timeline.mainSelectionEffect == effect)
            {
                SettingsUtils.SetSetting(timeline.effectsSelection,effect,"Color",colorInput1.Color);
                timeline.UpdateDisplay();
            }
        }

        private void StaticColorSettings_Load(object sender, EventArgs e)
        {

        }

    }
}
