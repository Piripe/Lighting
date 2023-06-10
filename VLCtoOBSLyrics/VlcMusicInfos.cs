using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VLCtoOBSLyrics
{
    public class VlcMusicInfos
    {
        public string Title { get; set; } = "";
        public string Artist { get; set; } = "";
        public string Album { get; set; } = "";
        public string Lyrics { get; set; } = "";
        public string ArtworkURL { get; set; } = "";
        public TimeSpan Duration { get; set; } = TimeSpan.Zero;
        private TimeSpan _position = TimeSpan.Zero;
        private DateTime _positionDate = DateTime.Now;
        public TimeSpan Position { get => _position.Add(DateTime.Now.Subtract(_positionDate)); }

        public event EventHandler? infosUpdated;

        private string _username;
        private string _password;
        private string _address;
        private short _port;
        private HttpClient _httpClient;

        private Timer? _periodicUpdater;
        private Timer? _autoUpdater;
        private DateTime _updateIn = DateTime.Now;

        public VlcMusicInfos(string username, string password, string address = "127.0.0.1", Int16 port = 8080)
        {
            _username = username;
            _password = password;
            _address = address;
            _port = port;

            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}")));

        }

        public void Connect()
        {
            _periodicUpdater = new Timer(new TimerCallback(_periodicUpdater_Tick), null, 0, 1000);
        }

        public string FormatTrackName()
        {
            return $"{Artist}-{Title}";
        }

        private async void _periodicUpdater_Tick(object? x)
        {
            TimeSpan lastPosition = Position;
            await _fetchInfos();

            if (Math.Abs(DateTime.Now.Subtract(_updateIn.Subtract(Duration.Subtract(Position))).TotalSeconds) > 0.2)
            {
                _updateIn = DateTime.Now.Add(Duration.Subtract(Position));
                Console.WriteLine("timer in " + (int)Duration.Subtract(Position).TotalMilliseconds + "ms");
                if (_autoUpdater != null) _autoUpdater.Dispose();
                _autoUpdater = new Timer(new TimerCallback(_periodicUpdater_Tick), new AutoResetEvent(false), Math.Max(1000,(int)Duration.TotalMilliseconds - (int)Position.TotalMilliseconds)+20, 0);
            }
        }

        private async Task _fetchInfos()
        {
            Console.WriteLine($"Fetch infos");
            HttpResponseMessage res = await _httpClient.GetAsync($"http://{_address}:{_port}/requests/status.json");

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                VlcData data = JsonConvert.DeserializeObject<VlcData>(await res.Content.ReadAsStringAsync());

                TimeSpan lastDuration = Duration;
                string lastTitle = Title, lastAlbum = Album;


                _position = TimeSpan.FromSeconds(data.time);
                _positionDate = DateTime.Now;

                Duration = TimeSpan.FromSeconds(Math.Max(1/Math.Max(data.position,0.00001)*data.time,data.length));

                Title = data.information.category.meta.title;
                Artist = data.information.category.meta.artist;
                Album = data.information.category.meta.album;
                Lyrics = data.information.category.meta.lyrics;
                ArtworkURL = data.information.category.meta.artwork_url;

                if ((Math.Abs(lastDuration.TotalSeconds - Duration.TotalSeconds) > 1) || lastTitle != Title ||lastAlbum != Album)
                {
                    infosUpdated?.Invoke(this,new EventArgs());
                }
            }
            else
            {
                Console.WriteLine("Http status: "+res.StatusCode);
            }
        }

        private struct VlcData
        {
            public float time;
            public float position;
            public float length;

            public VlcInfo information;
            public struct VlcInfo
            {
                public VlcCategory category;
                public struct VlcCategory
                {
                    public VlcMeta meta;
                    public struct VlcMeta
                    {
                        public string title;
                        public string artist;
                        public string album;
                        public string lyrics;
                        public string artwork_url;
                    }
                } 
            }
        }
    }
}
