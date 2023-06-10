using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCtoOBSLyrics
{
    public static class LyricsHelper
    {
        public static async Task<List<SyncedLyricItem>> FetchLyricsForCurrentItemAsync(string title, string artist)
        {
            try
            {
                var lyricsObj = await MusixmatchHelper.GetSyncedLyricsAsync(title, artist);
                var body = lyricsObj.Message.Body;

                if (body != null)
                {
                    return new List<SyncedLyricItem>(body.Subtitle.Subtitles.Where(i => !string.IsNullOrWhiteSpace(i.Text)));
                }
            }
            catch
            {
                Console.WriteLine("Lyrics not found.");
            }
            return new List<SyncedLyricItem>();
        }
    }
}
