using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using System.Windows.Forms;
using GenericObjects;
using UIControls;
using Utils;
using Abstraction;

namespace MViewer
{
    public partial class Controller : IController
    {
        #region public methods

        /// <summary>
        /// method used to send a file to specific partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void SendFileHandler(object sender, EventArgs args)
        {
            try
            {
                RoomActionEventArgs e = (RoomActionEventArgs)args;
                if (_model.ClientController.IsContactOnline(e.Identity))
                {
                    Contact contact = (Contact)_model.GetContact(e.Identity);
                    string filePath = string.Empty;
                    FileDialog fileDialog = new OpenFileDialog();
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = fileDialog.FileName;

                        FileInfo fileInfo = new FileInfo(filePath);
                        if (fileInfo.Length <= 10485760)
                        {

                            FormFileProgress fileProgressFrom = null;

                            Thread t = new Thread(delegate()
                            {
                                try
                                {
                                    fileProgressFrom = new FormFileProgress(
                                        Path.GetFileName(filePath), contact.FriendlyName);
                                    fileProgressFrom.ChangeLanguage(_language);
                                    Application.Run(fileProgressFrom);
                                }
                                catch (Exception ex)
                                {
                                    Tools.Instance.Logger.LogError(ex.ToString());
                                }
                            });
                            t.Start();
                            Thread.Sleep(500);
                            Thread t2 = new Thread(delegate()
                            {
                                try
                                {
                                    fileProgressFrom.StartPB();
                                }
                                catch (Exception ex)
                                {
                                    Tools.Instance.Logger.LogError(ex.ToString());
                                }
                            });
                            t2.Start();
                            _model.SendFile(filePath, e.Identity);

                            if (fileProgressFrom != null)
                            {
                                fileProgressFrom.StopProgress();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sorry, cannot transfer files larger than 10 MB in MViewer-lite", "Transfer not possible", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Selected person is offline", "Cannot send", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region private methods

        void FileTransferObserver(object sender, EventArgs e)
        {
            Thread t = new Thread(delegate()
            {
                // add a progress bar (into a TransfersForm)
                FormFileProgress fileProgressFrom = null;
                try
                {
                    RoomActionEventArgs args = (RoomActionEventArgs)e;
                    TransferInfo transferInfo = (TransferInfo)args.TransferInfo;
                    byte[] buffer = (byte[])sender; // this is the file sent

                    // open file path dialog
                    string extension = Path.GetExtension(transferInfo.FileName);// get file extension

                    // Displays a SaveFileDialog so the user can save the Image
                    // assigned to Button2.
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "File|*." + extension + "";
                    saveFileDialog1.Title = "Save File";
                    saveFileDialog1.FileName = transferInfo.FileName;
                    DialogResult dialogResult = saveFileDialog1.ShowDialog();

                    // If the file name is not an empty string open it for saving.
                    if (dialogResult == DialogResult.OK && saveFileDialog1.FileName != "")
                    {
                        // remove the existing file if the user confirmed
                        if (File.Exists(saveFileDialog1.FileName))
                        {
                            File.Delete(saveFileDialog1.FileName);
                        }

                        
                        Contact contact = (Contact)_model.GetContact(args.Identity);
                        Thread t3 = new Thread(delegate()
                        {
                            try
                            {
                                fileProgressFrom = new FormFileProgress(
                                    Path.GetFileName(saveFileDialog1.FileName), contact.FriendlyName);
                                fileProgressFrom.ChangeLanguage(_language);
                                Application.Run(fileProgressFrom);
                            }
                            catch (Exception ex)
                            {
                                Tools.Instance.Logger.LogError(ex.ToString());
                            }
                        });
                        t3.Start();
                        Thread.Sleep(500);
                        Thread t2 = new Thread(delegate()
                        {
                            try
                            {
                                fileProgressFrom.StartPB();
                            }
                            catch (Exception ex)
                            {
                                Tools.Instance.Logger.LogError(ex.ToString());
                            }
                        });
                        t2.Start();

                        // Saves the Image via a FileStream created by the OpenFile method.
                        System.IO.FileStream fs =
                           (System.IO.FileStream)saveFileDialog1.OpenFile();
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Close();

                        if (fileProgressFrom != null)
                        {
                            fileProgressFrom.StopProgress();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
                finally
                {
                    if (fileProgressFrom != null)
                    {
                        fileProgressFrom.Close();
                    }
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

        void FileTransferPermission(object sender, EventArgs e)
        {
            try
            {
                RoomActionEventArgs args = (RoomActionEventArgs)e;
                TransferInfo transferInfo = (TransferInfo)args.TransferInfo;
                bool canSend = _view.RequestTransferPermission(args.Identity, transferInfo.FileName, transferInfo.FileSize);
                transferInfo.HasPermission = canSend;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}
