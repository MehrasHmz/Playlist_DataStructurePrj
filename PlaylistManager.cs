using System;
using System.Collections.Generic;
using System.Linq;

namespace Playlist2
{
    public class PlaylistManager
    {
        private readonly Dictionary<string, Playlist> playlists = new(StringComparer.OrdinalIgnoreCase);
        public Playlist LikedSongs { get; }

        public PlaylistManager()
        {
            LikedSongs = new Playlist("Liked Songs");
            playlists[LikedSongs.Name] = LikedSongs;
        }

        public bool CreatePlaylist(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || playlists.ContainsKey(name)) return false;
            playlists[name] = new Playlist(name);
            return true;
        }

        public bool DeletePlaylist(string name)
        {
            if (string.Equals(name, LikedSongs.Name, StringComparison.OrdinalIgnoreCase)) return false;
            return playlists.Remove(name);
        }

        public Playlist? GetPlaylist(string name) => playlists.TryGetValue(name, out var p) ? p : null;

        public IEnumerable<string> GetAllPlaylistNames() => playlists.Keys.OrderBy(x => x);

        public bool AddMusicToPlaylist(string playlistName, Music m)
        {
            var p = GetPlaylist(playlistName);
            if (p == null) return false;
            p.AddMusic(m);
            return true;
        }

        public bool RemoveMusicFromPlaylist(string playlistName, string trackName)
        {
            var p = GetPlaylist(playlistName);
            if (p == null) return false;
            if (p.RemoveMusicByTrackName(trackName))
            {
                if (p.Name == LikedSongs.Name)
                {
                    var m = p.GetAll().FirstOrDefault(x =>
                        x.TrackName.Equals(trackName, StringComparison.OrdinalIgnoreCase));
                    if (m != null) m.IsLiked = false;
                }
            }

            return false;
        }

        public void Like(Music m)
        {
            if (!m.IsLiked)
            {
                m.IsLiked = true;
                LikedSongs.AddMusic(m);
            }
        }

        public void Unlike(Music m)
        {
            if (m.IsLiked)
            {
                m.IsLiked = false;
                LikedSongs.RemoveMusicByTrackName(m.TrackName);
            }
        }
    }
}