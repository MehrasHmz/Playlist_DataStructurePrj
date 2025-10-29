using System;
using System.Collections.Generic;
using System.Linq;

namespace Playlist2
{
    public class Playlist
    {
        public string Name { get; private set; }
        private LinkedList<Music> list;

        public Playlist(string name)
        {
            Name = name;
            list = new LinkedList<Music>();
        }

        public void AddMusic(Music m)
        {
            if (!list.ToList().Contains(m)) list.AddLast(m);
        }

        public bool RemoveMusicByTrackName(string trackName)
        {
            return list.Remove(m => string.Equals(m.TrackName, trackName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Music> GetAll() => list.ToList();
        public int Count() => list.Count();

        public void MergeFrom(Playlist other)
        {
            var existing = new HashSet<Music>(list.ToList());
            foreach (var m in other.GetAll())
            {
                if (!existing.Contains(m))
                {
                    list.AddLast(m);
                    existing.Add(m);
                }
            }
        }

        public static Playlist ShuffleMerge(string newName, params Playlist[] sources)
        {
            var result = new Playlist(newName);
            var set = new HashSet<Music>();
            var combined = new List<Music>();
            foreach (var p in sources)
            foreach (var m in p.GetAll())
                if (!set.Contains(m))
                {
                    set.Add(m);
                    combined.Add(m);
                }

            var rnd = new Random();
            for (int i = combined.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                var tmp = combined[i];
                combined[i] = combined[j];
                combined[j] = tmp;
            }

            foreach (var m in combined) result.AddMusic(m);
            return result;
        }

        public void SortByTrackName(bool ascending = true)
        {
            var arr = list.ToList();
            arr = ascending ? arr.OrderBy(m => m.TrackName).ToList() : arr.OrderByDescending(m => m.TrackName).ToList();
            list.ReplaceAll(arr);
        }

        public void SortByArtistName(bool ascending = true)
        {
            var arr = list.ToList();
            arr = ascending
                ? arr.OrderBy(m => m.ArtistName).ToList()
                : arr.OrderByDescending(m => m.ArtistName).ToList();
            list.ReplaceAll(arr);
        }

        public void SortByReleaseYear(bool ascending = true)
        {
            var arr = list.ToList();
            arr = ascending
                ? arr.OrderBy(m =>
                {
                    if (int.TryParse(m.ReleaseDate.Split('-')[0], out int y)) return y;
                    return int.MinValue;
                }).ToList()
                : arr.OrderByDescending(m =>
                {
                    if (int.TryParse(m.ReleaseDate.Split('-')[0], out int y)) return y;
                    return int.MinValue;
                }).ToList();

            list.ReplaceAll(arr);
        }

        public Playlist Filter(string newName, Predicate<Music> predicate)
        {
            var p = new Playlist(newName);
            foreach (var m in GetAll())
                if (predicate(m))
                    p.AddMusic(m);
            return p;
        }

        public void Play()
        {
            Console.WriteLine($"\n--- Playing playlist: {Name} ---");
            int i = 1;
            foreach (var m in GetAll())
            {
                Console.WriteLine($"{i}. {m}");
                i++;
            }

            if (i == 1) Console.WriteLine("Playlist is empty.");
            Console.WriteLine($"--- End of {Name} ---\n");
        }

        public void PlayShuffle()
        {
            var arr = GetAll().ToList();
            var rnd = new Random();
            for (int i = arr.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                var tmp = arr[i];
                arr[i] = arr[j];
                arr[j] = tmp;
            }

            Console.WriteLine($"\n--- Playing (shuffled): {Name} ---");
            int i2 = 1;
            foreach (var m in arr)
            {
                Console.WriteLine($"{i2}. {m}");
                i2++;
            }

            if (i2 == 1) Console.WriteLine("Playlist is empty.");
            Console.WriteLine($"--- End of shuffled {Name} ---\n");
        }
    }
}