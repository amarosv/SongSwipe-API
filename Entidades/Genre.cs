using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Genre
    {
        #region Propiedades
        public int Id { get; set; }
        public String Name { get; set; }
        public String Picture { get; set; }
        public String PictureSmall { get; set; }
        public String PictureMedium { get; set; }
        public String PictureBig { get; set; }
        public String PictureXL { get; set; }
        #endregion

        #region Constructores
        public Genre() { }

        public Genre(int id, string name, string picture, string pictureSmall, string pictureMedium, string pictureBig, string pictureXL)
        {
            Id = id;
            Name = name;
            Picture = picture;
            PictureSmall = pictureSmall;
            PictureMedium = pictureMedium;
            PictureBig = pictureBig;
            PictureXL = pictureXL;
        }
        #endregion

    }
}
