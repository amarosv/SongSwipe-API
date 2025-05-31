using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Artist
    {
        #region Propiedades
        // El nombre de las variables están en minúsculas para garantizar la compatibilidad con el JSON de Deezer

        public long id { get; set; }
        public String name { get; set; }
        public String link { get; set; }
        public String share { get; set; }
        public String picture { get; set; }
        public String picture_small { get; set; }
        public String picture_medium { get; set; }
        public String picture_big { get; set; }
        public String picture_xl { get; set; }
        public int nb_album {  get; set; }
        public int nb_fans { get; set; }
        public bool radio { get; set; }
        public String tracklist { get; set; }
        public int likes { get; set; }
        #endregion

        #region Constructores
        public Artist() { }

        public Artist(long id, string name, string link, string share, string picture, string pictureSmall, string pictureMedium, string pictureBig, string pictureXL, int nbAlbum, int nbFans, bool radio, string trackList)
        {
            this.id = id;
            this.name = name;
            this.link = link;
            this.share = share;
            this.picture = picture;
            picture_small = pictureSmall;
            picture_medium = pictureMedium;
            picture_big = pictureBig;
            picture_xl = pictureXL;
            nb_album = nbAlbum;
            nb_fans = nbFans;
            this.radio = radio;
            tracklist = trackList;
        }
        #endregion
    }
}
