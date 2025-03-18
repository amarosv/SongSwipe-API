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
        public long Id { get; set; }
        public String Title { get; set; }
        public String Link { get; set; }
        public String Cover { get; set; }
        public String Cover_Small { get; set; }
        public String Cover_Medium { get; set; }
        public String Cover_Big { get; set; }
        public String Cover_XL { get; set; }
        public String MD5_Image { get; set; }
        public String Release_Date { get; set; }
        public String TrackList { get; set; }
        public String Type { get; set; }
        #endregion
    }
}
