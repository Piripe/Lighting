using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using OBSWebsocketDotNet.Communication;
using System.Diagnostics;
using Newtonsoft.Json;
using OBSWebsocketDotNet.Types.Events;
using VLCtoOBSLyrics.Data;
using System.Drawing;
using VLCtoOBSLyrics.SongLighting;
using VLCtoOBSLyrics.Utils;
using VLCtoOBSLyrics.SongLighting.Scene;
using VLCtoOBSLyrics.SongLighting.SongLayer;

namespace VLCtoOBSLyrics
{
    static class Program
    {
        public static VlcMusicInfos musicInfos = new VlcMusicInfos("", "azerty","127.0.0.1",5947);
        public static OBSWebsocket obs = new OBSWebsocket();
        public static Config config = new Config();

        public static Timer displayUpdateTimer = new Timer(new TimerCallback(UpdateOBSDisplay), null, 0, 32);
        public static List<SyncedLyricItem>? lyrics;

        public static DateTime autoDisplayLastTransition = DateTime.Now;

        public static LightsColor oldLightsColor = new LightsColor();
        public static float oldColorRatio = 1;

        public static string currentScene = "";

        public static int Main(string[] args)
        {
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./config.json")) ?? config;
            musicInfos = new VlcMusicInfos("", "azerty", "127.0.0.1", 5947);
            obs = new OBSWebsocket();

            obs.Connected += OBSConnected;
            obs.CurrentPreviewSceneChanged += OBSCurrentPreviewSceneChanged;

            OBSConnect();

            while (true)
            {
                string? line = Console.ReadLine();

                if (line == "exit")
                {
                    break;
                }
                else if (line == "print_lyrics")
                {
                    Console.WriteLine(musicInfos.Lyrics);
                }
                else if (line == "update")
                {
                    musicInfosUpdated(null, new EventArgs());
                }
                else if (line == "obs_stats")
                {
                    Console.WriteLine("Obs.IsConnected: " + obs.IsConnected.ToString());
                }
                else if (line == "v0")
                {
                    config.Version = 0;
                }
                else if (line == "v1")
                {
                    config.Version = 1;
                }
                else if (line == "v2")
                {
                    config.Version = 2;
                }
                else if (line == "v3")
                {
                    config.Version = 3;
                }
                else if (line == "refresh")
                {
                    config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./config.json")) ?? config;
                }
                else if (line == "test")
                {
                    Console.WriteLine(string.Join("\n",obs.GetInputKindList()));
                }
            }

            return 0;
        }
        public static void OBSConnect()
        {
            obs.ConnectAsync("ws://192.168.1.105:4455", "");
        }
        public static void OBSConnected(object? sender, EventArgs e)
        {
            musicInfos.infosUpdated += musicInfosUpdated;

            musicInfos.Connect();
        }
        public static void OBSCurrentPreviewSceneChanged(object? sender, CurrentPreviewSceneChangedEventArgs e)
        {
            if (obs.GetStudioModeEnabled()) return;


        }
        public static async void musicInfosUpdated(object? sender, EventArgs e)
        {
            try
            {
                if (obs.IsConnected)
                {
                    oldColorRatio = 1;

                    Console.WriteLine("Fetching lyrics");
                    //ClearDisplay();
                    lyrics = null;
                    obs.SetInputSettings(config.V1.TrackNameSource, new JObject(new JProperty("text", $"{musicInfos.Artist} - {musicInfos.Title}     ")));
                    obs.SetInputSettings("Image", new JObject(new JProperty("file", new Uri(musicInfos.ArtworkURL).LocalPath)));
                    //if (autoDisplay)
                    //{
                        //obs.SetCurrentProgramScene("Radio tah les fous");
                        if(config.Version != 3) obs.SetSourceFilterEnabled("Lyrics", "Move Value 2", true);

                        autoDisplayLastTransition = DateTime.Now;
                    //}
                    lyrics = await LyricsHelper.FetchLyricsForCurrentItemAsync(musicInfos.Title, musicInfos.Artist);
                    currentLyrics = new SyncedLyricItem();

                    switch (config.Version)
                    {
                        case 3:
                            if (config.V3.SongsLighting.ContainsKey(musicInfos.FormatTrackName())) {
                                SongLightingV3 songLighting = config.V3.SongsLighting[musicInfos.FormatTrackName()];
                                V3LoadScene(currentScene, songLighting.Scene);
                                if (currentScene != songLighting.Scene)
                                    V3CurrentLayersValue = new Dictionary<ISongLayer, object>();
                                currentScene = songLighting.Scene;
                            }
                            else
                            {
                                V3LoadScene(currentScene, "default");
                                currentScene = "default";
                            }
                            break;
                    }
                }
            } catch (Exception ex) { Console.WriteLine("Error: " + ex.ToString()); }
        }


