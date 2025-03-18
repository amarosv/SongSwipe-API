using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Album
    {
        #region Propiedades
        public int Id { get; set; }
        public String Title { get; set; }
        public String UPC {  get; set; }
        public String Link { get; set; }
        public String Share { get; set; }
        public String Cover { get; set; }
        public String CoverSmall { get; set; }
        public String CoverMedium { get; set; }
        public String CoverBig { get; set; }
        public String CoverXL { get; set; }
        public String MD5Image { get; set; }
        public int GenreId { get; set; }
        public List<Genre> Genres { get; set; }
        public String Label { get; set; }
        public int NbTracks { get; set; }
        public int Duration { get; set; }
        public int Fans {  get; set; }
        public String ReleaseDate { get; set; }
        public String RecordType { get; set; }
        public bool Available { get; set; }
        public Album Alternative { get; set; }
        public String TrackList { get; set; }
        public bool ExplicitLyrics { get; set; }
        public int ExplicitContentLyrics { get; set; }
        public int ExplicitContentCover {  get; set; }
        public List<Object> Contributors { get; set; }
        public Object Fallback {  get; set; }
        public Artist Artist { get; set; }
        public List<Track> Tracks { get; set; }
        #endregion

        #region Constructores 
        public Album() { }

        public Album(int id , string title, string uPC, string link, string share, string cover, string coverSmall, string coverMedium, string coverBig, string coverXL, string mD5Image, int genreId, List<Genre> genres, string label, int nbTracks, int duration, int fans, string releaseDate, string recordType, bool available, Album alternative, string trackList, bool explicitLyrics, int explicitContentLyrics, int explicitContentCover, List<object> contributors, object fallback, Artist artist, List<Track> tracks)
        {
            Id = id;
            Title = title;
            UPC = uPC;
            Link = link;
            Share = share;
            Cover = cover;
            CoverSmall = coverSmall;
            CoverMedium = coverMedium;
            CoverBig = coverBig;
            CoverXL = coverXL;
            MD5Image = mD5Image;
            GenreId = genreId;
            Genres = genres;
            Label = label;
            NbTracks = nbTracks;
            Duration = duration;
            Fans = fans;
            ReleaseDate = releaseDate;
            RecordType = recordType;
            Available = available;
            Alternative = alternative;
            TrackList = trackList;
            ExplicitLyrics = explicitLyrics;
            ExplicitContentLyrics = explicitContentLyrics;
            ExplicitContentCover = explicitContentCover;
            Contributors = contributors;
            Fallback = fallback;
            Artist = artist;
            Tracks = tracks;
        }
        #endregion
    }
}
