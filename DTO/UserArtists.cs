using Entidades;

namespace DTO
{
    public class UserArtists
    {
        #region Atributos
        private Usuario _user;
        private List<int> _artists;
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

        public List<int> Artists
        {
            get { return _artists; }
            set
            {
                if (value != null)
                {
                    _artists = value;
                }
            }
        }
        #endregion

        #region Constructores
        public UserArtists() { }

        public UserArtists(Usuario user, List<int> artists)
        {
            if (user != null)
            {
                _user = user;
            }

            if (artists != null)
            {
                _artists = artists;
            }
        }
        #endregion
    }
}
