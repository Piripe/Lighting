namespace LightingProgrammator
{
    partial class Timeline
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
            this.render = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.render)).BeginInit();
            this.SuspendLayout();
            // 
            // render
            // 
            this.render.Dock = System.Windows.Forms.DockStyle.Fill;
            this.render.Location = new System.Drawing.Point(0, 0);
            this.render.Name = "render";
            this.render.Size = new System.Drawing.Size(649, 150);
            this.render.TabIndex = 0;
            this.render.TabStop = false;
            this.render.Paint += new System.Windows.Forms.PaintEventHandler(this.render_Paint);
            this.render.MouseDown += new System.Windows.Forms.MouseEventHandler(this.render_MouseDown);
            this.render.MouseMove += new System.Windows.Forms.MouseEventHandler(this.render_MouseMove);
            this.render.MouseUp += new System.Windows.Forms.MouseEventHandler(this.render_MouseUp);
            // 
            // Timeline
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.render);
            this.Name = "Timeline";
            this.Size = new System.Drawing.Size(649, 150);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Timeline_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Timeline_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.Timeline_DragOver);
            this.DragLeave += new System.EventHandler(this.Timeline_DragLeave);
            ((System.ComponentModel.ISupportInitialize)(this.render)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal PictureBox render;
    }
}
