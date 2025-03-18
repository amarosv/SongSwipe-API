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
        public long Id { get; set; }
        public String Name { get; set; }
        public String Link { get; set; }
        public String Share {  get; set; }
        public String Picture { get; set; }
        public String Picture_Small { get; set; }
        public String Picture_Medium { get; set; }
        public String Picture_Big { get; set; }
        public String Picture_XL { get; set; }
        public bool Radio { get; set; }
        public String TrackList { get; set; }
        public String Type { get; set; }
        #endregion
    }
}
