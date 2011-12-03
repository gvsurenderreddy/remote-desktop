using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [Serializable]
    public class Client
    {
        #region members

        private int _id;
        bool _connected;

        #endregion

        #region c-tor

        public Client(int id)
        {
            _id = id;
            _connected = false;
        }

        public Client()
        {
            _connected = false;
            _id = -1;
        }

        #endregion

        #region methods



        #endregion

        #region proprieties

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool Connected
        {
            get { return _connected; }
            set { _connected = value; }
        }

        #endregion
    }
}
