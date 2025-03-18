using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TrackCache
    {
        private static readonly Dictionary<long, (Track Track, DateTime Expiry)> _cache = new Dictionary<long, (Track, DateTime)>();

        public static bool TryGetTrack(long trackId, out Track track)
        {
            if (_cache.TryGetValue(trackId, out var cached) && cached.Expiry > DateTime.UtcNow)
            {
                track = cached.Track;
                return true;
            }
            track = null;
            return false;
        }

        public static void AddTrack(long trackId, Track track, TimeSpan? ttl = null)
        {
            _cache[trackId] = (track, DateTime.UtcNow.Add(ttl ?? TimeSpan.FromHours(1)));
        }
    }
}
