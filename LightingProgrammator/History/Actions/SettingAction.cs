using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator.History.Actions
{
    internal class SettingAction : IHistoryAction
    {
        private Type type;
        private string propertyName;
        private List<ILightingEffect> effects;
        private List<object> fromValue;
        private object toValue;

        public SettingAction(Type type, string propertyName, List<ILightingEffect> effects, List<object> fromValue, object toValue)
        {
            this.type = type;
            this.propertyName = propertyName;
            this.effects = effects;
            this.fromValue = fromValue;
            this.toValue = toValue;
        }

        public void Redo()
        {
            PropertyInfo? propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null) effects.ForEach((effect)=>propertyInfo.SetValue(effect, toValue));

            Program.form.timeline.UpdateDisplay();
            Program.form.SettingsPanel?.UpdateDatas();
        }

        public void Undo()
        {
            PropertyInfo? propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
            for (int i = 0; i < effects.Count; i++)
            {
                propertyInfo.SetValue(effects[i],fromValue[i]);
            }

            Program.form.timeline.UpdateDisplay();
            Program.form.SettingsPanel?.UpdateDatas();
        }
    }
}
