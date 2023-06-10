using LightingProgrammator.History;
using LightingProgrammator.Settings;
using NAudio.Utils;
using NAudio.Wave;
using NAudio.WaveFormRenderer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using VLCtoOBSLyrics;
using VLCtoOBSLyrics.SongLighting;
using VLCtoOBSLyrics.SongLighting.LightingEffects;
using VLCtoOBSLyrics.Utils;

namespace LightingProgrammator
{
    public partial class Form1 : Form
    {
        WaveOutEvent outputDevice = new WaveOutEvent();
        AudioFileReader? audioFile;

        Dictionary<Keys, Func<bool>> hotkeys;

        PreviewForm? previewForm;

        public ISettingsPanel? SettingsPanel { get; set; }

        public Form1()
        {
            hotkeys = new Dictionary<Keys, Func<bool>>()
            {
                { Keys.Space, () => TogglePlayPause() },
                { Keys.L, () => PlaySong() },
                { Keys.Control | Keys.O, () => OpenFile() },
                { Keys.Control | Keys.C, () => CopyTL() },
                { Keys.Control | Keys.X, () => CutTL() },
                { Keys.Control | Keys.V, () => PasteTL() },
                { Keys.Control | Keys.Z, () => Undo() },
                { Keys.Control | Keys.Y, () => Redo() },
                { Keys.Delete, () => DeleteTL() },
            };
            InitializeComponent();

            Static.config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./config.json")) ?? Static.config;

            //MessageBox.Show(JsonConvert.SerializeObject(Static.config,Formatting.Indented));

            timeline.PositionChanged += Timeline1_PositionChanged;
            outputDevice.Volume = trackBar2.Value / 100f;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
        private bool OpenFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                TagLib.File file = TagLib.File.Create(openDialog.FileName);
                textBox1.Text = $"{file.Tag.JoinedPerformers}-{file.Tag.Title}";
                Static.currentSongID = textBox1.Text;

                if (outputDevice.PlaybackState != PlaybackState.Stopped) outputDevice.Stop();
                audioFile = new AudioFileReader(openDialog.FileName);
                outputDevice.Init(audioFile);


                outputDevice.Play();
                outputDevice.Stop();

                //button2.Text = "";


                timeline.ResetPeaks(openDialog.FileName);
                timeline.UpdateLightsColor();
                timeline.Refresh();
            }
            return true;
        }

        private void timeline1_Load(object sender, EventArgs e)
        {

        }
        private void Timeline1_PositionChanged(object? sender, EventArgs e)
        {
            if (audioFile != null)
            {
                //if (outputDevice.PlaybackState == PlaybackState.Playing) outputDevice.Play();

                audioFile.Position = (long)Math.Floor(timeline.Position / 1000 * audioFile.WaveFormat.AverageBytesPerSecond);
                lastKnownPosition = (int)timeline.Position;
                lastKnownPositionTime = DateTime.Now;
            }
        }

