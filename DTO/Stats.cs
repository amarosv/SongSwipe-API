using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Stats
    {
        #region Propiedades
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Swipes { get; set; }
        #endregion

        #region Constructores
        public Stats() { }

        public Stats(int likes, int dislikes, int swipes)
        {
            Likes = likes;
            Dislikes = dislikes;
            Swipes = swipes;
        }
        #endregion
    }
}
