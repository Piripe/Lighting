using Avalonia.Platform;
using Avalonia.Media.Imaging;
using Avalonia;
using LightingProgrammatorAvalonia.Models;
using System;
using System.Collections.Generic;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammatorAvalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";
        public string CurrentSong => "";
        public List<ColorLayerEffectItem> Effects { get; set; } = new()
        {
            new("Static", new Bitmap(AssetLoader.Open(new Uri("avares://LightingProgrammatorAvalonia/Assets/Images/ColorEffectsToolbox/static.png"))),new StaticEffect()),
            new("Gradient", new Bitmap(AssetLoader.Open(new Uri("avares://LightingProgrammatorAvalonia/Assets/Images/ColorEffectsToolbox/gradient.png"))),new GradientEffect()),
            new("Blink", new Bitmap(AssetLoader.Open(new Uri("avares://LightingProgrammatorAvalonia/Assets/Images/ColorEffectsToolbox/blink.png"))),new BlinkEffect()),
            new("Breathe", new Bitmap(AssetLoader.Open(new Uri("avares://LightingProgrammatorAvalonia/Assets/Images/ColorEffectsToolbox/breathe.png"))),new BreatheEffect()),
        };
    }
}