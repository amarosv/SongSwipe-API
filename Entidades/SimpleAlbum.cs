using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class SimpleAlbum
    {
        #region Propiedades
        // El nombre de las variables están en minúsculas para garantizar la compatibilidad con el JSON de Deezer

        public long id { get; set; }
        public String title { get; set; }
        public String link { get; set; }
        public String cover { get; set; }
        public String cover_small { get; set; }
        public String cover_medium { get; set; }
        public String cover_big { get; set; }
        public String cover_xl { get; set; }
        public String md5_image { get; set; }
        public String release_date { get; set; }
        public String tracklist { get; set; }
        public String type { get; set; }
        #endregion
    }
}
