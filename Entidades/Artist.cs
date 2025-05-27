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
        public long Id { get; set; }
        public String Name { get; set; }
        public String Link { get; set; }
        public String Share { get; set; }
        public String Picture { get; set; }
        public String Picture_Small { get; set; }
        public String Picture_Medium { get; set; }
        public String Picture_Big { get; set; }
        public String Picture_XL { get; set; }
        public int Nb_Album {  get; set; }
        public int Nb_Fans { get; set; }
        public bool Radio { get; set; }
        public String TrackList { get; set; }

        public int Likes { get; set; }
        #endregion

        #region Constructores
        public Artist() { }

        public Artist(long id, string name, string link, string share, string picture, string pictureSmall, string pictureMedium, string pictureBig, string pictureXL, int nbAlbum, int nbFans, bool radio, string trackList)
        {
            Id = id;
            Name = name;
            Link = link;
            Share = share;
            Picture = picture;
            Picture_Small = pictureSmall;
            Picture_Medium = pictureMedium;
            Picture_Big = pictureBig;
            Picture_XL = pictureXL;
            Nb_Album = nbAlbum;
            Nb_Fans = nbFans;
            Radio = radio;
            TrackList = trackList;
        }
        #endregion
    }
}
