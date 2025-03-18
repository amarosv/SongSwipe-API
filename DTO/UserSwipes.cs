using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserSwipes
    {
        #region Atributos
        private User _user;
        private List<int> _likedTracks;
        private List<int> _dislikedTracks;
        #endregion

        #region Propiedades
        public User User { 
            get { return _user; } 
            set { _user = value; } 
        }

        public List<int> LikedTracks
        {
            get { return _likedTracks; }
            set { _likedTracks = value; }
        }

        public List<int> DislikedTracks
        {
            get { return _dislikedTracks; }
            set { _dislikedTracks = value; }
        }
        #endregion

        #region Constructores
        public UserSwipes() { }

        public UserSwipes(User user, List<int> likedTracks, List<int> dislikedTracks)
        {
            _user = user;
            _likedTracks = likedTracks;
            _dislikedTracks = dislikedTracks;
        }
        #endregion
    }
}
