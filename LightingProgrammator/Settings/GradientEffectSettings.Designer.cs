namespace LightingProgrammator.Settings
{
    partial class GradientEffectSettings
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
            this.colorInput2 = new LightingProgrammator.Settings.Controls.ColorInput();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "ColorA";
            // 
            // colorInput1
            // 
            this.colorInput1.Color = System.Drawing.Color.Transparent;
            this.colorInput1.Location = new System.Drawing.Point(49, 5);
            this.colorInput1.Name = "colorInput1";
            this.colorInput1.Size = new System.Drawing.Size(24, 24);
            this.colorInput1.TabIndex = 1;
            // 
            // colorInput2
            // 
            this.colorInput2.Color = System.Drawing.Color.Transparent;
            this.colorInput2.Location = new System.Drawing.Point(49, 35);
            this.colorInput2.Name = "colorInput2";
            this.colorInput2.Size = new System.Drawing.Size(24, 24);
            this.colorInput2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "ColorB";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Segoe Fluent Icons", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button2.Location = new System.Drawing.Point(79, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 24);
            this.button2.TabIndex = 5;
            this.button2.Text = "";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // GradientEffectSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.colorInput2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.colorInput1);
            this.Controls.Add(this.label1);
            this.Name = "GradientEffectSettings";
            this.Size = new System.Drawing.Size(367, 312);
            this.Load += new System.EventHandler(this.StaticColorSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Controls.ColorInput colorInput1;
        private Controls.ColorInput colorInput2;
        private Label label2;
        private Button button2;
    }
}
