using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopSharingViewer;
using DesktopSharing;
using System.Windows.Controls;
using System.Threading;

namespace WpfRemotingClient
{
    public class DesktopViewer : IDesktopViewer
    {
        #region members

        ViewerContext _context;
        CommandQueue _commandQueue;
        public delegate void ImageChangedEventHandler(Image display);
        event ImageChangedEventHandler OnImageChanged;

        //Thread _worker;
        //bool _stopThread;

        #endregion

        #region c-tor

        public DesktopViewer(ImageChangedEventHandler imageChangedHandler)
        {
            _commandQueue = new CommandQueue();
            OnImageChanged += imageChangedHandler;
        }

        #endregion

        #region methods

        //public void StartViewer()
        //{
        //    _stopThread = true;
        //    _worker.Join();
        //}

        //public void StopViewer()
        //{
        //    _worker = new Thread(WorkerThread);
        //    _worker.Start();
        //}

        //private void WorkerThread()
        //{

        //}

        #endregion

        #region IDesktopViewer Members

        public void UpdateDesktop(byte[] desktop)
        {
            throw new NotImplementedException();
        }

        public string UpdateMouse(byte[] mouse)
        {
            throw new NotImplementedException();
        }

        public string Ping()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
