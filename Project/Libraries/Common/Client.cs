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
        System.Timers.Timer _timer;

        #endregion

        #region c-tor

        public Client()
        {
            _connected = false;
            _id = -1;
        }

        public Client(int id, int timerInterval)
        {
            _id = id;
            _connected = false;
            _timer = new System.Timers.Timer();
            _timer.Interval = timerInterval;
        }

        public Client(int timerInterval)
        {
            _connected = false;
            _id = -1;
            _timer = new System.Timers.Timer();
            _timer.Interval = timerInterval;
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
