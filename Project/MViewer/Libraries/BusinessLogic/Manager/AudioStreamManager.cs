
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

        #endregion

        #region c-tor

        public AudioStreamManager(int timerInterval, EventHandler onCaptureAvailable)
        {
            _timerInterval = timerInterval;
            _onCaptureAvailable = onCaptureAvailable;
        }

        #endregion

        #region private methods

        void OnAudioReady(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();

                _syncAudioInstance.WaitOne();
                Thread.Sleep(200);

                _audioStream.SyncChunk.Reset();

                byte[] capture = _audioStream.Stream != null ? _audioStream.Stream.GetBuffer() : new byte[0];

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
                _audioStream.SyncChunk.Set();

                _timer.Start();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

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
        }
        SoundEffect sound;

        readonly object _syncPlay = new object();

        public void PlayAudioCapture(byte[] capture)
        {
            lock (_syncPlay)
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
                    sound.Play();
                    Thread.Sleep(2100);
                    sound.Dispose();
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
                finally
                {
                    GC.Collect();
                }
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
