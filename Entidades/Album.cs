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
        // El nombre de las variables están en minúsculas para garantizar la compatibilidad con el JSON de Deezer

        public long id { get; set; }
        public String title { get; set; }
        public String upc {  get; set; }
        public String link { get; set; }
        public String share { get; set; }
        public String cover { get; set; }
        public String cover_small { get; set; }
        public String cover_medium { get; set; }
        public String cover_big { get; set; }
        public String cover_xl { get; set; }
        public String md5_image { get; set; }
        public int genre_id { get; set; }
        public GenresWrapper genres { get; set; }
        public String label { get; set; }
        public int nb_tracks { get; set; }
        public int duration { get; set; }
        public int fans {  get; set; }
        public String release_date { get; set; }
        public String record_type { get; set; }
        public bool available { get; set; }
        public Album alternative { get; set; }
        public String tracklist { get; set; }
        public bool explicit_lyrics { get; set; }
        public int explicit_content_lyrics { get; set; }
        public int explicit_content_cover {  get; set; }
        public List<Contributor> contributors { get; set; }
        public Object fallback {  get; set; }
        public Artist artist { get; set; }
        public TracksWrapper tracks { get; set; }
        public int likes { get; set; }
        #endregion

        #region Constructores 
        public Album() { }

        public Album(long id , string title, string uPC, string link, string share, string cover, string coverSmall, string coverMedium, string coverBig, string coverXL, string mD5Image, int genreId, GenresWrapper genres, string label, int nbTracks, int duration, int fans, string releaseDate, string recordType, bool available, Album alternative, string trackList, bool explicitLyrics, int explicitContentLyrics, int explicitContentCover, List<Contributor> contributors, object fallback, Artist artist, TracksWrapper tracks)
        {
            this.id = id;
            this.title = title;
            upc = uPC;
            this.link = link;
            this.share = share;
            this.cover = cover;
            cover_small = coverSmall;
            cover_medium = coverMedium;
            cover_big = coverBig;
            cover_xl = coverXL;
            md5_image = mD5Image;
            genre_id = genreId;
            this.genres = genres;
            this.label = label;
            nb_tracks = nbTracks;
            this.duration = duration;
            this.fans = fans;
            release_date = releaseDate;
            record_type = recordType;
            this.available = available;
            this.alternative = alternative;
            tracklist = trackList;
            explicit_lyrics = explicitLyrics;
            explicit_content_lyrics = explicitContentLyrics;
            explicit_content_cover = explicitContentCover;
            this.contributors = contributors;
            this.fallback = fallback;
            this.artist = artist;
            this.tracks = tracks;
        }
        #endregion
    }
}
