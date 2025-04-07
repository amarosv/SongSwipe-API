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
        private Usuario _user;
        private List<int> _likedTracks;
        private List<int> _dislikedTracks;
        #endregion

        #region Propiedades
        public Usuario User
        {
            get { return _user; }
            set
            {
                if (value != null)
                {
                    _user = value;
                }
            }
        }

        public List<int> LikedTracks
        {
            get { return _likedTracks; }
            set
            {
                if (value != null)
                {
                    _likedTracks = value;
                }
            }
        }

        public List<int> DislikedTracks
        {
            get { return _dislikedTracks; }
            set
            {
                if (value != null)
                {
                    _dislikedTracks = value;
                }
            }
        }
        #endregion

        #region Constructores
        public UserSwipes() { }

        public UserSwipes(Usuario user, List<int> likedTracks, List<int> dislikedTracks)
        {
            if (user != null)
            {
                _user = user;
            }

            if (likedTracks != null)
            {
                _likedTracks = likedTracks;
            }

            if (dislikedTracks != null)
            {
                _dislikedTracks = dislikedTracks;
            }
        }
        #endregion
    }
}
