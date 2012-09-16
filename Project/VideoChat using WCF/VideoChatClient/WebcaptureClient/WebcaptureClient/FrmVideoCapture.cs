using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using WebcamCaptureLib;
using AudioCaptureLib;
using System.Threading;
using System.ServiceModel.Security;
using System.ServiceModel.Channels;

namespace VideoChatClient
{
    public partial class FrmVideoCapture : Form
    {
        #region private members
        
        VideoChatRoomClient _webcamClient;

        X509Certificate2 cert = new X509Certificate2("c:\\Client.pfx");

        string _serverAddress = "https://Mihai-PC:8002/WebcaptureService";

        WebcamCapture _webcamCapture;
        AudioCapture _audioCapture;
        int _timerInterval;

        #endregion

        #region c-tor

        public FrmVideoCapture(string serverAddress, int timerInterval)
        {
            InitializeComponent();

            _serverAddress = serverAddress;

            _timerInterval = timerInterval;

            // initialize the webcam capture obj
            _webcamCapture = new WebcamCapture(_timerInterval, this.Handle.ToInt32());
            _audioCapture = new AudioCapture(new AudioCapture.AudioEventHandler(this.AudioSoundCaptured));

            // initialize the image capture size
            _webcamCapture.ImageHeight = pbCapture.Height;
            _webcamCapture.ImageWidth = pbCapture.Width;

            // bind the image captured event
            _webcamCapture.ImageCaptured += new WebcamCapture.WebCamEventHandler(this.WebCamImageCaptured);

            InitializeProxy();

            StartVideoChat();
        }

        #endregion

        #region public methods

        #endregion

        #region private methods

        void StartVideoChat()
        {
            // update the capture timespan
            _webcamCapture.CaptureTimespan = _timerInterval;

            // start the video capturing
            _webcamCapture.StartCapturing();

            // Start Audio Streaming
            _audioCapture.StartAudioStreaming();
        }

        void StopVideoChat()
        {
            // stop the video capturing
            _webcamCapture.StopCapturing();
            _audioCapture.StopRecording();
        }

        void InitializeProxy()
        {
            //_webcamClient = new VideoChatRoomClient();

            WSHttpBinding binding = CreateServerBinding();
            EndpointAddress endpoint = CreateServerEndpoint();

            _webcamClient = new VideoChatRoomClient(binding, endpoint);

            ContractDescription contract = ContractDescription.GetContract(typeof(IVideoChatRoom), typeof(VideoChatRoomClient));
            _webcamClient.Endpoint.Contract = contract;
            _webcamClient.Endpoint.Binding = CreateServerBinding();
            _webcamClient.Endpoint.Binding.Name = "binding1_IVideoChatRoom";
            







            //IdentityClaim 
            //System.IdentityModel.Tokens.EncryptedKeyIdentifierClause clause = 
            //    new System.IdentityModel.Tokens.EncryptedKeyIdentifierClause(

            //_webcamClient.Endpoint.Address.Identity.IdentityClaim

            //_webcamClient.ClientCredentials.ClientCertificate.SetCertificate(

            _webcamClient.ClientCredentials.ClientCertificate.Certificate = cert;
        }

        WSHttpBinding CreateServerBinding()
        {
            //WSHttpBinding binding = new WSHttpBinding(SecurityMode.Message, true);
            WSHttpBinding binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpsBinding();
            //WSHttpBinding binding = (WSHttpBinding)MetadataExchangeBindings.CreateMexHttpBinding();
   
            binding.CloseTimeout = new TimeSpan(0, 1, 0);
            binding.OpenTimeout = new TimeSpan(0, 1, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 1, 0);

            binding.MaxBufferPoolSize = 100000000;
            binding.ReaderQuotas.MaxArrayLength = 100000000;
            binding.ReaderQuotas.MaxStringContentLength = 100000000;
            binding.ReaderQuotas.MaxBytesPerRead = 100000000;
            binding.MaxReceivedMessageSize = 100000000;

            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.Security.Mode = SecurityMode.Message;

            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Transport.Realm = string.Empty;

            binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
            binding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.NegotiateServiceCredential = false;

            binding.Name = "binding1_IVideoChatRoom";

            binding.ReliableSession.Ordered = true;
            binding.ReliableSession.InactivityTimeout = TimeSpan.FromMinutes(10);
            binding.ReliableSession.Enabled = false;

            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.TransactionFlow = false;
            binding.UseDefaultWebProxy = true;
            binding.TextEncoding = Encoding.UTF8;
            binding.MessageEncoding = WSMessageEncoding.Text;

            return binding;
        }

        EndpointAddress CreateServerEndpoint()
        {
            Uri uri = new Uri(_serverAddress);
            X509Certificate2 serverCert = new X509Certificate2("c:\\server2.cer");
            EndpointIdentity identity = EndpointIdentity.CreateX509CertificateIdentity(serverCert);
            EndpointAddress endpoint = new EndpointAddress(uri, identity);

            //EndpointAddress endpoint = new EndpointAddress(uri);

            return endpoint;
        }

        #endregion

        #region capture handlers

        private void AudioSoundCaptured(object source, AudioEventArgs e)
        {
            try
            {
                _webcamClient.SendMicrophoneCapture(e.CapturedSound);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// An image was capture
        /// </summary>
        /// <param name="source">control raising the event</param>
        /// <param name="e">WebCamEventArgs</param>
        private void WebCamImageCaptured(object source, CaptureEventArgs e)
        {
            try
            {
                // display the captured picture
                pbCapture.Image = e.CapturedImage;

                // send to server

                byte[] bytes = ImageConverter.imageToByteArray(e.CapturedImage);

                //while (!_audioCapture.AudioCaptureReady)
                //{
                //    Thread.Sleep(200);
                //}

                _webcamClient.SendWebcamCapture(bytes);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                _webcamCapture.StopCapturing();
            }
        }

        #endregion

        #region proprieties

        #endregion
    }
}
