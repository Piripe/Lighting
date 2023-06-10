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
    public partial class BlinkEffectSettings : UserControl, ISettingsPanel
    {
        public ILightingEffect Effect { get => effect; }
        private BlinkEffect effect;
        private Timeline timeline;

        public BlinkEffectSettings(BlinkEffect effect, Timeline timeline)
        {
            this.effect = effect;
            this.timeline = timeline;
            InitializeComponent();
            colorInput1.Color = effect.ColorA;
            colorInput1.ColorChanged += ColorInput1_ColorChanged;
            colorInput2.Color = effect.ColorB;
            colorInput2.ColorChanged += ColorInput2_ColorChanged;
            numericUpDown1.Value = effect.LengthA;
            numericUpDown2.Value = effect.LengthB;
            numericUpDown3.Value = effect.Offset;
        }

        public void UpdateDatas()
        {
            colorInput1.Color = effect.ColorA;
            colorInput2.Color = effect.ColorB;
            numericUpDown1.Value = effect.LengthA;
            numericUpDown2.Value = effect.LengthB;
            numericUpDown3.Value = effect.Offset;
        }

        private void ColorInput1_ColorChanged(object? sender, EventArgs e)
        {
            //effect.Color = colorInput1.Color;
            if (timeline.mainSelectionEffect == effect)
            {
                SettingsUtils.SetSetting(timeline.effectsSelection,effect,"ColorA",colorInput1.Color);
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
                SettingsUtils.SetSetting(timeline.effectsSelection, effect, "ColorA", colorInput2.Color);
                SettingsUtils.SetSetting(timeline.effectsSelection, effect, "ColorB", colorInput1.Color);
                UpdateDatas();
                timeline.UpdateDisplay();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (timeline.mainSelectionEffect == effect)
            {
                SettingsUtils.SetSetting(timeline.effectsSelection, effect, "LengthA", (int)numericUpDown1.Value);
                timeline.UpdateDisplay();
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (timeline.mainSelectionEffect == effect)
            {
                SettingsUtils.SetSetting(timeline.effectsSelection, effect, "LengthB", (int)numericUpDown2.Value);
                timeline.UpdateDisplay();
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (timeline.mainSelectionEffect == effect)
            {
                SettingsUtils.SetSetting(timeline.effectsSelection, effect, "Offset", (int)numericUpDown3.Value);
                timeline.UpdateDisplay();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timeline.mainSelectionEffect == effect)
            {
                SettingsUtils.SetSetting(timeline.effectsSelection, effect, "LengthA", (int)numericUpDown2.Value);
                SettingsUtils.SetSetting(timeline.effectsSelection, effect, "LengthB", (int)numericUpDown1.Value);
                UpdateDatas();
                timeline.UpdateDisplay();
            }
        }
    }
}