        public bool forceRender = false;
        private double lastPostion = 0;
        private void UpdateDisplay(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                //double position = audioFile.Position / (double)audioFile.WaveFormat.AverageBytesPerSecond * 1000;
                double position = lastKnownPosition + (outputDevice.PlaybackState == PlaybackState.Playing ? DateTime.Now.Subtract(lastKnownPositionTime).TotalMilliseconds : 0);

                if (position != timeline.Position) timeline.Position = position;
                //timeline1.UpdateLightsColor();
                if ((forceRender || timeline.ForceRender || outputDevice.PlaybackState == PlaybackState.Playing) && (Math.Abs(DateTime.Now.Millisecond - lastPostion) > 32))
                {

                    timeline.render.Refresh();
                    forceRender = false;
                    timeline.ForceRender = false;
                    lastPostion = DateTime.Now.Millisecond;
                    if (previewForm != null)
                    {
                        previewForm.Position = position;
                        previewForm.render.Invalidate();
                    }
                }
                UpdateOBSDisplay(new TimeSpan(0, 0, 0, 0, (int)position));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TogglePlayPause();
        }
        private bool TogglePlayPause()
        {

            if (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                PauseSong();
            }
            else
            {
                PlaySong();
            }
            return true;
        }
        int lastKnownPosition = 0;
        DateTime lastKnownPositionTime = DateTime.Now;
        private bool PlaySong()
        {
            if (audioFile != null)
            {
                button2.Text = "";
                if (outputDevice.PlaybackState == PlaybackState.Stopped)
                {
                    audioFile.Position = 0;
                    if (timeline.TLScroll - timeline.Width / timeline.TLZoom > 0 || timeline.TLScroll < 0)
                        timeline.TLScroll = 0;
                }
                outputDevice.Play();
                if (audioFile != null)
                {
                    //double position = audioFile.Position / (double)audioFile.WaveFormat.AverageBytesPerSecond * 1000;
                    //double position = lastKnownPosition + (outputDevice.PlaybackState == PlaybackState.Playing ? DateTime.Now.Subtract(lastKnownPositionTime).TotalMilliseconds : 0);
                    //lastKnownPosition = (int)position;
                    audioFile.Position = (long)Math.Floor(timeline.Position / 1000 * audioFile.WaveFormat.AverageBytesPerSecond);
                    lastKnownPositionTime = DateTime.Now;
                }
            }
            return true;
        }
        private void PauseSong()
        {
            button2.Text = "";
            outputDevice.Pause();
            if (audioFile != null)
            {
                //double position = audioFile.Position / (double)audioFile.WaveFormat.AverageBytesPerSecond * 1000;
                double position = lastKnownPosition + DateTime.Now.Subtract(lastKnownPositionTime).TotalMilliseconds;
                lastKnownPosition = (int)position;
                lastKnownPositionTime = DateTime.Now;
            }
        }
        private void StopSong()
        {
            if (audioFile != null && outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Stop();
                audioFile.Position = 0;
                button2.Text = "";
                lastKnownPosition = 0;
                lastKnownPositionTime = DateTime.Now;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StopSong();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Static.currentSongID = textBox1.Text;
            timeline.UpdateLightsColor();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (hotkeys.ContainsKey(keyData))
            {
                if (hotkeys[keyData]()) return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (listView1.GetItemAt(e.X, e.Y) != null)
                DoDragDrop(Static.LightingEffectIDs[listView1.GetItemAt(e.X, e.Y).Index], DragDropEffects.Move);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            previewForm = new PreviewForm();
            previewForm.Show();
        }

        private bool CopyTL()
        {
            if (splitContainer1.Panel2.ContainsFocus)
            {
                timeline.CopyTL();
                return true;
            }
            return false;
        }
        private bool CutTL()
        {
            if (splitContainer1.Panel2.ContainsFocus)
            {
                timeline.CutTL();
                return true;
            }
            return false;
        }
        private bool PasteTL()
        {
            if (splitContainer1.Panel2.ContainsFocus)
            {
                timeline.PasteTL();
                return true;
            }
            return false;
        }
        private bool DeleteTL()
        {
            if (splitContainer1.Panel2.ContainsFocus)
            {
                timeline.DeleteTL();
                return true;
            }
            return false;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            outputDevice.Volume = trackBar2.Value / 100f;
        }
        private bool Undo()
        {
            HistoryManager.Undo();
            return true;
        }
        private bool Redo()
        {
            HistoryManager.Redo();
            return true;
        }

        public void UpdateSettingsPanel(ILightingEffect? effect)
        {
            if (effect == null)
            {
                SettingsPanel = null;
                splitContainer2.Panel2.Controls.Clear();
                return;
            }
            if (SettingsPanel == null || SettingsPanel.Effect != effect)
            {
                Control? panel = null;

                Type type = effect.GetType();
                if (type == typeof(StaticEffect)) panel = new StaticEffectSettings((StaticEffect)effect, timeline);
                else if (type == typeof(GradientEffect)) panel = new GradientEffectSettings((GradientEffect)effect, timeline);
                else if (type == typeof(BlinkEffect)) panel = new BlinkEffectSettings((BlinkEffect)effect, timeline);
                else if (type == typeof(BreatheEffect)) panel = new BreatheEffectSettings((BreatheEffect)effect, timeline);


                if (panel != null)
                {
                    panel.Dock = DockStyle.Fill;
                    SettingsPanel = (ISettingsPanel)panel;

                    splitContainer2.Panel2.Controls.Clear();
                    splitContainer2.Panel2.Controls.Add(panel);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            File.WriteAllText("./config.json", JsonConvert.SerializeObject(Static.config));
        }


        OBSWebsocketDotNet.OBSWebsocket obs = new OBSWebsocketDotNet.OBSWebsocket();
        private void button6_Click(object sender, EventArgs e)
        {
            if (obs.IsConnected)
            {
                obs.Disconnect();
                button6.Text = "Connect to OBS";
            }
            else
            {
                obs = new OBSWebsocketDotNet.OBSWebsocket();

                button6.Text = "Disconnect from OBS";

                obs.ConnectAsync("ws://192.168.1.105:4455", "");
            }
        }
        private void UpdateOBSDisplay(TimeSpan position)
        {
            if (obs.IsConnected)
            {
                LightsColor lights = new LightsColor();
                if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
                {
                    lights = Static.config.V2.SongsLighting[Static.currentSongID].GetLightsColor(position);
                }
                obs.SetInputSettings(Static.config.V2.SourceFog, new JObject(new JProperty("color", lights.FogColor.ToInt())));
                obs.SetSourceFilterSettings(Static.config.V2.SourceL1, "Color Correction", new JObject(new JProperty("color_multiply", lights.LightColor1.ToInt())));
                obs.SetSourceFilterSettings(Static.config.V2.SourceL2, "Color Correction", new JObject(new JProperty("color_multiply", lights.LightColor2.ToInt())));
            }
        }
    }
}