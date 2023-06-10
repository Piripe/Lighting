using LightingProgrammator;
using LightingProgrammator.History;
using LightingProgrammator.History.Actions;
using NAudio.Wave;
using NAudio.WaveFormRenderer;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VLCtoOBSLyrics;
using VLCtoOBSLyrics.SongLighting;
using VLCtoOBSLyrics.SongLighting.LightingEffects;

namespace LightingProgrammator
{
    public partial class Timeline : UserControl
    {
        public event EventHandler PositionChanged;

        private double _Position = 0;
        public double Position { get => _Position; set
            {
                _Position = value;
                var ClientPosition = ((float)_Position / (1000.0 / 30) + TLScroll) * TLZoom;
                if (ClientPosition > Width && ClientPosition - (10 * TLZoom) < Width && !(scrollClick || quickScrollClick)) {
                    TLScroll -= (int)(Width / 3 / TLZoom);
                    oldDX -= (int)(Width / 3 / TLZoom);
                }
            } }

        public int TLScroll { get; set; } = 0;
        public float TLZoom { get; set; } = 1;
        public bool ForceRender { get; set; } = false;

        public Timeline()
        {
            InitializeComponent();
            this.MouseWheel += Timeline_MouseWheel;
        }

        public void ResetPeaks(string audioFilePath)
        {
            AudioFileReader audioFile = new AudioFileReader(audioFilePath);
            int width = (int)(audioFile.TotalTime.TotalMilliseconds / (1000f / 30));
            int height = 48;

            var maxPeakProvider = new MaxPeakProvider();
            var rmsPeakProvider = new RmsPeakProvider(200); // e.g. 200
            var samplingPeakProvider = new SamplingPeakProvider(200); // e.g. 200
            var averagePeakProvider = new AveragePeakProvider(0.9f); // e.g. 4

            SolidBrush brush = new SolidBrush(Color.FromArgb(168, 51, 58));

            var myRendererSettings = new StandardWaveFormRendererSettings();
            //var myRendererSettings = new SoundCloudBlockWaveFormSettings(Color.Red,Color.Green,Color.Yellow,Color.Blue);
            myRendererSettings.Width = width;
            myRendererSettings.TopHeight = height / 2;
            myRendererSettings.BottomHeight = height / 2;
            myRendererSettings.BackgroundColor = Color.Transparent;
            myRendererSettings.PixelsPerPeak = 1;
            myRendererSettings.TopPeakPen = new Pen(brush);
            myRendererSettings.BottomPeakPen = new Pen(brush);
            myRendererSettings.TopSpacerPen = new Pen(brush);
            myRendererSettings.DecibelScale = true;


            var renderer = new WaveFormRenderer();
            peaksImage = renderer.Render(audioFile, averagePeakProvider, myRendererSettings);
        }

        public void UpdateLightsColor()
        {
            if (peaksImage != null)
            {
                SongLighting? songLighting = null;
                if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
                {
                    songLighting = Static.config.V2.SongsLighting[Static.currentSongID];
                }

                Bitmap newLightsColor = new Bitmap(peaksImage.Width, 3);

                for (int x = 0; x < peaksImage.Width; x++)
                {
                    LightsColor lightsColor = new LightsColor();
                    if (songLighting != null) lightsColor = songLighting.GetLightsColor(x);
                    newLightsColor.SetPixel(x, 0, lightsColor.FogColor);
                    newLightsColor.SetPixel(x, 1, lightsColor.LightColor1);
                    newLightsColor.SetPixel(x, 2, lightsColor.LightColor2);
                }

                if (lightsColorImage != null) lightsColorImage.Dispose();
                lightsColorImage = newLightsColor;
            }
        }

        Image peaksImage = new Bitmap(1, 256);
        Image? lightsColorImage;
        static SolidBrush pointerBrush = new SolidBrush(Color.FromArgb(36, 101, 195));
        static Pen pointerPen = new Pen(pointerBrush, 1);
        static SolidBrush scrollbarBrush = new SolidBrush(Color.FromArgb(125, 0, 0, 0));
        static SolidBrush separatorBrush = new SolidBrush(Color.FromArgb(10, 10, 10));
        static SolidBrush ghostBrush = new SolidBrush(Color.FromArgb(125, 128, 128, 128));
        static SolidBrush lightGhostBrush = new SolidBrush(Color.FromArgb(64, 128, 128, 128));
        static SolidBrush selectorBrush = new SolidBrush(Color.FromArgb(125, 47, 132, 255));
        static SolidBrush transitionBrush = new SolidBrush(Color.FromArgb(90, 160, 160, 160));
        static SolidBrush lightSeparator = new SolidBrush(Color.FromArgb(200, 10, 10, 10));
        private void render_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;


