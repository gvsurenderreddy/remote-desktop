
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
        System.Timers.Timer _timer;
        AudioStream _audioStream;
        EventHandler _onCaptureAvailable;
        int _timerInterval;
        ManualResetEvent _syncCaptures = new ManualResetEvent(true);
        SoundEffect sound;
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
                sound = new SoundEffect(capture, Microphone.Default.SampleRate, AudioChannels.Mono);
                //sound = Content
                SoundEffect.MasterVolume = 1f;

                //todo: remove this log
                Tools.Instance.Logger.LogInfo("playing capture of " + capture.Length + " bytes");

                sound.Play();

                Tools.Instance.Logger.LogInfo("played capture of " + capture.Length + " bytes");

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

        void OnAudioReady(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                _syncCaptures.WaitOne();
                _syncAudioInstance.WaitOne();

                _audioStream.SyncChunk.Reset();

                byte[] capture = _audioStream.Stream != null ? _audioStream.Stream.GetBuffer() : new byte[0];
                _audioStream.SyncChunk.Set();

                _audioStream.Stream = new MemoryStream();

                if (capture != null && capture.Length > 0)
                {
                    _onCaptureAvailable.Invoke(this, new AudioCaptureEventArgs()
                    {
                        Capture = capture,
                        CaptureTimestamp = DateTime.Now
                    });
                }
                else
                {
                    if (_audioStream.IsRunning)
                    {
                        //_audioStream.StopAudio();
                        _audioStream.StartAudio();
                    }
                }

            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                if (_timer != null && _timer.Enabled == false)
                {
                    _timer.Start();
                }
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
                if (_timer == null)
                {
                    _timer = new System.Timers.Timer(_timerInterval);
                    _timer.Elapsed += new ElapsedEventHandler(OnAudioReady);
                }

                if (_audioStream == null)
                {
                    Thread t = new Thread(delegate()
                    {
                        try
                        {
                            _syncAudioInstance.Reset();

                            _audioStream = new AudioStream();

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
                _timer.Start();
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
                if (_timer != null)
                {
                    _timer.Stop();
                }
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
