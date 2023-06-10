namespace LightingProgrammator
{
    partial class PreviewForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.render = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.render)).BeginInit();
            this.SuspendLayout();
            // 
            // render
            // 
            this.render.Dock = System.Windows.Forms.DockStyle.Fill;
            this.render.Location = new System.Drawing.Point(0, 0);
            this.render.Name = "render";
            this.render.Size = new System.Drawing.Size(800, 450);
            this.render.TabIndex = 0;
            this.render.TabStop = false;
            this.render.Paint += new System.Windows.Forms.PaintEventHandler(this.render_Paint);
            // 
            // PreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.render);
            this.DoubleBuffered = true;
            this.Name = "PreviewForm";
            this.Text = "PreviewForm";
            ((System.ComponentModel.ISupportInitialize)(this.render)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal PictureBox render;
    }
}