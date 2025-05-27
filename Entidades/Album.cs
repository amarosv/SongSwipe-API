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
        public long Id { get; set; }
        public String Title { get; set; }
        public String UPC {  get; set; }
        public String Link { get; set; }
        public String Share { get; set; }
        public String Cover { get; set; }
        public String Cover_Small { get; set; }
        public String Cover_Medium { get; set; }
        public String Cover_Big { get; set; }
        public String Cover_XL { get; set; }
        public String MD5_Image { get; set; }
        public int Genre_Id { get; set; }
        public GenresWrapper Genres { get; set; }
        public String Label { get; set; }
        public int Nb_Tracks { get; set; }
        public int Duration { get; set; }
        public int Fans {  get; set; }
        public String Release_Date { get; set; }
        public String Record_Type { get; set; }
        public bool Available { get; set; }
        public Album Alternative { get; set; }
        public String TrackList { get; set; }
        public bool Explicit_Lyrics { get; set; }
        public int Explicit_Content_Lyrics { get; set; }
        public int Explicit_Content_Cover {  get; set; }
        public List<Contributor> Contributors { get; set; }
        public Object Fallback {  get; set; }
        public Artist Artist { get; set; }
        public TracksWrapper Tracks { get; set; }
        public int Likes { get; set; }
        #endregion

        #region Constructores 
        public Album() { }

        public Album(long id , string title, string uPC, string link, string share, string cover, string coverSmall, string coverMedium, string coverBig, string coverXL, string mD5Image, int genreId, GenresWrapper genres, string label, int nbTracks, int duration, int fans, string releaseDate, string recordType, bool available, Album alternative, string trackList, bool explicitLyrics, int explicitContentLyrics, int explicitContentCover, List<Contributor> contributors, object fallback, Artist artist, TracksWrapper tracks)
        {
            Id = id;
            Title = title;
            UPC = uPC;
            Link = link;
            Share = share;
            Cover = cover;
            Cover_Small = coverSmall;
            Cover_Medium = coverMedium;
            Cover_Big = coverBig;
            Cover_XL = coverXL;
            MD5_Image = mD5Image;
            Genre_Id = genreId;
            Genres = genres;
            Label = label;
            Nb_Tracks = nbTracks;
            Duration = duration;
            Fans = fans;
            Release_Date = releaseDate;
            Record_Type = recordType;
            Available = available;
            Alternative = alternative;
            TrackList = trackList;
            Explicit_Lyrics = explicitLyrics;
            Explicit_Content_Lyrics = explicitContentLyrics;
            Explicit_Content_Cover = explicitContentCover;
            Contributors = contributors;
            Fallback = fallback;
            Artist = artist;
            Tracks = tracks;
        }
        #endregion
    }
}
