namespace LightingProgrammator.Settings
{
    partial class StaticEffectSettings
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.colorInput1 = new LightingProgrammator.Settings.Controls.ColorInput();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Color";
            // 
            // colorInput1
            // 
            this.colorInput1.Color = System.Drawing.Color.Transparent;
            this.colorInput1.Location = new System.Drawing.Point(49, 5);
            this.colorInput1.Name = "colorInput1";
            this.colorInput1.Size = new System.Drawing.Size(24, 24);
            this.colorInput1.TabIndex = 1;
            // 
            // StaticEffectSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.colorInput1);
            this.Controls.Add(this.label1);
            this.Name = "StaticEffectSettings";
            this.Size = new System.Drawing.Size(367, 312);
            this.Load += new System.EventHandler(this.StaticColorSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Controls.ColorInput colorInput1;
    }
}
