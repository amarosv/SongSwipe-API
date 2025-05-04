using System.Globalization;

namespace Entidades
{
    public class Usuario
    {
        #region Atributos
        private String _uid;
        private String _name;
        private String _lastName;
        private String _email;
        private String _photoURL;
        private String _dateJoining;
        private String _username;
        private String _supplier;
        private bool _userDeleted;
        private bool _userBlocked;
        #endregion

        #region Propiedades
        public String UID
        {
            get
            {
                return _uid;
            }
            set {
                if (!string.IsNullOrEmpty(value)) { 
                    _uid = value;
                }
            }
        }

        public String Name
        {
            get { return _name; }
            set {
                if (!string.IsNullOrEmpty(value)) { 
                    _name = value;
                }
            }
        }

        public String LastName
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

        public String Email
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

        public String PhotoURL
        {
            get { return _photoURL; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _photoURL = value;
                }
            }
        }

        public String DateJoining
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

        public String Username
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

        public String Supplier
        {
            get { return _supplier; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _supplier = value;
                }
            }
        }

        public bool UserDeleted
        {
            get { return _userDeleted; }
            set
            {
                _userDeleted = value;
            }
        }

        public bool UserBlocked
        {
            get { return _userBlocked; }
            set
            {
                _userBlocked = value;
            }
        }
        #endregion

        #region Constructores
        public Usuario() { }

        public Usuario(string uid, string name, string lastName, string email, string photoURL, string dateJoining, string username, string supplier, bool userDeleted, bool userBlocked)
        {
            if (!string.IsNullOrEmpty(uid))
            {
                _uid = uid;
            }

            if (!string.IsNullOrEmpty(name)) {
                _name = name;
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                _lastName = lastName;
            }

            if (!string.IsNullOrEmpty(email))
            {
                _email = email;
            }

            if (!string.IsNullOrEmpty(photoURL)) { 
                _photoURL = photoURL;
            }

            if (!string.IsNullOrEmpty(dateJoining)) { 
                _dateJoining = dateJoining;
            }

            if (!string.IsNullOrEmpty(username)) { 
                _username = username;
            }

            if (!string.IsNullOrEmpty(supplier))
            {
                _supplier = supplier;
            }

            _userDeleted = userDeleted;
            _userBlocked = userBlocked;
        }


        #endregion
    }
}
