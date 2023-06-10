namespace LightingProgrammator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ListViewItem listViewItem1 = new ListViewItem("Solid", "solidEffect.png");
            ListViewItem listViewItem2 = new ListViewItem("Gradient", "gradientEffect.png");
            ListViewItem listViewItem3 = new ListViewItem("Blink", "blinkEffect.png");
            ListViewItem listViewItem4 = new ListViewItem("Breathe", "breatheEffect.png");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            timeline = new Timeline();
            updateDisplayTimer = new System.Windows.Forms.Timer(components);
            splitContainer1 = new SplitContainer();
            button6 = new Button();
            button5 = new Button();
            trackBar2 = new TrackBar();
            button4 = new Button();
            splitContainer2 = new SplitContainer();
            listView1 = new ListView();
            effectsImages = new ImageList(components);
            label1 = new Label();
            textBox1 = new TextBox();
            button3 = new Button();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Open";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // timeline
            // 
            timeline.AllowDrop = true;
            timeline.BackColor = Color.FromArgb(40, 40, 40);
            timeline.Dock = DockStyle.Fill;
            timeline.ForceRender = false;
            timeline.Location = new Point(0, 0);
            timeline.Name = "timeline";
            timeline.Position = 0D;
            timeline.Size = new Size(800, 250);
            timeline.TabIndex = 2;
            timeline.TLScroll = 0;
            timeline.TLZoom = 1F;
            timeline.Load += timeline1_Load;
            // 
            // updateDisplayTimer
            // 
            updateDisplayTimer.Enabled = true;
            updateDisplayTimer.Interval = 1;
            updateDisplayTimer.Tick += UpdateDisplay;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(button6);
            splitContainer1.Panel1.Controls.Add(button5);
            splitContainer1.Panel1.Controls.Add(trackBar2);
            splitContainer1.Panel1.Controls.Add(button4);
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(textBox1);
            splitContainer1.Panel1.Controls.Add(button3);
            splitContainer1.Panel1.Controls.Add(button2);
            splitContainer1.Panel1.Controls.Add(button1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(timeline);
            splitContainer1.Size = new Size(800, 507);
            splitContainer1.SplitterDistance = 253;
            splitContainer1.TabIndex = 3;
            // 
            // button6
            // 
            button6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button6.Location = new Point(549, 3);
            button6.Name = "button6";
            button6.Size = new Size(137, 23);
            button6.TabIndex = 10;
            button6.Text = "Connect to OBS";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.Location = new Point(345, 4);
            button5.Name = "button5";
            button5.Size = new Size(87, 23);
            button5.TabIndex = 9;
            button5.Text = "Save Config";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // trackBar2
            // 
            trackBar2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            trackBar2.Location = new Point(88, 209);
            trackBar2.Maximum = 100;
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(104, 45);
            trackBar2.TabIndex = 8;
            trackBar2.TickFrequency = 10;
            trackBar2.Value = 10;
            trackBar2.Scroll += trackBar2_Scroll;
            // 
            // button4
            // 
            button4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button4.Location = new Point(692, 3);
            button4.Name = "button4";
            button4.Size = new Size(96, 23);
            button4.TabIndex = 7;
            button4.Text = "Open Preview";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // splitContainer2
            // 
            splitContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer2.Location = new Point(0, 32);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(listView1);
            splitContainer2.Size = new Size(800, 171);
            splitContainer2.SplitterDistance = 266;
            splitContainer2.TabIndex = 6;
            // 
            // listView1
            // 
            listView1.BackColor = Color.FromArgb(40, 40, 40);
            listView1.BorderStyle = BorderStyle.None;
            listView1.Dock = DockStyle.Fill;
            listView1.ForeColor = SystemColors.Control;
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView1.HideSelection = true;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listView1.Items.AddRange(new ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4 });
            listView1.LargeImageList = effectsImages;
            listView1.Location = new Point(0, 0);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.ShowGroups = false;
            listView1.Size = new Size(266, 171);
            listView1.SmallImageList = effectsImages;
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.MouseDown += listView1_MouseDown;
            // 
            // effectsImages
            // 
            effectsImages.ColorDepth = ColorDepth.Depth24Bit;
            effectsImages.ImageStream = (ImageListStreamer)resources.GetObject("effectsImages.ImageStream");
            effectsImages.TransparentColor = Color.Transparent;
            effectsImages.Images.SetKeyName(0, "solidEffect.png");
            effectsImages.Images.SetKeyName(1, "gradientEffect.png");
            effectsImages.Images.SetKeyName(2, "blinkEffect.png");
            effectsImages.Images.SetKeyName(3, "breatheEffect.png");
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(84, 7);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 5;
            label1.Text = "Song ID";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(138, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(201, 23);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.Leave += textBox1_Leave;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button3.Font = new Font("Segoe Fluent Icons", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button3.Location = new Point(50, 209);
            button3.Name = "button3";
            button3.Size = new Size(32, 32);
            button3.TabIndex = 3;
            button3.Text = "";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button2.Font = new Font("Segoe Fluent Icons", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(12, 209);
            button2.Name = "button2";
            button2.Size = new Size(32, 32);
            button2.TabIndex = 2;
            button2.Text = "";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(44, 44, 44);
            ClientSize = new Size(800, 507);
            Controls.Add(splitContainer1);
            ForeColor = SystemColors.ControlText;
            Name = "Form1";
            Text = "Lighting Programmator";
            Load += Form1_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private System.Windows.Forms.Timer updateDisplayTimer;
        private SplitContainer splitContainer1;
        private Button button3;
        private Button button2;
        private Label label1;
        private TextBox textBox1;
        private TrackBar trackBar1;
        private Label label2;
        private SplitContainer splitContainer2;
        private ListView listView1;
        private Button button4;
        private TrackBar trackBar2;
        internal Timeline timeline;
        private Settings.Controls.ColorInput colorInput1;
        private ImageList effectsImages;
        private Button button5;
        private Button button6;
    }
}