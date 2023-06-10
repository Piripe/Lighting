using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLCtoOBSLyrics.SongLighting;

namespace LightingProgrammator
{
    public partial class PreviewForm : Form
    {

        public PreviewForm()
        {
            InitializeComponent();
        }

        public double Position { get; set; }


        private Image Light1 = (Image)(Properties.Resources.ResourceManager.GetObject("light1") ?? new Bitmap(0, 0));
        private Image Light2 = (Image)(Properties.Resources.ResourceManager.GetObject("light2") ?? new Bitmap(0, 0));
        private void render_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (Static.config.V2.SongsLighting.ContainsKey(Static.currentSongID)) {
                LightsColor lights = Static.config.V2.SongsLighting[Static.currentSongID].GetLightsColor(new TimeSpan(0, 0, 0, 0, (int)Position));


                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                g.Clear(lights.FogColor);

                float widthScale = render.Width / 960.0f;
                float heightScale = render.Height / 540.0f;
                float scale = Math.Max(widthScale, heightScale);
                int finalWidth = (int)(960 * scale);
                int finalHeight = (int)(540 * scale);
                int xOffset = (render.Width - finalWidth) / 2;
                int yOffset = (render.Height - finalHeight) / 2;

                //Debug.WriteLine($"xOffset({xOffset}), yOffset({yOffset}), finalWidth({finalWidth}), finalHeight({finalHeight})");
                using (Bitmap lower = new Bitmap(Light1))
                using (Bitmap upper = new Bitmap(Light2))
                using (Bitmap output = new Bitmap(lower.Width, lower.Height))
                {
                    int width = lower.Width;
                    int height = lower.Height;
                    var rect = new Rectangle(0, 0, width, height);

                    BitmapData lowerData = lower.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    BitmapData upperData = upper.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    BitmapData outputData = output.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                    unsafe
                    {
                        byte* lowerPointer = (byte*)lowerData.Scan0;
                        byte* upperPointer = (byte*)upperData.Scan0;
                        byte* outputPointer = (byte*)outputData.Scan0;

                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {

                                outputPointer[0] = (byte)Math.Min(255,lowerPointer[0] * lights.LightColor1.B / 255 + upperPointer[0] * lights.LightColor2.B / 255);
                                outputPointer[1] = (byte)Math.Min(255,lowerPointer[1] * lights.LightColor1.G / 255 + upperPointer[1] * lights.LightColor2.G / 255);
                                outputPointer[2] = (byte)Math.Min(255,lowerPointer[2] * lights.LightColor1.R / 255 + upperPointer[2] * lights.LightColor2.R / 255);
                                outputPointer[3] = (byte)Math.Min(255,lowerPointer[3] * lights.LightColor1.A / 255 + upperPointer[3] * lights.LightColor2.A / 255);

                                // Moving the pointers by 3 bytes per pixel
                                lowerPointer += 4;
                                upperPointer += 4;
                                outputPointer += 4;
                            }

                            // Moving the pointers to the next pixel row
                            lowerPointer += lowerData.Stride - (width * 4);
                            upperPointer += upperData.Stride - (width * 4);
                            outputPointer += outputData.Stride - (width * 4);
                        }
                    }

                    lower.UnlockBits(lowerData);
                    upper.UnlockBits(upperData);
                    output.UnlockBits(outputData);

                    g.DrawImage(output, xOffset, yOffset, finalWidth, finalHeight);
                }
            }
        }

    }


        
    }
