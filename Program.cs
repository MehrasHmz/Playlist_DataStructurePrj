using System;
using System.Collections.Generic;
using System.Linq;

namespace Playlist2
{
    internal class Program
    {
        static void Main()
        {
            string csvPath = "/Users/m/RiderProjects/Playlist2/musics.csv";
            var reader = new MusicCsvReader(csvPath);
            var library = reader.ReadAll();

            var manager = new PlaylistManager();

            Console.WriteLine($"Loaded {library.Count} tracks from CSV.\n");

            while (true)
            {
                ShowMenu();
                Console.Write("Choice: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": CreatePlaylist(manager); break;
                    case "2": DeletePlaylist(manager); break;
                    case "3": AddMusicInteractive(manager, library); break;
                    case "4": RemoveMusicInteractive(manager); break;
                    case "5": MergePlaylistsInteractive(manager); break;
                    case "6": ShuffleMergeInteractive(manager); break;
                    case "7": SortPlaylistInteractive(manager); break;
                    case "8": FilterPlaylistInteractive(manager); break;
                    case "9": LikeUnlikeInteractive(manager, library); break;
                    case "10": PlayPlaylistInteractive(manager, false); break;
                    case "11": PlayPlaylistInteractive(manager, true); break;
                    case "12": ShowAllPlaylists(manager); break;
                    case "13": ShowLibrary(library); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n--- Playlist2 Menu ---");
            Console.WriteLine("1. Create playlist");
            Console.WriteLine("2. Delete playlist");
            Console.WriteLine("3. Add music to playlist");
            Console.WriteLine("4. Remove music from playlist");
            Console.WriteLine("5. Merge playlists");
            Console.WriteLine("6. Shuffle-merge playlists");
            Console.WriteLine("7. Sort playlist");
            Console.WriteLine("8. Filter playlist");
            Console.WriteLine("9. Like/Unlike a track");
            Console.WriteLine("10. Play playlist");
            Console.WriteLine("11. Play playlist (shuffle)");
            Console.WriteLine("12. Show all playlists");
            Console.WriteLine("13. Show music library");
            Console.WriteLine("0. Exit");
        }

        static void ShowAllPlaylists(PlaylistManager manager)
        {
            Console.WriteLine("\nPlaylists:");
            foreach (var name in manager.GetAllPlaylistNames())
            {
                var p = manager.GetPlaylist(name)!;
                Console.WriteLine($"- {name} ({p.Count()} tracks)");
            }
        }

        static void ShowLibrary(List<Music> library)
        {
            Console.WriteLine("\nMusic Library:");
            for (int i = 0; i < library.Count; i++)
                Console.WriteLine($"{i + 1}. {library[i]}");
        }

        // --- Interactive functions ---
        static void CreatePlaylist(PlaylistManager manager)
        {
            Console.Write("Enter playlist name: ");
            var name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Invalid name.");
                return;
            }

            if (manager.CreatePlaylist(name)) Console.WriteLine("Playlist created.");
            else Console.WriteLine("Playlist already exists.");
        }

        static void DeletePlaylist(PlaylistManager manager)
        {
            Console.Write("Enter playlist name to delete: ");
            var name = Console.ReadLine()?.Trim();
            if (manager.DeletePlaylist(name!)) Console.WriteLine("Deleted.");
            else Console.WriteLine("Cannot delete.");
        }

        static void AddMusicInteractive(PlaylistManager manager, List<Music> library)
        {
            Console.Write("Enter playlist name: ");
            var pname = Console.ReadLine()?.Trim();
            var playlist = manager.GetPlaylist(pname!);
            if (playlist == null)
            {
                Console.WriteLine("Playlist not found.");
                return;
            }

            ShowLibrary(library);
            Console.Write("Enter music number to add: ");
            if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= library.Count)
            {
                playlist.AddMusic(library[num - 1]);
                Console.WriteLine("Added.");
            }
            else Console.WriteLine("Invalid number.");
        }

        static void RemoveMusicInteractive(PlaylistManager manager)
        {
            Console.Write("Enter playlist name: ");
            var pname = Console.ReadLine()?.Trim();
            var playlist = manager.GetPlaylist(pname!);
            if (playlist == null)
            {
                Console.WriteLine("Playlist not found.");
                return;
            }

            Console.Write("Enter track name to remove: ");
            var tname = Console.ReadLine()?.Trim();
            if (playlist.RemoveMusicByTrackName(tname!)) Console.WriteLine("Removed.");
            else Console.WriteLine("Track not found.");
        }

