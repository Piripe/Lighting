using LightingProgrammator.History;
using LightingProgrammator.History.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator.Utils
{
    internal static class SettingsUtils
    {
        public static void SetSetting(List<ILightingEffect> effects, ILightingEffect mainEffect, string propertyName, object value)
        {
            Type type = mainEffect.GetType();
            PropertyInfo? propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
                if (effects.All((effect)=>effect.GetType() == type))
                {
                    List<object> fromValue = new List<object>();
                    effects.ForEach((effect) =>
                    {
                        fromValue.Add(propertyInfo.GetValue(effect)??0);
                        propertyInfo.SetValue(effect, value);
                    });

                    HistoryManager.RegisterAction(new SettingAction(type, propertyName, new List<ILightingEffect>(effects),fromValue,value));
                }
                else
                {
                    object fromValue = propertyInfo.GetValue(mainEffect) ?? 0;
                    propertyInfo.SetValue(mainEffect, value);
                    HistoryManager.RegisterAction(new SettingAction(type, propertyName, new ILightingEffect[] { mainEffect }.ToList(),new object[] { fromValue }.ToList(), value));
                }
        }
    }
}
