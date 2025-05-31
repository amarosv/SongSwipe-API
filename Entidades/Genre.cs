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
        // El nombre de las variables están en minúsculas para garantizar la compatibilidad con el JSON de Deezer

        public long id { get; set; }
        public String name { get; set; }
        public String picture { get; set; }
        public String picture_small { get; set; }
        public String picture_medium { get; set; }
        public String picture_big { get; set; }
        public String picture_xl { get; set; }
        #endregion

        #region Constructores
        public Genre() { }

        public Genre(long id, string name, string picture, string pictureSmall, string pictureMedium, string pictureBig, string pictureXL)
        {
            this.id = id;
            this.name = name;
            this.picture = picture;
            picture_small = pictureSmall;
            picture_medium = pictureMedium;
            picture_big = pictureBig;
            picture_xl = pictureXL;
        }
        #endregion

    }
}
