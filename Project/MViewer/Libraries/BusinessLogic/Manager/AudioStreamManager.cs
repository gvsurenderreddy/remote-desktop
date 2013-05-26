
using Alvas.Audio;
using AudioStreaming;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Utils;

namespace GenericObjects
{
    public class AudioStreamManager : IAudioStreamManager
    {
        #region private members

        ManualResetEvent _syncAudioInstance = new ManualResetEvent(false);
        AudioStream _audioStream;
        EventHandler _onCaptureAvailable;
        int _timerInterval;
        ManualResetEvent _syncCaptures = new ManualResetEvent(true);
        readonly object _syncPlay = new object();

        #endregion

        #region c-tor

        public AudioStreamManager(int timerInterval, EventHandler onCaptureAvailable)
        {
            _timerInterval = timerInterval;
            _onCaptureAvailable = onCaptureAvailable;
        }

        #endregion

        #region private methods
       
        void PlayCapture(byte[] capture)
        {
            try
            {
                if (capture == null || capture.Length == 0)
                {
                    return;
                }

                byte[] resampled = Resample(capture);

                SoundEffect sound = new SoundEffect(resampled, Microphone.Default.SampleRate, AudioChannels.Mono);
                //sound = Content
                SoundEffect.MasterVolume = 1f;

                sound.Play();

                Tools.Instance.Logger.LogInfo("played capture of " + resampled.Length + " bytes");

                Thread.Sleep(2100);
                
                sound.Dispose();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                Tools.Instance.Logger.LogInfo("play capture exit");

                GC.Collect();
            }
        }

        byte[] Resample(byte[] receivedCapture)
        {
            MemoryStream ms = new MemoryStream();
            MemoryStream input = new MemoryStream(receivedCapture);
            IntPtr formatNew = AudioCompressionManager.GetPcmFormat(2, 16, 44100);
            for (int i = 0; i < 100; i++)
            {
                WaveReader wr = new WaveReader(input);

                IntPtr format = wr.ReadFormat();

                byte[] data = wr.ReadData();

                wr.Close();

                byte[] dataNew = AudioCompressionManager.Resample(format, data, formatNew);
                ms.Write(dataNew, 0, dataNew.Length);

            }
            return ms.GetBuffer();
        }

        void OnAudioReady(object sender, EventArgs e)
        {
            try
            {
                _syncCaptures.WaitOne();
                _syncAudioInstance.WaitOne();

                AudioCaptureEventArgs eventArgs = (AudioCaptureEventArgs)e;
                byte[] capture = eventArgs.Capture;
    
                if (capture != null && capture.Length > 0)
                {
                    _onCaptureAvailable.BeginInvoke(this, eventArgs
                   , null, null);
                }
                else
                {
                    if (_audioStream.IsRunning)
                    {
                        _audioStream.StartAudio();
                    }
                }

            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        public void WaitRoomButtonAction(bool wait)
        {
            try
            {
                if (wait)
                {
                    _syncCaptures.Reset();
                }
                else
                {
                    _syncCaptures.Set();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StartStreaming()
        {
            try
            {
                if (_audioStream == null)
                {
                    Thread t = new Thread(delegate()
                    {
                        try
                        {
                            _syncAudioInstance.Reset();

                            _audioStream = new AudioStream(this.OnAudioReady);

                            _syncAudioInstance.Set();

                            _audioStream.Run();
                        }
                        catch (Exception ex)
                        {
                            Tools.Instance.Logger.LogError(ex.ToString());
                        }
                    });
                    t.Start();
                }
                else
                {
                    if (_audioStream.IsRunning == false)
                    {
                        _audioStream.StartAudio();
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StopStreaming()
        {
            try
            {
                _syncCaptures.Reset();
                if (_audioStream != null)
                {
                    _audioStream.StopAudio();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                _syncCaptures.Set();
            }
        }

        public void PlayAudioCapture(byte[] capture)
        {
            lock (_syncPlay)
            {
                Thread t = new Thread(delegate()
                {
                    PlayCapture(capture);
                });
                t.Start();
            }
        }

        #endregion

        public bool AudioCaptureClosed
        {
            get
            {
                return _audioStream == null? true : _audioStream.IsRunning == false;
            }
        }
    }
}
