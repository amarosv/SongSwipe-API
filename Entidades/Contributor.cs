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
        public long Id { get; set; }
        public String Name { get; set; }
        public String Link { get; set; }
        public String Share { get; set; }
        public String Picture { get; set; }
        public String Picture_Small { get; set; }
        public String Picture_Medium { get; set; }
        public String Picture_XL { get; set; }
        public bool Radio { get; set; }
        public String TrackList { get; set; }
        public String Type { get; set; }
        public String Role { get; set; }
        #endregion

        #region Constructores
        public Contributor() { }

        public Contributor(long id, string name, string link, string share, string picture, string picture_Small, string picture_Medium, string picture_XL, bool radio, string trackList, string type, string role)
        {
            Id = id;
            Name = name;
            Link = link;
            Share = share;
            Picture = picture;
            Picture_Small = picture_Small;
            Picture_Medium = picture_Medium;
            Picture_XL = picture_XL;
            Radio = radio;
            TrackList = trackList;
            Type = type;
            Role = role;
        }
        #endregion
    }
}
