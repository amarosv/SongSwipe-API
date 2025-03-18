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
        public int Id { get; set; }
        public String Name { get; set; }
        public String Link { get; set; }
        public String Share { get; set; }
        public String Picture { get; set; }
        public String PictureSmall { get; set; }
        public String PictureMedium { get; set; }
        public String PictureBig { get; set; }
        public String PictureXL { get; set; }
        public int NbAlbum {  get; set; }
        public int NbFans { get; set; }
        public bool Radio { get; set; }
        public String TrackList { get; set; }
        #endregion

        #region Constructores
        public Artist() { }

        public Artist(int id, string name, string link, string share, string picture, string pictureSmall, string pictureMedium, string pictureBig, string pictureXL, int nbAlbum, int nbFans, bool radio, string trackList)
        {
            Id = id;
            Name = name;
            Link = link;
            Share = share;
            Picture = picture;
            PictureSmall = pictureSmall;
            PictureMedium = pictureMedium;
            PictureBig = pictureBig;
            PictureXL = pictureXL;
            NbAlbum = nbAlbum;
            NbFans = nbFans;
            Radio = radio;
            TrackList = trackList;
        }
        #endregion
    }
}
