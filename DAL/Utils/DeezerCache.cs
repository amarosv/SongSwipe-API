using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Utils
{
    public class DeezerCache
    {
        private static readonly Dictionary<long, (Track Track, DateTime Expiry)> _cacheTrack = new Dictionary<long, (Track, DateTime)>();
        private static readonly Dictionary<long, (Album Album, DateTime Expiry)> _cacheAlbum = new Dictionary<long, (Album, DateTime)>();
        private static readonly Dictionary<long, (Artist Artist, DateTime Expiry)> _cacheArtist = new Dictionary<long, (Artist, DateTime)>();
        private static readonly Dictionary<long, (Genre Genre, DateTime Expiry)> _cacheGenre = new Dictionary<long, (Genre, DateTime)>();

        public static bool TryGetTrack(long trackId, out Track track)
        {
            if (_cacheTrack.TryGetValue(trackId, out var cached) && cached.Expiry > DateTime.UtcNow)
            {
                track = cached.Track;
                return true;
            }
            track = null;
            return false;
        }

        public static void AddTrack(long trackId, Track track, TimeSpan? ttl = null)
        {
            _cacheTrack[trackId] = (track, DateTime.UtcNow.Add(ttl ?? TimeSpan.FromHours(1)));
        }

        public static bool TryGetAlbum(long albumId, out Album album)
        {
            if (_cacheAlbum.TryGetValue(albumId, out var cached) && cached.Expiry > DateTime.UtcNow)
            {
                album = cached.Album;
                return true;
            }
            album = null;
            return false;
        }

        public static void AddAlbum(long albumId, Album album, TimeSpan? ttl = null)
        {
            _cacheAlbum[albumId] = (album, DateTime.UtcNow.Add(ttl ?? TimeSpan.FromHours(1)));
        }

        public static bool TryGetArtist(long artistId, out Artist artist)
        {
            if (_cacheArtist.TryGetValue(artistId, out var cached) && cached.Expiry > DateTime.UtcNow)
            {
                artist = cached.Artist;
                return true;
            }
            artist = null;
            return false;
        }

        public static void AddArtist(long artistId, Artist artist, TimeSpan? ttl = null)
        {
            _cacheArtist[artistId] = (artist, DateTime.UtcNow.Add(ttl ?? TimeSpan.FromHours(1)));
        }

        public static bool TryGetGenre(long genreId, out Genre genre)
        {
            if (_cacheGenre.TryGetValue(genreId, out var cached) && cached.Expiry > DateTime.UtcNow)
            {
                genre = cached.Genre;
                return true;
            }
            genre = null;
            return false;
        }

        public static void AddGenre(long genreId, Genre genre, TimeSpan? ttl = null)
        {
            _cacheGenre[genreId] = (genre, DateTime.UtcNow.Add(ttl ?? TimeSpan.FromHours(1)));
        }
    }
}
