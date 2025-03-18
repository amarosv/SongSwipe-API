using Entidades;

namespace DTO
{
    public class UserArtists
    {
        #region Atributos
        private User _user;
        private List<int> _artists;
        #endregion

        #region Propiedades
        public User User
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

        public UserArtists(User user, List<int> artists) { 
            _user = user;
            _artists = artists;
        }
        #endregion
    }
}
