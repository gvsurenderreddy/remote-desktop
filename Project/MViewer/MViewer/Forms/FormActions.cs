using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UIControls;
using Utils;
using GenericObjects;
using System.Threading;

namespace MViewer
{
    public partial class FormActions : Form
    {
        #region private members

        public readonly Delegates.ActionsEventHandler ActionsObserver;
        EventHandler _roomActionEventHandler;
        public delegate void UpdateLabelsDel(bool start, bool pause, GenericEnums.RoomType roomType);
        public UpdateLabelsDel myDelegate;

        #endregion

        #region c-tor

        public FormActions()
        {
            try
            {
                InitializeComponent();
                myDelegate = new UpdateLabelsDel(actionsControl.UpdateLabels);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public FormActions(EventHandler roomActionEventHandler)
        {
            try
            {
                InitializeComponent();
                _roomActionEventHandler = roomActionEventHandler;
                ActionsObserver = new Delegates.ActionsEventHandler(OnActionTriggered);
                myDelegate = new UpdateLabelsDel(actionsControl.UpdateLabels);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region event callbacks

        private void FormActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // this form should not be closed while the app is running
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void OnActionTriggered(object sender, EventArgs e)
        {
            try
            {
                // use the Controller and take specific action when event has been triggered using the Actions control
                _roomActionEventHandler.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        public void ChangeLanguage(string language)
        {
            try
            {
                actionsControl.ChangeLanguage(language);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void UpdateLabels(bool start, bool pause, GenericEnums.RoomType roomType)
        {
            try
            {
                bool retry = true;
                while (retry)
                {
                    try
                    {
                        this.Invoke(myDelegate, start, pause, roomType);
                        retry = false;
                    }
                    catch (Exception)
                    {
                        retry = true;
                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                // todo: fix the actions form focus issue
                //if (!this.Focused)
                //{
                //    this.Invoke(new MethodInvoker(delegate()
                //    {
                //        this.TopMost = true;
                //        this.BringToFront();
                //        this.Activate();
                //    }));
                //}
            }
        }

        #endregion
    }
}
