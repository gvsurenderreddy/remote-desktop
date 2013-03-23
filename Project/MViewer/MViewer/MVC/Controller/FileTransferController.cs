using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;
using GenericDataLayer;
using UIControls;

namespace MViewer
{
    public partial class Controller : IController
    {
        #region public methods

        public void SendFileHandler(object sender, RoomActionEventArgs args)
        {
            if (_model.ClientController.IsContactOnline(args.Identity))
            {
                Contact contact = _model.GetContact(args.Identity);
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
                            fileProgressFrom = new FormFileProgress(
                                Path.GetFileName(filePath), contact.FriendlyName);

                            Application.Run(fileProgressFrom);

                        });
                        t.Start();
                        Thread.Sleep(500);
                        Thread t2 = new Thread(delegate()
                        {
                            fileProgressFrom.StartPB();
                        });
                        t2.Start();
                        _model.SendFile(filePath, args.Identity);

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

        #endregion

        #region private methods

        void FileTransferObserver(object sender, EventArgs e)
        {
            Thread t = new Thread(delegate()
            {
                RoomActionEventArgs args = (RoomActionEventArgs)e;

                byte[] buffer = (byte[])sender; // this is the file sent

                // open file path dialog
                string extension = Path.GetExtension(args.TransferInfo.FileName);// get file extension

                // Displays a SaveFileDialog so the user can save the Image
                // assigned to Button2.
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "File|*." + extension + "";
                saveFileDialog1.Title = "Save File";
                saveFileDialog1.FileName = args.TransferInfo.FileName;
                DialogResult dialogResult = saveFileDialog1.ShowDialog();

                // If the file name is not an empty string open it for saving.
                if (dialogResult == DialogResult.OK && saveFileDialog1.FileName != "")
                {
                    // remove the existing file if the user confirmed
                    if (File.Exists(saveFileDialog1.FileName))
                    {
                        File.Delete(saveFileDialog1.FileName);
                    }

                    // add a progress bar (into a TransfersForm)
                    FormFileProgress fileProgressFrom = null;
                    Contact contact = _model.GetContact(args.Identity);
                    Thread t3 = new Thread(delegate()
                    {
                        fileProgressFrom = new FormFileProgress(
                            Path.GetFileName(saveFileDialog1.FileName), contact.FriendlyName);

                        Application.Run(fileProgressFrom);

                    });
                    t3.Start();
                    Thread.Sleep(500);
                    Thread t2 = new Thread(delegate()
                    {
                        fileProgressFrom.StartPB();
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

            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

        void FileTransferPermission(object sender, EventArgs e)
        {
            RoomActionEventArgs args = (RoomActionEventArgs)e;
            bool canSend = _view.RequestTransferPermission(args.Identity, args.TransferInfo.FileName, args.TransferInfo.FileSize);
            args.TransferInfo.HasPermission = canSend;
        }

        #endregion
    }
}
