using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AudioStreaming
{
    public class AudioStream : Microsoft.Xna.Framework.Game
    {
        #region private members

        GraphicsDeviceManager _graphicsManager;
        ManualResetEvent _syncChunk = new ManualResetEvent(true);
        ManualResetEvent _syncStatus = new ManualResetEvent(true);
        ManualResetEvent _syncStop = new ManualResetEvent(false);

        byte[] _buffer;
        MemoryStream _stream;
        private Microphone _microphone;
        bool _isRunning;

        #endregion

        #region proprieties

        public ManualResetEvent SyncStop
        {
            get { return _syncStop; }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public MemoryStream Stream
        {
            get { return _stream; }
            set { _stream = value; }
        }

        public ManualResetEvent SyncChunk
        {
            get
            {
                return _syncChunk;
            }
            set
            {
                _syncChunk = value;
            }
        }

        #endregion

        #region c-tor

        public AudioStream()
        {
            InitializeMicrophone();
        }

        #endregion

        #region private methods

        void InitializeMicrophone()
        {
            bool retry = true;
            while (retry)
            {
                try
                {
                    _microphone = Microphone.Default;

                    _graphicsManager = new GraphicsDeviceManager(this);
                    _graphicsManager.PreferredBackBufferHeight = 1;
                    _graphicsManager.PreferredBackBufferWidth = 1;

                    Form gameWindowForm = (Form)Form.FromHandle(this.Window.Handle);
                    gameWindowForm.Hide();
                    gameWindowForm.ShowInTaskbar = false;
                    gameWindowForm.Opacity = 0;

                    FrameworkDispatcher.Update();
                    _microphone.BufferDuration = TimeSpan.FromSeconds(1);
                    _buffer = new byte[_microphone.GetSampleSizeInBytes(_microphone.BufferDuration)];
                    _microphone.BufferReady += OnBufferReady;
                    _isRunning = true;
                    _stream = new MemoryStream();
                    retry = false;

                }
                catch (Exception)
                {
                    //MessageBox.Show("Cannot initialize the audio module. Please restart the app.",
                    //    "Audio Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    retry = true;
                    if (_microphone != null)
                    {
                        _microphone.Stop();
                    }
                }
            }
        }

        private void OnBufferReady(object sender, EventArgs e)
        {
            SyncChunk.WaitOne();
            if (_isRunning)
            {
                _microphone.GetData(_buffer);
                if (_stream == null)
                {
                    _stream = new MemoryStream();
                }
                _stream.Write(_buffer, 0, _buffer.Length);
            }
            else
            {
                _microphone = Microphone.Default;
                _microphone.Stop();
                _microphone.BufferReady -= this.OnBufferReady;
            }
        }

        #endregion

        #region override methods

        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                
                base.Draw(gameTime);
            }
            catch { }
        }

        protected override void Initialize()
        {
            base.Initialize();
            _stream = new MemoryStream();
            _microphone.Start();
            _isRunning = true;
        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                _syncStatus.WaitOne();
                if (_microphone == null)
                {
                    _microphone = Microphone.Default;
                }
                if (!_isRunning && _microphone.State == MicrophoneState.Started)
                {
                    _syncStop.Reset();

                    _microphone.Stop();
                }

                base.Update(gameTime);
                _syncStop.Set();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region public methods

        public void StartAudio()
        {
            _syncStatus.Reset();
            if (_microphone == null)
            {
                InitializeMicrophone();
            }
            _microphone.Start();
            _isRunning = true;
            
            _syncStatus.Set();
        }

        public void StopAudio()
        {
            _syncStatus.Reset();
            _isRunning = false;
            _syncStatus.Set();
        }

        #endregion
    }
}   