using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Contributor
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
        public String picture_xl { get; set; }
        public bool radio { get; set; }
        public String tracklist { get; set; }
        public String type { get; set; }
        public String role { get; set; }
        #endregion

        #region Constructores
        public Contributor() { }

        public Contributor(long id, string name, string link, string share, string picture, string picture_Small, string picture_Medium, string picture_XL, bool radio, string trackList, string type, string role)
        {
            this.id = id;
            this.name = name;
            this.link = link;
            this.share = share;
            this.picture = picture;
            picture_small = picture_Small;
            picture_medium = picture_Medium;
            picture_xl = picture_XL;
            this.radio = radio;
            tracklist = trackList;
            this.type = type;
            this.role = role;
        }
        #endregion
    }
}
