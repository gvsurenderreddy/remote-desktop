using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopSharingViewer;
using DesktopSharing;
using System.Threading;
using System.Drawing;

namespace DesktopSharingViewer
{
    public class DesktopViewer : IDesktopViewer
    {
        #region members

        readonly Dictionary<Guid, ViewerContext> _contexts = new Dictionary<Guid, ViewerContext>();
        CommandQueue _commandQueue;
        public delegate void DesktopChangedEventHandler(System.Drawing.Image display, string remoteIp);
        event DesktopChangedEventHandler OnDesktopChanged;
        

        //Thread _worker;
        //bool _stopThread;

        #endregion

        #region c-tor

        public DesktopViewer(DesktopChangedEventHandler imageChangedHandler)
        {
            _commandQueue = new CommandQueue();
            _contexts = new Dictionary<Guid, ViewerContext>();
            OnDesktopChanged += imageChangedHandler;
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

        void UpdateDisplay(Guid id)
        {
            ViewerContext viewContext = _contexts[id];
            if (viewContext == null)
            {
                return;
            }
            if (viewContext.Display != null)
            {
                if (viewContext.Mouse != null)
                {
                    viewContext.Display = DesktopViewerUtils.AppendMouseToDesktop(viewContext.Display, viewContext.Mouse, viewContext.CursorX, viewContext.CursorY);
                }
                else
                {
                    viewContext.Display = viewContext.Display;
                }

                if (OnDesktopChanged != null)
                {
                    OnDesktopChanged(viewContext.Display, viewContext.Id.ToString());
                }
            }
        }

        public void UpdateDesktop(byte[] desktop)
        {
            if (desktop != null)
            {
                // deserialize the data
                System.Drawing.Image partialDesktop;
                Rectangle rect;
                Guid id;
                DesktopViewerUtils.Deserialize(desktop, out partialDesktop, out rect, out id);

                // Update the current desktop
                ViewerContext viewContext;
                if (_contexts.ContainsKey(id))
                {
                    
                    _contexts.Remove(id);
                }
                // create new viewer context
                viewContext = new ViewerContext(id);
                System.Drawing.Image img = null;

                DesktopViewerUtils.UpdateScreen(ref img, partialDesktop, rect);
                viewContext.Display = img;
                _contexts[id] = viewContext;
                
                UpdateDisplay(id);
            }
        }

        public string UpdateMouse(byte[] mouse)
        {
            if (mouse != null)
            {
                // Unpack the data.
                //
                System.Drawing.Image cursor;
                int cursorX, cursorY;
                Guid id;
                DesktopViewerUtils.Deserialize(mouse, out cursor, out cursorX, out cursorY, out id);

                // Update the current screen
                //
                ViewerContext viewContext;
                if (!_contexts.ContainsKey(id))
                {
                    // Create a new session.
                    //
                    viewContext = new ViewerContext(id);
                    _contexts[id] = viewContext;
                }
                else
                {
                    viewContext = _contexts[id];
                }
                viewContext.Mouse = cursor;
                viewContext.CursorX = cursorX;
                viewContext.CursorY = cursorY;
                UpdateDisplay(id);
            }

            return _commandQueue.Serialize();
        }

        public string Ping()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