        static void MergePlaylistsInteractive(PlaylistManager manager)
        {
            Console.Write("Enter first playlist name: ");
            var first = Console.ReadLine()?.Trim();
            Console.Write("Enter second playlist name: ");
            var second = Console.ReadLine()?.Trim();
            var p1 = manager.GetPlaylist(first!);
            var p2 = manager.GetPlaylist(second!);
            if (p1 == null || p2 == null)
            {
                Console.WriteLine("Playlist not found.");
                return;
            }

            p1.MergeFrom(p2);
            Console.WriteLine("Merged.");
        }

        static void ShuffleMergeInteractive(PlaylistManager manager)
        {
            Console.Write("Enter new playlist name: ");
            var name = Console.ReadLine()?.Trim();
            var names = manager.GetAllPlaylistNames()
                .Where(n => !string.Equals(n, "Liked Songs", StringComparison.OrdinalIgnoreCase)).ToList();
            var sources = new List<Playlist>();
            foreach (var n in names)
                sources.Add(manager.GetPlaylist(n)!);

            var newP = Playlist.ShuffleMerge(name!, sources.ToArray());
            manager.CreatePlaylist(name!);
            var p = manager.GetPlaylist(name!)!;
            foreach (var m in newP.GetAll()) p.AddMusic(m);
            Console.WriteLine("Shuffle-merge completed.");
        }

        static void SortPlaylistInteractive(PlaylistManager manager)
        {
            Console.Write("Enter playlist name to sort: ");
            var name = Console.ReadLine()?.Trim();
            var p = manager.GetPlaylist(name!);
            if (p == null)
            {
                Console.WriteLine("Playlist not found.");
                return;
            }

            Console.WriteLine("Sort by: 1.Track 2.Artist 3.Year");
            var opt = Console.ReadLine()?.Trim();
            switch (opt)
            {
                case "1": p.SortByTrackName(); break;
                case "2": p.SortByArtistName(); break;
                case "3": p.SortByReleaseYear(); break;
                default: Console.WriteLine("Invalid."); break;
            }

            Console.WriteLine("Sorted.");
        }

        static void FilterPlaylistInteractive(PlaylistManager manager)
        {
            Console.Write("Enter playlist name to filter: ");
            var name = Console.ReadLine()?.Trim();
            var p = manager.GetPlaylist(name!);
            if (p == null)
            {
                Console.WriteLine("Playlist not found.");
                return;
            }

            Console.WriteLine("Filter by: 1.Artist 2.Genre 3.Year");
            var opt = Console.ReadLine()?.Trim();
            Predicate<Music> pred = _ => true;
            Console.Write("Enter value: ");
            var val = Console.ReadLine()?.Trim();
            switch (opt)
            {
                case "1": pred = m => string.Equals(m.ArtistName, val, StringComparison.OrdinalIgnoreCase); break;
                case "2": pred = m => string.Equals(m.Genre, val, StringComparison.OrdinalIgnoreCase); break;
                case "3": pred = m => m.ReleaseDate.StartsWith(val ?? ""); break;
                default:
                    Console.WriteLine("Invalid.");
                    return;
            }

            var newName = name + "_filtered";
            var filtered = p.Filter(newName, pred);
            manager.CreatePlaylist(newName);
            var newP = manager.GetPlaylist(newName)!;
            foreach (var m in filtered.GetAll()) newP.AddMusic(m);
            Console.WriteLine("Filtered playlist created: " + newName);
        }

        static void LikeUnlikeInteractive(PlaylistManager manager, List<Music> library)
        {
            ShowLibrary(library);
            Console.Write("Enter music number to like/unlike: ");
            if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= library.Count)
            {
                var m = library[num - 1];
                if (!m.IsLiked)
                {
                    manager.Like(m);
                    Console.WriteLine("Liked.");
                }
                else
                {
                    manager.Unlike(m);
                    Console.WriteLine("Unliked.");
                }
            }
            else Console.WriteLine("Invalid number.");
        }

        static void PlayPlaylistInteractive(PlaylistManager manager, bool shuffle)
        {
            Console.Write("Enter playlist name to play: ");
            var p = manager.GetPlaylist(Console.ReadLine()?.Trim()!);
            if (p == null)
            {
                Console.WriteLine("Playlist not found.");
                return;
            }

            if (!shuffle) p.Play();
            else p.PlayShuffle();
        }
    }
}