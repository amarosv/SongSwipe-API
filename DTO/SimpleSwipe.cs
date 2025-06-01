using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SimpleSwipe
    {
        #region Propiedades
        public int Like { get; set; }
        public long Id { get; set; }
        #endregion

        #region Constructores
        public SimpleSwipe() { }

        public SimpleSwipe(int like, long id)
        {
            Like = like;
            Id = id;
        }
        #endregion
    }
}
