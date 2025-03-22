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
        }

        public List<int> Artists
        {
            get { return _artists; }
        }
        #endregion

        #region Constructores
        public UserArtists() {}

        public UserArtists(Usuario user, List<int> artists) { 
            _user = user;
            _artists = artists;
        }
        #endregion
    }
}