            if (peaksImage != null)
            {
                g.SmoothingMode = SmoothingMode.None;
                //g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //g.CompositingQuality = CompositingQuality.AssumeLinear;

                if (TLZoom >= 1) g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(peaksImage, (int)(TLScroll * TLZoom), 36 + (int)(Height / 12.0) * 3, (int)(peaksImage.Width * TLZoom), (int)(Height / 4.0) * 3 - 34);

                if (lightsColorImage != null)
                {
                    g.DrawImage(lightsColorImage, (int)(TLScroll * TLZoom), 33, (int)(lightsColorImage.Width * TLZoom), (int)(Height / 12.0) * 3 + 3);

                    g.InterpolationMode = InterpolationMode.Default;
                    g.PixelOffsetMode = PixelOffsetMode.Default;

                    //Point mousePos = GetMouseLocationInFrame();
                    //ILightingEffect? effect = GetLightingEffectOnPoint(mousePos);

                    if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
                    {
                        SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

                        lighting.FogEffects.Concat(lighting.LightEffects1).Concat(lighting.LightEffects2).ToList().ForEach((effect) =>
                        {
                            if (isEffectVisible(effect))
                            {
                                List<ILightingEffect> hoveringEffects = GetLightingEffectsOnPoint(new Point(effect.Frame, effect.Layer));

                                int startY = 33 + effect.Layer * (1 + (int)(Height / 12.0)) + (int)(Height / 24.0);
                                int endY = 33 + effect.Layer * (1 + (int)(Height / 12.0)) + (int)(Height / 12.0);

                                if (hoveringEffects.Count == 2)
                                {
                                    if (hoveringEffects[0] != effect)
                                    {
                                        int startX = FrameToPixel(hoveringEffects[1].Frame);
                                        int endX = FrameToPixel(hoveringEffects[0].Frame + hoveringEffects[0].Length);

                                        /*Point[] path = new Point[]
                                        {
                                            new Point(startX,endY),
                                            new Point(startX,startY),
                                            new Point(endX,endY),
                                            new Point(startX,endY),
                                            new Point(endX,startY),
                                            new Point(endX,endY),
                                        };
                                        g.FillPolygon(transitionBrush, path, FillMode.Winding);*/
                                        g.SmoothingMode = SmoothingMode.HighQuality;
                                        Point[] path = new Point[]
                                        {
                                            new Point(startX,endY),
                                            new Point(startX,startY),
                                            new Point(endX,endY),
                                        };
                                        g.FillPolygon(transitionBrush, path);
                                        path = new Point[]
                                        {
                                            new Point(endX,endY),
                                            new Point(endX,startY),
                                            new Point(startX,endY),
                                        };
                                        g.FillPolygon(transitionBrush, path);
                                        g.SmoothingMode = SmoothingMode.None;

                                        //g.FillRectangle(selectorBrush, , 33 + effect.Layer * (1 + (int)(Height / 12.0)) + (int)(Height / 12.0) - 4, (int)(effect.Length * TLZoom), 4);
                                    }
                                }

                                float startEffectX = FrameToPixelF(effect.Frame);
                                float endEffectX = FrameToPixelF(effect.Frame + effect.Length);
                                //g.DrawRectangle(new Pen(Color.FromArgb(200,10,10,10)), startEffectX, startY, endEffectX-startEffectX-1, endY-startY);

                            }
                        });
                    }


                    if (hoverEffect != null)
                        g.FillRectangle(lightGhostBrush, (int)((hoverEffect.Frame + TLScroll) * TLZoom), 33 + hoverEffect.Layer * (1 + (int)(Height / 12.0)), (int)(hoverEffect.Length * TLZoom), (int)(Height / 12.0));

                }

                g.FillRectangle(separatorBrush, 0, 32, e.ClipRectangle.Width, 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(200, 10, 10, 10)), 0, 32 + (int)(Height / 24.0), e.ClipRectangle.Width, 1);
                g.FillRectangle(separatorBrush, 0, 33 + (int)(Height / 12.0), e.ClipRectangle.Width, 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(200, 10, 10, 10)), 0, 33 + (int)(Height / 24.0) * 3, e.ClipRectangle.Width, 1);
                g.FillRectangle(separatorBrush, 0, 34 + (int)(Height / 12.0) * 2, e.ClipRectangle.Width, 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(200, 10, 10, 10)), 0, 34 + (int)(Height / 24.0) * 5, e.ClipRectangle.Width, 1);
                g.FillRectangle(separatorBrush, 0, 35 + (int)(Height / 12.0) * 3, e.ClipRectangle.Width, 1);

                //g.DrawImage(peaksImage, TLScroll * TLZoom, 36 + (int)(Height / 12.0) * 3, peaksImage.Width * TLZoom, (int)(Height / 4.0) * 3 - 34);

                float xPosition = (int)((float)(_Position / (1000f / 30) + TLScroll) * TLZoom) + 0.5f;

                g.SmoothingMode = SmoothingMode.HighQuality;

                g.FillPolygon(pointerBrush, new PointF[] {
                    new PointF(xPosition-4,14),
                    new PointF(xPosition+4,14),
                    new PointF(xPosition+4,23),
                    new PointF(xPosition,29),
                    new PointF(xPosition-4,23),
                });
                g.DrawLine(pointerPen, xPosition, 16, xPosition, Height);

                g.DrawImage(peaksImage, 0, 0, Width, 12);
                g.FillRectangle(scrollbarBrush, (int)((double)Width / peaksImage.Width * (-TLScroll)), 0, (int)((double)Width / peaksImage.Width * Width / TLZoom), 12);

                float scrollbarPositionX = (int)((double)Width / peaksImage.Width * _Position / (1000f / 30)) + 0.5f;
                g.DrawLine(pointerPen, scrollbarPositionX, 0, scrollbarPositionX, 12);

                effectsSelection.ForEach((effect) =>
                {
                    if (isEffectVisible(effect))
                    {
                        if (moveSelection)
                        {
                            g.FillRectangle(ghostBrush, FrameToPixelF(Math.Min(peaksImage.Width - effect.Length, Math.Max(0, effect.Frame))), 33 + effect.Layer * (1 + (int)(Height / 12.0)), (int)(effect.Length * TLZoom), (int)(Height / 12.0));
                        }
                        else
                        {
                        }
                            g.FillRectangle(selectorBrush, (int)((effect.Frame + TLScroll) * TLZoom), 33 + effect.Layer * (1 + (int)(Height / 12.0)) + (int)(Height / 12.0) - 4, (int)(effect.Length * TLZoom), 4);
                    }
                });
            }

            if (isDragging)
            {
                g.FillRectangle(ghostBrush, (int)((dragFrame + TLScroll) * TLZoom), 33 + dragLayer * (1 + (int)(Height / 12.0)), (int)(30 * TLZoom), (int)(Height / 12.0));
            }
        }

        private void render_Click(object sender, EventArgs e)
        {
            if (MouseButtons != MouseButtons.Middle)
            {
                _Position = PointToClient(MousePosition).X * (1000.0 / 30);
                PositionChanged.Invoke(this, new EventArgs());
                ForceRender = true;
            }
        }

        int oldDX, oldX, oldSelectionX, oldResizeX, selectionOffset, resizeFromFrame, resizeFromLength = 0;
        bool seekClick, scrollClick, quickScrollClick, moveSelection, moveSelectionLayer, resizeStart, resizeEnd = false;
        public List<ILightingEffect> effectsSelection = new List<ILightingEffect>();
        public ILightingEffect? mainSelectionEffect;
        ILightingEffect? hoverEffect;
        ILightingEffect? resizeEffect;
        List<int> moveFromFrame, moveFromLayer = new List<int>();
        Point lastMouseClick;
        private void render_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                render.Focus();
                Point mouseFrame = GetMouseLocationInFrame();
                Point mousePos = PointToClient(MousePosition);

                if (e.Y <= 12) quickScrollClick = true;
                else if (e.Y <= 32) seekClick = true;
                else if (e.Y <= 35 + (int)(Height / 12.0) * 3) {
                    //ILightingEffect? effect = GetLightingEffectOnPoint(mouseFrame);

                    if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        List<ILightingEffect> largeSelectEffects = GetLightingEffectsOnRect(new Rectangle(Math.Min(mouseFrame.X, lastMouseClick.X), Math.Min(mouseFrame.Y, lastMouseClick.Y), Math.Abs(mouseFrame.X - lastMouseClick.X)+1, Math.Abs(mouseFrame.Y - lastMouseClick.Y)+1));

                        effectsSelection.AddRange(largeSelectEffects.FindAll((effect)=>!effectsSelection.Contains(effect)));
                    }
                    else
                    {
                        lastMouseClick = mouseFrame;
                    }


                    if (hoverEffect != null)
                    {
                        if ((ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            if (effectsSelection.Contains(hoverEffect))
                            {
                                effectsSelection.Remove(hoverEffect);
                            }
                            else
                            {
                                effectsSelection.Add(hoverEffect);
                            }
                        }
                        else
                        {
                            if (!effectsSelection.Contains(hoverEffect))
                            {
                                effectsSelection.Clear();
                                effectsSelection.Add(hoverEffect);
                            }
                            mainSelectionEffect = hoverEffect;


                            selectionOffset = mouseFrame.X - hoverEffect.Frame;

                            int precision = 8;
                            precision = (int)Math.Min(precision * Math.Max(precision / 2, TLZoom / 3), (hoverEffect.Length * TLZoom) / 3f);
                            int effectStartX = FrameToPixel(hoverEffect.Frame);
                            int effectEndX = FrameToPixel(hoverEffect.Frame + hoverEffect.Length);
                            if (Math.Abs(effectStartX - mousePos.X) < precision)
                            {
                                oldResizeX = mouseFrame.X;
                                resizeStart = true;
                                resizeEffect = hoverEffect;
                                resizeFromFrame = resizeEffect.Frame;
                                resizeFromLength = resizeEffect.Length;
                            }
                            else if (Math.Abs(effectEndX - mousePos.X) < precision)
                            {
                                oldResizeX = mouseFrame.X;
                                selectionOffset = mouseFrame.X - hoverEffect.Frame - hoverEffect.Length;
                                resizeEnd = true;
                                resizeEffect = hoverEffect;
                                resizeFromFrame = resizeEffect.Frame;
                                resizeFromLength = resizeEffect.Length;
                            }
                            else
                            {
                                oldSelectionX = mouseFrame.X;
                                moveSelection = true;

                                moveFromFrame = new List<int>();
                                moveFromLayer = new List<int>();
                                effectsSelection.ForEach((effect) =>
                                {
                                    moveFromFrame.Add(effect.Frame);
                                    moveFromLayer.Add(effect.Layer);
                                });

                                int firstLayer = effectsSelection[0].Layer;
                                moveSelectionLayer = effectsSelection.Skip(1).All((selectedEffect) => selectedEffect.Layer == firstLayer);
                            }
                        }
                    }
                    else
                    {
                        if ((ModifierKeys & Keys.Control) != Keys.Control)
                        {
                            effectsSelection.Clear();
                            mainSelectionEffect = null;
                        }

                    }
                }

                if (Program.form.SettingsPanel == null || mainSelectionEffect != Program.form.SettingsPanel.Effect)
                {
                    Program.form.UpdateSettingsPanel(mainSelectionEffect);
                }
            }
            if (e.Button == MouseButtons.Middle)
            {
                oldDX = TLScroll;
                oldX = e.Location.X;
                if (e.Y >= 12) scrollClick = true;
                else quickScrollClick = true;
            }
            MouseActionUpdate(e);
        }

        private void render_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                seekClick = false;
                quickScrollClick = false;
                if (moveSelection)
                {
                    List<int> moveToFrame = new List<int>();
                    List<int> moveToLayer = new List<int>();

                    effectsSelection.ForEach((effect) =>
                    {
                        moveToFrame.Add(effect.Frame);
                        moveToLayer.Add(effect.Layer);
                        effect.Frame = Math.Min(peaksImage.Width - effect.Length, Math.Max(0, effect.Frame));
                    });

                    HistoryManager.RegisterAction(new MoveAction(new List<ILightingEffect>(effectsSelection), moveFromFrame, moveToFrame, moveFromLayer, moveToLayer));
                    Static.config.V2.SongsLighting[Static.currentSongID].SortLightingEffects();
                }
                if (moveSelection || resizeStart || resizeEnd) UpdateLightsColor();
                moveSelection = false;
                if ((resizeStart || resizeEnd) && resizeEffect != null)
                {
                    HistoryManager.RegisterAction(new ResizeAction(resizeEffect, resizeFromFrame, resizeEffect.Frame, resizeFromLength, resizeEffect.Length));
                }
                resizeStart = false;
                resizeEnd = false;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                scrollClick = false;
                quickScrollClick = false;
            }
        }

        private void render_MouseMove(object sender, MouseEventArgs e)
        {
            bool hasCursor = false;
            MouseActionUpdate(e);
            if (moveSelection)
            {
                Point mousePos = GetMouseLocationInFrame(true, false);
                int allignedMouseX = mousePos.X;
                if (mainSelectionEffect != null)
                    allignedMouseX = GetAllignedMouseFrame(mainSelectionEffect.Length, selectionOffset, effectsSelection);
                int frameDiff = allignedMouseX - oldSelectionX;
                oldSelectionX = allignedMouseX;

                effectsSelection.ForEach((effect) =>
                {
                    effect.Frame += frameDiff;
                    if (moveSelectionLayer) effect.Layer = mousePos.Y;
                });
            }
            else if (resizeStart)
            {
                if (resizeEffect != null)
                {
                    int allignedMouseX = GetAllignedMouseFrame(0, selectionOffset, new ILightingEffect[] { resizeEffect });

                    int frameDiff = allignedMouseX - oldResizeX;
                    oldResizeX = allignedMouseX;

                    resizeEffect.Frame += frameDiff;
                    resizeEffect.Length -= frameDiff;
                }
            }
            else if (resizeEnd)
            {
                if (resizeEffect != null)
                {
                    int allignedMouseX = GetAllignedMouseFrame(0, selectionOffset, new ILightingEffect[] { resizeEffect });

                    int frameDiff = allignedMouseX - oldResizeX;
                    oldResizeX = allignedMouseX;

                    resizeEffect.Length += frameDiff;
                }
            }
            else
            {
                Point mouseFrame = GetMouseLocationInFrame();
                Point mousePos = PointToClient(MousePosition);
                ILightingEffect? effect = GetLightingEffectOnPoint(mouseFrame);

                if (effect != null)
                {
                    ILightingEffect? resizeEffect = GetLightingEffectOnPoint(mouseFrame, true);
                    if (resizeEffect == null) return;
                    int precision = 8;
                    precision = (int)Math.Min(precision * Math.Max(precision / 2, TLZoom / 3), (resizeEffect.Length * TLZoom) / 3f);
                    int effectStartX = FrameToPixel(resizeEffect.Frame);
                    int effectEndX = FrameToPixel(resizeEffect.Frame + resizeEffect.Length);
                    if ((ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        hoverEffect = effect;
                    } else { 
                        if (Math.Abs(effectStartX - mousePos.X) < precision)
                        {
                            Cursor = Cursors.PanWest;
                            hoverEffect = resizeEffect;
                        }
                        else if (Math.Abs(effectEndX - mousePos.X) < precision)
                        {
                            Cursor = Cursors.PanEast;
                            hoverEffect = resizeEffect;
                        }
                        else {
                            Cursor = Cursors.SizeAll;
                            hoverEffect = effect;
                        }
                        hasCursor = true;
                    }
                }
                else
                {
                    hoverEffect = null;
                }
            }
            if (!hasCursor)
            {
                Cursor = Cursors.Default;
            }
        }
        private void MouseActionUpdate(MouseEventArgs e)
        {
            if (seekClick)
            {
                _Position = Math.Max(0, (e.X - TLScroll * TLZoom) / TLZoom * (1000f / 30));
                PositionChanged.Invoke(this, new EventArgs());
            }
            else if (scrollClick)
            {
                TLScroll = oldDX + (int)((e.X - oldX) / TLZoom);
            }
            else if (quickScrollClick && peaksImage != null)
            {
                TLScroll = 0 - (int)Math.Floor((double)peaksImage.Width / Width * e.X - (double)Width / peaksImage.Width * peaksImage.Width / 2 / TLZoom);
            }
            ForceRender = true;
        }

        private void Timeline_MouseWheel(object? sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift) {
                TLScroll += (int)(e.Delta / TLZoom);
            }
            else
            {
                float oldZoom = TLZoom;
                TLZoom = (float)Math.Max(0.1, Math.Min(16, TLZoom + (e.Delta * Math.Max(1, TLZoom)) / 1000f));

                TLScroll += (int)(e.X / TLZoom - e.X / oldZoom);
                if (e.Delta < 0 && TLZoom < 1 && TLScroll > 0 && peaksImage != null) TLScroll = (int)Math.Max(TLScroll, (Width - (peaksImage.Width * TLZoom)) / TLZoom / 2);
            }
            ForceRender = true;
        }


        bool isDragging = false;
        string dragData = "";
        int dragFrame = 0;
        int dragLayer = 0;


        private void Timeline_DragDrop(object sender, DragEventArgs e)
        {
            isDragging = false;

            if (!Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
            {
                Static.config.V2.SongsLighting.Add(Static.currentSongID, new SongLighting(SongLightingType.Effects, Color.Black, Color.Black, Color.Black, null, null, null));
            }

            Type effectType = Static.LightingEffectTypes[Static.LightingEffectIDs.IndexOf(dragData)];
            ILightingEffect effect = (ILightingEffect)(Activator.CreateInstance(effectType) ?? new StaticEffect());

            effect.Frame = dragFrame;
            effect.Layer = dragLayer;

            SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

            if (dragLayer == 0) lighting.FogEffects.Add(effect);
            else if (dragLayer == 1) lighting.LightEffects1.Add(effect);
            else if (dragLayer == 2) lighting.LightEffects2.Add(effect);

            HistoryManager.RegisterAction(new DropAction(effect, dragFrame, dragLayer));

            UpdateLightsColor();
            ForceRender = true;
        }

        private void Timeline_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null)
                if (e.Data.GetDataPresent(DataFormats.Text))
                {
                    string data = e.Data.GetData(DataFormats.Text).ToString() ?? "";

                    if (Static.LightingEffectIDs.Contains(data))
                    {
                        dragData = data;
                        isDragging = true;
                        e.Effect = DragDropEffects.Move;
                    }

                }
        }

        private void Timeline_DragLeave(object sender, EventArgs e)
        {
            isDragging = false;
            dragData = "";
        }

        private void Timeline_DragOver(object sender, DragEventArgs e)
        {
            if (isDragging)
            {
                Point mouseLocation = PointToClient(new Point(e.X, e.Y));
                dragLayer = 0;
                if (mouseLocation.Y > 33 + (int)(Height / 12.0)) dragLayer++;
                if (mouseLocation.Y > 34 + (int)(Height / 12.0) * 2) dragLayer++;

                float pointerPosition = (float)(_Position / (1000.0f / 30) + TLScroll) * TLZoom;

                /*if (Math.Abs(mouseLocation.X - pointerPosition) < 10) dragFrame = (int)(_Position / (1000.0f / 30));
                else
                {
                    dragFrame = (int)Math.Min(peaksImage.Width, Math.Max(0, mouseLocation.X / TLZoom - TLScroll));
                }*/
                dragFrame = GetAllignedMouseFrame(30);
                ForceRender = true;
            }
        }

        private Point GetMouseLocationInFrame(bool ignoreY = false, bool minMax = true)
        {
            if (peaksImage != null)
            {
                Point mouseLocation = PointToClient(MousePosition);
                if (mouseLocation.Y > 33 && mouseLocation.Y <= 35 + (int)(Height / 12.0) * 3 || ignoreY)
                {
                    int frame = (int)(mouseLocation.X / TLZoom - TLScroll);
                    if (minMax)
                        frame = Math.Min(peaksImage.Width, Math.Max(0, frame));

                    int hoverLayer = 0;
                    if (mouseLocation.Y > 33 + (int)(Height / 12.0)) hoverLayer++;
                    if (mouseLocation.Y > 34 + (int)(Height / 12.0) * 2) hoverLayer++;

                    return new Point(frame, hoverLayer);
                }
            }
            return new Point(0, -1);
        }
        private int GetAllignedMouseFrame(int allignWidth = 30, int offset = 0, IEnumerable<ILightingEffect>? ignoreEffects = null, int precision = 8, bool ignoreY = false, bool minMax = true)
        {
            if (peaksImage != null)
            {
                precision = TLZoom < 8 ? (int)(precision * Math.Max(1, TLZoom / 3)) : 0;
                Point mouseLocation = PointToClient(MousePosition);

                int hoverLayer = 0;
                if (mouseLocation.Y > 33 + (int)(Height / 12.0)) hoverLayer++;
                if (mouseLocation.Y > 34 + (int)(Height / 12.0) * 2) hoverLayer++;

                int pointerFrame = (int)(_Position / (1000f / 30));
                float pointerPosition = (float)(_Position / (1000f / 30) + TLScroll) * TLZoom;

                if (mouseLocation.X - offset * TLZoom - pointerPosition < precision && mouseLocation.X - offset * TLZoom - pointerPosition >= 0) return pointerFrame + offset;
                else if (mouseLocation.X + (allignWidth + 1 - offset) * TLZoom - pointerPosition > -precision && mouseLocation.X + (allignWidth + 1 - offset) * TLZoom - pointerPosition <= 0) return pointerFrame - allignWidth + 1 + offset;
                else {
                    SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

                    List<ILightingEffect> combinedEffects = lighting.FogEffects.Concat(lighting.LightEffects1).Concat(lighting.LightEffects2).ToList();

                    if (ignoreEffects != null)
                    {
                        combinedEffects.RemoveAll(ignoreEffects.Contains);
                    }

                    combinedEffects.Sort((a, b) => (a.Layer == hoverLayer ? -1 : a.Layer) - (b.Layer == hoverLayer ? -1 : b.Layer));

                    ILightingEffect? closestEffect = combinedEffects.Find((effect) =>
                    (Math.Abs(FrameToPixel(effect.Frame) - mouseLocation.X + offset * TLZoom) < precision)
                    || (Math.Abs(FrameToPixel(effect.Frame) - mouseLocation.X - (allignWidth + 1 - offset) * TLZoom) < precision)
                    || (Math.Abs(FrameToPixel(effect.Frame + effect.Length - 1) - mouseLocation.X + offset * TLZoom) < precision)
                    || (Math.Abs(FrameToPixel(effect.Frame + effect.Length - 1) - mouseLocation.X - (allignWidth + 1 - offset) * TLZoom) < precision));

                    if (closestEffect != null)
                    {
                        float diff = FrameToPixel(closestEffect.Frame) - mouseLocation.X + offset * TLZoom;

                        if (Math.Abs(diff) < precision) return closestEffect.Frame + offset;
                        diff -= (allignWidth + 1) * TLZoom;
                        if (Math.Abs(diff) < precision) return closestEffect.Frame - allignWidth + offset;
                        diff = FrameToPixel(closestEffect.Frame + closestEffect.Length - 1) - mouseLocation.X + offset * TLZoom;
                        if (Math.Abs(diff) < precision) return closestEffect.Frame + closestEffect.Length + offset;
                        diff -= (allignWidth + 1) * TLZoom;
                        if (Math.Abs(diff) < precision) return closestEffect.Frame + closestEffect.Length - allignWidth + offset;
                    }
                    return Math.Min(peaksImage.Width, Math.Max(0, PixelToFrame(mouseLocation.X)));
                }
            }
            return 0;
        }

        private ILightingEffect? GetLightingEffectOnPoint(Point point, bool closestSide = false)
        {
            if (point.Y == -1) return null;
            if (peaksImage != null && lightsColorImage != null && Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
            {
                SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

                List<ILightingEffect> hoveringEffects = (point.Y == 0 ? lighting.FogEffects : point.Y == 1 ? lighting.LightEffects1 : lighting.LightEffects2).FindAll((effect) => effect.Frame <= point.X && effect.Frame + effect.Length > point.X);


                if (hoveringEffects.Count > 0)
                {
                    if (closestSide)
                    {
                        Point MousePos = PointToClient(MousePosition);
                        hoveringEffects.Sort((a, b) => Math.Min(Math.Abs(FrameToPixel(a.Frame) - MousePos.X), Math.Abs(FrameToPixel(a.Frame + a.Length) - MousePos.X)) - Math.Min(Math.Abs(FrameToPixel(b.Frame) - MousePos.X), Math.Abs(FrameToPixel(b.Frame + b.Length) - MousePos.X)));

                        return hoveringEffects[0];
                    }
                    else
                    {
                        hoveringEffects.Sort((a, b) => a.Frame.CompareTo(b.Frame));
                        ILightingEffect effect = hoveringEffects.Last();

                        return effect;
                    }
                }

            }
            return null;
        }
        private List<ILightingEffect> GetLightingEffectsOnPoint(Point point)
        {
            List<ILightingEffect> hoveringEffects = new List<ILightingEffect>();
            if (point.Y == -1) return hoveringEffects;
            if (peaksImage != null && lightsColorImage != null && Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
            {
                SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

                hoveringEffects = (point.Y == 0 ? lighting.FogEffects : point.Y == 1 ? lighting.LightEffects1 : lighting.LightEffects2).FindAll((effect) => !effectsSelection.Contains(effect)).Concat(effectsSelection).ToList().FindAll((effect) => effect.Frame <= point.X && effect.Frame + effect.Length > point.X && effect.Layer == point.Y);

                hoveringEffects.Sort((a, b) => a.Frame.CompareTo(b.Frame));

            }
            return hoveringEffects;
        }
        private List<ILightingEffect> GetLightingEffectsOnRect(Rectangle rect)
        {
            List<ILightingEffect> hoveringEffects = new List<ILightingEffect>();
            if (rect.Bottom == -1) return hoveringEffects;
            if (peaksImage != null && lightsColorImage != null && Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
            {
                SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

                hoveringEffects = lighting.FogEffects.Concat(lighting.LightEffects1).Concat(lighting.LightEffects2).ToList().FindAll((effect) => rect.Contains(effect.Frame,effect.Layer) || rect.Contains(effect.Frame + effect.Length, effect.Layer));

                hoveringEffects.Sort((a, b) => a.Frame.CompareTo(b.Frame));

            }
            return hoveringEffects;
        }

        private int FrameToPixel(int frame) => (int)((frame + TLScroll) * TLZoom + 0.5f);
        private float FrameToPixelF(int frame) => ((frame + TLScroll) * TLZoom + 0.5f);
        private int PixelToFrame(int pixel) => (int)(pixel / TLZoom - TLScroll);
        private bool isEffectVisible(ILightingEffect effect) => FrameToPixel(effect.Frame) < render.Width || FrameToPixel(effect.Frame + effect.Length) > 0;
    
        public void UpdateDisplay()
        {
            UpdateLightsColor();
            ForceRender = true;
        }

        public void CopyTL()
        {
            Clipboard.SetData("ILightingEffects", effectsSelection);
        }
        public void CutTL()
        {
            CopyTL();
            DeleteTL();
        }
        public void PasteTL()
        {
            if (Clipboard.ContainsData("ILightingEffects")) { 
            List<ILightingEffect>? effects = Clipboard.GetData("ILightingEffects") as List<ILightingEffect>;

                if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID) && effects != null)
                {
                    if (effects.Count > 0)
                    {
                        SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

                        int minFrame = effects[0].Frame;
                        effects.Skip(1).ToList().ForEach((effect) => minFrame = Math.Min(minFrame, effect.Frame));

                        Point mousePos = GetMouseLocationInFrame(true);

                        int firstLayer = effects[0].Layer;
                        bool oneLayerPaste = effects.Skip(1).All((selectedEffect) => selectedEffect.Layer == firstLayer);

                        effects = effects.Select((effect) =>
                        {
                            effect.Frame = effect.Frame - minFrame + mousePos.X;
                            if (oneLayerPaste) effect.Layer = mousePos.Y;
                            return effect;
                        }).ToList();

                        lighting.FogEffects.AddRange(effects);
                        lighting.SortLightingEffects();
                        effectsSelection.Clear();
                        effectsSelection.AddRange(effects);
                        CopyTL();
                        UpdateLightsColor();
                        ForceRender = true;

                        HistoryManager.RegisterAction(new PasteAction(new List<ILightingEffect>(effects)));
                    }
                }
            }
        }
        public void DeleteTL()
        {
            HistoryManager.RegisterAction(new DeleteAction(new List<ILightingEffect>(effectsSelection)));
            DeleteEffects(effectsSelection);
        }

        public void DeleteEffect(ILightingEffect effect)
        {
            if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
            {
                SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

                lighting.FogEffects.Remove(effect);
                lighting.LightEffects1.Remove(effect);
                lighting.LightEffects2.Remove(effect);
                effectsSelection.Remove(effect);
                if (mainSelectionEffect == effect) mainSelectionEffect = null;
                if (hoverEffect == effect) hoverEffect = null;

                UpdateLightsColor();
                ForceRender = true;
            }
        }
        public void DeleteEffects(IEnumerable<ILightingEffect> effects)
        {
            Debug.WriteLine("Deleting effects...");
            if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID))
            {
                SongLighting lighting = Static.config.V2.SongsLighting[Static.currentSongID];

                Debug.WriteLine("Lightings lenght: " + lighting.FogEffects.Count + lighting.LightEffects1.Count + lighting.LightEffects2.Count);
                lighting.FogEffects.RemoveAll(effects.Contains);
                lighting.LightEffects1.RemoveAll(effects.Contains);
                lighting.LightEffects2.RemoveAll(effects.Contains);
                if (effects.Contains(mainSelectionEffect)) mainSelectionEffect = null;
                if (effects.Contains(hoverEffect)) hoverEffect = null;
                effectsSelection.RemoveAll(effects.Contains);
                UpdateLightsColor();
                ForceRender = true;
                Debug.WriteLine("Effects deleted.");
                Debug.WriteLine("Lightings lenght: " + lighting.FogEffects.Count + lighting.LightEffects1.Count + lighting.LightEffects2.Count);
            }
        }
    }
}
