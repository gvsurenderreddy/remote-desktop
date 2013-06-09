using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using Utils;

namespace Utils
{
    public class Tools
    {
        #region private members

        DataCompression _dataCompression;
        ImageConverter _imageConverter;
        RemotingUtils _desktopViewerUtils;
        Cryptography _cryptography;
        GenericMethods _genericMethods;
        Logger _logger;
        ControlCrossThreading _crossControl;

        static Tools _instance = new Tools();
        static readonly object _syncInstance = new object();

        #endregion

        #region c-tor

        private Tools()
        {
            _logger = new Logger();
            _crossControl = new ControlCrossThreading();
            _imageConverter = new ImageConverter();
            _desktopViewerUtils = new RemotingUtils();
            _cryptography = new Cryptography();
            _genericMethods = new GenericMethods();
            _dataCompression = new DataCompression();
        }

        #endregion

        #region public methods

        public static Tools Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncInstance)
                    {
                        if (_instance == null)
                        {
                            _instance = new Tools();
                        }
                    }
                }
                return _instance;
            }
        }

        public DataCompression DataCompression
        {
            get
            {
                return this._dataCompression;
            }
        }

        public ControlCrossThreading CrossThreadingControl
        {
            get { return _crossControl; }
        }

        public Logger Logger
        {
            get { return _logger; }
        }

        public ImageConverter ImageConverter
        {
            get { return _imageConverter; }
        }

        public RemotingUtils RemotingUtils
        {
            get { return _desktopViewerUtils; }
        }

        public Cryptography Cryptography
        {
            get { return _cryptography; }
        }

        public GenericMethods GenericMethods
        {
            get { return _genericMethods; }
        }

        #endregion
    }
}
