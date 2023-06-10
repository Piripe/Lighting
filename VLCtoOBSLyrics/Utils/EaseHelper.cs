using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting;

namespace VLCtoOBSLyrics.Utils
{
    internal static class EaseHelper
    {
        internal static double CubicIn(double x) => x * x * x;
        internal static double CubicOut(double x) => 1 - Math.Pow(1 - x, 3);
        internal static double CubicInOut(double x) => x < 0.5 ? 4 * x * x * x : 1 - Math.Pow(-2 * x + 2, 3) / 2;

        internal static float GetEaseValue<T>(int originKeyframe, KeyframeData<T> originKeyframeData, int nextKeyframe, KeyframeData<T> nextKeyframeData, int frame) where T : IFormattable
        {
            double ratio = (double)(frame - originKeyframe) / (nextKeyframe - originKeyframe);

            ratio = nextKeyframeData.Ease switch
            {
                Ease.Linear     => ratio,
                Ease.CubicIn    => CubicIn(ratio),
                Ease.CubicOut   => CubicIn(ratio),
                Ease.CubicInOut => CubicIn(ratio),
                _               => 0,
            };
            return float.Parse(nextKeyframeData.Value.ToString()??"0") + (float.Parse(nextKeyframeData.Value.ToString()??"0") - float.Parse(originKeyframeData.Value.ToString()??"0")) * (float)ratio;
        }
    }
}