using System;
using System.Collections.Generic;
using System.IO;

namespace Playlist2
{
    public class MusicCsvReader
    {
        private readonly string filePath;

        public MusicCsvReader(string filePath) => this.filePath = filePath;

        public List<Music> ReadAll(bool hasHeader = true)
        {
            var list = new List<Music>();
            if (!File.Exists(filePath)) return list;

            var lines = File.ReadAllLines(filePath);
            int start = hasHeader ? 1 : 0;

            for (int i = start; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;
                try
                {
                    list.Add(Music.FromCsvLine(line));
                }
                catch
                {
                }
            }

            return list;
        }
    }
}