using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Swipe
    {
        #region Propiedades
        public int Like { get; set; }
        public long Id { get; set; }
        public long IdAlbum { get; set; }
        public long IdArtist { get; set; }
        #endregion

        #region Constructores
        public Swipe() { }

        public Swipe(int like, long id, long idAlbum, long idArtist)
        {
            Like = like;
            Id = id;
            IdAlbum = idAlbum;
            IdArtist = idArtist;
        }
        #endregion
    }
}