        public static List<string> lyricsOrder = new string[] { "Lyrics - Line 1", "Lyrics - Line 2", "Lyrics - Line 3", "Lyrics - Line 4", "Lyrics - Line 5", "Lyrics - Line 6", "Lyrics - Line 7" }.ToList();
        //public static List<string> lyricsOrder = new string[] { "Lyrics - Line 1", "-", "-", "-", "-", "-", "-" }.ToList();
        public static SyncedLyricItem? currentLyrics;

        public static Dictionary<ISongLayer, object> V3CurrentLayersValue = new Dictionary<ISongLayer, object>();

        public static void UpdateOBSDisplay(object? x)
        {
            try
            {
                SyncedLyricItem? lyricsItem = lyrics?.LastOrDefault(item => item.TimeSpan.TotalSeconds < musicInfos.Position.TotalSeconds);

                //Console.WriteLine("Lyrics count: "+lyrics?.Count);
                
                if (lyricsItem != currentLyrics && lyrics != null && obs.IsConnected && config.Version != 3)
                {
                    if (DateTime.Now.Subtract(autoDisplayLastTransition).TotalSeconds > 0.5)
                    {

                        Console.WriteLine("Updating display");
                        if (lyrics.Count > 0)
                        {
                            obs.SetSourceFilterEnabled("Lyrics", "Move Value", true);
                        }

                        switch (config.Version) {
                            case 1:
                            if (lyrics.Count > 0)
                            {
                                if (obs.GetCurrentProgramScene() != config.V1.SceneToSetName)
                                    obs.SetCurrentProgramScene(config.V1.SceneToSetName);
                            }
                            else if (obs.GetCurrentProgramScene() != config.V1.SceneWOLyricsToSetName)
                            {
                                obs.SetCurrentProgramScene(config.V1.SceneWOLyricsToSetName);
                            }
                            break;
                            case 2:
                                obs.SetCurrentProgramScene(config.V2.SceneName);
                                break;
                            case 3:
                                obs.SetCurrentProgramScene(config.V3.SceneName);
                                break;
                        }
                    }

                    currentLyrics = lyricsItem;
                    lyricsOrder.Add(lyricsOrder[0]);
                    lyricsOrder.RemoveAt(0);

                    int currentLyricsIndex = lyrics.IndexOf(lyricsItem);

                    string[] currentLyricsInOrder = new string[] { "", "", "", "", "", "", "" };

                    for (int i = currentLyricsIndex - 2; i < currentLyricsIndex + 5; i++)
                    {
                        if (i >= 0 && i < lyrics.Count)
                            currentLyricsInOrder[i - (currentLyricsIndex - 2)] = lyrics[i].Text;
                    }

                    //Console.WriteLine($"Display Order:\n -{string.Join("\n -",lyricsOrder)}");
                    for (int i =0; i<7;i++)
                    {
                        try
                        {

                            obs.SetInputSettings(lyricsOrder[i], new JObject(new JProperty("text", currentLyricsInOrder[i])), true);
                            obs.SetSourceFilterSettings(lyricsOrder[i], "Move Value",
                                i switch
                                //{
                                //    0 => new JObject(new JProperty("Scale.X", 60), new JProperty("Scale.Y", 60), new JProperty("Position.Y", -120)),
                                //    1 => new JObject(new JProperty("Scale.X", 80), new JProperty("Scale.Y", 80), new JProperty("Position.Y",-80)),
                                //    2 => new JObject(new JProperty("Scale.X", 90), new JProperty("Scale.Y", 90), new JProperty("Position.Y", -40)),
                                //    3 => new JObject(new JProperty("Scale.X", 100), new JProperty("Scale.Y", 100), new JProperty("Position.Y", 0)),
                                //    4 => new JObject(new JProperty("Scale.X", 90), new JProperty("Scale.Y", 90), new JProperty("Position.Y", 40)),
                                //    5 => new JObject(new JProperty("Scale.X", 80), new JProperty("Scale.Y", 80), new JProperty("Position.Y", 80)),
                                //    _ => new JObject(new JProperty("Scale.X", 60), new JProperty("Scale.Y", 60), new JProperty("Position.Y", 120)),
                                //}
                                {
                                    0 => new JObject(new JProperty("Scale.X", 60), new JProperty("Scale.Y", 60), new JProperty("Position.Y", -120)),
                                    1 => new JObject(new JProperty("Scale.X", 90), new JProperty("Scale.Y", 90), new JProperty("Position.Y", -80)),
                                    2 => new JObject(new JProperty("Scale.X", 100), new JProperty("Scale.Y", 100), new JProperty("Position.Y", -40)),
                                    3 => new JObject(new JProperty("Scale.X", 92), new JProperty("Scale.Y", 92), new JProperty("Position.Y", 0)),
                                    4 => new JObject(new JProperty("Scale.X", 88), new JProperty("Scale.Y", 88), new JProperty("Position.Y", 40)),
                                    5 => new JObject(new JProperty("Scale.X", 84), new JProperty("Scale.Y", 84), new JProperty("Position.Y", 80)),
                                    _ => new JObject(new JProperty("Scale.X", 60), new JProperty("Scale.Y", 60), new JProperty("Position.Y", 120)),
                                }
                                , true);
                            obs.SetSourceFilterEnabled(lyricsOrder[i], "Move Value", true);
                            obs.SetSourceFilterSettings(lyricsOrder[i], "Move Value 2",
                                i switch
                                //{
                                //    0 => new JObject(new JProperty("opacity", 0)),
                                //    1 => new JObject(new JProperty("opacity", 0.50)),
                                //    2 => new JObject(new JProperty("opacity", 0.75)),
                                //    3 => new JObject(new JProperty("opacity", 1)),
                                //    4 => new JObject(new JProperty("opacity", 0.75)),
                                //    5 => new JObject(new JProperty("opacity", 0.50)),
                                //    _ => new JObject(new JProperty("opacity", 0.0)),
                                //}
                                {
                                    0 => new JObject(new JProperty("opacity", 0)),
                                    1 => new JObject(new JProperty("opacity", 0.3)),
                                    2 => new JObject(new JProperty("opacity", 1)),
                                    3 => new JObject(new JProperty("opacity", 0.4)),
                                    4 => new JObject(new JProperty("opacity", 0.3)),
                                    5 => new JObject(new JProperty("opacity", 0.2)),
                                    _ => new JObject(new JProperty("opacity", 0)),
                                }
                                , true);
                            obs.SetSourceFilterEnabled(lyricsOrder[i], "Move Value 2", true);

                        }
                        catch
                            (Exception ex)
            {
                        Console.WriteLine("Error: " + ex.ToString());
                    }
                
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " +ex.ToString());
            }

            try
            {
                if (config.Version == 2)
                {
                    LightsColor lightsColor = new LightsColor();
                    if (config.V2.SongsLighting.ContainsKey(musicInfos.FormatTrackName()))
                    {
                        lightsColor = config.V2.SongsLighting[musicInfos.FormatTrackName()].GetLightsColor(musicInfos.Position);
                    }
                    if (oldColorRatio == 0)
                    {
                        if (oldLightsColor != lightsColor)
                        {
                            oldLightsColor = lightsColor;

                            UpdateLightsColor(lightsColor);
                        }
                    }
                    else
                    {
                        oldColorRatio = (float)Math.Max(0, oldColorRatio - 0.01);
                        UpdateLightsColor(LightsColor.BlendLightsColor(lightsColor, oldLightsColor, oldColorRatio));
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }

            try
            {
                if (config.Version == 3)
                {
                    if (config.V3.SongsLighting.ContainsKey(musicInfos.FormatTrackName()))
                    {
                        SongLightingV3 songLighting = config.V3.SongsLighting[musicInfos.FormatTrackName()];

                        Dictionary<string, JObject> elementsSettings = new Dictionary<string, JObject>();
                        Dictionary<string, Dictionary<string, JObject>> elementsEffectSettings = new Dictionary<string, Dictionary<string, JObject>>();

                        JObject getSettingJObject(IEnumerable<string> path, object value)
                        {
                            if (path.Count() == 1) return new JObject(new JProperty(path.ToList()[0],value));
                            return new JObject(new JProperty(path.ToList()[0], getSettingJObject(path.Skip(1),value)));
                        }

                        songLighting.Layers.ForEach((ISongLayer layer) =>
                        {
                            object value = layer.GetValueAtFrame((int)(musicInfos.Position.TotalMilliseconds / 1000f * 30));

                            if (!V3CurrentLayersValue.ContainsKey(layer)) V3CurrentLayersValue.Add(layer, 0);

                            //Console.WriteLine($"LayersCacheLength: {V3CurrentLayersValue.Count}");
                            if (V3CurrentLayersValue[layer].GetHashCode() != value.GetHashCode())
                            {
                                //Console.WriteLine($"Last value: {V3CurrentLayersValue[layer]}\tNew value: {value}");
                                V3CurrentLayersValue[layer] = value;

                                switch (layer.Action)
                                {
                                    case SongLayerAction.Setting:

                                        if (!elementsSettings.ContainsKey(layer.ActionElement)) elementsSettings.Add(layer.ActionElement, new JObject());

                                        elementsSettings[layer.ActionElement].Merge(getSettingJObject(layer.ActionData, value));

                                        break;
                                    case SongLayerAction.FilterSetting:

                                        if (!elementsEffectSettings.ContainsKey(layer.ActionElement)) elementsEffectSettings.Add(layer.ActionElement, new Dictionary<string, JObject>());
                                        if (!elementsEffectSettings[layer.ActionElement].ContainsKey(layer.ActionData[0])) elementsEffectSettings[layer.ActionElement].Add(layer.ActionData[0], new JObject());

                                        elementsEffectSettings[layer.ActionElement][layer.ActionData[0]].Merge(getSettingJObject(layer.ActionData.Skip(1), value));

                                        break;
                                    case SongLayerAction.EnableSource:
                                        obs.SetSceneItemEnabled(config.V3.SceneName, obs.GetSceneItemId(config.V3.SceneName, layer.ActionElement, 0), (bool)value);
                                        break;
                                    case SongLayerAction.EnableFilter:
                                        obs.SetSourceFilterEnabled(layer.ActionElement, layer.ActionData[0], (bool)value);
                                        break;
                                    case SongLayerAction.Media:
                                        MediaLayerKeyframe keyframe = (MediaLayerKeyframe)value;

                                        switch (keyframe.Status)
                                        {
                                            case MediaLayerAction.Play:
                                                obs.TriggerMediaInputAction(layer.ActionElement, "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PLAY");
                                                break;
                                            case MediaLayerAction.Pause:
                                                obs.TriggerMediaInputAction(layer.ActionElement, "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PAUSE");
                                                break;
                                            case MediaLayerAction.Stop:
                                                obs.TriggerMediaInputAction(layer.ActionElement, "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_STOP");
                                                break;
                                        }

                                        if (keyframe.Cursor != null) obs.SetMediaInputCursor(layer.ActionElement, keyframe.Cursor??0);
                                        
                                        break;
                                }
                            }
                        });

                        foreach (KeyValuePair<string, JObject> setting in elementsSettings)
                        {
                            obs.SetInputSettings(setting.Key, setting.Value);
                        }
                        foreach (KeyValuePair<string, Dictionary<string, JObject>> element in elementsEffectSettings)
                        {
                            foreach (KeyValuePair<string, JObject> setting in element.Value)
                            {
                                //Console.WriteLine($"Setting settings of {element.Key}:{setting.Key} to {setting.Value.ToString(Formatting.None)}");
                                obs.SetSourceFilterSettings(element.Key, setting.Key, setting.Value,true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }
        public static void UpdateLightsColor(LightsColor lights)
        {
            if (obs.IsConnected)
            {
                obs.SetInputSettings(config.V2.SourceFog, new JObject(new JProperty("color",lights.FogColor.ToInt())));
                obs.SetSourceFilterSettings(config.V2.SourceL1, "Color Correction", new JObject(new JProperty("color_multiply", lights.LightColor1.ToInt())));
                obs.SetSourceFilterSettings(config.V2.SourceL2, "Color Correction", new JObject(new JProperty("color_multiply", lights.LightColor2.ToInt())));
            }
        }
        public static void ClearDisplay()
        {
            for (int i = 0; i < 7; i++)
            {
                try
                {
                    obs.SetInputSettings(lyricsOrder[i], new JObject(new JProperty("text", "")), true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.ToString());
                }
            }
        }


        #region "V3"



        public static void V3LoadScene(string fromSceneName, string loadSceneName)
        {
            Console.WriteLine($"Loading scene \"{loadSceneName}\"");

            Scene? fromScene = config.V3.Scenes.ContainsKey(fromSceneName) ? config.V3.Scenes[fromSceneName] : null;
            Scene? loadScene = config.V3.Scenes.ContainsKey(loadSceneName) ? config.V3.Scenes[loadSceneName] : null;

            List<SceneItemDetails> sceneItems = obs.GetSceneItemList(config.V3.SceneName);
            List<InputBasicInfo> sources = obs.GetInputList();
            //IEnumerable<string> elementsList = sceneItemDetails.Select((SceneItemDetails sceneItem) => sceneItem.SourceName + sceneItem.SourceKind);

            if (fromScene == null || loadScene == null)
            {
                //elementsList = new List<string>();
                sceneItems.ForEach((SceneItemDetails sceneItem) => {
                    //obs.RemoveSceneItem(config.V3.SceneName, sceneItem.ItemId);
                    obs.RemoveInput(sceneItem.SourceName);
                    });
            }
            if (loadScene != null)
            {

                loadScene.Elements.ForEach((SceneElement element) =>
                {
                    InputBasicInfo? source = sources.Find((InputBasicInfo e) => e.InputName == element.Name);
                    int id = 0;
                    if (source == null)
                    {
                        id = obs.CreateInput(config.V3.SceneName, element.Name, element.Kind, element.Settings,true);
                    }
                    else
                    {
                        SceneItemDetails? sceneItem = sceneItems.Find((SceneItemDetails e) => e.SourceName == element.Name);
                        if (sceneItem == null)
                        {
                            id = obs.CreateSceneItem(config.V3.SceneName, element.Name, true);
                        }
                        else id = sceneItem.ItemId;
                    }

                    obs.SetSceneItemIndex(config.V3.SceneName,id,loadScene.Elements.IndexOf(element));

                    if (element.Transform != null) obs.SetSceneItemTransform(config.V3.SceneName, id, element.Transform);

                    obs.SetSceneItemBlendMode(config.V3.SceneName, id,element.BlendMode);

                    List<FilterSettings>? filters = source != null ? obs.GetSourceFilterList(element.Name) : null;

                    element.Filters.ForEach((SceneElementFilter elementFilter) =>
                    {
                        if (filters?.Any((FilterSettings f) => f.Name == elementFilter.Name) ?? false)
                        {
                            if(elementFilter.Settings != null) obs.SetSourceFilterSettings(element.Name, elementFilter.Name, elementFilter.Settings);
                        }
                        else
                        {
                            if (elementFilter.Settings != null) obs.CreateSourceFilter(element.Name,elementFilter.Name,elementFilter.Kind,elementFilter.Settings);
                        }
                        obs.SetSourceFilterIndex(element.Name, elementFilter.Name,element.Filters.IndexOf(elementFilter));
                    });
                });

                if (fromScene != null)
                {

                    fromScene.Elements.ForEach((SceneElement element) =>
                    {
                        SceneElement? commonElement = loadScene.Elements.Find((SceneElement e) => e.Name == element.Name);
                        if (commonElement != null)
                        {
                            element.Filters.ForEach((SceneElementFilter filter) =>
                            {
                                if (!commonElement.Filters.Any((SceneElementFilter f) => f.Name == filter.Name))
                                {
                                    obs.RemoveSourceFilter(element.Name,filter.Name);
                                }
                            });
                        }
                        else
                        {
                            InputBasicInfo? source = sources.Find((InputBasicInfo e) => e.InputName == element.Name);
                            
                            if (source != null) obs.RemoveInput(element.Name);
                        }
                });
                }
            }
        }



        #endregion
    }
}