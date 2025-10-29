using System;

namespace Playlist2
{
    public class Music
    {
        public string ArtistName { get; set; } = string.Empty;
        public string TrackName { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Length { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public bool IsLiked { get; set; } = false;

        public static Music FromCsvLine(string csvLine)
        {
            var parts = csvLine.Split(',');
            if (parts.Length < 6)
                throw new FormatException("CSV line doesn't contain enough columns.");

            return new Music
            {
                ArtistName = parts[0].Trim(),
                TrackName  = parts[1].Trim(),
                ReleaseDate= parts[2].Trim(),
                Genre      = parts[3].Trim(),
                Length     = parts[4].Trim(),
                Topic      = parts[5].Trim()
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Music other) return false;
            return string.Equals(ArtistName, other.ArtistName, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(TrackName, other.TrackName, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(ReleaseDate, other.ReleaseDate, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                ArtistName?.ToLowerInvariant(),
                TrackName?.ToLowerInvariant(),
                ReleaseDate?.ToLowerInvariant()
            );
        }

        public override string ToString()
        {
            return $"{ArtistName} - {TrackName} ({Genre}, {ReleaseDate}, {Length}){(IsLiked ? " ❤️" : "")}";
        }
    }
}