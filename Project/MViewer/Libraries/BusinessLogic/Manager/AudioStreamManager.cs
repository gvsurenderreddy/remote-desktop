
using AudioStreaming;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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

        AudioStream _audioStream;
        EventHandler _onCaptureAvailable;
        int _timerInterval;
        ManualResetEvent _syncCaptures = new ManualResetEvent(true);

        readonly object _syncReceivedCaptures = new object();
        IDictionary<string, IDictionary<DateTime, byte[]>> _captures;

        #endregion

        #region c-tor

        public AudioStreamManager(int timerInterval, EventHandler onCaptureAvailable)
        {
            _timerInterval = timerInterval;
            _onCaptureAvailable = onCaptureAvailable;
            _captures = new Dictionary<string, IDictionary<DateTime, byte[]>>();
        }

        #endregion

        #region private methods

        void AddCapture(string senderIdentity, byte[] toAdd)
        {
            if (_captures.ContainsKey(senderIdentity) == false)
            {
                _captures.Add(senderIdentity, new Dictionary<DateTime, byte[]>());
            }
            _captures[senderIdentity].Add(DateTime.Now, toAdd);
        }

        byte[] PopOldestCapture(string senderIdentity)
        {
            byte[] oldest = _captures[senderIdentity][_captures[senderIdentity].Keys.Min()];
            try
            {
                _captures[senderIdentity].Remove(_captures[senderIdentity].Keys.Min());
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return oldest;
        }

        void PlayCapture(byte[] capture, string senderIdentity, double captureLengthInSeconds)
        {
            try
            {
                if (capture == null || capture.Length == 0)
                {
                    return;
                }
                lock (_syncReceivedCaptures)
                {
                    byte[] toPlay = capture;
                    if (_captures.Count == 0)
                    {
                        this.AddCapture(senderIdentity, capture);
                    }
                    else
                    {
                        toPlay = PopOldestCapture(senderIdentity);
                        this.AddCapture(senderIdentity, capture);
                    }
                }
                bool eliminateNoise = bool.Parse(ConfigurationManager.AppSettings["eliminateNoise"]);
                if (eliminateNoise)
                {
                    NoiseEliminator eliminator = new NoiseEliminator(capture);
                    byte[] clear = eliminator.EliminateNoise();
                    if (clear != null && clear.Length > 0)
                    {
                        PlaySound(clear, senderIdentity, captureLengthInSeconds);
                    }
                    else
                    {
                        PlaySound(capture, senderIdentity, captureLengthInSeconds);
                    }
                }
                else
                {
                    PlaySound(capture, senderIdentity, captureLengthInSeconds);
                }
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

        void PlaySound(byte[] capture, string senderIdentity, double captureLengthInSeconds)
        {
            try
            {
                byte[] uncompressed = Tools.Instance.DataCompression.Decompress(capture);
                SoundEffect sound = new SoundEffect(uncompressed, Microphone.Default.SampleRate, AudioChannels.Mono);
                SoundEffect.MasterVolume = 1f;
                sound.Play();
                TimeSpan ts = TimeSpan.FromMilliseconds(captureLengthInSeconds * 1000);
                Thread.Sleep(ts);
                sound.Dispose();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void OnAudioReady(object sender, EventArgs e)
        {
            try
            {
                _syncCaptures.WaitOne(); // used to wait until the room button action is processed
 
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
                            //_syncAudioInstance.Reset();
                            _audioStream = new AudioStream(this.OnAudioReady);
                            //_syncAudioInstance.Set();
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

        public void PlayAudioCapture(byte[] capture, string senderIdentity, double captureLengthInSeconds)
        {
            Thread t = new Thread(delegate()
            {
                PlayCapture(capture, senderIdentity, captureLengthInSeconds);
            });
            t.Start();
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
