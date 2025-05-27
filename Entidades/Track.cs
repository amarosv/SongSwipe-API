using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entidades
{
    public class Track
    {
        #region Propiedades
        public long Id { get; set; }
        public bool Readable { get; set; }
        public String Title { get; set; }
        public String Title_Short { get; set; }
        public String Title_Version { get; set; }
        public bool Unseen { get; set; }
        public String ISRC { get; set; }
        public String Link { get; set; }
        public String Share { get; set; }
        public int Duration { get; set; }
        public int Track_Position { get; set; }
        public int Disk_Number { get; set; }
        public int Rank { get; set; }
        public String Release_Date { get; set; }
        public bool Explicit_Lyrics { get; set; }
        public int Explicit_Content_Lyrics { get; set; }
        public int Explicit_Content_Cover {  get; set; }
        public String Preview { get; set; }
        public float BPM { get; set; }
        public float Gain { get; set; }
        public List<String> Available_Countries { get; set; }
        public Track Alternative { get; set; }
        public List<Contributor> Contributors { get; set; }
        public String MD5_Image { get; set; }
        public String Track_Token { get; set; }
        public SimpleArtist Artist { get; set; }
        public SimpleAlbum Album { get; set; }
        public String Type { get; set; }

        public bool Like { get; set; }
        #endregion

        #region Constructores 
        public Track() { }

        public Track(long id, bool readable, string title, string titleShort, string titleVersion, bool unseen, string iSRC, string link, string share, int duration, int trackPosition, int diskNumber, int rank, string date, bool explicitLyrics, int explicitContentLyrics, int explicitContentCover, string preview, float bPM, float gain, List<String> availableCountries, Track alternative, List<Contributor> contributors, string mD5Image, string trackToken, SimpleArtist artist, SimpleAlbum album, string type)
        {
            Id = id;
            Readable = readable;
            Title = title;
            Title_Short = titleShort;
            Title_Version = titleVersion;
            Unseen = unseen;
            ISRC = iSRC;
            Link = link;
            Share = share;
            Duration = duration;
            Track_Position = trackPosition;
            Disk_Number = diskNumber;
            Rank = rank;
            Release_Date = date;
            Explicit_Lyrics = explicitLyrics;
            Explicit_Content_Lyrics = explicitContentLyrics;
            Explicit_Content_Cover = explicitContentCover;
            Preview = preview;
            BPM = bPM;
            Gain = gain;
            Available_Countries = availableCountries;
            Alternative = alternative;
            Contributors = contributors;
            MD5_Image = mD5Image;
            Track_Token = trackToken;
            Artist = artist;
            Album = album;
            Type = type;
        }
        #endregion
    }
}
