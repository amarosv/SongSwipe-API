using System;

namespace DTO
{
    public class UserProfile
    {
        #region Atributos
        private string _uid;
        private string _username;
        private string _name;
        private string _lastName;
        private string _photoUrl;
        private string _dateJoining;
        private string _email;
        private int _savedSongs;
        private int _followers;
        private int _following;
        #endregion

        #region Propiedades
        public string UID
        {
            get { return _uid; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _uid = value;
                }
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _username = value;
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _name = value;
                }
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _lastName = value;
                }
            }
        }

        public string PhotoUrl
        {
            get { return _photoUrl; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _photoUrl = value;
                }
            }
        }

        public string DateJoining
        {
            get { return _dateJoining; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _dateJoining = value;
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _email = value;
                }
            }
        }

        public int SavedSongs
        {
            get { return _savedSongs; }
            set
            {
                if (value >= 0)
                {
                    _savedSongs = value;
                }
            }
        }

        public int Followers
        {
            get { return _followers; }
            set
            {
                if (value >= 0)
                {
                    _followers = value;
                }
            }
        }

        public int Following
        {
            get { return _following; }
            set
            {
                if (value >= 0)
                {
                    _following = value;
                }
            }
        }
        #endregion

        #region Constructores
        public UserProfile() { }

        public UserProfile(string uid, string username, string name, string lastName, string photoUrl, string dateJoining, string email, int savedSongs, int followers, int following)
        {
            UID = uid;
            Username = username;
            Name = name;
            LastName = lastName;
            PhotoUrl = photoUrl;
            DateJoining = dateJoining;
            Email = email;
            SavedSongs = savedSongs;
            Followers = followers;
            Following = following;
        }
        #endregion
    }
}
