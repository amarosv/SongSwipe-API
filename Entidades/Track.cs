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
        // El nombre de las variables están en minúsculas para garantizar la compatibilidad con el JSON de Deezer


        public long id { get; set; }
        public bool readable { get; set; }
        public String title { get; set; }
        public String title_Short { get; set; }
        public String title_version { get; set; }
        public bool unseen { get; set; }
        public String isrc { get; set; }
        public String link { get; set; }
        public String share { get; set; }
        public int duration { get; set; }
        public int track_position { get; set; }
        public int disk_number { get; set; }
        public int rank { get; set; }
        public String release_date { get; set; }
        public bool explicit_lyrics { get; set; }
        public int explicit_content_lyrics { get; set; }
        public int explicit_content_cover {  get; set; }
        public String preview { get; set; }
        public float bpm { get; set; }
        public float gain { get; set; }
        public List<String> available_countries { get; set; }
        public Track alternative { get; set; }
        public List<Contributor> contributors { get; set; }
        public String md5_image { get; set; }
        public String track_token { get; set; }
        public SimpleArtist artist { get; set; }
        public SimpleAlbum album { get; set; }
        public String type { get; set; }
        public bool like { get; set; }
        #endregion

        #region Constructores 
        public Track() { }

        public Track(long id, bool readable, string title, string titleShort, string titleVersion, bool unseen, string iSRC, string link, string share, int duration, int trackPosition, int diskNumber, int rank, string date, bool explicitLyrics, int explicitContentLyrics, int explicitContentCover, string preview, float bPM, float gain, List<String> availableCountries, Track alternative, List<Contributor> contributors, string mD5Image, string trackToken, SimpleArtist artist, SimpleAlbum album, string type)
        {
            this.id = id;
            this.readable = readable;
            this.title = title;
            title_Short = titleShort;
            title_version = titleVersion;
            this.unseen = unseen;
            isrc = iSRC;
            this.link = link;
            this.share = share;
            this.duration = duration;
            track_position = trackPosition;
            disk_number = diskNumber;
            this.rank = rank;
            release_date = date;
            explicit_lyrics = explicitLyrics;
            explicit_content_lyrics = explicitContentLyrics;
            explicit_content_cover = explicitContentCover;
            this.preview = preview;
            bpm = bPM;
            this.gain = gain;
            available_countries = availableCountries;
            this.alternative = alternative;
            this.contributors = contributors;
            md5_image = mD5Image;
            track_token = trackToken;
            this.artist = artist;
            this.album = album;
            this.type = type;
        }
        #endregion
    }
}
