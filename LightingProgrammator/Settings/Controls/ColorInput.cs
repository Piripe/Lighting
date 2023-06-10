using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightingProgrammator.Settings.Controls
{
    public partial class ColorInput : UserControl
    {
        public event EventHandler? ColorChanged;
        public Color Color { get => button1.BackColor; set => button1.BackColor = value; }
        public ColorInput()
        {
            InitializeComponent();
        }

        private static int[]? customColors;

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = Color;
            dialog.FullOpen = true;
            dialog.SolidColorOnly = false;
            if (customColors != null) dialog.CustomColors = customColors;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Color = dialog.Color;
                if (ColorChanged != null) ColorChanged(this, EventArgs.Empty);
            }
            customColors = dialog.CustomColors;
        }
    }
}
