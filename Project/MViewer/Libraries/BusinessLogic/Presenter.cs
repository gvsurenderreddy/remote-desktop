using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using System.Threading;
using Utils;
using Abstraction;
using DesktopSharing;

namespace GenericObjects
{
    public class Presenter : IPresenter
    {
        #region private members

        IWebcamCapture _videoCapture;
        IScreenCaptureTool _screenCaptureTool;
        bool _firstTimeCapturing;
        IAudioStreamManager _audioStreamManager;
        PresenterSettings _presenterSettings;

        #endregion

        #region c-tor

        public Presenter(PresenterSettings presenterSettings)
        {
            try
            {
                _presenterSettings = presenterSettings;
                _videoCapture = _presenterSettings.VideoCaptureControl;
                _audioStreamManager = new AudioStreamManager(_presenterSettings.AudioTimerInterval, 
                    _presenterSettings.OnAudioCaptureAvailable);

                // initialize the image capture size
                if (_videoCapture != null)
                {
                    _videoCapture.ImageHeight = presenterSettings.VideoScreenSize.Height;
                    _videoCapture.ImageWidth = presenterSettings.VideoScreenSize.Width;

                    _firstTimeCapturing = true;

                    // bind the image captured event
                    _videoCapture.ImageCaptured += new Delegates.WebCamEventHandler(presenterSettings.OnVideoImageCaptured);
                }

                _screenCaptureTool = new ScreenCaptureTool(_presenterSettings.RemotingTimerInterval, 
                    _presenterSettings.OnRemotingImageCaptured);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region IPresenter Members

        public void StartVideoPresentation()
        {
            try
            {
                if (_videoCapture == null)
                {
                    _videoCapture = _presenterSettings.VideoCaptureControl;
                    _videoCapture.ImageHeight = _presenterSettings.VideoScreenSize.Height;
                    _videoCapture.ImageWidth = _presenterSettings.VideoScreenSize.Width;
                    // bind the image captured event
                    _videoCapture.ImageCaptured += new Delegates.WebCamEventHandler(_presenterSettings.OnVideoImageCaptured);
                }
                // start the video capturing
                _videoCapture.StartCapturing(_firstTimeCapturing);
                _firstTimeCapturing = false;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StopVideoPresentation()
        {
            try
            {
                if (_videoCapture != null)
                {
                    _videoCapture.StopCapturing();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StartAudioPresentation()
        {
            try
            {
                if (_audioStreamManager.AudioCaptureClosed)
                {
                    _audioStreamManager.StartStreaming();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StopAudioPresentation()
        {
            try
            {
                _audioStreamManager.StopStreaming();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void FreezeAudio(bool wait)
        {
            try
            {
                _audioStreamManager.WaitRoomButtonAction(wait);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void FreezeRemoting(bool wait)
        {
            try
            {
                _screenCaptureTool.WaitRoomButtonAction(wait);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StartRemotingPresentation()
        {
            try
            {
                _screenCaptureTool.TogglerTimer(true);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StopRemotingPresentation()
        {
            try
            {
                _screenCaptureTool.TogglerTimer(false);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void PlayAudioCapture(byte[] capture, string senderIdentity, double captureLengthInSeconds)
        {
            try
            {
                _audioStreamManager.PlayAudioCapture(capture, senderIdentity, captureLengthInSeconds);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region proprieties

        public bool RemotingCaptureClosed()
        {
            return _screenCaptureTool.RemotingCaptureClosed;
        }

        public bool AudioCaptureClosed
        {
            get
            {
                return _audioStreamManager.AudioCaptureClosed;
            }
        }

        #endregion
    }
}
