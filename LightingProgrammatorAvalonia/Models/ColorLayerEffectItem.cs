using Avalonia.Input;
using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammatorAvalonia.Models
{
    public class ColorLayerEffectItem
    {
        public string Name { get; set; }
        public Bitmap Icon { get; set; }
        public ILightingEffect DragElement { get; set; }

        public ColorLayerEffectItem(string name, Bitmap icon, ILightingEffect dragElement)
        { 
            Name = name; 
            Icon = icon;
            DragElement = dragElement;
        }

        public void DoDrag (object? sender, PointerPressedEventArgs e)
        {
            var dragData = new DataObject();
            dragData.Set(Static.DragDropFormats.ColorEffect, DragElement);

            DragDrop.DoDragDrop(e, dragData, DragDropEffects.Move);
        }
    }
}
