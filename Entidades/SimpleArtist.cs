using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class SimpleArtist
    {
        #region Propiedades
        // El nombre de las variables están en minúsculas para garantizar la compatibilidad con el JSON de Deezer

        public long id { get; set; }
        public String name { get; set; }
        public String link { get; set; }
        public String share {  get; set; }
        public String picture { get; set; }
        public String picture_small { get; set; }
        public String picture_medium { get; set; }
        public String picture_big { get; set; }
        public String picture_xl { get; set; }
        public bool radio { get; set; }
        public String tracklist { get; set; }
        public String type { get; set; }
        #endregion
    }
}
