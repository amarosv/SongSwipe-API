using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Track
    {
        #region Propiedades
        public int Id { get; set; }
        public bool Readable { get; set; }
        public String Title { get; set; }
        public String TitleShort { get; set; }
        public String TitleVersion { get; set; }
        public bool Unseen { get; set; }
        public String ISRC { get; set; }
        public String Link { get; set; }
        public String Share { get; set; }
        public int Duration { get; set; }
        public int TrackPosition { get; set; }
        public int DiskNumber { get; set; }
        public int Rank { get; set; }
        public String Date { get; set; }
        public bool ExplicitLyrics { get; set; }
        public int ExplicitContentLyrics { get; set; }
        public int ExplicitContentCover {  get; set; }
        public String Preview { get; set; }
        public float BPM { get; set; }
        public float Gain { get; set; }
        public List<Object> AvailableCountries { get; set; }
        public Track Alternative { get; set; }
        public List<Object> Contributors { get; set; }
        public String MD5Image { get; set; }
        public String TrackToken { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }
        #endregion

        #region Constructores 
        public Track() { }

        public Track(int id, bool readable, string title, string titleShort, string titleVersion, bool unseen, string iSRC, string link, string share, int duration, int trackPosition, int diskNumber, int rank, string date, bool explicitLyrics, int explicitContentLyrics, int explicitContentCover, string preview, float bPM, float gain, List<object> availableCountries, Track alternative, List<object> contributors, string mD5Image, string trackToken, Artist artist, Album album)
        {
            Id = id;
            Readable = readable;
            Title = title;
            TitleShort = titleShort;
            TitleVersion = titleVersion;
            Unseen = unseen;
            ISRC = iSRC;
            Link = link;
            Share = share;
            Duration = duration;
            TrackPosition = trackPosition;
            DiskNumber = diskNumber;
            Rank = rank;
            Date = date;
            ExplicitLyrics = explicitLyrics;
            ExplicitContentLyrics = explicitContentLyrics;
            ExplicitContentCover = explicitContentCover;
            Preview = preview;
            BPM = bPM;
            Gain = gain;
            AvailableCountries = availableCountries;
            Alternative = alternative;
            Contributors = contributors;
            MD5Image = mD5Image;
            TrackToken = trackToken;
            Artist = artist;
            Album = album;
        }
        #endregion
    }
}
