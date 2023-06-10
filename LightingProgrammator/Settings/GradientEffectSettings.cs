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
using System.Windows.Media.Effects;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator.Settings
{
    public partial class GradientEffectSettings : UserControl, ISettingsPanel
    {
        public ILightingEffect Effect { get => effect; }
        private GradientEffect effect;
        private Timeline timeline;

        public GradientEffectSettings(GradientEffect effect, Timeline timeline)
        {
            this.effect = effect;
            this.timeline = timeline;
            InitializeComponent();
            colorInput1.Color = effect.Color;
            colorInput1.ColorChanged += ColorInput1_ColorChanged;
            colorInput2.Color = effect.ColorB;
            colorInput2.ColorChanged += ColorInput2_ColorChanged;
        }

        public void UpdateDatas()
        {
            colorInput1.Color = effect.Color;
            colorInput2.Color = effect.ColorB;
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

        private void ColorInput2_ColorChanged(object? sender, EventArgs e)
        {
            if (timeline.mainSelectionEffect == effect)
            {
                SettingsUtils.SetSetting(timeline.effectsSelection, effect, "ColorB", colorInput2.Color);
                timeline.UpdateDisplay();
            }
        }

        private void StaticColorSettings_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (timeline.mainSelectionEffect == effect)
            {
                SettingsUtils.SetSetting(timeline.effectsSelection, effect, "Color", colorInput2.Color);
                SettingsUtils.SetSetting(timeline.effectsSelection, effect, "ColorB", colorInput1.Color);
                UpdateDatas();
                timeline.UpdateDisplay();
            }
        }
    }
}
