using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using FluentAvalonia.Core;
using LightingProgrammatorAvalonia.ViewModels;
using System.Diagnostics;

namespace LightingProgrammatorAvalonia.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = DataContext as MainWindowViewModel ?? new MainWindowViewModel();

            Loaded += (s, e) => InitColorEffectDragnDrop();

        }

        public void InitColorEffectDragnDrop()
        {
            int i = 0;
            _viewModel.Effects.ForEach((effect) =>
            {
                //Debug.WriteLine($"ColorEffectsToolbox[{i}]: {((Button)this.Get<ItemsRepeater>("ColorEffectsToolbox").Children[i].GetLogicalChildren().ElementAt(1)).Name} / {"ColorEffectPicker" + effect.Name}");
                //Debug.WriteLine($"ColorEffectPicker{effect.Name}: {this.Get<ItemsRepeater>("ColorEffectsToolbox").Children[i].Find<Button>($"ColorEffectPicker{effect.Name}").Name}");
                //var dragButton = this.Get<ItemsRepeater>("ColorEffectsToolbox").Children[i].GetControl<Button>("ColorEffectPicker" + effect.Name);

                //dragButton.PointerPressed += effect.DoDrag;
                i++;
            });
        }
        public void ToolboxButtonPointerPressed(object? sender, PointerPressedEventArgs e)
        {

        }
    }
}